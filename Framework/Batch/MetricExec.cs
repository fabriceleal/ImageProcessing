using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Core.Metrics;

namespace Framework.Batch
{
    /// <summary>
    /// Regular metric associable to a Node. The input and output images of the filter
    /// are given to the MetricExec after the execution of the filter to calculate the metric.
    /// </summary>
    public class MetricExec : MetricExecBase
    {

        #region Constructors

        /// <summary>
        /// Regular metric associable to a Node. The input and output images of the filter
        /// are given to the MetricExec after the execution of the filter to calculate the metric.
        /// </summary>
        /// <param name="key">The key of the metric.</param>
        /// <param name="method">The metric function.</param>
        public MetricExec(string key, Metric.MetricDelegate method)
            : base(key, method)
        {

        }

        #endregion

    }
}
