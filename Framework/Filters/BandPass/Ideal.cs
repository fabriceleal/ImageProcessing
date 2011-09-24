using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Frequency;
using Framework.Transforms;
using Framework.Core.Metrics;
using Framework.Range;

namespace Framework.Filters.BandPass
{

    /// <summary>
    /// Apply the ideal band pass to an image.
    /// </summary>
    /// <remarks>
    /// The ideal band pass filter cuts off all frequencies outside a range of 
    /// distances from the center.
    /// (Gonzalez, R. C., Woods, R. E.. Digital Image Processing.2.Prentice Hall)
    /// </remarks>
    [Filter("Ideal", "Ideal Band Pass Filter",
            "The ideal band pass filter cuts off all frequencies outside" + 
            " a range of distances from the center.",
            new string[] { "Band Pass" }, true)]
    [SmoothMetric]
    public class Ideal : FrequencyDomainFilter<FourierTransform>
    {

        /// <summary>
        /// Generate default configurations for the filter.
        /// </summary>
        /// <returns>Dictionary used to pass configuration though the 
        /// configs parameter of the method ApplyFilter. 
        /// null if there is the filter doesn't use configurations.</returns>
        public override SortedDictionary<string, object> GetDefaultConfigs()
        {
            SortedDictionary<string, object> ret = new SortedDictionary<string, object>();

            ret.Add("D0", new Rangeable(15, 1, 3000, 1));
            ret.Add("W", new Rangeable(10, 1, 3000, 1));
            
            return ret;
        }

        /// <summary>
        /// Apply filter to a ComplexImage.
        /// </summary>
        /// <param name="complexImg"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public override ComplexImage ApplyFilter(ComplexImage complexImg, SortedDictionary<string, object> configs)
        {
            double d0 = double.Parse(configs["D0"].ToString());
            double w = double.Parse(configs["W"].ToString());

            int width = complexImg.Width, height = complexImg.Height;
            double d0_lower = d0 - w / 2.0, d0_upper = d0 + w / 2.0, d;

            return FrequencyFilter.ApplyFilter(complexImg, delegate(int u, int v)
            {
                d = FrequencyFilter.D(u, v, width, height);
                if (d < d0_lower || d > d0_upper)
                {
                    return 0.0;
                }
                return 1.0;
            });
        }

    }
}
