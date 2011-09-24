using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;
using Framework.Filters.Smoothing;
using Framework.Core.Metrics;
using Framework.Filters.Simple;
using Framework.Range;

namespace Framework.Filters.EdgeDetection.Probabilistic
{
    /// <summary>
    /// Apply the Shen-Castan edge detection algorithm.
    /// </summary>
    /// <remarks>
    /// The Shen-Castan edge detection algorithm smooths the image with the ISEF filter; it then
    /// calculates zero-crossings, applies non-maximum supreession and applies hystersis.
    /// Based in http://www.hackchina.com/en/r/25044/SHEN.C__html
    /// </remarks>
    [Filter("Shen-Castan", "Shen-Castan Edge Detection",
            "The Shen-Castan edge detection algorithm smooths the image with the ISEF filter; it then" +
            " calculates zero-crossings, applies non-maximum supreession and applies hystersis." +
            "Based in http://www.hackchina.com/en/r/25044/SHEN.C__html",
            new string[] { "Edge Detection", "Probabilistic" }, true)]
    [EdgeDetectionMetric()]
    public class ShenCastan : SpatialDomainFilter
    {

        // Based in http://www.hackchina.com/en/r/25044/SHEN.C__html

        private Isef isef;
        private Hystersis hystersis;
        private NonMaximumSupression nonMaxSupression;
                

        /// <summary>
        /// Generate default configurations for the filter.
        /// </summary>
        /// <returns>Dictionary used to pass configuration though the 
        /// configs parameter of the method ApplyFilter. 
        /// null if there is the filter doesn't use configurations.</returns>
        public override SortedDictionary<string, object> GetDefaultConfigs()
        {
            SortedDictionary<string, object> ret = new SortedDictionary<string, object>();
            // --
            SortedDictionary<string, object> isefConf = isef.GetDefaultConfigs();
            SortedDictionary<string, object> hystersisConf = hystersis.GetDefaultConfigs();
            SortedDictionary<string, object> nonMaxSupressionConf = nonMaxSupression.GetDefaultConfigs();

            if (null != isefConf)
            {
                foreach (KeyValuePair<string, object> it in isefConf)
                {
                    ret.Add(it.Key, it.Value);
                }
            }
            if (null != hystersisConf)
            {
                foreach (KeyValuePair<string, object> it in hystersisConf)
                {
                    ret.Add(it.Key, it.Value);
                }
            }
            if (null != nonMaxSupressionConf)
            {
                foreach (KeyValuePair<string, object> it in nonMaxSupressionConf)
                {
                    ret.Add(it.Key, it.Value);
                }
            }

            ret.Add("AdaptiveGradWinSize", new Rangeable(7, 1, 333, 2));

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
            // Apply ISEF. Apply on the double data directly, when need the actual calculated doubles,
            // not the trimmed bytes
            int width = img.GetLength(0), height = img.GetLength(1);
            double[,] smoothed = new double[width, height];
            int x, y;
            double b = double.Parse(configs["b"].ToString());

            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    smoothed[x, y] = (double)img[x, y];
                }
            }


            smoothed = isef.ApplyIsefHorizontal(smoothed, b);
            smoothed = isef.ApplyIsefVertical(smoothed, b);


            bool[,] bli = new bool[width, height];

            // minimum of 1, to avoid out-of-bounds exceptions !
            int adaptiveGradWinSize = int.Parse(configs["AdaptiveGradWinSize"].ToString());
            int BAND = (int)Math.Max(7, adaptiveGradWinSize);

            // Calculate BLI
            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    if (x > BAND && y > BAND && y < height - BAND && x < width - BAND)
                        bli[x, y] = (smoothed[x, y] - img[x, y] > 0);
                }
            }

            byte[,] zeroCrossed = new byte[width, height];

            // Calculate Zero Crossings (using BLI)
            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    if (x > BAND && y > BAND && y < height - BAND && x < width - BAND)
                    {
                        if (IsCandidate(bli, smoothed, x, y))
                        {
                            zeroCrossed[x, y] = ComputeAdaptiveGradient(bli, smoothed, x, y, adaptiveGradWinSize);
                        }
                    }
                }
            }
            smoothed = null; bli = null; // elegible for garbage collection


            //byte[,] ret = new byte[width, height];
            //for (x = 0; x < width; ++x)
            //{
            //    for (y = 0; y < height; ++y)
            //    {
            //        ret[x, y] = (byte)Math.Min(byte.MaxValue, Math.Max(byte.MinValue, zeroCrossed[x, y]));
            //    }
            //}
            //zeroCrossed = null; // elegible for garbage collection

            // Non-Maximum supression
            //zeroCrossed = nonMaxSupression.ApplyFilter(zeroCrossed, configs);

            // Hystersis
            zeroCrossed = hystersis.ApplyFilter(zeroCrossed, configs);

            return zeroCrossed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bli"></param>
        /// <param name="smoothed"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool IsCandidate(bool[,] bli, double[,] smoothed, int x, int y)
        {
            if (bli[x, y] && !bli[x, y + 1])
            {
                return (smoothed[x, y + 1] - smoothed[x, y - 1] > 0);
            }
            else if (bli[x, y] && !bli[x + 1, y])
            {
                return (smoothed[x + 1, y] - smoothed[x - 1, y] > 0);
            }
            else if (bli[x, y] && !bli[x, y - 1])
            {
                return (smoothed[x, y + 1] - smoothed[x, y - 1] < 0);
            }
            else if (bli[x, y] && !bli[x - 1, y])
            {
                return (smoothed[x + 1, y] - smoothed[x - 1, y] < 0);
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bli"></param>
        /// <param name="smoothed"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        private byte ComputeAdaptiveGradient(bool[,] bli, double[,] smoothed, int x, int y, int windowSize)
        {
            int i, j;
            double sumOn = 0, sumOff = 0, avgOn = 0, avgOff = 0;
            int totOn = 0, totOff = 0;

            for (i = (-windowSize / 2); i <= (windowSize / 2); ++i)
            {
                for (j = (-windowSize / 2); j <= (windowSize / 2); ++j)
                {
                    if (bli[x + i, y + j])
                    {
                        sumOn += smoothed[x + i, y + j];
                        ++totOn;
                    }
                    else
                    {
                        sumOff += smoothed[x + i, y + j];
                        ++totOff;
                    }
                }
            }

            if (totOn != 0)
            {
                avgOn = sumOn / totOn;
            }
            if (totOff != 0)
            {
                avgOff = sumOff / totOff;
            }

            return (byte)Math.Min(byte.MaxValue, Math.Max(byte.MinValue, avgOff - avgOn));
        }

        /// <summary>
        /// 
        /// </summary>
        public ShenCastan()
        {
            isef = new Isef();
            hystersis = new Hystersis();
            nonMaxSupression = new NonMaximumSupression();
        }

    }
}
