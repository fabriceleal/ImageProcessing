using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Framework.Core.Filters.Spatial
{
    /// <summary>
    /// Implementation of convolution.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class Kernel2D
    {

        #region Attributes

        /// <summary>
        /// Threshold to apply to the final result of the convolution.
        /// </summary>
        private double _threeshold;

        /// <summary>
        /// Value to multiply to the final result of the convolution.
        /// </summary>
        private double _multiplier;

        /// <summary>
        /// The mask to use to apply the convolution.
        /// </summary>
        private double[,] _2d_matrix;

        #endregion

        #region Constructors

        /// <summary>
        /// New Kernel2D.
        /// </summary>
        /// <param name="threeshold">Threshold to apply to the final result of the convolution.</param>
        /// <param name="multiplier">Value to multiply to the final result of the convolution.</param>
        /// <param name="mask">The mask to use to apply the convolution.</param>
        public Kernel2D(double threeshold, double multiplier, double[,] matrix)
        {
            _threeshold = threeshold;

            // <> of zero!!!
            if (multiplier == 0.0)
                throw new Exception("The multiplier should be different of zero!");

            _multiplier = multiplier;

            // Matrix just be square
            if (matrix.GetLength(0) != matrix.GetLength(1))
                throw new Exception("You must provide a squared matrix.");

            // Matrix should have odd width and height 
            if (matrix.GetLength(0) % 2 == 0)
                throw new Exception("The matrix should have odd dimensions (Ex: 5x5).");

            _2d_matrix = matrix;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Map a index in kernel to a pixel location in the image.
        /// </summary>
        /// <remarks>
        /// The provided point (xImage, yImage) will be mapped to central element in the kernel's mask.
        /// </remarks>
        /// <param name="xImage">The x coordinate of the pixel in the image to parse. </param>
        /// <param name="yImage">The y coordinate of the pixel in the image to parse. </param>
        /// <param name="imageWidth">The width of the image to parse.</param>
        /// <param name="imageHeight">The height of the image to parse.</param>
        /// <returns>An array of Tuple that map a point in the Kernel (Item1) to a point in the Image (Item2);
        /// An invalid location in the image will be mapped with (-1, -1).
        /// </returns>
        public Tuple<Point, Point>[] ResolvePositions(
                int xImage, int yImage, int imageWidth, int imageHeight)
        {
            List<Tuple<Point, Point>> ret = new List<Tuple<Point, Point>>();
            int xUpperB = _2d_matrix.GetUpperBound(0);
            int yUpperB = _2d_matrix.GetUpperBound(1);

            if (xUpperB % 2 != 0)
            {
                // Even-Dim mask

                throw new Exception("The Kernel2D class does not support even dimension matrixes.");
            }
            else
            {
                // Odd-Dim mask
                // The central point corresponds to the pixel being calculated ...

                int center = (xUpperB / 2);

                Point kernelIdxs; Point imageIdxs;

                for (int x = 0; x <= xUpperB; ++x)
                {
                    for (int y = 0; y <= yUpperB; ++y)
                    {
                        if (_2d_matrix[x, y] != 0)
                        {
                            kernelIdxs = new Point(x, y);
                            imageIdxs = new Point(xImage - center + x, yImage - center + y);

                            // Reset all invalid indexes to -1
                            if (imageIdxs.X < 0 || imageIdxs.X >= imageWidth)
                                imageIdxs.X = -1;

                            if (imageIdxs.Y < 0 || imageIdxs.Y >= imageHeight)
                                imageIdxs.Y = -1;

                            ret.Add(new Tuple<Point, Point>(kernelIdxs, imageIdxs));
                        }
                    }
                }
            }

            return ret.ToArray();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The identity kernel.
        /// </summary>
        private static Kernel2D _identity = null;

        /// <summary>
        /// Gets the identity kernel; when applied to an Image, returns the Image itself.
        /// </summary>
        public static Kernel2D Identity
        {
            get
            {
                if (null == _identity)
                {
                    _identity = new Kernel2D(
                            0, 1, new double[,] { { 1 } });
                    // --
                }
                return _identity;
            }
        }

        /// <summary>
        /// Gets the value to multiply to the final result of the convolution.
        /// </summary>
        public double Multiplier
        {
            get
            {
                return _multiplier;
            }
        }

        /// <summary>
        /// Gets the threshold to apply to the final result of the convolution.
        /// </summary>
        public double Threeshold
        {
            get
            {
                return _threeshold;
            }
        }

        /// <summary>
        /// Gets the mask to use to apply the convolution.
        /// </summary>
        public double[,] Matrix
        {
            get
            {
                return _2d_matrix;
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Function signature to use in the ApplyFunction(Kernel2D, byte[,], ByteKernel2DCallBack) function.
        /// </summary>
        /// <param name="mappings">The resolved mappings between the kernel and the image.</param>
        /// <param name="image">The image to parse.</param>
        /// <returns>The calculated byte component.</returns>
        public delegate byte ByteKernel2DCallBack(Tuple<Point, Point>[] mappings, byte[,] image);

        /// <summary>
        /// Function signature to use in the ApplyFunction(Kernel2D, double[,], DoubleKernel2DCallBack) function.
        /// </summary>
        /// <param name="mappings">The resolved mappings between the kernel and the image.</param>
        /// <param name="image">The image to parse.</param>
        /// <returns>The calculated double component.</returns>
        public delegate double DoubleKernel2DCallBack(Tuple<Point, Point>[] mappings, double[,] image);

        /// <summary>
        /// Apply a function to a double[,] image, using the Kernel2D kernel.
        /// </summary>
        /// <param name="kernel">The kernel to use in the parsing.</param>
        /// <param name="operand">The image to apply the function.</param>
        /// <param name="callback">The function to apply.</param>
        /// <returns>The transformed double[,].</returns>
        public static double[,] ApplyFunction(Kernel2D kernel, double[,] operand, DoubleKernel2DCallBack callback)
        {
            int width = operand.GetLength(0); int height = operand.GetLength(1);

            double[,] ret = new double[width, height];
            Tuple<Point, Point>[] mappings = null;
            int x, y;

            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    try
                    {
                        mappings = kernel.ResolvePositions(x, y, width, height);

                        ret[x, y] = callback(mappings, operand);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Exception at Kernel2D.ApplyFunction", e);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Apply a function to a byte[,] image, using the Kernel2D kernel.
        /// </summary>
        /// <param name="kernel">The kernel to use in the parsing.</param>
        /// <param name="operand">The image to apply the function.</param>
        /// <param name="callback">The function to apply.</param>
        /// <returns>The transformed byte[,].</returns>
        public static byte[,] ApplyFunction(Kernel2D kernel, byte[,] operand, ByteKernel2DCallBack callback)
        {
            int width = operand.GetLength(0); int height = operand.GetLength(1);

            byte[,] ret = new byte[width, height];
            Tuple<Point, Point>[] mappings = null;
            int x, y;

            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    try
                    {
                        mappings = kernel.ResolvePositions(x, y, width, height);

                        ret[x, y] = callback(mappings, operand);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Exception at Kernel2D.ApplyFunction", e);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Apply a convolution to the double[,] image using the Kernel2D kernel.
        /// </summary>
        /// <remarks>
        /// The output will not be clamped to be stored as byte [0 .. 255].
        /// </remarks>
        /// <param name="kernel">The kernel to use in the convolution.</param>
        /// <param name="operand">The image to apply the convolution.</param>
        /// <returns>The transformed double[,].</returns>
        public static double[,] ApplyConvolution(Kernel2D kernel, double[,] operand)
        {
            double tot; int i; double normalized;

            DoubleKernel2DCallBack function =
                    delegate(Tuple<Point, Point>[] mappings, double[,] op)
                    {
                        tot = 0; normalized = 0;

                        for (i = 0; i < mappings.Length; ++i)
                        {
                            if (mappings[i].Item2.X != -1 && mappings[i].Item2.Y != -1)
                            {
                                tot += (
                                    kernel._2d_matrix[mappings[i].Item1.X, mappings[i].Item1.Y] *
                                    op[mappings[i].Item2.X, mappings[i].Item2.Y]);
                            }
                        }
                        tot = tot * kernel._multiplier + kernel._threeshold;

                        normalized = tot;

                        return normalized;
                    };

            return ApplyFunction(kernel, operand, function);
        }

        /// <summary>
        /// Apply a convolution to the byte[,] image using the Kernel2D kernel.
        /// </summary>
        /// <remarks>
        /// As the output is a byte[,], the temporary result is stored in a double, but it will
        /// be clamped to be stored in a byte variable [0 .. 255].
        /// </remarks>
        /// <param name="kernel">The kernel to use in the convolution.</param>
        /// <param name="operand">The image to apply the convolution.</param>
        /// <returns>The transformed byte[,].</returns>
        public static byte[,] ApplyConvolution(Kernel2D kernel, byte[,] operand)
        {
            double tot; int i; byte normalized;

            return ApplyFunction(
                    kernel, operand,
                    delegate(Tuple<Point, Point>[] mappings, byte[,] op)
                    {
                        tot = 0; normalized = byte.MinValue;

                        for (i = 0; i < mappings.Length; ++i)
                        {
                            if (mappings[i].Item2.X != -1 && mappings[i].Item2.Y != -1)
                            {
                                tot += (
                                    kernel._2d_matrix[mappings[i].Item1.X, mappings[i].Item1.Y] *
                                    op[mappings[i].Item2.X, mappings[i].Item2.Y]);
                            }
                        }
                        tot = tot * kernel._multiplier + kernel._threeshold;

                        normalized = (byte)Math.Max(byte.MinValue, Math.Min(byte.MaxValue, tot));

                        return normalized;
                    });
        }

        #endregion

    }
}
