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
    /// Apply the invert filter to an image.
    /// </summary>
    /// <remarks>
    /// Inverts the values of the intensities of the pixels in an image.
    /// If the image is in a RGB color model, the values of the Red, Green and Blue components
    /// will be inverted individually.
    /// </remarks>
    [Filter("Invert", "Invert Filter",
            "Inverts the values of the intensities of the pixels in an image." +
            " If the image is in a RGB color model, the values of the Red, Green and Blue components" +
            " will be inverted individually.", new string[] { "Simple" }, true)]
    public class Invert : SpatialDomainFilter
    {

        #region ImageFilterCore

        /// <summary>
        /// Apply filter to an Image.
        /// </summary>
        /// <param name="imageSrc"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public override Image ApplyFilter(Image imageSrc, SortedDictionary<string, object> config)
        {
            Color[,] colors = Facilities.ToColor(
                    Facilities.ToBitmap(imageSrc));

            int width = colors.GetLength(0), height = colors.GetLength(1);
            Color[,] ret = new Color[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    ret[x, y] = Color.FromArgb(
                            255 - colors[x, y].R,
                            255 - colors[x, y].G,
                            255 - colors[x, y].B);
                }
            }

            return Facilities.ToImage(Facilities.ToBitmap(ret));
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
            byte[,] ret = new byte[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    ret[x, y] = (byte)(255 - img[x, y]);
                }
            }

            return ret;
        }

        #endregion

    }
}
