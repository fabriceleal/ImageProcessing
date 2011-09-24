using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;

namespace Framework.Filters.Simple
{
    /// <summary>
    /// Apply the padding filter to an Image.
    /// </summary>
    /// <remarks>
    /// Padds an image with zeros, until the image as base-2 dimensions.
    /// </remarks>
    [Filter("Padding", "Padding filter",
            "Padds an image with zeros, until the image as base-2 dimensions.", 
            new string[] { "Simple" }, true)]
    public class Padding : SpatialDomainFilter
    {

        /// <summary>
        /// Apply filter to an Image.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public override Image ApplyFilter(Image img, SortedDictionary<string, object> configs)
        {
            int width = img.Width, height = img.Height;

            int new_width = (int)Math.Pow(2, Math.Ceiling(Math.Log(width, 2)));
            int new_height = (int)Math.Pow(2, Math.Ceiling(Math.Log(height, 2)));

            Bitmap ret_tmp = new Bitmap(new_width, new_height);
            Image ret = Facilities.ToImage(ret_tmp);

            using (Graphics g = Graphics.FromImage(ret))
            {
                g.Clear(Color.Black);
                g.DrawImage(img, 0, 0, width, height);
            }
            ret_tmp.Dispose();

            return ret;
        }

        /// <summary>
        /// Apply filter to a byte[,].
        /// </summary>
        /// <param name="img"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public override byte[,] ApplyFilter(byte[,] img, SortedDictionary<string, object> configs)
        {
            int width = img.GetLength(0), height = img.GetLength(1);

            int width_new = (int)Math.Pow(2, Math.Ceiling(Math.Log(width, 2)));
            int height_new = (int)Math.Pow(2, Math.Ceiling(Math.Log(height, 2)));

            byte[,] ret = new byte[width_new, height_new];

            // --
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    ret[x, y] = img[x, y];
                }
            }

            return ret;
        }

    }
}
