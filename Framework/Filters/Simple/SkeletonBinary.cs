using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;
using System.Collections;
using Framework.Core.Metrics;

namespace Framework.Filters.Simple
{

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    [Filter("Skeleton (Binary)", "Skeleton (Binary) Filter",
        "" +
        "" +
        ".", new string[] { "Simple" }, true)]
    [AllMetricAttribute()]
    public class SkeletonBinary : SpatialDomainFilter
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override SortedDictionary<string, object> GetDefaultConfigs()
        {
            return base.GetDefaultConfigs();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public override byte[,] ApplyFilter(byte[,] img, SortedDictionary<string, object> configs)
        {
            int width = img.GetLength(0), height = img.GetLength(1);
            byte[,] ret = new byte[width, height];
            int x, y;

            bool[,] mask = new bool[,] { { false, false, false }, { false, true, true }, { false, true, false } };

            bool fTrue = true, fFalse = false, fPixelOn = true;
            byte[,] tmp; int N = mask.GetLength(0); int offset = N / 2;

            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    ret[x, y] = byte.MinValue;
                }
            }

            byte pixel;

            while (fPixelOn)
            {
                fPixelOn = false;

                tmp = Dilation_Bin(Erosion_Bin(img, mask), mask);

                for (x = offset; x < width - offset; ++x)
                {
                    for (y = offset; y < height - offset; ++y)
                    {
                        pixel = (byte)(img[x, y] - tmp[x, y]);
                        ret[x, y] = (byte)((int)ret[x, y] | (int)pixel);
                        
                        fPixelOn = (pixel == byte.MaxValue);

                        img[x, y] = tmp[x, y];
                    }
                }

            }

            return ret;
        }

        private byte[,] Dilation_Bin(byte[,] image, bool[,] mask)
        {
            int width = image.GetLength(0), height = image.GetLength(1);
            byte[,] ret = new byte[width, height];
            int N = mask.GetLength(0);
            int x, y, i, j; int offset = N / 2;
            byte max;

            for (x = offset; x < width - offset; ++x)
            {
                for (y = offset; y < height - offset; ++y)
                {
                    max = byte.MinValue;
                    for (i = -offset; i <= offset; ++i)
                    {
                        for (j = -offset; j <= offset; ++j)
                        {
                            if (mask[i + offset, j + offset])
                            {
                                if (image[x + i, y + j] > max)
                                {
                                    max = byte.MaxValue;
                                }
                            }
                        }
                    }
                    ret[x, y] = max;
                }
            }

            return ret;
        }

        private byte[,] Erosion_Bin(byte[,] image, bool[,] mask)
        {
            int width = image.GetLength(0), height = image.GetLength(1);
            byte[,] ret = new byte[width, height];
            int N = mask.GetLength(0);
            int x, y, i, j; int offset = N / 2;
            byte min;

            for (x = offset; x < width - offset; ++x)
            {
                for (y = offset; y < height - offset; ++y)
                {
                    min = byte.MaxValue;
                    for (i = -offset; i <= offset; ++i)
                    {
                        for (j = -offset; j <= offset; ++j)
                        {
                            if (mask[i + offset, j + offset])
                            {
                                if (image[x + i, y + j] < min)
                                {
                                    min = byte.MinValue;
                                }
                            }
                        }
                    }
                    ret[x, y] = min;
                }
            }

            return ret;
        }

    }

}
