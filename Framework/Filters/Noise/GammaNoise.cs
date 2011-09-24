using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;
using Framework.Core.Metrics;
using Framework.Range;

namespace Framework.Filters.Noise
{
    /// <summary>
    /// Apply gamma noise to an image.
    /// </summary>
    /// <remarks>
    /// Implemented from
    /// (Myler, H.R., Weeks, A. R. . Pocket Handbook of Image Processing Algorithms in C.Prentice Hall).
    /// </remarks>
    [Filter("Gamma Noise", "Gamma Noise Filter", 
            "Apply gamma noise to an image.", new string[] { "Noise" }, true)]
    [SmoothMetric]
    public class GammaNoise : SpatialDomainFilter
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

            ret.Add("Var", new Rangeable(0.5, 0, 255, 0.5));
            ret.Add("Alpha", new Rangeable(255, 0, 255, 0.5));

            return ret;
        }

        /// <summary>
        /// Apply filter to a byte[,].
        /// </summary>
        /// <param name="img"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public override byte[,] ApplyFilter(byte[,] img, SortedDictionary<string, object> configs)
        {

            // Pocket Handbook of Image Processing Algorithms- Gaussian Noise
            Random rnd = new Random(); byte[] rnd_buffer = new byte[4]; ushort rnd_value;

            int width = img.GetLength(0), height = img.GetLength(1);

            byte[,] ret = new byte[width, height];

            double var = double.Parse(configs["Var"].ToString());
            int alpha = int.Parse(configs["Alpha"].ToString());

            int x, y, i, noise_int;
            double noise, a, theta, image1, rx, ry;
            const double MAGIC = 1.9175345E-4;

            a = Math.Sqrt(var / (double)alpha) / 2.0;

            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    image1 = 0.0;
                    for (i = 1; i <= alpha; ++i)
                    {
                        rnd.NextBytes(rnd_buffer);
                        // Force to ushort, in range [0; 32767]
                        rnd_value = (ushort)(BitConverter.ToUInt16(rnd_buffer, 0) % 32768);
                        noise = Math.Sqrt(-2 * a * Math.Log(1.0 - rnd_value / 32767.1));

                        rnd.NextBytes(rnd_buffer);
                        // Force to ushort, in range [0; 32767]
                        rnd_value = (ushort)(BitConverter.ToUInt16(rnd_buffer, 0) % 32768);
                        theta = rnd_value * MAGIC - Math.PI;

                        rx = noise * Math.Cos(theta);
                        ry = noise * Math.Sin(theta);

                        noise = rx * rx + ry * ry;
                        image1 += noise;
                    }

                    noise_int = (int)Math.Ceiling(image1); // (image1 + .5);
                    noise_int += img[x, y]; // add noise to image 
                    noise_int = (int)Math.Min(byte.MaxValue, Math.Max(byte.MinValue, noise_int));

                    ret[x, y] = (Byte)noise_int;
                }
            }

            return ret;
        }

    }
}
