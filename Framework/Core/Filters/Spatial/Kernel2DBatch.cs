using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Core.Filters.Spatial
{
    /// <summary>
    /// Provides functionality for applying several kernels to calculate an image.
    /// </summary>
    /// <remarks>
    /// It follows a pattern resembling the Map-Reduce pattern: each kernel to apply will be evaluated
    /// and all the outputs will be "merged", resulting in the pixel to calculate.
    /// </remarks>
    public class Kernel2DBatch
    {

        #region Methods

        /// <summary>
        /// The function signature for applying a kernel to a image.
        /// </summary>
        /// <param name="kernel">The kernel to use.</param>
        /// <param name="operand">The image to parse.</param>
        /// <param name="x">The x coordinate of the pixel to generate.</param>
        /// <param name="y">The y coordinate of the pixel to generate.</param>
        /// <returns>The output of the evaluation of the kernel to the image.</returns>
        public delegate double EvaluateKernelDelegate(Kernel2D kernel, byte[,] operand, int x, int y);
        
        /// <summary>
        /// The function signature for merging the outputs of the evaluations of the several kernels.
        /// </summary>
        /// <param name="values">List of the outputs generated.</param>
        /// <returns>The final value of the pixel being generated.</returns>
        public delegate byte ReduceDelegate(List<double> values);

        /// <summary>
        /// Evaluates a list of kernels against a byte[,] image.
        /// </summary>
        /// <param name="operand">The image to parse.</param>
        /// <param name="kernels">The kernels to apply.</param>
        /// <param name="evaluateKernel">The "map" function to use. For further explaination, 
        /// refer to the documentation of Kernel2DBatch.</param>
        /// <param name="reduce">The "reduce" function to use. For further explaination, 
        /// refer to the documentation of Kernel2DBatch.</param>
        /// <returns>The transformed byte[,].</returns>
        public static byte[,] Evaluate(
                byte[,] operand,
                Kernel2D[] kernels,
                EvaluateKernelDelegate evaluateKernel,
                ReduceDelegate reduce)
        {
            int width = operand.GetLength(0);
            int height = operand.GetLength(1);

            byte[,] ret = new byte[width, height];
            List<double> evaluations = new List<double>();

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    evaluations.Clear();

                    // -> Map
                    for (int i = 0; i < kernels.Length; ++i)
                    {
                        // Evaluate
                        evaluations.Add(evaluateKernel(kernels[i], operand, x, y));
                    }

                    // -> Reduce
                    ret[x, y] = reduce(evaluations);
                }
            }

            return ret;
        }

        #endregion

    }
}
