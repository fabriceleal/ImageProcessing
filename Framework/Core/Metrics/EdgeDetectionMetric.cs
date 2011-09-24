using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Core;

namespace Framework.Core.Metrics
{
    /// <summary>
    /// Implementation of Metric with the metric functions to evaluate a Edge Detection filter.
    /// </summary>
    /// <remarks>
    /// This class should not be used directly when associating metrics with a filter. Use instead EdgeDetectionMetricAttribute.</remarks>
    /// <see cref="EdgeDetectionMetricAttribute"/>
    internal class EdgeDetectionMetric : Metric
    {

        /// <summary>
        /// Implementation of Metric to associate with a Edge Detection filter.
        /// </summary>
        public EdgeDetectionMetric()
            : base(new Metric.MetricDelegate[] { 
                    Facilities.CountErrorTolerant,
                    Facilities.CountWhitebytesInput,
                    Facilities.CountWhitebytesOutput, 
                    Facilities.CountMatchWhitebytes, 
                    Facilities.CountMatchWhitebytesRatio })
        {
        }

    }
}
