using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Core;

namespace Framework.Core.Metrics
{
    /// <summary>
    /// Implementation of Metric with the metric functions to evaluate a Smoothing filter.
    /// </summary>
    /// <remarks>
    /// This class should not be used directly when associating metrics with a filter. Use instead SmoothMetricAttribute.
    /// </remarks>
    /// <see cref="SmoothMetricAttribute"/>
    internal class SmoothMetric : Metric
    {

        /// <summary>
        /// Implementation of Metric to associate with a Smoothing filter.
        /// </summary>
        public SmoothMetric()
            : base(new Metric.MetricDelegate[] { 
                    Facilities.SignalToNoiseRatio, Facilities.SignalToNoiseRatioSimple })
        {
        }

    }
}
