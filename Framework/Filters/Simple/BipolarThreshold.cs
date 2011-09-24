using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;
using Framework.Core.Metrics;
using Framework.Range;

namespace Framework.Filters.Simple
{

    /// <summary>
    /// Apply a bipolar threshold to an image. 
    /// </summary>
    /// <remarks>
    /// All values equal or greater than a threshold will be set to white, 
    /// all values smaller to that value will be set to black
    /// </remarks>
    [Filter("Bipolar Threshold", "Bipolar Threshold filter",
        "All values equal or greater than a threshold will be set to white " +
        " all values smaller to that value will be set to black",
        new string[] { "Simple" }, true)]
    [AllMetricAttribute()]
    public class BipolarThreshold : SpatialDomainFilter
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

            ret.Add("Threshold", Rangeable.ForByte(124));

            return ret;
        }

        public BipolarThreshold()
        {

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
            byte t = byte.Parse(configs["Threshold"].ToString());

            byte[,] ret = new byte[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    // Forces intensities equal or higher than a value to White. All other intensities are set to Black.
                    ret[x, y] = img[x, y] >= t ? byte.MaxValue : byte.MinValue;
                }
            }

            return ret;
        }
    }
}
