using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Transforms;

namespace Framework.Core.Filters.Frequency
{
    /// <summary>
    /// Provides functionality to apply a filter to a ComplexImage.
    /// </summary>
    /// <remarks>
    /// Assumes that the transform is centered in (M / 2, N / 2).
    /// </remarks>
    public class FrequencyFilter
    {

        /// <summary>
        /// Function signature for a frequency domain filter. It only depends of the 
        /// frequency domain coordinates (u, v).
        /// </summary>
        /// <param name="u">The "x".</param>
        /// <param name="v">The "y".</param>
        /// <returns></returns>
        public delegate double FrequencyFilterEquation(int u, int v);

        /// <summary>
        /// Calculates the distance to the center of the coordinantes (u, v) in a
        /// M x N mask.
        /// </summary>
        /// <param name="u">The "x".</param>
        /// <param name="v">The "y".</param>
        /// <param name="M">The "width".</param>
        /// <param name="N">The "height".</param>
        /// <returns></returns>
        public static double D(int u, int v, int M, int N)
        {
            return Math.Sqrt(Math.Pow(u - M / 2.0, 2) + Math.Pow(v - N / 2.0, 2));
        }

        /// <summary>
        /// Apply a function with the signature FrequencyFilterEquation to the real and the imaginary
        /// components of each complex "pixel" in a ComplexImage.
        /// </summary>
        /// <param name="complex">The image to apply the filter to.</param>
        /// <param name="equation">The equation of the filter.</param>
        /// <returns>The resulting ComplexImage.</returns>
        public static ComplexImage ApplyFilter(
                ComplexImage complex,
                FrequencyFilterEquation equation)
        {
            ComplexImage ret = new ComplexImage(complex);
            double filter_mult;

            // Apply equation...
            for (int u = 0; u < ret.Width; ++u)
            {
                for (int v = 0; v < ret.Height; ++v)
                {
                    filter_mult = equation(u, v);

                    ret[u, v].real *= filter_mult;
                    ret[u, v].imag *= filter_mult;
                }
            }

            return ret;
        }
    }

}
