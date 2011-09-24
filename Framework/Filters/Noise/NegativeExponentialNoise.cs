﻿using System;
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
    /// Apply negative exponential noise to an image.
    /// </summary>
    /// <remarks>
    /// Implemented from
    /// (Myler, H.R., Weeks, A. R. . Pocket Handbook of Image Processing Algorithms in C.Prentice Hall).
    /// </remarks>
    [Filter("Negative Exponential Noise", "Negative Exponential Noise Filter",
            "Apply negative exponential noise to an image.", new string[] { "Noise" }, true)]
    [SmoothMetric]
    public class NegativeExponentialNoise : SpatialDomainFilter
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

            ret.Add("Var", new Rangeable(10, 0, 255, 0.5));

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
            // Pocket Handbook of Image Processing Algorithms- Negative Exponential Noise
            Random rnd = new Random(); byte[] rnd_buffer = new byte[4]; ushort rnd_value;

            int width = img.GetLength(0), height = img.GetLength(1);

            byte[,] ret = new byte[width, height];

            double var = double.Parse(configs["Var"].ToString()), noise, theta, rx, ry;
            double sqrt_var_half = Math.Sqrt(var) / 2.0;
            const double MAGIC = 1.9175345E-4;

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    rnd.NextBytes(rnd_buffer);
                    // Force to ushort, in range [0; 32767]
                    rnd_value = (ushort)(BitConverter.ToUInt16(rnd_buffer, 0) % 32768);

                    noise = Math.Sqrt(-2 * sqrt_var_half * Math.Log(1.0 - rnd_value / 32767.1));

                    rnd.NextBytes(rnd_buffer);
                    // Force to ushort, in range [0; 32767]
                    rnd_value = (ushort)(BitConverter.ToUInt16(rnd_buffer, 0) % 32768);

                    theta = rnd_value * MAGIC - Math.PI;
                    rx = noise * Math.Cos(theta);
                    ry = noise * Math.Sin(theta);
                    noise = rx * rx + ry * ry;

                    noise += img[x, y]; // add noise to image 
                    noise = Math.Max(byte.MinValue, Math.Min(byte.MaxValue, noise)); // trim

                    ret[x, y] = (Byte)Math.Ceiling(noise); // (Byte)(noise + 0.5);
                }
            }

            return ret;
        }

    }
}
