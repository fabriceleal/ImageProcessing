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

namespace Framework.Filters.EdgeDetection.Template
{

    /// <summary>
    /// 
    /// </summary>
    [Filter("Roberts", "Roberts Edge Detection", "***",
            new string[] { "Edge Detection", "Template Based" }, true)]
    [EdgeDetectionMetric()]
    public class Roberts : SpatialDomainFilter
    {

        #region SpatialDomainFilter

        /// <summary>
        /// Generate default configurations for the filter.
        /// </summary>
        /// <returns>Dictionary used to pass configuration though the 
        /// configs parameter of the method ApplyFilter. 
        /// null if there is the filter doesn't use configurations.</returns>
        public override SortedDictionary<string, object> GetDefaultConfigs()
        {
            SortedDictionary<string, object> ret = new SortedDictionary<string, object>();

            ret.Add("Threshold", Rangeable.ForByte(5));

            return ret;
        }

        /// <summary>
        /// Apply filter to a byte[,].
        /// </summary>
        /// <param name="iImageSrc"></param>
        /// <param name="iConfigs"></param>
        /// <returns></returns>
        public override byte[,] ApplyFilter(byte[,] iImageSrc, SortedDictionary<string, object> iConfigs)
        {
            int width = iImageSrc.GetLength(0), height = iImageSrc.GetLength(1);
            byte threshold = byte.Parse(iConfigs["Threshold"].ToString());

            // In 3x3 style ... :D
            // By Gonzalez ...
            //
            // Simplified Approach to Image Processing says:
            //
            // 0 0 -1
            // 0 1 0
            // 0 0 0
            //
            // -1 0 0
            // 0 1 0
            // 0 0 0
            //
            byte[,] tmp_ret = Kernel2DBatch.Evaluate(
                    iImageSrc,
                    new Kernel2D[] {
                            new Kernel2D(0, (1/1.0), new double[,] { 
                                { 0, 0, 0 }, 
                                { 0, -1, 0 }, 
                                { 0, 0, 1 } }),
                            new Kernel2D(0, (1/1.0), new double[,] { 
                                { 0, 0, 0 }, 
                                { 0, 0, -1 }, 
                                { 0, 1, 0 } }) 
                        },
                    delegate(Kernel2D k, byte[,] op, int x, int y)
                    {
                        double total = 0;
                        Tuple<Point, Point>[] mappings = k.ResolvePositions(x, y, width, height);

                        for (int i = 0; i < mappings.Length; ++i)
                        {
                            if (mappings[i].Item2.X != -1 && mappings[i].Item2.Y != -1)
                            {
                                total += (
                                        op[mappings[i].Item2.X, mappings[i].Item2.Y]
                                        *
                                        k.Matrix[mappings[i].Item1.X, mappings[i].Item1.Y]
                                    );
                            }
                        }

                        total = total * k.Multiplier + k.Threeshold;

                        return total;
                        // subtle bug discovered at 02-06-2011
                        //return Math.Min(byte.MaxValue, Math.Max(byte.MinValue, total));
                    },
                    delegate(List<double> values)
                    {
                        double tot = 0;
                        //for (int i = 0; i < values.Count; ++i)
                        //{
                        //    tot += Math.Pow(values[i], 2);
                        //}
                        //tot = Math.Sqrt(tot);
                        for (int i = 0; i < values.Count; ++i)
                        {
                            tot += Math.Abs(values[i]);
                        }

                        // Values smaller than the threshold are forced to zero
                        if (tot < threshold)
                        {
                            tot = 0;
                        }

                        return (byte)Math.Min(byte.MaxValue, Math.Max(byte.MinValue, tot));
                    });

            return tmp_ret;
        }

        #endregion

    }
}
