using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Framework.Core.Filters.Frequency
{
    /// <summary>
    /// Implementation of a image in the frequency domain. 
    /// </summary>
    public class ComplexImage
    {
        #region Attributes

        /// <summary>
        /// The mask of "Complex pixels"
        /// </summary>
        private Complex[,] _elemns;

        #endregion

        #region Constructors

        /// <summary>
        /// Clean ComplexImage of size width x height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public ComplexImage(int width, int height)
        {
            _elemns = new Complex[width, height];
        }

        /// <summary>
        /// New ComplexImage, from a greyscale byte array.
        /// The values are not re-normalized.
        /// </summary>
        /// <param name="greyscale"></param>
        public ComplexImage(byte[,] greyscale)
        {
            int width = greyscale.GetLength(0);
            int height = greyscale.GetLength(1);
            _elemns = new Complex[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    _elemns[x, y] = new Complex(greyscale[x, y], 0);
                }
            }
        }

        /// <summary>
        /// New ComplexImage, from a Complex[,].
        /// </summary>
        /// <param name="greyscale"></param>
        public ComplexImage(Complex[,] complexArray)
        {
            _elemns = complexArray;
        }

        /// <summary>
        /// New ComplexImage, from a greyscale double array.
        /// The values are not re-normalized.
        /// </summary>
        /// <param name="grayscale"></param>
        public ComplexImage(double[,] grayscale)
        {
            int width = grayscale.GetLength(0);
            int height = grayscale.GetLength(1);
            _elemns = new Complex[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    _elemns[x, y] = new Complex(grayscale[x, y], 0);
                }
            }
        }

        /// <summary>
        /// Creates a copy of a ComplexImage.
        /// </summary>
        /// <param name="copy">The ComplexImage to copy.</param>
        public ComplexImage(ComplexImage copy)
        {
            int width = copy.Width;
            int height = copy.Height;

            _elemns = new Complex[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    _elemns[x, y] = copy[x, y].Clone();
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indexer for the mask of Complex.
        /// </summary>
        /// <param name="row">The row index (0 based).</param>
        /// <param name="column">The column index (0 based).</param>
        /// <returns></returns>
        public Complex this[int row, int column]
        {
            get
            {
                return _elemns[row, column];
            }
            set
            {
                _elemns[row, column] = value;
            }
        }

        /// <summary>
        /// Width of the ComplexImage.
        /// </summary>
        public int Width
        {
            get
            {
                return _elemns.GetLength(0);
            }
        }

        /// <summary>
        /// Height of the ComplexImage.
        /// </summary>
        public int Height
        {
            get
            {
                return _elemns.GetLength(1);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Type of bitmap to generate in the ToBitmap() function.
        /// </summary>
        public enum ComplexImageBitmapType
        {
            /// <summary>
            /// Uses the Phase component of the complex elements of the mask to generate
            /// the bitmap.
            /// </summary>
            Phase,
            /// <summary>
            /// Uses the Magnitude component of the complex elements of the mask to generate
            /// the bitmap.
            /// </summary>
            Magnitude
        }

        /// <summary>
        /// Converts the ComplexImage to a bitmap (for display purposes).
        /// Is applied a log to the data for better plotting.
        /// </summary>
        /// <param name="bitmapType">The type of bitmap to generate.</param>
        /// <returns>Bitmap representation of the ComplexImage.</returns>
        public Bitmap ToBitmap(ComplexImageBitmapType bitmapType)
        {
            int x, y;
            int width = this.Width, height = this.Height;
            Bitmap ret = null;

            ComplexImage transform = new ComplexImage(this);

            double[,] logs = new double[width, height];
            double[,] data = new double[width, height];
            int[,] normalized = new int[width, height];

            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    switch (bitmapType)
                    {
                        case ComplexImageBitmapType.Magnitude:
                            data[x, y] = transform[x, y].Magnitude();
                            logs[x, y] = (float)(Math.Log(0.1 + data[x, y]));
                            break;
                        case ComplexImageBitmapType.Phase:
                            data[x, y] = transform[x, y].Phase();
                            logs[x, y] = (float)Math.Log(0.1 + Math.Abs(data[x, y]));
                            break;
                    }
                }
            }

            // get max and min
            double max = double.MinValue, min = double.MaxValue;
            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    max = Math.Max(max, logs[x, y]);
                    min = Math.Min(min, logs[x, y]);
                }
            }

            double range = max - min;

            // Normalize data

            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    //normalized[x, y] = (int)(byte.MaxValue * (logs[x, y] / max));
                    normalized[x, y] = (int)(byte.MaxValue * ((logs[x, y] - min) / range));
                }
            }

            // write to bitmap ...
            ret = Facilities.ToBitmap(normalized);


            //switch (bitmapType)
            //{
            //    case ComplexImageBitmapType.Magnitude:
            //        // divide by max and normalize
            //        for (x = 0; x < width; ++x)
            //        {
            //            for (y = 0; y < height; ++y)
            //            {
            //                //normalized[x, y] = (int)(byte.MaxValue * (logs[x, y] / max));                            
            //                normalized[x, y] = (int) (byte.MaxValue * ((logs[x, y] - min) / range));
            //            }
            //        }
            //        // write to bitmap ...
            //        ret = Facilities.ToBitmap(normalized);
            //        break;
            //    case ComplexImageBitmapType.Phase:
            //        // divide by max and normalize
            //        for (x = 0; x < width; ++x)
            //        {
            //            for (y = 0; y < height; ++y)
            //            {
            //                //normalized[x, y] = (int)(byte.MaxValue * (logs[x, y] / max));
            //                normalized[x, y] = (int)(byte.MaxValue * ((logs[x, y] - min) / range));
            //            }
            //        }
            //        // write to bitmap ...
            //        ret = Facilities.ToBitmap(normalized);
            //        break;
            //}

            return ret;
        }



        ///// <summary>
        ///// Shift The FFT of the Image
        ///// </summary>
        //private ComplexImage FFTShift()
        //{
        //    ComplexImage FFTShifted = null;
        //    try
        //    {
        //        int i, j;
        //        int width = this.Width, height = this.Height;
        //        FFTShifted = new ComplexImage(width, height);
        //        for (i = 0; i <= (width / 2) - 1; i++)
        //            for (j = 0; j <= (height / 2) - 1; j++)
        //            {
        //                FFTShifted[i + (width / 2), j + (height / 2)] = _elemns[i, j].Clone();
        //                FFTShifted[i, j] = _elemns[i + (width / 2), j + (height / 2)].Clone();
        //                FFTShifted[i + (width / 2), j] = _elemns[i, j + (height / 2)].Clone();
        //                FFTShifted[i, j + (width / 2)] = _elemns[i + (width / 2), j].Clone();
        //            }
        //    }
        //    catch
        //    {
        //        FFTShifted = new ComplexImage(this);
        //    }
        //    return FFTShifted;
        //}

        ///// <summary>
        ///// Removes FFT Shift for FFTshift Array
        ///// </summary>
        //private ComplexImage RemoveFFTShift()
        //{
        //    int i, j;
        //    ComplexImage FFTNormal = new ComplexImage(this.Width, this.Height);
        //    for (i = 0; i <= (this.Width / 2) - 1; i++)
        //        for (j = 0; j <= (this.Height / 2) - 1; j++)
        //        {
        //            FFTNormal[i + (this.Width / 2), j + (this.Height / 2)] = this[i, j].Clone();
        //            FFTNormal[i, j] = this[i + (this.Width / 2), j + (this.Height / 2)].Clone();
        //            FFTNormal[i + (this.Width / 2), j] = this[i, j + (this.Height / 2)].Clone();
        //            FFTNormal[i, j + (this.Width / 2)] = this[i + (this.Width / 2), j].Clone();
        //        }
        //    return FFTNormal;
        //}

        #endregion

    }
}
