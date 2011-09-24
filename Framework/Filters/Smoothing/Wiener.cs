using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;
using Framework.Core.Filters.Frequency;
using Framework.Transforms;
using Framework.Core.Metrics;
using Framework.Filters.Smoothing.Mean;
using Framework.Filters.Smoothing.OrderStatistics;
using Framework.Range;

namespace Framework.Filters.Smoothing
{
    /// <summary>
    /// Apply the wiener filter to an image.
    /// </summary>
    /// <remarks>
    /// Despite the fact that the Wiener restoration filter is defined as a Frequency Domain filter (Gonzalez), 
    /// it is implemented in the Framework as a SpatialDomainFilter. This implementation is a direct
    /// translation from the MATLAB function wiener2d().
    /// </remarks>
    [Filter("Wiener", "Wiener Filter", 
        "Implementation of the Wiener filter from the MATLAB function wiener2d.", 
        new string[] { "Smoothing" }, true)]
    [SmoothMetric]
    public class Wiener : SpatialDomainFilter
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
        /// <param name="img"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public override byte[,] ApplyFilter(byte[,] img, SortedDictionary<string, object> configs)
        {
            int width = img.GetLength(0), height = img.GetLength(1);
            byte[,] ret = new byte[width, height];
            int window_size = int.Parse(configs["window size"].ToString());
            int x, y; double val;

            double[,] window = new double[window_size, window_size];

            for (x = 0; x < window_size; ++x)
            {
                for (y = 0; y < window_size; ++y)
                {
                    window[x, y] = 1.0;
                }
            }

            ArithmeticMean mean = new ArithmeticMean();

            SortedDictionary<string, object> meanConf = mean.GetDefaultConfigs();
            meanConf["window size"] = configs["window size"];


            byte[,] meaned = mean.ApplyFilter(img, meanConf);
            double[,] img_sqrd = new double[width, height];
            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    img_sqrd[x, y] = (double)img[x, y] * (double)img[x, y];
                }
            }

            double[,] varianced = Kernel2D.ApplyConvolution(
                    new Kernel2D(0, 1 / (1.0 * window_size * window_size),
                    window), img_sqrd);

            double noise = 0;

            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    varianced[x, y] -= (double)meaned[x, y] * (double)meaned[x, y];
                    noise += varianced[x, y];
                }
            }

            noise /= (width * height * 1.0);


            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    val = (((img[x, y] - meaned[x, y]) / Math.Max(varianced[x, y], noise)) *
                        Math.Max(varianced[x, y] - noise, 0)) + meaned[x, y];

                    ret[x, y] = (byte)Math.Min(byte.MaxValue, Math.Max(byte.MinValue, val));
                }
            }

            return ret;
        }

        #endregion

    }
}
