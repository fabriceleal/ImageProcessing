using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Framework.Core;
using Framework.Core.Filters.Frequency;
using Framework.Core.Transforms;

namespace Framework.Transforms
{

    /// <summary>
    /// Implementation of the fast fourier transform, using the Cooley-Tukey algorithm, with O( n.log(n) ) complexity.
    /// Implemented filters or external users should use Framework.Transforms.FourierTransform instead.
    /// </summary>
    /// <remarks>
    /// Source: http://www.codeproject.com/KB/GDI/FFT.aspx
    /// </remarks>
    public class FastFourierTransform : TransformCoreBase<ComplexImage>
    {
        // Code is duplicated between ApplyTransformBase-ApplyTransform and 
        // ReverseTransform-ReverseTransformBase 
        // to avoid the perfomance hit of 
        // converting a full double[,] image to a byte[,] image

        #region TransformCoreBase ( double[,] )

        /// <summary>
        /// Apply the fast fourier transform on dataSpatialDomain. If any dimension is not base 2, an 
        /// ArgumentException exception will be thrown. This will perform the centered fourier transform,
        /// as pointed by Gonzelez.
        /// </summary>
        /// <param name="dataSpatialDomain">The data to transform.</param>
        /// <returns>The fourier transfrom of dataSpatialDomain.</returns>
        public override ComplexImage ApplyTransformBase(double[,] dataSpatialDomain)
        {
            // (1) Check dimensions
            int width = dataSpatialDomain.GetLength(0), height = dataSpatialDomain.GetLength(1);

            if (Math.Log(width, 2) % 1 > 0)
                throw new ArgumentException("Width of image is not base 2.");

            if (Math.Log(height, 2) % 1 > 0)
                throw new ArgumentException("Height of image is not base 2.");

            double[,] transf = new double[width, height];

            // As pointed by Gonzalez, to center the transform
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    // Dummy
                    //transf[x, y] = dataSpatialDomain[x, y] * Math.Pow(-1, x + y);

                    // Clever
                    transf[x, y] = dataSpatialDomain[x, y];
                    if ((x + y) % 2 != 0)
                    {
                        transf[x, y] = -1 * transf[x, y];
                    }
                }
            }

            // (2) 
            ComplexImage complex = new ComplexImage(transf);

            ComplexImage output = FFT2D(complex, FFTDirection.Forward);

