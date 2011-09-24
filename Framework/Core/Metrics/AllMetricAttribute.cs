using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Core.Metrics
{
    /// <summary>
    /// Holds all implemented metric functions.
    /// </summary>
    /// <remarks>
    /// Associate this metric profile to a "simple" filter, that may pre-process / pro-process a
    /// smoothing / sharpening / edge detection filter
    /// </remarks>
    public class AllMetricAttribute : FilterMetricAttribute
    {
        public AllMetricAttribute()
            : base(new AllMetric())
        {

        }
    }
}
