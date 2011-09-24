using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Frequency;
using Framework.Transforms;
using Framework.Core.Metrics;
using Framework.Range;

namespace Framework.Filters.Smoothing.Frequency
{

    /// <summary>
    /// Apply the ideal low pass to an image.
    /// </summary>
    /// <remarks>
    /// The ideal low pass filter cuts off all frequencies above a certain distance from the center 
    /// (cutting off high frequency values).
    /// (Gonzalez, R. C., Woods, R. E.. Digital Image Processing.2.Prentice Hall)
    /// </remarks>
    [Filter("Ideal", "Ideal Lowpass Filter",
            "The ideal lowpass filter cuts off all frequencies above" + 
            " a certain distance from the center.", 
            new string[] { "Smoothing", "Frequency" }, true)]
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

            int width = complexImg.Width;
            int height = complexImg.Height;

            return FrequencyFilter.ApplyFilter(complexImg, delegate(int u, int v)
            {
                if (FrequencyFilter.D(u, v, width, height) <= d0)
                {
                    return 1.0;
                }
                return 0.0;
            });
        }

    }
}
