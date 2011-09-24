using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;
using System.Collections;
using Framework.Range;
using Framework.Core.Metrics;

namespace Framework.Filters.Simple
{

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    [Filter("Thinning (Binary)", "Thinning (Binary) Filter",
        "" +
        "" +
        ".", new string[] { "Simple" }, true)]
    [AllMetricAttribute()]
    public class ThinningBinary : SpatialDomainFilter
    {

        private ArrayList _masksPro = new ArrayList();
        private ArrayList _masksAnti = new ArrayList();

        public ThinningBinary()
        {
            _masksPro.Add(new bool[,] { { false, false, false }, { true, true, true }, { true, true, true } });
            _masksAnti.Add(new bool[,] { { true, true, true }, { false, false, false }, { false, false, false } });

            _masksPro.Add(new bool[,] { { true, true, true }, { true, true, true }, { false, false, false } });
            _masksAnti.Add(new bool[,] { { false, false, false }, { false, false, false }, { true, true, true } });

            _masksPro.Add(new bool[,] { { false, true, true }, { false, true, true }, { false, true, true } });
            _masksAnti.Add(new bool[,] { { true, false, false }, { true, false, false }, { true, false, false } });

            _masksPro.Add(new bool[,] { { true, true, false }, { true, true, false }, { true, true, false } });
            _masksAnti.Add(new bool[,] { { false, false, true }, { false, false, true }, { false, false, true } });

            _masksPro.Add(new bool[,] { { true, false, false }, { true, true, false }, { true, true, true } });
            _masksAnti.Add(new bool[,] { { false, true, true }, { false, false, true }, { false, false, false } });

            _masksPro.Add(new bool[,] { { true, true, true }, { false, true, true }, { false, false, true } });
            _masksAnti.Add(new bool[,] { { false, false, false }, { true, false, false }, { true, true, false } });

            _masksPro.Add(new bool[,] { { true, true, true }, { true, true, false }, { true, false, false } });
            _masksAnti.Add(new bool[,] { { false, false, false }, { false, false, true }, { false, true, true } });

            _masksPro.Add(new bool[,] { { false, false, true }, { false, true, true }, { true, true, true } });
            _masksAnti.Add(new bool[,] { { true, true, false }, { true, false, false }, { false, false, false } });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override SortedDictionary<string, object> GetDefaultConfigs()
        {
            SortedDictionary<string, object> ret = new SortedDictionary<string, object>();

            ret.Add("Iterations", new Rangeable(1, 1, 500, 1));

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public override byte[,] ApplyFilter(byte[,] img, SortedDictionary<string, object> configs)
        {
            int it = int.Parse(configs["Iterations"].ToString());

            if (it <= 0)
                return img;

            int width = img.GetLength(0), heigh = img.GetLength(1);
            byte[,] ret = new byte[width, heigh];
            byte[,] tmp; int x, y; bool hasChanged = false;
            
            // Init ret with img
            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < heigh; ++y)
                {
                    ret[x, y] = img[x, y];
                }
            }

            for (int i = 0; i < _masksPro.Count; ++i)
            {
                tmp = HitMiss_Bin(img, (bool[,])_masksPro[i], (bool[,])_masksAnti[i]);

                for (x = 0; x < width; ++x)
                {
                    for (y = 0; y < heigh; ++y)
                    {
                        ret[x, y] = (byte)((int)ret[x, y] & (byte.MaxValue - (int)tmp[x, y]));

                        if (ret[x, y] != img[x, y])
                            hasChanged = true;
                    }
                }
            }

            if (hasChanged)
            {
                // Call recursion
                // Execute for all iterations in Iterations
                if (it > 1)
                {
                    SortedDictionary<string, object> configsRecurse = new SortedDictionary<string, object>();
                    configsRecurse["Iterations"] = it - 1;

                    ret = ApplyFilter(ret, configsRecurse);
                }
            }

            return ret;
        }

        private byte[,] HitMiss_Bin(byte[,] image, bool[,] maskPro, bool[,] maskAnti)
        {
            int width = image.GetLength(0), height = image.GetLength(1);
            int x, y;
            byte[,] imageSc = new byte[width, height];

            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    imageSc[x, y] = (byte)(byte.MaxValue - image[x, y]);
                }
            }

            byte[,] ret = new byte[width, height];

            byte[,] imagePro = Erosion_Bin(image, maskPro);
            byte[,] imageAnti = Erosion_Bin(imageSc, maskAnti);

            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    ret[x, y] = (byte)((int)imageAnti[x, y] & (int)imagePro[x, y]);
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
