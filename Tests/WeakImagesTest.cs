using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Core;
using System.Collections;
using System.IO;

namespace Tests
{
    [TestClass]
    public class WeakImagesTest
    {
        /// <summary>
        /// Testing memory releaf:
        /// * Use of WeakImages should show a decrease of used memory after a collect.
        /// </summary>
        [TestMethod]
        public void WeakImagesTestMemoryReleaf()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            long beforeLoad = GC.GetTotalMemory(false);

            // Load images
            List<Image> _images = new List<Image>();

            // Get big bunch of big images
            for (int i = 0; i < 1; ++i)
            {
                _images.Add((Image)Properties.Resources.big_img.Clone());
            }

            long afterLoad = GC.GetTotalMemory(false);

            // --- Create list of weak references. Dispose the images.
            List<WeakImage> _weaks = new List<WeakImage>();
            foreach (Image i in _images)
            {
                _weaks.Add(new WeakImage(i));
                i.Dispose();
            }
            _images = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            long afterCollect = GC.GetTotalMemory(false);

            Trace.WriteLine(string.Format("Before Load = {0}; After Load = {1}, Diff = {2}", beforeLoad, afterLoad, afterLoad - beforeLoad));
            Trace.WriteLine(string.Format("After Load = {0}; After Col. = {1}, Diff = {2}", afterLoad, afterCollect, afterCollect - afterLoad));

        }


        /// <summary>
        /// Testing data integrity:
        /// * image in WeakImage should be not null and equal to the original image even after a collect.
        /// </summary>
        [TestMethod()]
        public void WeakImagesTestDataIntegrity()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Trace.WriteLine(string.Format("Init: GC.GetTotalMemory = {0}", GC.GetTotalMemory(false)));

            // Load images
            Hashtable _images = new Hashtable();
            Hashtable _weaks = new Hashtable();
            foreach (Image i in new Image[] { (Image)Properties.Resources.edges_0.Clone() })
            {
                string f = Guid.NewGuid().ToString();
                MemoryStream ms = new MemoryStream();

                i.Save(ms, i.RawFormat);

                //Trace.WriteLine(i.RawFormat.Guid);

                _images.Add(f, ms);
                _weaks.Add(f, new WeakImage(i));
            }

            Trace.WriteLine(string.Format("Before Collect: GC.GetTotalMemory = {0}", GC.GetTotalMemory(false)));
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Trace.WriteLine(string.Format("After Collect: GC.GetTotalMemory = {0}", GC.GetTotalMemory(false)));

