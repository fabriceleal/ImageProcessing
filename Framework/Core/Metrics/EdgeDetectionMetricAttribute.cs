using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Core.Metrics
{
    /// <summary>
    /// Edge Detection metric profile.
    /// </summary>
    /// <see cref="FilterMetricAttribute"/>
    public class EdgeDetectionMetricAttribute : FilterMetricAttribute
    {

        /// <summary>
        /// Edge Detection metric profile.
        /// </summary>
        public EdgeDetectionMetricAttribute()
            : base(new EdgeDetectionMetric())
        {
        }

    }
}