            // --
            return output;
        }

        /// <summary>
        /// Apply the inverse fourier transform on an image on the frequency domain.
        /// </summary>
        /// <param name="dataFrequencyDomain">The image in frequency domain.</param>
        /// <returns>The inverse fourier transform of dataFrequencyDomain.</returns>
        public override double[,] ApplyReverseTransformBase(ComplexImage dataFrequencyDomain)
        {
            // Initializing Fourier Transform Array
            int i, j; int width = dataFrequencyDomain.Width, height = dataFrequencyDomain.Height;

            // Calling Forward Fourier Transform
            ComplexImage output = FFT2D(dataFrequencyDomain, FFTDirection.Reverse);

            // Convert to gray bitmap ...
            double[,] ret = new double[width, height];

            // Copying Real Image Back to Greyscale
            // Copy Image Data to the Complex Array
            for (i = 0; i < width; i++)
                for (j = 0; j < height; j++)
                {
                    // The algorithm uses Magnitude... Gonzalez says real. Gonzalez wins.

                    // Dummy
                    //ret[i, j] = output[i, j].real * Math.Pow(-1, i + j);

                    // Clever
                    ret[i, j] = output[i, j].real;
                    if ((i + j) % 2 != 0)
                    {
                        ret[i, j] = -1 * ret[i, j];
                    }
                }

            return ret;
        }

        #endregion

        #region TransformCoreBase ( byte[,] )

        /// <summary>
        /// Apply the fast fourier transform on imageSpatialDomain. If any dimension is not base 2, an 
        /// ArgumentException exception will be thrown. This will perform the centered fourier transform,
        /// as pointed by Gonzelez.
        /// </summary>
        /// <param name="imageSpatialDomain">The image (greyscale) to transform.</param>
        /// <returns>The fourier transfrom of imageSpatialDomain.</returns>
        public override ComplexImage ApplyTransform(byte[,] imageSpatialDomain)
        {
            // (1) Check dimensions
            int width = imageSpatialDomain.GetLength(0), height = imageSpatialDomain.GetLength(1);

            if (Math.Log(width, 2) % 1 > 0)
                throw new ArgumentException("Width of image is not base 2.");

            if (Math.Log(height, 2) % 1 > 0)
                throw new ArgumentException("Height of image is not base 2.");

            double[,] transf = new double[width, height];

            // As pointed by Gonzalez
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    transf[x, y] = imageSpatialDomain[x, y] * Math.Pow(-1, x + y);
                }
            }

            // (2) 
            ComplexImage complex = new ComplexImage(transf);

            ComplexImage output = FFT2D(complex, FFTDirection.Forward);

            // --
            return output;
        }

        /// <summary>
        /// Apply the inverse fourier transform on an image on the frequency domain.
        /// </summary>
        /// <param name="imageFrequencyDomain">The image in frequency domain.</param>
        /// <returns>The inverse fourier transform of imageFrequencyDomain.</returns>
        public override byte[,] ApplyReverseTransform(ComplexImage imageFrequencyDomain)
        {
            // Initializing Fourier Transform Array
            int i, j; int width = imageFrequencyDomain.Width, height = imageFrequencyDomain.Height;

            // Calling Forward Fourier Transform
            ComplexImage output = FFT2D(imageFrequencyDomain, FFTDirection.Reverse);

            // Convert to gray bitmap ...
            byte[,] ret = new byte[width, height];

            // Copying Real Image Back to Greyscale
            // Copy Image Data to the Complex Array
            for (i = 0; i < width; i++)
                for (j = 0; j < height; j++)
                {
                    // The algorithm uses Magnitude... Gonzalez says real. Gonzalez wins.
                    ret[i, j] = (byte)Math.Min(
                        byte.MaxValue,
                        Math.Max(
                            byte.MinValue,
                            output[i, j].real * Math.Pow(-1, i + j)));
                }

            return ret;
        }

        #endregion

        #region Aux

        private enum FFTDirection
        {
            Forward = 1,
            Reverse = -1
        }

        ///// <summary>
        ///// FFT Plot Execute for Shifted FFT
        ///// </summary>
        ///// <param name="Output"></param>
        //public void FFTPlot(ComplexImage src)
        //{
        //    int width = src.Width, height = src.Height;
        //    int i, j;
        //    float max;
        //    float[,] FFTLog = new float[width, height];
        //    float[,] FFTPhaseLog = new float[width, height];
        //    float[,] FourierMagnitude = new float[width, height];
        //    float[,] FourierPhase = new float[width, height];
        //    int[,] FFTNormalized = new int[width, height];
        //    int[,] FFTPhaseNormalized = new int[width, height];
        //    for (i = 0; i < width; i++)
        //        for (j = 0; j < height; j++)
        //        {
        //            FourierMagnitude[i, j] = src[i, j].Magnitude();
        //            FourierPhase[i, j] = src[i, j].Phase();
        //            FFTLog[i, j] = (float)Math.Log(1 + FourierMagnitude[i, j]);
        //            FFTPhaseLog[i, j] = (float)Math.Log(1 + Math.Abs(FourierPhase[i, j]));
        //        }
        //    //Generating Magnitude Bitmap
        //    max = FFTLog[0, 0];
        //    for (i = 0; i < width; i++)
        //        for (j = 0; j < height; j++)
        //        {
        //            if (FFTLog[i, j] > max)
        //                max = FFTLog[i, j];
        //        }
        //    for (i = 0; i < width; i++)
        //        for (j = 0; j < height; j++)
        //        {
        //            FFTLog[i, j] = FFTLog[i, j] / max;
        //        }
        //    for (i = 0; i < width; i++)
        //        for (j = 0; j < height; j++)
        //        {
        //            FFTNormalized[i, j] = (int)(2000 * FFTLog[i, j]);
        //        }
        //    //Transferring Image to Fourier Plot
        //    Bitmap FourierPlot = Displayimage(FFTNormalized);
        //    //generating phase Bitmap
        //    FFTPhaseLog[0, 0] = 0;
        //    max = FFTPhaseLog[1, 1];
        //    for (i = 0; i < width; i++)
        //        for (j = 0; j < height; j++)
        //        {
        //            if (FFTPhaseLog[i, j] > max)
        //                max = FFTPhaseLog[i, j];
        //        }
        //    for (i = 0; i < width; i++)
        //        for (j = 0; j < height; j++)
        //        {
        //            FFTPhaseLog[i, j] = FFTPhaseLog[i, j] / max;
        //        }
        //    for (i = 0; i < width; i++)
        //        for (j = 0; j < height; j++)
        //        {
        //            FFTPhaseNormalized[i, j] = (int)(255 * FFTPhaseLog[i, j]);
        //        }
        //    //Transferring Image to Fourier Plot
        //    Bitmap PhasePlot = Displayimage(FFTPhaseNormalized);
        //}

        //public Bitmap Displayimage(int[,] image)
        //{
        //    int i, j;
        //    Bitmap output = new Bitmap(image.GetLength(0), image.GetLength(1));
        //    BitmapData bitmapData1 = output.LockBits(new Rectangle(0, 0, image.GetLength(0), image.GetLength(1)),
        //                             ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        //    unsafe
        //    {
        //        byte* imagePointer1 = (byte*)bitmapData1.Scan0;
        //        for (i = 0; i < bitmapData1.Height; i++)
        //        {
        //            for (j = 0; j < bitmapData1.Width; j++)
        //            {
        //                imagePointer1[0] = (byte)image[j, i];
        //                imagePointer1[1] = (byte)image[j, i];
        //                imagePointer1[2] = (byte)image[j, i];
        //                imagePointer1[3] = 255;
        //                //4 bytes per pixel
        //                imagePointer1 += 4;
        //            }//end for j
        //            //4 bytes per pixel
        //            imagePointer1 += (bitmapData1.Stride - (bitmapData1.Width * 4));
        //        }//end for i
        //    }//end unsafe
        //    output.UnlockBits(bitmapData1);
        //    return output;// col;
        //}

        ///// <summary>
        ///// generate FFT Image for Display Purpose
        ///// </summary>
        //public void FFTPlot(ComplexImage src)
        //{
        //    int width = src.Width, height = src.Height;
        //    int i, j;
        //    float max;
        //    float[,] FFTLog = new float[width, height];
        //    float[,] FFTPhaseLog = new float[width, height];
        //    float[,] FourierMagnitude = new float[width, height];
        //    float[,] FourierPhase = new float[width, height];
        //    int[,] FFTNormalized = new int[width, height];
        //    int[,] FFTPhaseNormalized = new int[width, height];
        //    for (i = 0; i < width; i++)
        //        for (j = 0; j < height; j++)
        //        {
        //            FourierMagnitude[i, j] = src[i, j].Magnitude();
        //            FourierPhase[i, j] = src[i, j].Phase();
        //            FFTLog[i, j] = (float)Math.Log(1 + FourierMagnitude[i, j]);
        //            FFTPhaseLog[i, j] = (float)Math.Log(1 + Math.Abs(FourierPhase[i, j]));
        //        }
        //    //Generating Magnitude Bitmap
        //    max = FFTLog[0, 0];
        //    for (i = 0; i < width; i++)
        //        for (j = 0; j < height; j++)
        //        {
        //            if (FFTLog[i, j] > max)
        //                max = FFTLog[i, j];
        //        }
        //    for (i = 0; i < width; i++)
        //        for (j = 0; j < height; j++)
        //        {
        //            FFTLog[i, j] = FFTLog[i, j] / max;
        //        }
        //    for (i = 0; i < width; i++)
        //        for (j = 0; j < height; j++)
        //        {
        //            FFTNormalized[i, j] = (int)(1000 * FFTLog[i, j]);
        //        }
        //    //Transferring Image to Fourier Plot
        //    Bitmap FourierPlot = Displayimage(FFTNormalized);
        //    //generating phase Bitmap
        //    max = FFTPhaseLog[0, 0];
        //    for (i = 0; i < width; i++)
        //        for (j = 0; j < height; j++)
        //        {
        //            if (FFTPhaseLog[i, j] > max)
        //                max = FFTPhaseLog[i, j];
        //        }
        //    for (i = 0; i < width; i++)
        //        for (j = 0; j < height; j++)
        //        {
        //            FFTPhaseLog[i, j] = FFTPhaseLog[i, j] / max;
        //        }
        //    for (i = 0; i < width; i++)
        //        for (j = 0; j < height; j++)
        //        {
        //            FFTPhaseNormalized[i, j] = (int)(2000 * FFTLog[i, j]);
        //        }
        //    //Transferring Image to Fourier Plot
        //    Bitmap PhasePlot = Displayimage(FFTPhaseNormalized);
        //}

        ///// <summary>
        ///// Generates Inverse FFT of Given Input Fourier
        ///// </summary>
        ///// <param name="Fourier"></param>
        //public void InverseFFT(COMPLEX[,] Fourier)
        //{
        //    //Initializing Fourier Transform Array
        //    int i, j;
        //    //Calling Forward Fourier Transform
        //    Output = new COMPLEX[nx, ny];
        //    Output = FFT2D(Fourier, nx, ny, -1);
        //    //Copying Real Image Back to Greyscale
        //    //Copy Image Data to the Complex Array
        //    for (i = 0; i <= Width - 1; i++)
        //        for (j = 0; j <= Height - 1; j++)
        //        {
        //            GreyImage[i, j] = (int)Output[i, j].Magnitude();
        //        }
        //    Obj = Displayimage(GreyImage);
        //    return;
        //}

        /*-------------------------------------------------------------------------
            Perform a 2D FFT inplace given a complex 2D array
            The direction dir, 1 for forward, -1 for reverse
            The size of the array (nx,ny)
            Return false if there are memory problems or
            the dimensions are not powers of 2
        */
        /// <summary>
        /// Implementation of the Fast Fourier transform using the Cooley-Tukey algorithm (2D).
        /// </summary>
        /// <param name="c">The complex representation of the image.</param>
        /// <param name="dir">The direction of the transform (forward / reverse)</param>
        /// <returns>The forward / reverse tranform of the input.</returns>
        private ComplexImage FFT2D(ComplexImage c, FFTDirection dir)
        {
            int i, j;
            int m;//Power of 2 for current number of points
            double[] real;
            double[] imag;
            ComplexImage output = new ComplexImage(c); // Copying array
            // Transform the Rows 
            real = new double[c.Width];
            imag = new double[c.Width];

            for (j = 0; j < c.Height; j++)
            {
                for (i = 0; i < c.Width; i++)
                {
                    real[i] = c[i, j].real;
                    imag[i] = c[i, j].imag;
                }
                // Calling 1D FFT Function for Rows
                m = (int)Math.Log((double)c.Width, 2);//Finding power of 2 for current number of points e.g. for nx=512 m=9
                FFT1D(dir, m, ref real, ref imag);

                for (i = 0; i < c.Width; i++)
                {
                    //  c[i,j].real = real[i];
                    //  c[i,j].imag = imag[i];
                    output[i, j].real = real[i];
                    output[i, j].imag = imag[i];
                }
            }
            // Transform the columns  
            real = new double[c.Height];
            imag = new double[c.Height];

            for (i = 0; i < c.Width; i++)
            {
                for (j = 0; j < c.Height; j++)
                {
                    //real[j] = c[i,j].real;
                    //imag[j] = c[i,j].imag;
                    real[j] = output[i, j].real;
                    imag[j] = output[i, j].imag;
                }
                // Calling 1D FFT Function for Columns
                m = (int)Math.Log((double)c.Height, 2);//Finding power of 2 for current number of points e.g. for nx=512 m=9

                FFT1D(dir, m, ref real, ref imag);

                for (j = 0; j < c.Height; j++)
                {
                    //c[i,j].real = real[j];
                    //c[i,j].imag = imag[j];
                    output[i, j].real = real[j];
                    output[i, j].imag = imag[j];
                }
            }

            // return(true);
            return (output);
        }

        /*-------------------------------------------------------------------------
            This computes an in-place complex-to-complex FFT
            x and y are the real and imaginary arrays of 2^m points.
            dir = 1 gives forward transform
            dir = -1 gives reverse transform
            Formula: forward
                     N-1
                      ---
                    1 \         - j k 2 pi n / N
            X(K) = --- > x(n) e                  = Forward transform
                    N /                            n=0..N-1
                      ---
                     n=0
            Formula: reverse
                     N-1
                     ---
                     \          j k 2 pi n / N
            X(n) =    > x(k) e                  = Inverse transform
                     /                             k=0..N-1
                     ---
                     k=0
            */
        /// <summary>
        /// Implementation of the Fast Fourier transform using the Cooley-Tukey algorithm (1D).
        /// </summary>
        /// <param name="dir">The direction of the transform (forward / reverse)</param>
        /// <param name="m">Base 2 power for the number of points.</param>
        /// <param name="x">The real part of the input, it will be overwriten with the output's real part</param>
        /// <param name="y">The imaginary part of the input, it will be overwriten with the imaginary's real part</param>
        private void FFT1D(FFTDirection dir, int m, ref double[] x, ref double[] y)
        {
            long nn, i, i1, j, k, i2, l, l1, l2;
            double c1, c2, tx, ty, t1, t2, u1, u2, z;
            /* Calculate the number of points */
            nn = 1;
            for (i = 0; i < m; i++)
                nn *= 2;
            /* Do the bit reversal */
            i2 = nn >> 1;
            j = 0;
            for (i = 0; i < nn - 1; i++)
            {
                if (i < j)
                {
                    tx = x[i];
                    ty = y[i];
                    x[i] = x[j];
                    y[i] = y[j];
                    x[j] = tx;
                    y[j] = ty;
                }
                k = i2;
                while (k <= j)
                {
                    j -= k;
                    k >>= 1;
                }
                j += k;
            }
            /* Compute the FFT */
            c1 = -1.0;
            c2 = 0.0;
            l2 = 1;
            for (l = 0; l < m; l++)
            {
                l1 = l2;
                l2 <<= 1;
                u1 = 1.0;
                u2 = 0.0;
                for (j = 0; j < l1; j++)
                {
                    for (i = j; i < nn; i += l2)
                    {
                        i1 = i + l1;
                        t1 = u1 * x[i1] - u2 * y[i1];
                        t2 = u1 * y[i1] + u2 * x[i1];
                        x[i1] = x[i] - t1;
                        y[i1] = y[i] - t2;
                        x[i] += t1;
                        y[i] += t2;
                    }
                    z = u1 * c1 - u2 * c2;
                    u2 = u1 * c2 + u2 * c1;
                    u1 = z;
                }
                c2 = Math.Sqrt((1.0 - c1) / 2.0);
                if (dir == FFTDirection.Forward)
                    c2 = -c2;
                c1 = Math.Sqrt((1.0 + c1) / 2.0);
            }
            /* Scaling for reverse transform 
              Fabrice changed from Forward to Reverse on 14-05-2011
             */
            if (dir == FFTDirection.Reverse)
            {
                for (i = 0; i < nn; i++)
                {
                    x[i] /= (double)nn;
                    y[i] /= (double)nn;

                }
            }

            //  return(true) ;
            return;
        }

        #endregion

    }
}