            foreach (string key in _images.Keys)
            {
                Trace.WriteLine("Using key " + key);

                Assert.IsTrue(_images.ContainsKey(key), "_images dont have key = {0}", key);
                Assert.IsTrue(_weaks.ContainsKey(key), "_weaks dont have key = {0}", key);

                MemoryStream saved = _images[key] as MemoryStream;

                Assert.IsNotNull(saved, "Memory Stream saved is null, key = {0}.", key);

                Image img = (_weaks[key] as WeakImage).Image;

                Assert.IsNotNull(img, "Image gotten from WeakImage.Image is null, key = {0}.", key);

                MemoryStream toCompare = new MemoryStream();
                //Trace.WriteLine(System.Drawing.Imaging.ImageFormat.Bmp.Guid);
                img.Save(toCompare, System.Drawing.Imaging.ImageFormat.Bmp);

                byte[] streamSaved = saved.ToArray();
                byte[] streamToCompare = toCompare.ToArray();

                Assert.AreEqual(streamSaved.Length, streamToCompare.Length, "Sizes do not match, key = {0}.", key);

                // Compare, byte-byte, the two arrays ...
                for (int i = 0; i < streamSaved.Length; ++i)
                {
                    Assert.AreEqual(streamSaved[i], streamToCompare[i], "Streams do not match at index {0}, key = {1}", i, key);
                }
            }

        }

        /// <summary>
        /// Test the IDisposable implementation of WeakImage
        /// * The image should be cached to a file immediatelly after the creation of the instance ...
        /// * After being disposed, the temp files should be removed ...
        /// </summary>
        [TestMethod()]
        public void WeakImagesTestDisposable()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Trace.WriteLine(string.Format("Init: GC.GetTotalMemory = {0}", GC.GetTotalMemory(false)));

            List<WeakImage> _weaks = new List<WeakImage>();
            List<string> tempFiles = new List<string>();
            foreach (Image i in new Image[] { (Image)Properties.Resources.big_img.Clone() })
            {
                WeakImage w = new WeakImage(i);

                _weaks.Add(w);
                tempFiles.Add(w.Filename);

                long size = (new FileInfo(w.Filename)).Length;

                Assert.AreNotEqual(0, size, "Size of the file {0} is zero!");

                Trace.WriteLine(string.Format("File {0}, size = {1}.", w.Filename, size));

                i.Dispose();
            }

            // Check that every files exists before a collect
            Trace.WriteLine(string.Format("Before Collect: GC.GetTotalMemory = {0}", GC.GetTotalMemory(false)));
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Trace.WriteLine(string.Format("After Collect: GC.GetTotalMemory = {0}", GC.GetTotalMemory(false)));

            // Check that every files exists after a collect
            foreach (string file in tempFiles)
            {
                Trace.WriteLine(string.Format("Test if {0} is alive.", file));
                Assert.IsTrue(File.Exists(file), "File {0} was expected to be alive, is dead.", file);
            }

            // Dispose every WeakImage
            foreach (WeakImage weak in _weaks)
            {
                weak.Dispose();
            }

            // Check that every files is deleted
            foreach (string file in tempFiles)
            {
                Trace.WriteLine(string.Format("Test if {0} is dead.", file));
                Assert.IsFalse(File.Exists(file), "File {0} was expected to be dead, is alive.", file);
            }
        }

        /// <summary>
        /// Test the IDisposable implementation of WeakImage, without calling the Dispose() method
        /// * The image should be cached to a file immediatelly after the creation of the instance ...
        /// * After being collected, the temp files should be removed ...
        /// </summary>
        [TestMethod()]
        public void WeakImagesTestCollection()
        {
            List<string> tempFiles = new List<string>();

            // Creation of local scope ...
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Trace.WriteLine(string.Format("Init: GC.GetTotalMemory = {0}", GC.GetTotalMemory(false)));

                List<WeakImage> _weaks = new List<WeakImage>();

                foreach (Image i in new Image[] { (Image)Properties.Resources.big_img.Clone() })
                {
                    WeakImage w = new WeakImage(i);

                    _weaks.Add(w);
                    tempFiles.Add(w.Filename);

                    long size = (new FileInfo(w.Filename)).Length;

                    Assert.AreNotEqual(0, size, "Size of the file {0} is zero!");

                    Trace.WriteLine(string.Format("File {0}, size = {1}.", w.Filename, size));

                }

                // Check that every files exists before a collect
                Trace.WriteLine(string.Format("Before Collect: GC.GetTotalMemory = {0}", GC.GetTotalMemory(false)));
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Trace.WriteLine(string.Format("After Collect: GC.GetTotalMemory = {0}", GC.GetTotalMemory(false)));

                // Check that every files exists after a collect
                foreach (string file in tempFiles)
                {
                    Trace.WriteLine(string.Format("Test if {0} is alive.", file));
                    Assert.IsTrue(File.Exists(file), "File {0} was expected to be alive, is dead.", file);
                }

                // Resets the list.
                // Do not call Dispose directly!!!
                _weaks.Clear(); _weaks = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            //// Force new Collection.
            //// You have to call it n times. DOC!!!
            //for (int i = 0; i < 10; ++i)
            //{
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //}

            //System.IO.Directory.GetFiles(System.IO.Path.GetTempPath());
            // Wait for pending IO operations ...
            System.Threading.Thread.Sleep(10000);
            // Dummy way to refresh file system
            foreach (string file in tempFiles)
            {
                Trace.WriteLine(string.Format("{0} exists = {1}", file, File.Exists(file)));
            }

            // Check that every files is deleted
            foreach (string file in tempFiles)
            {
                Trace.WriteLine(string.Format("Test if {0} is dead.", file));

                Assert.IsFalse(File.Exists(file), "File {0} was expected to be dead, is alive.", file);
            }


        }
    }
}
