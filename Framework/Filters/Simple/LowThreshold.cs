using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;
using Framework.Range;

namespace Framework.Filters.Simple
{
    /// <summary>
    /// Apply a low threshold to an image.
    /// </summary>
    /// <remarks>
    /// All intensities lower than a threshold are forced to black.
    /// </remarks>
    [Filter("Low Threshold", "Low Threshold filter",
            "All intensities lower than a threshold are forced to black.", 
            new string[] { "Simple" }, true)]
    public class LowThreshold : SpatialDomainFilter
    {

        /// <summary>
        /// Generate default configurations for the filter.
        /// </summary>
        /// <returns>Dictionary used to pass configuration though the 
        /// configs parameter of the method ApplyFilter. 
        /// null if there is the filter doesn't use configurations.</returns>
        public override SortedDictionary<string, object> GetDefaultConfigs()
        {
            SortedDictionary<string, object> ret = new SortedDictionary<string, object>();

            ret.Add("Low-Threshold", Rangeable.ForByte(124));

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
            byte t = byte.Parse(configs["Low-Threshold"].ToString());

            byte[,] ret = new byte[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    // Forces intensities smaller than a value to Black. All other intensities stay the same.
                    ret[x, y] = img[x, y] >= t ? img[x, y] : byte.MinValue;
                }
            }

            return ret;
        }
    }
}
