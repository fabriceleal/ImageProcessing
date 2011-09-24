using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Framework.Core;
using Framework.Core.Filters.Frequency;
using Framework.Core.Transforms;

namespace Framework.Transforms
{

    /// <summary>
    /// Implementation of the fourier transform with O( n^2 ) complexity.
    /// Implemented filters or external users should use Framework.Transforms.FourierTransform instead.
    /// </summary>
    public class DiscreteFourierTransform : TransformCoreBase<ComplexImage>
    {

        /// <summary>
        /// Apply the fourier transform on dataSpatialDomain.
        /// </summary>
        /// <param name="dataSpatialDomain">The data to transform.</param>
        /// <returns>The fourier transfrom of dataSpatialDomain.</returns>
        public override ComplexImage ApplyTransformBase(double[,] dataSpatialDomain)
        {
            int u, v, x, y, width = dataSpatialDomain.GetLength(0), height = dataSpatialDomain.GetLength(1);
            ComplexImage ret = new ComplexImage(width, height);

            double C_0, C_1;
            double pi_times2 = Math.PI * 2.0;
            Complex calculated;
            int j = 0;
            double theta;

            for (u = 0; u < width; ++u)
            {
                for (v = 0; v < height; ++v)
                {
                    calculated = new Complex(0.0, 0.0);
                    C_0 = u / (height * 1.0);
                    C_1 = v / (width * 1.0);

                    for (x = 0; x < width; ++x)
                    {
                        for (y = 0; y < height; ++y)
                        {
                            theta = pi_times2 * (x * C_0 + y * C_1);

                            calculated += dataSpatialDomain[x, y] * Math.Pow(-1, x + y) * new Complex(Math.Cos(theta), -1 * Math.Sin(theta));
                        }
                    }

                    ret[u, v] = calculated;
                }
            }

            return ret;
        }

        /// <summary>
        /// Apply the inverse fourier transform on an image on the frequency domain.
        /// </summary>
        /// <param name="dataFrequencyDomain">The image in frequency domain.</param>
        /// <returns>The inverse fourier transform of dataFrequencyDomain.</returns>
        public override double[,] ApplyReverseTransformBase(ComplexImage dataFrequencyDomain)
        {
            int x, y, u, v, width = dataFrequencyDomain.Width, height = dataFrequencyDomain.Height;
            double[,] ret = new double[width, height];
            double C = 1 / (height * width * 1.0);
            double C_0, C_1; double pi_times2 = Math.PI * 2.0;
            Complex calculated; int j = 0; double theta;

            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    calculated = new Complex(0.0, 0.0);
                    C_0 = x / (height * 1.0);
                    C_1 = y / (width * 1.0);

                    for (u = 0; u < width; ++u)
                    {
                        for (v = 0; v < height; ++v)
                        {
                            theta = pi_times2 * (u * C_0 + v * C_1);

                            calculated += dataFrequencyDomain[u, v] * new Complex(Math.Cos(theta), Math.Sin(theta));
                        }
                    }

                    ret[x, y] = C * calculated.real * Math.Pow(-1, x + y);
                }
            }

            return ret;
        }

        /// <summary>
        /// Apply the fourier transform on imageSpatialDomain
        /// </summary>
        /// <param name="src">The image (greyscale) to transform.</param>
        /// <returns>The fourier transfrom of imageSpatialDomain.</returns>
        public override ComplexImage ApplyTransform(byte[,] src)
        {
            int width = src.GetLength(0), height = src.GetLength(1);
            double[,] srcTransf = new double[width, height];
            int x, y;

            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    srcTransf[x, y] = src[x, y];
                }
            }

            return ApplyTransformBase(srcTransf);
        }

        /// <summary>
        /// Apply the inverse fourier transform on an image on the frequency domain.
        /// </summary>
        /// <param name="dest">The image in frequency domain.</param>
        /// <returns>The inverse fourier transform of imageFrequencyDomain.</returns>
        public override byte[,] ApplyReverseTransform(ComplexImage dest)
        {
            double[,] calc = ApplyReverseTransformBase(dest);
            int width = calc.GetLength(0), height = calc.GetLength(1);
            byte[,] ret = new byte[width, height];
            int x, y;

            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    ret[x, y] = (byte)Math.Max(byte.MinValue, Math.Min(byte.MaxValue, calc[x, y]));
                }
            }

            return ret;
        }


    }
}
