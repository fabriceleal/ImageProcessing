using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;

namespace Framework.Filters.Simple
{
    /// <summary>
    /// Apply the greyscale normalize filter to an image.
    /// </summary>
    /// <remarks>Normalizes the values of the intensities so that they are from 0 to 255.
    /// </remarks>
    [Filter("Greyscale Normalize", "Greyscale Normalize filter",
            "Normalizes the values of the intensities so that they are from 0 to 255.", 
            new string[] { "Simple" }, true)]
    public class Normalize : SpatialDomainFilter
    {

        /// <summary>
        /// Apply filter to a byte[,].
        /// </summary>
        /// <param name="img"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public override byte[,] ApplyFilter(byte[,] img, SortedDictionary<string, object> configs)
        {
            int width = img.GetLength(0); int height = img.GetLength(1);
            byte[,] ret = new byte[width, height];

            // find max
            double max = byte.MinValue, dummy;
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    max = Math.Max(max, img[x, y]);

                    // as soon as possible, break the fors...
                    if (max == byte.MaxValue)
                        break;
                }

                // as soon as possible, break the fors...
                if (max == byte.MaxValue)
                    break;
            }

            // force values to range from 0 to 255 ...
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    dummy = byte.MaxValue * (img[x, y] / max);

                    ret[x, y] = (Byte)Math.Max(byte.MinValue, Math.Min(byte.MaxValue, dummy));
                }
            }

            return ret;
        }

    }
}
