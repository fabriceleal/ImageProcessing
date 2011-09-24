using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Core.Metrics
{

    /// <summary>
    /// Implementation of Metric with all implemented metric functions.
    /// </summary>
    /// <remarks>
    /// This class should not be used directly when associating metrics with a filter. Use instead AllMetricAttribute.
    /// </remarks>
    /// <see cref="AllMetricAttribute"/>
    internal class AllMetric : Metric
    {

        /// <summary>
        /// Implementation of Metric with all implemented metric functions.
        /// </summary>
        public AllMetric()
            : base(new Metric.MetricDelegate[] { 
                    Facilities.CountErrorTolerant,
                    Facilities.CountWhitebytesInput, 
                    Facilities.CountWhitebytesOutput, 
                    Facilities.CountMatchWhitebytes, 
                    Facilities.CountMatchWhitebytesRatio, 
                    Facilities.SignalToNoiseRatio, 
                    Facilities.SignalToNoiseRatioSimple})
        {

        }

    }
}
