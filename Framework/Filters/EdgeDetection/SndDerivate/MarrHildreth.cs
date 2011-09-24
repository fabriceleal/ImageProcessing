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

namespace Framework.Filters.EdgeDetection.SndDerivate
{
    /// <summary>
    /// 
    /// </summary>
    [Filter("Marr-Hildreth", "Marr-Hildreth Edge Detection", "***",
            new string[] { "Edge Detection", "Second Derivate" }, true)]
    [EdgeDetectionMetric()]
    public class MarrHildreth : SpatialDomainFilter
    {
        Gaussian g;
        LaplacianOfGauss logauss;

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

            // Add configs from Gaussian
            SortedDictionary<string, object> tmp_conf = g.GetDefaultConfigs();
            if (null != tmp_conf)
            {
                foreach (KeyValuePair<string, object> item in tmp_conf)
                {
                    ret.Add(item.Key, item.Value);
                }
            }

            // Add configs from Laplacian of Gauss
            tmp_conf = logauss.GetDefaultConfigs();
            if (null != tmp_conf)
            {
                foreach (KeyValuePair<string, object> item in tmp_conf)
                {
                    if (!ret.ContainsKey(item.Key))
                    {
                        ret.Add(item.Key, item.Value);
                    }
                }
            }

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
            return logauss.ApplyFilter(g.ApplyFilter(iImageSrc, iConfigs), iConfigs);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public MarrHildreth()
        {
            g = new Gaussian();
            logauss = new LaplacianOfGauss();
        }

    }
}
