using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.IO;

namespace Framework.Core
{
    /// <summary>
    /// Wrapper to the System.Drawing.Image class that enables caching of images in disk.
    /// </summary>
    /// <example>
    /// <code>
    /// // Read image from disk
    /// Image img = Image.FromFile("test.bmp");
    /// 
    /// // Store in WeakImage
    /// WeakImage weakImg = new WeakImage(img);
    /// 
    /// // Dispose instance and reset reference
    /// img.Dispose(); img = null;
    /// 
    /// // ## After a collect, img will be collected and saved in disk. ##
    /// 
    /// // The image will be read from disk and restored to memory.
    /// img = weakImg.Image;
    /// 
    /// </code>
    /// </example>
    public class WeakImage :
        IDisposable
    {

        /// <summary>
        /// The name of the temp file used to cache the image.
        /// </summary>
        private string _filename;

        /// <summary>
        /// The weak reference to the image.
        /// </summary>
        private WeakReference _image;

        // Attention: this may not be enough.
        // Do a wrapper to set the tag to the image, or changes to tag directly into the image reference
        // will not be detected ...

        /// <summary>
        /// The information stored in the Image.Tag property will be stored here.
        /// </summary>
        private object _imageTag;

        /// <summary>
        /// Creates a clean WeakImage
        /// </summary>
        public WeakImage()
        {
            // Generates filename
            _filename = Path.GetTempFileName();
            string dirname = Path.GetDirectoryName(_filename);
            string filename = Path.GetFileName(_filename);
            // Add "private" folder to the Path of the temporary folder;
            // if these files are all in an unique, private folder, its easier to delete them all
            _filename = Path.Combine(Path.Combine(dirname, "Framework"), filename);
        }

        /// <summary>
        /// New WeakImage
        /// </summary>
        /// <param name="image">The image to cache.</param>
        /// <param name="forceImageDispose">Set to true to dispose the image immediatelly.</param>
        public WeakImage(Image image, bool forceImageDispose)
            : this()
        {
            Image = image;
            if (forceImageDispose)
            {
                image.Dispose(); image = null;
            }
        }

        /// <summary>
        /// New WeakImage
        /// </summary>
        /// <remarks>The image instance will not disposed. You may want to do it manually whenever you want.
        /// </remarks>
        /// <param name="image">The image to cache.</param>
        public WeakImage(Image image)
            : this(image, false)
        {            
        }

        /// <summary>
        /// Get the file used to cache the image.
        /// </summary>
        public string Filename
        {
            get
            {
                return _filename;
            }
        }

        /// <summary>
        /// Gets the image. If is cached in the file system, it will be ressurrected.
        /// </summary>
        public Image Image
        {
            get
            {
                // http://www.codeproject.com/KB/cs/ImageManager.aspx
                if (_image == null)
                    return null;

                Image theImage = _image.Target as Image;
                if (theImage == null)
                {
                    // ressurect from the temp file
                    if (string.IsNullOrEmpty(_filename) || !File.Exists(_filename))
                        return null;

                    theImage = Image.FromFile(_filename);
                    theImage.Tag = _imageTag;
                    _image.Target = theImage;
                }
                return theImage;
            }
            set
            {
                if (value == null)
                {
                    // Reset weak reference
                    if (_image != null)
                        _image = null;

                    _imageTag = null;

                    // Delete temp file
                    if (!string.IsNullOrEmpty(_filename))
                    {
                        if (File.Exists(_filename))
                            File.Delete(_filename);
                    }
                }
                else
                {
                    // Generate and set weak reference
                    if (_image == null)
                    {
                        _image = new WeakReference(value, false);
                    }
                    else
                    {
                        _image.Target = value;
                    }

                    _imageTag = value.Tag;

                    // Check if dir exists, create if not
                    if (!Directory.Exists(Path.GetDirectoryName(_filename)))
                        Directory.CreateDirectory(Path.GetDirectoryName(_filename));

                    // Persist to temp file
                    value.Save(_filename, value.RawFormat);
                }
            }
        }

        #region IDisposable

        // Accordingly to:
        // http://msdn.microsoft.com/en-us/library/system.idisposable.aspx

        protected bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                // Delete the temp file
                if (File.Exists(_filename))
                {
                    try
                    {
                        File.Delete(_filename);
                    }
                    catch (Exception e)
                    {
                        // The deletion of a file may throw an Exception if it is
                        // unexpectedly being used by another process ...
                        // Silent exception.
                    }
                    _filename = "";
                }

                if (disposing)
                {
                    // Dispose the image ...
                    // access directly the instance of the image, not the property wrapper.
                    if (_image.Target != null)
                    {
                        ((Image)_image.Target).Dispose();
                    }
                }

                _disposed = true;
            }
        }

        // Destructor
        ~WeakImage()
        {
            Dispose(false);
        }

        #endregion

        #region Cast operators, implicit and explicit

        //// Image
        //public static explicit operator Image(WeakImage weakIm)
        //{
        //    return weakIm.Image;
        //}

        public static explicit operator Image(WeakImage weakIm)
        {
            return weakIm.Image;
        }

        //// Bitmap
        //public static explicit operator Bitmap(WeakImage weakIm)
        //{
        //    return weakIm.Image as Bitmap;
        //}

        public static explicit operator Bitmap(WeakImage weakIm)
        {
            return weakIm.Image as Bitmap;
        }

        #endregion

    }
}
