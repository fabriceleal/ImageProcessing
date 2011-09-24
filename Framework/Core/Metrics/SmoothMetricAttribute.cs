using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Core.Metrics
{
    /// <summary>
    /// Smoothing metric profile.
    /// </summary>
    /// <see cref="FilterMetricAttribute"/>
    public class SmoothMetricAttribute : FilterMetricAttribute
    {

        /// <summary>
        /// Smoothing metric profile.
        /// </summary>
        public SmoothMetricAttribute()
            : base(new SmoothMetric())
        {
        }

    }
}
