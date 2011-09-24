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
    /// Apply the butterworth band pass to an image.
    /// </summary>
    /// <remarks>
    /// Apply a Butterworth equation to the Fourier Transform of the image to achieve
    /// a band pass filtered image.
    /// (Gonzalez, R. C., Woods, R. E.. Digital Image Processing.2.Prentice Hall)
    /// </remarks>
    [Filter("Butterworth", "Butterworth Band Pass Filter",
            "Apply a Butterworth equation to the Fourier Transform of the image to achieve" +
            " a band pass filtered image.",
            new string[] { "Band Pass" }, true)]
    [SmoothMetric]
    public class Butterworth : FrequencyDomainFilter<FourierTransform>
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
            ret.Add("n", new Rangeable(2.0, 1.0, 10.0, 1));

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
            double n = double.Parse(configs["n"].ToString());
            double w = double.Parse(configs["W"].ToString());

            int width = complexImg.Width, height = complexImg.Height;

            double d0_squared = d0 * d0, d;

            return FrequencyFilter.ApplyFilter(complexImg,
                delegate(int u, int v)
                {
                    d = FrequencyFilter.D(u, v, width, height);

                    return 1.0 - 1 / (1.0 + Math.Pow(d * w / (d * d - d0_squared), 2 * n));
                });
        }

    }
}
