using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;
using Framework.Core.Metrics;
using Framework.Filters.Smoothing;
using Framework.Filters.Simple;
using Framework.Range;

namespace Framework.Filters.EdgeDetection.SndDerivate
{

    /// <summary>
    /// 
    /// </summary>
    [Filter("Laplacian of Gauss", "Laplacian of Gauss Edge Detection", "***",
            new string[] { "Edge Detection", "Second Derivate" }, true)]
    [EdgeDetectionMetric()]
    public class LaplacianOfGauss : SpatialDomainFilter
    {

        #region SpatialDomainFilter

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public static double[,] GenerateFilter(double sigma)
        {
            double[,] gFilter = Gaussian.GenerateFilter(sigma);
            //double[,] h = new double[size, size];

            // Get any dimension...
            int size = gFilter.GetLength(0);

            double sum = 0.0;
            double sigma_sqrd = sigma * sigma;
            double sigma_sqrd_times_2 = 2 * sigma_sqrd;

            sigma_sqrd *= sigma_sqrd;

            int x, y, x0, y0, size_over_2 = size / 2;

            for (x = 0; x < size; ++x)
            {
                for (y = 0; y < size; ++y)
                {
                    x0 = x - size_over_2; y0 = y - size_over_2;

                    gFilter[x, y] *= (x0 * x0 + y0 * y0 - sigma_sqrd_times_2) / sigma_sqrd;

                    sum += gFilter[x, y];
                }
            }


            sum /= size * size;

            for (x = 0; x < size; ++x)
            {
                for (y = 0; y < size; ++y)
                {
                    gFilter[x, y] -= sum;
                }
            }

            return gFilter;
        }

        /// <summary>
        /// Generate default configurations for the filter.
        /// </summary>
        /// <returns>Dictionary used to pass configuration though the 
        /// configs parameter of the method ApplyFilter. 
        /// null if there is the filter doesn't use configurations.</returns>
        public override SortedDictionary<string, object> GetDefaultConfigs()
        {
            SortedDictionary<string, object> ret = new SortedDictionary<string, object>();

            ret.Add("Threshold", Rangeable.ForByte(10));
            //ret.Add("Size", 5);
            ret.Add("Sigma", new Rangeable(0.5, 0.5, 20, 0.5));

            return ret;
        }

        /// <summary>
        /// Apply filter to a byte[,].
        /// </summary>
        /// <param name="iImageSrc"></param>
        /// <param name="iConfigs"></param>
        /// <remarks>Final implementation based on edge.m</remarks>
        /// <returns></returns>
        public override byte[,] ApplyFilter(byte[,] iImageSrc, SortedDictionary<string, object> iConfigs)
        {
            // Generating _masksAnti: Simplified Approach to Image Processing.pdf (pdf p.99)
            // See also de Difference of Gaussians: Simplified Approach to Image Processing.pdf (pdf p.100)

            //int dim = int.Parse(iConfigs["dim"].ToString());
            //double std = double.Parse(iConfigs["std dev"].ToString());

            //if (dim % 2 == 0)
            //    throw new Exception("Dim should be odd!");

            //double[,] lap_gauss = (double[,])Array.CreateInstance(typeof(double), dim, dim);
            //double funny_const = -1.0 / (Math.PI * std * std * std * std);
            //double funny_const2;

            //for (int x = 0; x < dim; ++x)
            //{
            //    for (int y = 0; y < dim; ++y)
            //    {
            //        funny_const2 =
            //                -1.0 * (x*x + y*y) /
            //                    (2.0 * std * std);

            //        lap_gauss[x, y] = (int) (
            //                funny_const
            //                * (1.0 + funny_const2)
            //                * Math.Pow(Math.E, funny_const2));

            //    }
            //}


            //Kernel2D kernel = new Kernel2D(
            //       0,
            //       1 / (9.0 * 9.0),
            //    //lap_gauss
            //           new double[,] { 
            //                { 0, 1, 1, 2, 2, 2, 1, 1, 0 }, 
            //                { 1, 2, 4, 5, 5, 5 , 4, 2, 1 },
            //                { 1, 4, 5, 3, 0, 3 , 5, 4, 1 },
            //                { 2, 5, 3, -12, -24, -12 , 3, 5, 2 },
            //                { 2, 5, 0, -24, -40, -24 , 0, 5, 2 },
            //                { 2, 5, 3, -12, -24, -12 , 3, 5, 2 },
            //                { 1, 4, 5, 3, 0, 3 , 5, 4, 1 },
            //                { 1, 2, 4, 5, 5, 5 , 4, 2, 1 },
            //                { 0, 1, 1, 2, 2, 2 , 1, 1, 0 }
            //           }
            //       );


            double sigma = double.Parse(iConfigs["Sigma"].ToString());
            
            Kernel2D kernel = new Kernel2D(0, 1.0, GenerateFilter(sigma));

            double[,] tmp = Facilities.ToDouble(iImageSrc);
            tmp = Kernel2D.ApplyConvolution(kernel, tmp);

            int x, y, width = tmp.GetLength(0), height = tmp.GetLength(1);

            double t = double.Parse(iConfigs["Threshold"].ToString()) / (double)byte.MaxValue;
            // Find zero crossings
            bool[,] edges = new bool[width, height];

            for (x = 1; x < width - 1; ++x)
            {
                for (y = 1; y < height - 1; ++y)
                {
                    if (tmp[x, y] < 0 && tmp[x, y + 1] > 0 && Math.Abs(tmp[x, y] - tmp[x, y + 1]) > t)
                    {
                        edges[x, y] = true;
                    }
                    if (tmp[x, y] < 0 && tmp[x, y - 1] > 0 && Math.Abs(tmp[x, y] - tmp[x, y - 1]) > t)
                    {
                        edges[x, y] = true;
                    }
                    if (tmp[x, y] < 0 && tmp[x + 1, y] > 0 && Math.Abs(tmp[x, y] - tmp[x + 1, y]) > t)
                    {
                        edges[x, y] = true;
                    }
                    if (tmp[x, y] < 0 && tmp[x - 1, y] > 0 && Math.Abs(tmp[x, y] - tmp[x - 1, y]) > t)
                    {
                        edges[x, y] = true;
                    }
                    if (tmp[x, y] == 0.0)
                    {
                        if (tmp[x - 1, y - 1] < 0 && tmp[x + 1, y + 1] > 0 && Math.Abs(tmp[x - 1, y - 1] - tmp[x + 1, y + 1]) > t)
                        {
                            edges[x, y] = true;
                        }
                        if (tmp[x - 1, y - 1] > 0 && tmp[x + 1, y + 1] < 0 && Math.Abs(tmp[x - 1, y - 1] - tmp[x + 1, y + 1]) > t)
                        {
                            edges[x, y] = true;
                        }
                        if (tmp[x + 1, y - 1] < 0 && tmp[x - 1, y + 1] > 0 && Math.Abs(tmp[x + 1, y - 1] - tmp[x - 1, y + 1]) > t)
                        {
                            edges[x, y] = true;
                        }
                        if (tmp[x + 1, y - 1] > 0 && tmp[x - 1, y + 1] < 0 && Math.Abs(tmp[x + 1, y - 1] - tmp[x - 1, y + 1]) > t)
                        {
                            edges[x, y] = true;
                        }
                    }
                }
            }

            byte[,] tmp_ret = Facilities.To8bppGreyScale(edges);      

            return tmp_ret;
        }

        #endregion

    }
}
