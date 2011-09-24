using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.IO;

namespace Tests
{
    public class Functions
    {

        public static Image[] OpenAllImages()
        {
            List<Image> images = new List<Image>();
            DirectoryInfo dir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));

            FileInfo[] files = dir.GetFiles();

            foreach(FileInfo f in files){
                try
                {
                    images.Add(Image.FromFile(f.FullName));
                }
                catch (Exception)
                {
                    // Ignore all Non-images ...
                }
            }

            return images.ToArray();
        }

    }
}
