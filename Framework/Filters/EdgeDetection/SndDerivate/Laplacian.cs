using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;
using Framework.Core.Metrics;
using Framework.Filters.Simple;
using Framework.Range;

namespace Framework.Filters.EdgeDetection.SndDerivate
{

    /// <summary>
    /// 
    /// </summary>
    [Filter("Laplacian", "Laplacian Edge Detection", "***",
            new string[] { "Edge Detection", "Second Derivate" }, true)]
    [EdgeDetectionMetric()]
    public class Laplacian : SpatialDomainFilter
    {

        #region SpatialDomainFilter

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alpha"></param>
        /// <remarks>based on fspecial.m('laplacian', ...)</remarks>
        /// <returns></returns>
        public static double[,] GenerateFilter(double alpha)
        {
            //alpha = 0.2;

            if (alpha > 1.0 || alpha < 0.0)
                throw new Exception("Invalid alpha value");

            double h1 = alpha / (alpha + 1.0); double h2 = (1 - alpha) / (alpha + 1);

            double[,] ret = new double[,] { 
                    { h1, h2, h1 }, 
                    { h2, -4/ (alpha + 1), h2 }, 
                    { h1, h2 , h1 } };

            return ret;
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

            ret.Add("Threshold", Rangeable.ForByte(5));
            ret.Add("Alpha", new Rangeable(0.2, 0.0001, 0.9999, 0.0001));

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
            // Kernels in Gonzalez and Woods: Figure 3.39, 3.41

            //3x3:
            // 0 1 0
            // 1 -4 1
            // 0 1 0
            //
            // 0 -1 0
            // -1 4 -1
            // 0 -1 0
            //
            // 1 1 1
            // 1 -8 1
            // 1 1 1
            //
            // -1 2 -1
            // 2 -4 2
            // -1 2 -1

            double alpha = double.Parse(iConfigs["Alpha"].ToString());

            Kernel2D kernel = new Kernel2D(0, 1.0, GenerateFilter(alpha));

            byte[,] tmp_ret = Kernel2D.ApplyConvolution(kernel, iImageSrc);

            double t = double.Parse(iConfigs["Threshold"].ToString());

            if (t > 0)
            {
                // Apply High and Low Thresholding ...

                HighThreshold h = new HighThreshold();
                SortedDictionary<string, object> hConf = h.GetDefaultConfigs();
                hConf["High-Threshold"] = t + 1;

                LowThreshold l = new LowThreshold();
                SortedDictionary<string, object> lConf = l.GetDefaultConfigs();
                lConf["Low-Thresold"] = t;

                tmp_ret = h.ApplyFilter(tmp_ret, hConf);
                tmp_ret = l.ApplyFilter(tmp_ret, lConf);
            }


            return tmp_ret;
        }

        #endregion

    }
}
