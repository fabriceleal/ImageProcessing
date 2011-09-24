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

namespace Framework.Filters.Smoothing
{
    /// <summary>
    /// Apply the gaussian filter to an image.
    /// </summary>
    /// <remarks>
    /// Based on fspecial.m('gaussian', ...).
    /// </remarks>
    [Filter("Gaussian", "Gaussian Filter", "xxx", new string[] { "Smoothing" }, true)]
    [SmoothMetric]
    public class Gaussian : SpatialDomainFilter
    {

        #region SpatialDomainFilter

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alpha"></param>
        /// <remarks>based on fspecial.m('gaussian', ...)</remarks>
        /// <returns></returns>
        public static double[,] GenerateFilter(double sigma)
        {
            int size = (int)(Math.Ceiling(sigma * 3) * 2 + 1);
            if (size % 2 == 0)
                throw new Exception(string.Format("Size must be odd! (size = {0} for sigma = {1})", size, sigma));

            double[,] window = new double[size, size];
            double sigma_sqrd_2 = sigma * sigma * 2.0;
            double max = double.MinValue;

            // used by Matlab ... :S
            double epsilon = Math.Pow(2, -52);

            int x, y, x0, y0, size_over_2 = size / 2;
            for (x = 0; x < size; ++x)
            {
                for (y = 0; y < size; ++y)
                {
                    x0 = x - size_over_2; y0 = y - size_over_2;

                    window[x, y] = Math.Exp(-(x0 * x0 + y0 * y0) / sigma_sqrd_2);

                    max = Math.Max(window[x, y], max);
                }
            }

            // In matlab: h( h<eps*max(h(:)) ) = 0;
            epsilon *= max;
            max = 0.0;
            for (x = 0; x < size; ++x)
            {
                for (y = 0; y < size; ++y)
                {
                    if (window[x, y] < epsilon)
                        window[x, y] = 0;
                    max += window[x, y];
                }
            }

            // Normalize: 0.0 to 1.0
            if (max != 0)
            {
                for (x = 0; x < size; ++x)
                {
                    for (y = 0; y < size; ++y)
                    {
                        window[x, y] /= max;
                    }
                }
            }

            return window;
        }

        /// <summary>
        /// Generate default configurations for the filter.
        /// </summary>
        /// <returns>Dictionary used to pass configuration though the 
        /// configs parameter of the method ApplyFilter. 
        /// null if there is the filter doesn't use configurations.</returns>
        public override SortedDictionary<string, object> GetDefaultConfigs()
        {
            SortedDictionary<string, object> ret = new SortedDictionary<string, object>();

            ret.Add("Sigma", new Rangeable(0.5, 0.5, 20, 0.5));

            return ret;
        }

        /// <summary>
        /// Apply filter to a byte[,].
        /// </summary>
        /// <param name="iImageSrc"></param>
        /// <param name="iConfigs"></param>
        /// <returns></returns>
        public override byte[,] ApplyFilter(byte[,] iImageSrc, SortedDictionary<string, object> iConfigs)
        {
            double sigma = double.Parse(iConfigs["Sigma"].ToString());

            Kernel2D kernel = new Kernel2D(0, 1.0, GenerateFilter(sigma));

            byte[,] tmp_ret = Kernel2D.ApplyConvolution(kernel, iImageSrc);

            return tmp_ret;
        }

        #endregion

    }
}
