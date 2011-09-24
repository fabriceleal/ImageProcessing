using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;
using Framework.Core.Metrics;
using Framework.Range;

namespace Framework.Filters.Smoothing.OrderStatistics
{
    /// <summary>
    /// Apply the min filter to an image.
    /// </summary>
    /// <remarks>
    /// Computes the output pixels by taking the minimum intensity
    /// of the neighborhood of the pixel.
    /// </remarks>
    [Filter("Min", "Min Filter",
            "Computes the output pixels by taking the minimum intensity" +
            " of the neighborhood of the pixel.",
            new string[] { "Smoothing", "Order-Statistics" }, true)]
    [SmoothMetric]
    public class Min : SpatialDomainFilter
    {

        #region SpatialDomainFilter

        /// <summary>
        /// Generate default configurations for the filter.
        /// </summary>
        /// <returns>Dictionary used to pass configuration though the 
        /// configs parameter of the method ApplyFilter. 
        /// null if there is the filter doesn't use configurations.</returns>
        public override SortedDictionary<string, object> GetDefaultConfigs()
        {
            SortedDictionary<string, object> ret = new SortedDictionary<string, object>();

            ret.Add("window size", new Rangeable(3, 3, 333, 2));

            return ret;
        }

        /// <summary>
        /// Apply filter to a byte[,].
        /// </summary>
        /// <param name="imageSrc"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public override byte[,] ApplyFilter(byte[,] imageSrc, SortedDictionary<string, object> configs)
        {
            // Generate window ...
            int window_size = int.Parse(configs["window size"].ToString());

            if (window_size % 2 == 0)
                throw new Exception("Window size must be odd!");

            double[,] window = new double[window_size, window_size];
            for (int x = 0; x < window_size; ++x)
            {
                for (int y = 0; y < window_size; ++y)
                {
                    window[x, y] = 1;
                }
            }

            // Run window throgh image
            Kernel2D kernel = new Kernel2D(0, 1, window);

            int i; byte ret;

            byte[,] tmp_ret = Kernel2D.ApplyFunction(
                    kernel, imageSrc,
                    delegate(Tuple<Point, Point>[] mappings, byte[,] op)
                    {
                        ret = byte.MaxValue;

                        // Get all pixels
                        for (i = 0; i < mappings.Length; ++i)
                        {
                            if (mappings[i].Item2.X != -1 && mappings[i].Item2.Y != -1)
                            {
                                ret = Math.Min(ret, op[mappings[i].Item2.X, mappings[i].Item2.Y]);
                            }
                        }

                        return ret;
                    });

            return tmp_ret;
        }

        #endregion

    }
}
