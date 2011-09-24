using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Core;
using Framework.Core.Metrics;
using System.Drawing;
using Framework.Batch;

namespace Framework.Batch
{
    /// <summary>
    /// Metric associable to a node that takes a previously defined 
    /// reference and the output of the filter to calculate the metric after the filter execution.
    /// </summary>
    public class MetricExecReference : MetricExecBase
    {

        #region Constructors

        /// <summary>
        /// Metric associable to a node that takes a previously defined 
        /// reference and the output of the filter to calculate the metric after the filter execution.
        /// </summary>
        /// <param name="key">The key of the metric.</param>
        /// <param name="method">The metric function.</param>
        /// <param name="reference">The reference to use in the calculation of the metric.</param>
        public MetricExecReference(string key, Metric.MetricDelegate method, WeakImage reference)
            : base(key, method)
        {
            base.SetInput(reference);
        }

        #endregion

        #region SetInput

        /// <summary>
        /// The SetInput method is overrided to leave the attribute untouched.
        /// </summary>
        /// <param name="img"></param>
        public override void SetInput(WeakImage img)
        {
            // SKIP
        }

        #endregion

    }
}
