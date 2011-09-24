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
    /// Apply the alpha-trimmed mean filter to an image.
    /// </summary>
    /// <remarks>
    /// Computes the output pixels by first ordering the intensities in the neighborhood, trimming the start
    /// and the end (p) values, and then calculating the mean of the remaining values.
    /// </remarks>
    [Filter("Alpha-Trimmed Mean", "Alpha-Trimmed Mean Filter",
            "Computes the output pixels by first ordering the intensities in the neighborhood, trimming" +
            " the start and the end (p) values, and then calculating the mean of the remaining values.",
            new string[] { "Smoothing", "Order-Statistics" }, true)]
    [SmoothMetric]
    public class AlphaTrimmedMean : SpatialDomainFilter
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
            ret.Add("p", new Rangeable(1, 1, 20, 1));

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

            // Run window through image
            Kernel2D kernel = new Kernel2D(0, 1, window);
            int p = int.Parse(iConfigs["p"].ToString());

            List<byte> tmp = new List<byte>(); int i; double ret;

            byte[,] tmp_ret = Kernel2D.ApplyFunction(
                    kernel, imageSrc,
                    delegate(Tuple<Point, Point>[] mappings, byte[,] op)
                    {
                        tmp.Clear();

                        // Get all pixels
                        for (i = 0; i < mappings.Length; ++i)
                        {
                            if (mappings[i].Item2.X != -1 && mappings[i].Item2.Y != -1)
                            {
                                tmp.Add(op[mappings[i].Item2.X, mappings[i].Item2.Y]);
                            }
                        }

                        // Do paddings with 0s and 255s. 
                        // Put more 255 on end if difference is odd
                        {
                            int diff = window_size * window_size - tmp.Count;
                            if (diff > 0)
                            {
                                byte[] zeros = new byte[diff / 2];
                                byte[] maxes = new byte[(int)Math.Ceiling(diff / 2.0)];

                                // zeros automatically initialized with zeros
                                // ...

                                // init maxes to byte.MaxValue
                                for (i = 0; i < maxes.Length; ++i)
                                    maxes[i] = byte.MaxValue;

                                tmp.InsertRange(0, zeros);
                                tmp.AddRange(maxes);
                            }
                        }

                        if (tmp.Count > 0)
                        {
                            // Sort
                            tmp.Sort();

                            ret = 0.0;

                            for (i = p; i < tmp.Count - p; ++i)
                            {
                                ret += tmp[i];
                            }

                            ret = 1.0 / ((double)tmp.Count - 2.0 * (double)p) * ret;

                            // trim
                            ret = Math.Min(byte.MaxValue, Math.Max(byte.MinValue, ret));

                            return (Byte)ret;
                        }

                        return 0;
                    });

            return tmp_ret;
        }

        #endregion

    }
}
