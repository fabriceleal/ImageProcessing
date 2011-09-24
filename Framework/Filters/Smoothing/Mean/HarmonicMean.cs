using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;
using Framework.Core.Filters.Frequency;
using Framework.Core.Metrics;
using Framework.Range;

namespace Framework.Filters.Smoothing.Mean
{
    /// <summary>
    /// Apply the harmonic mean filter to an image.
    /// </summary>
    /// <remarks>
    /// Computes the output pixels by evaluating the harmonic mean 
    /// of the neighborhood of the pixel.
    /// </remarks>
    [Filter("Harmonic Mean", "Harmonic Mean Filter",
            "Computes the output pixels by evaluating the harmonic mean" +
            " of the neighborhood of the pixel.",
            new string[] { "Smoothing", "Mean" }, true)]
    [SmoothMetric]
    public class HarmonicMean : SpatialDomainFilter
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
        /// <param name="iConfigs"></param>
        /// <returns></returns>
        public override byte[,] ApplyFilter(byte[,] imageSrc, SortedDictionary<string, object> iConfigs)
        {
            // Generate window ...
            int window_size = int.Parse(iConfigs["window size"].ToString());

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

            Kernel2D kernel = new Kernel2D(0, 1.0, window);
            int i; double numerator = (window_size * window_size);

            // Apply convolution with window
            byte[,] tmp_ret = Kernel2D.ApplyFunction(
                    kernel, imageSrc,
                    delegate(Tuple<Point, Point>[] mappings, byte[,] operand)
                    {
                        double ret = 0.0;

                        for (i = 0; i < mappings.Length; ++i)
                        {
                            if (mappings[i].Item2.X != -1 && mappings[i].Item2.Y != -1)
                            {
                                ret += 1.0 / (double)operand[mappings[i].Item2.X, mappings[i].Item2.Y];
                            }
                        }

                        if (ret != 0)
                        {
                            ret = numerator / ret;
                        }

                        // trim
                        ret = Math.Min(byte.MaxValue, Math.Max(byte.MinValue, ret));

                        return (Byte)ret;
                    });

            return tmp_ret;
        }

        #endregion

    }
}
