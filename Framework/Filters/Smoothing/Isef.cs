using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Core.Filters.Spatial;
using Framework.Transforms;
using Framework.Filters;
using Framework.Core.Filters.Base;
using Framework.Core.Metrics;
using Framework.Range;

namespace Framework.Filters.Smoothing
{

    /// <summary>
    /// Apply the ISEF filter to an image.
    /// </summary>
    /// <remarks>
    /// Apply the Infinite Symmetric Exponential Filter filter to an image.
    /// </remarks>
    [Filter("ISEF", "ISEF Filter",
    "Apply the Infinite Symmetric Exponential Filter filter to an image.",
    new string[] { "Smoothing" }, true)]
    [SmoothMetric]
    public class Isef : SpatialDomainFilter
    {

        /// <summary>
        ///  Generate default configurations for the filter.
        /// </summary>
        /// <returns>Dictionary used to pass configuration though the 
        /// configs parameter of the method ApplyFilter. 
        /// null if there is the filter doesn't use configurations.</returns>
        public override SortedDictionary<string, object> GetDefaultConfigs()
        {
            SortedDictionary<string, object> ret = new SortedDictionary<string, object>();
            ret.Add("b", new Rangeable(0.9, 0.0001, 0.9999, 0.0001));
            return ret;
        }

        /// <summary>
        /// Apply the ISEF filter to an image
        /// </summary>
        /// <param name="img"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public override byte[,] ApplyFilter(byte[,] img, SortedDictionary<string, object> configs)
        {
            int width = img.GetLength(0), height = img.GetLength(1);
            int x, y; double b = double.Parse(configs["b"].ToString());

            // Force to double ...
            double[,] dblRet = new double[width, height];
            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    dblRet[x, y] = (double)img[x, y];
                }
            }

            dblRet = ApplyIsefHorizontal(dblRet, b);
            dblRet = ApplyIsefVertical(dblRet, b);

            byte[,] ret = new byte[width, height];
            // Force to byte ...
            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    ret[x, y] = (byte)Math.Min(byte.MaxValue, Math.Max(byte.MinValue, dblRet[x, y]));
                }
            }

            return ret;
        }

        /// <summary>
        /// Apply the ISEF filter on a double[,] in the horizontal direction.
        /// </summary>
        /// <param name="data">The data to perform the operation on.</param>
        /// <param name="b">The ISEF parameter b.</param>
        /// <returns>Filtered data on the horizontal direction.</returns>
        public double[,] ApplyIsefHorizontal(double[,] data, double b)
        {
            int width = data.GetLength(0), height = data.GetLength(1);
            double[,] ret = new double[width, height];

            double b1 = (1.0 - b) / (1.0 + b);
            double b2 = b * b1;

            double[,] causal = new double[width, height], anticausal = new double[width, height];
            int x, y;


            // Boundary conditions
            for (x = 0; x < width; ++x)
            {
                causal[x, 0] = b1 * data[x, 0];
                anticausal[x, height - 1] = b2 * data[x, height - 1];

                //// Changed at 22-05-2011
                //anticausal[x, height - 2] = b2 * data[x, height - 2]; // ??? -2 ????
            }


            // "Causal" component
            for (x = 0; x < width; ++x)
            {
                for (y = 1; y < height; ++y)
                {
                    causal[x, y] = b1 * data[x, y] + b * causal[x, y - 1];
                }
            }


            // "Anti-causal" component
            for (x = 0; x < width; ++x)
            {
                for (y = height - 2; y >= 0; --y)
                {
                    anticausal[x, y] = b2 * data[x, y] + b * anticausal[x, y + 1];
                }
            }
            // ***

            // Boundary case for output
            for (x = 0; x < width; ++x)
                ret[x, height - 1] = causal[x, height - 1];


            // Compute output
            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height - 1; ++y)
                {
                    ret[x, y] = causal[x, y] + anticausal[x, y + 1];
                }
            }

            //// Changed at 22-05-2011
            //// Boundary case for output
            //for (x = 0; x < width - 1; ++x)
            //    ret[x, height - 1] = causal[x, height - 1];


            //// Compute output
            //for (x = 0; x < width - 1; ++x)
            //{
            //    for (y = 0; y < height - 2; ++y)
            //    {
            //        ret[x, y] = causal[x, y] + anticausal[x, y + 1];
            //    }
            //}

            return ret;
        }

        /// <summary>
        /// Apply the ISEF filter on a double[,] in the vertical direction.
        /// </summary>
        /// <param name="data">The data to perform the operation on.</param>
        /// <param name="b">The ISEF parameter b.</param>
        /// <returns>Filtered data on the vertical direction.</returns>
        public double[,] ApplyIsefVertical(double[,] data, double b)
        {
            int width = data.GetLength(0), height = data.GetLength(1);
            double[,] ret = new double[width, height];

            double b1 = (1.0 - b) / (1.0 + b);
            double b2 = b * b1;

            double[,] causal = new double[width, height], anticausal = new double[width, height];
            int x, y;


            // Boundary conditions
            for (y = 0; y < height; ++y)
            {
                causal[0, y] = b1 * data[0, y];
                anticausal[width - 1, y] = b2 * data[width - 1, y];
            }


            // "Causal" component
            for (x = 1; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    causal[x, y] = b1 * data[x, y] + b * causal[x - 1, y];
                }
            }


            // "Anti-causal" component
            for (x = width - 2; x >= 0; --x)
            {
                for (y = 0; y < height; ++y)
                {
                    anticausal[x, y] = b2 * data[x, y] + b * anticausal[x + 1, y];
                }
            }

            // ***

            // Boundary case for output
            for (y = 0; y < height; ++y)
                ret[width - 1, y] = causal[width - 1, y];


            // Compute output
            for (x = 0; x < width - 1; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    ret[x, y] = causal[x, y] + anticausal[x + 1, y];
                }
            }

            //// Changed at 22-05-2011
            //// Boundary case for output
            //for (y = 0; y < height - 1; ++y)
            //    ret[width - 1, y] = causal[width - 1, y];


            //// Compute output
            //for (x = 0; x < width - 2; ++x)
            //{
            //    for (y = 0; y < height - 1; ++y)
            //    {
            //        ret[x, y] = causal[x, y] + anticausal[x + 1, y];
            //    }
            //}

            return ret;
        }


    }
}
