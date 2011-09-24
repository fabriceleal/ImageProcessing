using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;
using Framework.Filters.Smoothing;
using Framework.Core.Metrics;
using Framework.Filters.Simple;

namespace Framework.Filters.EdgeDetection.Probabilistic
{
    /// <summary>
    /// Apply the Canny edge detection algorithm.
    /// </summary>
    /// The Canny edge detection algorithm smooths the image with a gaussian kernel; it then
    /// calculates the direction of the edges, applies non-maximum supreession and applies hystersis.
    /// <remarks>
    /// </remarks>
    [Filter("Canny", "Canny Edge Detection",
            "The Canny edge detection algorithm smooths the image with a gaussian kernel; it then" +
            " calculates the direction of the edges, applies non-maximum supreession and applies hystersis.",
            new string[] { "Edge Detection", "Probabilistic" }, true)]
    [EdgeDetectionMetric()]
    public class Canny : SpatialDomainFilter
    {

        private Hystersis hystersis;
        private Gaussian gaussian;
        private NonMaximumSupression nonMaximum;

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
            SortedDictionary<string, object> hRet = hystersis.GetDefaultConfigs();
            SortedDictionary<string, object> gRet = gaussian.GetDefaultConfigs();

            if (null != hRet)
            {
                foreach (KeyValuePair<string, object> it in hRet)
                {
                    ret.Add(it.Key, it.Value);
                }
            }
            if (null != gRet)
            {
                foreach (KeyValuePair<string, object> it in gRet)
                {
                    ret.Add(it.Key, it.Value);
                }
            }

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
            // From AfED.doc, Canny Edge Detection.pdf (from article in codeproject.com)

            // (1) Smoothing, with the Gaussian filter
            byte[,] step1 = gaussian.ApplyFilter(img, configs);

            // (2) Apply non-maximum supression
            byte[,] step2 = nonMaximum.ApplyFilter(step1, configs);

            // (3) Apply hystersis
            return hystersis.ApplyFilter(step2, configs);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Canny()
        {
            hystersis = new Hystersis();
            gaussian = new Gaussian();
            nonMaximum = new NonMaximumSupression();
        }


    }
}
