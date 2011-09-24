using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core;

namespace Framework.Core.Metrics
{
    /// <summary>
    /// Provides a wrapper to a list of functions that will be used to
    /// measure the performance of a filter.
    /// </summary>
    /// <remarks>
    /// This class should not be used directly when associating metrics with a filter.
    /// </remarks>
    public abstract class Metric
    {

        #region Attributes

        /// <summary>
        /// Function signature for a metric function.
        /// </summary>
        /// <param name="iImageSrc">The image before the execution of the filter, or a reference.</param>
        /// <param name="iImageDest">The image after the execution of the filter.</param>
        /// <returns>Value calculated; Range / meaning of counterdomain is unconstrained.</returns>
        public delegate double MetricDelegate(Image iImageSrc, Image iImageDest);

        /// <summary>
        /// List of metric functions to use to calculate the filter's performance.
        /// </summary>
        private List<MetricMethod> _listMetrics;

        #endregion

        #region Constructors

        /// <summary>
        /// Provides a wrapper to a list a functions that can be used to
        /// measure the performance of a filter.
        /// </summary>
        /// <param name="listMetrics">
        /// List of metric functions to use to calculate the filter's performance.
        /// </param>
        internal Metric(MetricDelegate[] listMetrics)
        {
            // Preparse list of metrics ...
            _listMetrics = new List<MetricMethod>();

            if (null != listMetrics)
            {
                foreach (MetricDelegate metric in listMetrics)
                {
                    MetricMethod mm = new MetricMethod(metric);

                    _listMetrics.Add(mm);
                }
            }
        }

        //internal Metric(SortedDictionary<string, MetricDelegate> listMetrics)
        //{
        //    _listMetrics = listMetrics;
        //}

        #endregion

        #region Methods

        /// <summary>
        /// Gets the list of metric functions to use to calculate the filter's performance.
        /// </summary>
        public SortedDictionary<string, MetricDelegate> GetListMetrics()
        {
            SortedDictionary<string, MetricDelegate> ret = new SortedDictionary<string, MetricDelegate>();
            foreach (MetricMethod mm in _listMetrics)
            {
                ret.Add(mm.Attributes["Name"].ToString(), mm.Method);
            }
            return ret;
        }

        #endregion

        ////Obsolete
        //#region CalculateMetrics(Image, Image)

        //public List<MetricResult> CalculateMetrics(Image iImageRef, Image iImageDest)
        //{
        //    // Convert to Bitmap
        //    return CalculateMetrics(Facilities.ToBitmap(iImageRef), Facilities.ToBitmap(iImageDest));
        //}

        //public List<MetricResult> CalculateMetrics(Bitmap iImageRef, Bitmap iImageDest)
        //{
        //    // Convert to Color[,]
        //    return CalculateMetrics(Facilities.ToColor(iImageRef), Facilities.ToColor(iImageDest));
        //}

        //public List<MetricResult> CalculateMetrics(Color[,] iImageRef, Color[,] iImageDest)
        //{
        //    // Convert to Byte[,]
        //    return CalculateMetrics(Facilities.To8bppGreyScale(iImageRef), Facilities.To8bppGreyScale(iImageDest));
        //}

        //public List<MetricResult> CalculateMetrics(byte[,] iImageRef, byte[,] iImageDest)
        //{
        //    List<MetricResult> ret = new List<MetricResult>();
        //    // Calculate !
        //    if (null != _listMetrics)
        //    {
        //        double value;
        //        SortedDictionary<string, MetricDelegate> dMetrics = GetListMetrics();
        //        foreach (string _metricKey in dMetrics.Keys)
        //        {
        //            if (!string.IsNullOrEmpty(_metricKey))
        //            {
        //                value = dMetrics[_metricKey](iImageRef, iImageDest);
        //                ret.Add(new MetricResult(iImageRef, _metricKey, value));
        //            }
        //        }
        //    }
        //    return ret;
        //}

        //#endregion

    }
}
