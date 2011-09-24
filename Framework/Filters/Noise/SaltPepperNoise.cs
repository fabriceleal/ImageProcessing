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
    /// Apply salt and pepper noise to an image.
    /// </summary>
    /// <remarks>
    /// Implemented from
    /// (Myler, H.R., Weeks, A. R. . Pocket Handbook of Image Processing Algorithms in C.Prentice Hall).
    /// </remarks>
    [Filter("Salt and Pepper Noise", "Salt and Pepper Noise Filter",
            "Apply salt and pepper noise to an image.", new string[] { "Noise" }, true)]
    [SmoothMetric]
    public class SaltPepperNoise : SpatialDomainFilter
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

            ret.Add("Probability", new Rangeable(0.15, 0, 1, 0.0001));

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
            // Pocket Handbook of Image Processing Algorithms- Salt and Pepper Noise
            Random rnd = new Random(); byte[] rnd_buffer = new byte[4]; ushort rnd_value;

            int width = img.GetLength(0), height = img.GetLength(1);

            byte[,] ret = new byte[width, height];

            double probability = double.Parse(configs["Probability"].ToString());
            const int MAGIC = 16384;
            int prob_norm = (int)(probability * 32768 / 2);
            int prob_norm1 = prob_norm + MAGIC;
            int prob_norm2 = MAGIC - prob_norm;


            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    rnd.NextBytes(rnd_buffer);
                    // Force to ushort, in range [0; 32767]
                    rnd_value = (ushort)(BitConverter.ToUInt16(rnd_buffer, 0) % 32768);

                    if (rnd_value >= MAGIC && rnd_value < prob_norm1)
                        ret[x, y] = byte.MinValue; // pepper forces to 0 (BLACK)
                    else if (rnd_value < MAGIC && rnd_value >= prob_norm2)
                        ret[x, y] = byte.MaxValue;// salt forces to 255 (WHITE)
                    else
                        ret[x, y] = img[x, y];

                }
            }

            return ret;
        }

    }
}
