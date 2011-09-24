using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Framework.Core.Metrics;
using Framework.Core.Filters.Base;
using System.Globalization;

namespace Framework.Core
{
    /// <summary>
    /// Class to group functions to handle image parsing by the Framework. 
    /// </summary>
    /// <remarks>
    /// A image may be represented using one of 
    /// the following structures: System.Drawing.Image, System.Drawing.Bitmap, Color[,], byte[,],
    /// double[,] [0.0.. 1.0], int[,], bool[,] (false = Black, true = White) or String (base-64 encoded).
    /// </remarks>
    public class Facilities
    {

        /// <summary>
        /// Function signature for a metric function, associable to a filter.
        /// </summary>
        /// <param name="iImageSrc">The image before the execution of the filter, or a reference</param>
        /// <param name="iImageDest">The image after the execution of the filter</param>
        /// <returns>Value calculated; Range / meaning of counterdomain is unconstrained.</returns>
        public delegate double MetricDelegate(Image imageSrc, Image imageDest);

        // Functions to iterate through the pixels of a bitmap using either safe.
        // using either safe or unsafe code
        #region Iterators

        /// <summary>
        /// Signature of the callback to pass to the method Facilities.IterateBitmapRGB().
        /// </summary>
        /// <param name="b">Reference to the Bitmap being iterated.</param>
        /// <param name="red">Reference to the red component of the current pixel.</param>
        /// <param name="green">Reference to the green component of the current pixel.</param>
        /// <param name="blue">Reference to the blue component of the current pixel.</param>
        /// <param name="x">Column index of the current pixel.</param>
        /// <param name="y">Row index of the current pixel.</param>
        /// <returns>false to signal some error or to cancel the iteration; true otherwise.</returns>
        public delegate bool PixelRGBBitmapCallback(ref Bitmap b, ref byte red, ref byte green, ref byte blue, int x, int y);

        /// <summary>
        /// Signature of the unsafe callback to pass to the method Facilities.UnsafeIterateBitmap().
        /// </summary>
        /// <param name="bitmap">Reference to the Bitmap being iterated.</param>
        /// <param name="start">Pointer to the start of the buffer.</param>
        /// <param name="noffset">Offset to add to the row iterator after iterating by the columns to handle the stride issue.</param>
        /// <param name="height">The height of the bitmap.</param>
        /// <param name="width">The width of the bitmap. Doesn't consider the stride issue.</param>
        public unsafe delegate void BitmapByteArrayParserDelegate(ref Bitmap bitmap, byte* start, int noffset, int height, int width);

        /// <summary>
        /// Implementation of a generic "Bitmap iterator". Parsing of the "current pixel" is handled by the 
        /// function passed though the callback parameter. 
        /// </summary>
        /// <remarks>This function is implemented in both unsafe and safe code.
        /// </remarks>
        /// <param name="b">The Bitmap passed by reference.</param>
        /// <param name="callback">The function to call for each pixel to parse.</param>
        public static void IterateBitmapRGB(ref Bitmap b, PixelRGBBitmapCallback callback)
        {
            try
            {
#if UNSAFE
                unsafe
                {
                    UnsafeIterateBitmap(
                            ref b,
                            delegate(ref Bitmap bitmap, byte* start, int noffset, int height, int width)
                            {
                                for (int y = 0; y < height; ++y)
                                {
                                    for (int x = 0; x < width; ++x)
                                    {
                                        callback(ref bitmap, ref start[2], ref start[1], ref start[0], x, y);

                                        start += 3;
                                    }
                                    start += noffset;
                                }
                            });

                }
#else
                Color pixel;
                // SAFE MODE
                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < b.Width; ++x)
                    {
                        pixel = b.GetPixel(x, y);
                        byte red = pixel.R;
                        byte green = pixel.G;
                        byte blue = pixel.B;

                        callback(ref b, ref red, ref blue, ref green, x, y);

                        pixel = Color.FromArgb(red, green, blue);                        
                        b.SetPixel(x, y, pixel);
                    }
                }                         
#endif
            }
            catch (Exception e)
            {
                //Debug.Print("Facilities::IterateBitmap" + Environment.NewLine +
                //            "   Exception {0}: {1}", new object[] { e.GetType().FullName, e.Message });
                throw new Exception(string.Format("Error while reading Bitmap: {0}", e.Message), e);
            }
        }

        /// <summary>
        /// Unsafe implementation of the "Bitmap iterator". Gives the possibility to costumize the 
        /// looping through the bitmap buffer.
        /// </summary>
        /// <param name="b">The Bitmap passed by reference.</param>
        /// <param name="parseArray">The function to call to actually iterate through the buffer.</param>
        public static void UnsafeIterateBitmap(ref Bitmap b, BitmapByteArrayParserDelegate parseArray)
        {
#if UNSAFE
            try
            {
                // UNSAFE MODE
                BitmapData bmData = b.LockBits(
                        new Rectangle(0, 0, b.Width, b.Height),
                        ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                // Bitmap.Stride ------------------------------
                //  - aka stride width, scan width
                //
                // The memory allocated for Microsoft Bitmaps must be 
                // aligned on a 32bit boundary.
                //
                // The stride refers to the number of bytes allocated 
                // for one scanline of the bitmap
                //
                // Equals to ((width * bpp) + 7) / 8
                //
                int stride = bmData.Stride;
                System.IntPtr Scan0 = bmData.Scan0;
                unsafe
                {
                    byte* p = (byte*)(void*)Scan0;
                    int nOffset = stride - b.Width * 3;

                    parseArray(ref b, p, nOffset, b.Height, b.Width);
                }
                b.UnlockBits(bmData);
            }
            catch (Exception e)
            {
                //Debug.Print("Facilities::IterateBitmap" + Environment.NewLine +
                //            "   Exception {0}: {1}", new object[] { e.GetType().FullName, e.Message });
                throw new Exception(string.Format("Error while reading Bitmap using unsafe code: {0}", e.Message), e);
            }
#endif
        }

        #endregion

        // Functions to convert between "image-formats".
        #region Convert

        /// <summary>
        /// Converts from Image to Color[,].
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Color[,] ToColor(Image img)
        {
            Color[,] ret = null;

            ret = new Color[img.Width, img.Height];

            Bitmap bitmap = img as Bitmap;

            IterateBitmapRGB(
                    ref bitmap,
                    delegate(ref Bitmap bm, ref byte r, ref byte g, ref byte b, int x, int y)
                    {
                        try
                        {
                            ret[x, y] = Color.FromArgb(r, g, b);

                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    });
            // --

            return ret;
        }

        /// <summary>
        /// Converts from Bitmap to Image.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Image ToImage(Bitmap bitmap)
        {
            if (bitmap == null)
                return null;

            MemoryStream ms;
            try
            {
                ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Bmp);

                return Image.FromStream(ms);
            }
            catch (Exception e)
            {
                throw new Exception("Error saving Bitmap to Image.", e);
            }
        }

        /// <summary>
        /// Converts from Image to Bitmap.
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Bitmap ToBitmap(Image img)
        {
            if (img == null)
                return null;

            MemoryStream ms;
            try
            {
                ms = new MemoryStream();
                img.Save(ms, img.RawFormat);

                return new Bitmap(ms);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Error saving Image to Bitmap: {0}", e.Message), e);
            }
        }

        /// <summary>
        /// Converts from Image to a base-64 encoded string.
        /// </summary>
        /// <remarks>Used as "serialization".</remarks>
        /// <param name="img"></param>
        /// <returns></returns>
        public static string ToBase64String(Image img)
        {
            if (img == null)
                return string.Empty;

            MemoryStream ms;
            try
            {
                ms = new MemoryStream();
                img.Save(ms, img.RawFormat);

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error converting Image to base-64 encoded string.", ex), ex);
            }
        }

        /// <summary>
        /// Converts from a base-64 encoded string to Image.
        /// </summary>
        /// <remarks>Used as "deserialization".</remarks>
        /// <param name="base64image"></param>
        /// <returns></returns>
        public static Image ToImage(string base64image)
        {
            if (string.IsNullOrEmpty(base64image))
                return null;

            MemoryStream ms;
            try
            {
                ms = new MemoryStream();

                byte[] buffer = Convert.FromBase64String(base64image);
                ms.Write(buffer, 0, buffer.Count());

                return Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                throw new Exception( string.Format("Error converting base-64 encoded string to Image.", ex), ex);
            }
        }

        /// <summary>
        /// Converts from Color[,] to Bitmap.
        /// </summary>
        /// <param name="colors"></param>
        /// <returns></returns>
        public static Bitmap ToBitmap(Color[,] colors)
        {
            Bitmap ret = new Bitmap(colors.GetLength(0), colors.GetLength(1));

            IterateBitmapRGB(
                    ref ret,
                    delegate(ref Bitmap bm, ref byte r, ref byte g, ref byte b, int x, int y)
                    {
                        r = colors[x, y].R;
                        g = colors[x, y].G;
                        b = colors[x, y].B;

                        return true;
                    });

            return ret;
        }

        #region ToBitmap overloads for 2D arrays of primitive types
        // Is not possible to use generics here ...

        /// <summary>
        /// Converts from byte[,] to Bitmap.
        /// </summary>
        /// <param name="greyscale"></param>
        /// <returns></returns>
        public static Bitmap ToBitmap(byte[,] greyscale)
        {
            Bitmap ret = new Bitmap(greyscale.GetLength(0), greyscale.GetLength(1));
            byte normalized;

            IterateBitmapRGB(
                    ref ret,
                    delegate(ref Bitmap bm, ref byte r, ref byte g, ref byte b, int x, int y)
                    {
                        normalized = greyscale[x, y];
                        r = normalized;
                        g = normalized;
                        b = normalized;

                        return true;
                    });

            return ret;
        }

        /// <summary>
        /// Converts from int[,] to Bitmap.
        /// </summary>
        /// <param name="greyscale"></param>
        /// <returns></returns>
        public static Bitmap ToBitmap(int[,] greyscale)
        {
            Bitmap ret = new Bitmap(greyscale.GetLength(0), greyscale.GetLength(1));
            byte normalized;

            IterateBitmapRGB(
                    ref ret,
                    delegate(ref Bitmap bm, ref byte r, ref byte g, ref byte b, int x, int y)
                    {
                        normalized = (byte)Math.Max(byte.MinValue, Math.Min(byte.MaxValue, greyscale[x, y]));
                        r = normalized;
                        g = normalized;
                        b = normalized;

                        return true;
                    });

            return ret;
        }

        #endregion

        // For conversion / display only !!

        /// <summary>
        /// Converts from bitmap to RGB in greyscale.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Color[,] ToRGBGreyScale(Bitmap bitmap)
        {
            Color[,] ret = new Color[bitmap.Width, bitmap.Height];

            IterateBitmapRGB(
                    ref bitmap,
                    delegate(ref Bitmap bm, ref byte r, ref byte g, ref byte b, int x, int y)
                    {
                        int lum = (int)(r * 0.3 + g * 0.59 + b * 0.11);

                        ret[x, y] = Color.FromArgb(lum, lum, lum);

                        return true;
                    });

            return ret;
        }

        // For conversion / display only !!

        /// <summary>
        /// Converts from byte[,] to RGB in greyscale.
        /// </summary>
        /// <param name="greyscale"></param>
        /// <returns></returns>
        public static Color[,] ToRGBGreyScale(byte[,] greyscale)
        {
            int width = greyscale.GetLength(0), height = greyscale.GetLength(1);
            Color[,] ret = new Color[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    ret[x, y] = Color.FromArgb(greyscale[x, y], greyscale[x, y], greyscale[x, y]);
                }
            }

            return ret;
        }

        /// <summary>
        /// Converts from Bitmap to byte[,].
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[,] To8bppGreyScale(Bitmap bitmap)
        {
            if (bitmap == null)
                return null;

            byte[,] ret = new byte[bitmap.Width, bitmap.Height];
            int res;

            IterateBitmapRGB(
                    ref bitmap,
                    delegate(ref Bitmap bm, ref byte r, ref byte g, ref byte b, int x, int y)
                    {
                        res = (int)((r + g + b) / 3);

                        res = Math.Min(res, byte.MaxValue);
                        res = Math.Max(res, byte.MinValue);

                        ret[x, y] = (byte)res;

                        return true;
                    });

            return ret;
        }

        /// <summary>
        /// Converts from Color[,] to byte[,].
        /// </summary>
        /// <param name="colors"></param>
        /// <returns></returns>
        public static byte[,] To8bppGreyScale(Color[,] colors)
        {
            int width = colors.GetLength(0), height = colors.GetLength(1);
            byte[,] ret = new byte[width, height];
            int res;

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    res = (int)((colors[x, y].R + colors[x, y].G + colors[x, y].B) / 3);

                    res = Math.Min(res, byte.MaxValue);
                    res = Math.Max(res, byte.MinValue);

                    ret[x, y] = (byte)res;
                }
            }

            return ret;
        }

        /// <summary>
        /// Converts from byte[,] to double[,].
        /// </summary>
        /// <param name="greyscale"></param>
        /// <returns>double[,]; the values are normalized between [0.0 .. 0.1].</returns>
        public static double[,] ToDouble(byte[,] greyscale)
        {
            int width = greyscale.GetLength(0), height = greyscale.GetLength(1), x, y;
            double[,] ret = new double[width, height];
            double max_byte = (double)byte.MaxValue;
            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    ret[x, y] = greyscale[x, y] / max_byte;
                }
            }
            return ret;
        }

        /// <summary>
        /// Converts from bool[,] to byte[,]. 
        /// </summary>
        /// <param name="logical"></param>
        /// <returns>byte[,]; a value of true is equal to 255 and a value of false
        /// is equal to 0.</returns>
        public static byte[,] To8bppGreyScale(bool[,] logical)
        {
            int width = logical.GetLength(0), height = logical.GetLength(1), x, y;
            byte[,] ret = new byte[width, height];

            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    ret[x, y] = logical[x, y] ? byte.MaxValue : byte.MinValue;
                }
            }
            return ret;
        }

        /// <summary>
        /// Converts from double[,] to byte[,]
        /// </summary>
        /// <param name="grayscale">Values are assumed to be normalized between [0.0 .. 1.0].</param>
        /// <returns></returns>
        public static byte[,] To8bppGreyScale(double[,] grayscale)
        {
            int width = grayscale.GetLength(0), height = grayscale.GetLength(1), x, y;
            byte[,] ret = new byte[width, height];
            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    ret[x, y] = (byte)(grayscale[x, y] * byte.MaxValue);
                }
            }
            return ret;
        }

        #endregion

        // Implementation of some basic metrics for use in filters.
        #region Metrics

        /// <summary>
        /// Computes the Signal to Noise Ratio between 2 images, one assumed to be
        ///  "noisy", the other assumed to be "clean".
        /// </summary>
        /// <remarks>Sizes must match.
        /// </remarks>
        /// <param name="noisy">The "noisy" image.</param>
        /// <param name="clean">The "clean" image.</param>
        /// <returns>
        /// The Signal to Noise ratio, in decibels.
        /// </returns>
        [MetricAttribute("SignalToNoiseRatio", "SignalToNoiseRatio")]
        public static double SignalToNoiseRatio(Image imgNoisy, Image imgClean)
        {
            byte[,] noisy = To8bppGreyScale(imgNoisy as Bitmap);
            byte[,] clean = To8bppGreyScale(imgClean as Bitmap);


            double ret = 0.0;

            // Sizes must match !!!
            if (clean.GetLength(0) != clean.GetLength(0) ||
                    noisy.GetLength(1) != noisy.GetLength(1))
                throw new Exception(
                        string.Format(
                                "Image's sizes does not match! img0=({0},{1}) img1=({2},{3}) ",
                                clean.GetLength(0), clean.GetLength(1),
                                noisy.GetLength(0), noisy.GetLength(1)));

            int width = clean.GetLength(0); int height = clean.GetLength(1);

            // Translation of the following Matlab .m file:
            //
            // error_diff = A - B;
            // decibels = 20*log10(255/(sqrt(mean(mean(error_diff.^2)))));
            // disp(sprintf('SNR = +%5.2f dB',decibels))

            // temp calcs...

            double tot = 0;
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    tot += Math.Pow(clean[x, y] - noisy[x, y], 2);
                }
            }

            tot /= (width * height * 1.0); // Average
            tot = Math.Sqrt(tot);

            ret = 20 * Math.Log10(byte.MaxValue / tot);

            return ret;
        }

        /// <summary>
        /// Computes the Signal to Noise Ratio of an image, assumed to be "clean". 
        /// </summary>
        /// <param name="imgVoid">This parameter will be ignored.</param>
        /// <param name="imgClean">The "clean" image.</param>
        /// <returns>The Signal to Noise ratio, calculated as the average(imgClean) / stdDeviation(imgClean)</returns>
        [MetricAttribute("SignalToNoiseRatio Simple", "SignalToNoiseRatio Simple")]
        public static double SignalToNoiseRatioSimple(Image imgVoid, Image imgClean)
        {
            double tot = 0; double stdDev = 0; int x, y;
            byte[,] clean = To8bppGreyScale(imgClean as Bitmap);
            int width = clean.GetLength(0), height = clean.GetLength(1);

            // Calculate average
            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    tot += (double)clean[x, y];
                }
            }
            tot /= (width * height);

            // Calculate standard deviation
            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    stdDev += Math.Pow((double)clean[x, y] - tot, 2);
                }
            }

            stdDev /= (width * height);
            stdDev = Math.Sqrt(stdDev);

            return (tot / stdDev);
        }


        /// <summary>
        /// Counts the number of white bytes in image; reference will be ignored.
        /// </summary>
        /// <param name="reference">Ignored (pass null).</param>
        /// <param name="image">The image from which to extract the number of white bytes.</param>
        /// <returns></returns>
        [MetricAttribute("CountWhitebytesOutput", "CountWhitebytesOutput")]
        public static double CountWhitebytesOutput(Image voidImage, Image newImage)
        {
            if (newImage == null)
                return double.NegativeInfinity;

            byte[,] bytesNewImg = To8bppGreyScale(newImage as Bitmap);


            double ret = 0;

            int width = bytesNewImg.GetLength(0);
            int height = bytesNewImg.GetLength(1);

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    if (byte.MaxValue == bytesNewImg[x, y])
                    {
                        ++ret;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Counts the number of matching white bytes between a "reference" and an "image."
        /// </summary>
        /// <remarks>Sizes must match.</remarks>
        /// <param name="referenceImg">The "reference".</param>
        /// <param name="newImageImg">The "image".</param>
        /// <returns>Number of matching white bytes between the 2 images. 
        /// If any supplied image is null, returns double.NegativeInfinity.</returns>
        [MetricAttribute("CountErrorTolerant", "CountErrorTolerant")]
        public static double CountErrorTolerant(Image referenceImg, Image newImageImg)
        {
            if (referenceImg == null || newImageImg == null)
                return double.NegativeInfinity;

            byte[,] reference = Facilities.To8bppGreyScale(referenceImg as Bitmap);
            byte[,] newImage = Facilities.To8bppGreyScale(newImageImg as Bitmap);

            // Odd size
            const int SIZE = 5;

            // Initialize squared mask with 1.0
            double[,] mask = new double[SIZE, SIZE];
            for (int x = 0; x < SIZE; ++x)
            {
                for (int y = 0; y < SIZE; ++y)
                {
                    mask[x, y] = 1.0;
                }
            }

            int width = reference.GetLength(0), height = reference.GetLength(1);
            double error = 0;

            {
                int x0, y0, total0;
                Core.Filters.Spatial.Kernel2D k2 = new Filters.Spatial.Kernel2D(0, 1, mask);
                Core.Filters.Spatial.Kernel2DBatch.Evaluate(
                        reference,
                        new Filters.Spatial.Kernel2D[] { k2 },
                        delegate(Framework.Core.Filters.Spatial.Kernel2D k, byte[,] op, int x, int y)
                        {
                            Tuple<Point, Point>[] mappings = k.ResolvePositions(x, y, width, height);
                            total0 = 0;

                            for (int i = 0; i < mappings.Length; ++i)
                            {
                                x0 = mappings[i].Item2.X;
                                y0 = mappings[i].Item2.Y;
                                if (x0 != -1 && y0 != -1)
                                {
                                    total0 += reference[x0, y0] - newImage[x0, y0];
                                }
                            }

                            return Math.Abs(total0);
                        },
                        delegate(List<double> values)
                        {
                            error += values[0];

                            return 0;
                        });
            }

            return error;
            //return double.NegativeInfinity;
        }

        /// <summary>
        /// Counts the number of white bytes in reference; image will be ignored.
        /// </summary>
        /// <param name="reference">The image from which to extract the number of white bytes.</param>
        /// <param name="image">Ignored (pass null).</param>
        /// <returns></returns>
        [MetricAttribute("CountWhitebytesInput", "CountWhitebytesInput")]
        public static double CountWhitebytesInput(Image oldImage, Image voidImage)
        {
            return CountWhitebytesOutput(null, oldImage);
        }

        /// <summary>
        /// Counts the number of matching white bytes between a "reference" and an "image."
        /// </summary>
        /// <remarks>Sizes must match.</remarks>
        /// <param name="reference">The "reference".</param>
        /// <param name="image">The "image".</param>
        /// <returns>Number of matching white bytes between the 2 images. 
        /// If any supplied image is null, returns double.NegativeInfinity.</returns>
        [MetricAttribute("CountMatchWhitebytes", "CountMatchWhitebytes")]
        public static double CountMatchWhitebytes(Image referenceImg, Image newImageImg)
        {
            if (referenceImg == null || newImageImg == null)
                return double.NegativeInfinity;

            byte[,] reference = Facilities.To8bppGreyScale(referenceImg as Bitmap);
            byte[,] newImage = Facilities.To8bppGreyScale(newImageImg as Bitmap);


            double ret = 0;

            // Sizes must match !!!
            if (reference.GetLength(0) != newImage.GetLength(0) ||
                    reference.GetLength(1) != newImage.GetLength(1))
                throw new Exception(
                        string.Format(
                                "Image's sizes does not match! img0=({0},{1}) img1=({2},{3}) ",
                                reference.GetLength(0), reference.GetLength(1),
                                newImage.GetLength(0), newImage.GetLength(1)));


            int width = reference.GetLength(0); int height = reference.GetLength(1);

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    if (byte.MaxValue == reference[x, y] && byte.MaxValue == newImage[x, y])
                    {
                        ++ret;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Calculates the ratio between the matching white bytes between referenceImg and newImageImg
        /// and the number of white bytes in newImageImg
        /// </summary>
        /// <remarks>Sizes must match.</remarks>
        /// <param name="referenceImg">The "reference".</param>
        /// <param name="newImageImg">The "image".</param>
        /// <returns>Ratio of white bytes between the 2 images.</returns>
        [MetricAttribute("CountWhitebytesRatio", "CountWhitebytesRatio")]
        public static double CountMatchWhitebytesRatio(Image referenceImg, Image newImageImg)
        {
            double tot = 0.0;
            byte[,] reference = Facilities.To8bppGreyScale(referenceImg as Bitmap);
            byte[,] newImage = Facilities.To8bppGreyScale(newImageImg as Bitmap);

            // Check sizes ...
            if (reference.GetLength(0) != newImage.GetLength(0) ||
                reference.GetLength(1) != newImage.GetLength(1))
                throw new Exception(
                        string.Format(
                                "Image's sizes does not match! img0=({0},{1}) img1=({2},{3}) ",
                                reference.GetLength(0), reference.GetLength(1),
                                newImage.GetLength(0), newImage.GetLength(1)));

            //// Compute error 
            //int x, y;
            //for (x = 0; x < reference.GetLength(0); ++x)
            //{
            //    for (y = 0; y < reference.GetLength(1); ++y)
            //    {
            //        tot += Math.Abs((double)reference[x, y] - (double)image[x, y]);
            //    }
            //}

            //// --
            //return tot;
            double totWhites = CountWhitebytesOutput(null, referenceImg);
            double totWhitesDetected = CountWhitebytesOutput(null, newImageImg);
            if (totWhites != 0)
            {
                return totWhitesDetected - CountMatchWhitebytes(referenceImg, newImageImg) / totWhites;
            }

            // never gonna happen anyway
            return double.PositiveInfinity;
        }

        #endregion

        #region Image Custom Data
        // Associate custom data to an System.Drawing.Image instance using the System.Drawing.Image.Tag property
        // The tag will be set to a Hashtable; the custom values will be put under the key Facilities.PROJECT_FRAMEWORK_ROOT_DATA

        /// <summary>
        /// The name of the main bucket used to store Framework's data (buckets) in the Image's Tag property.
        /// </summary>
        public const string PROJECT_FRAMEWORK_ROOT_DATA = "PROJECT_FRAMEWORK_ROOT_DATA";

        /// <summary>
        /// The name of bucket used to store information about the executed filters in an Image.
        /// </summary>
        public const string EXECUTED_FILTERS = "EXECUTED_FILTERS";

        /// <summary>
        /// Returns a copy of the "tree" structure of the Tag property of the parameter img
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static object CloneTag(Image img)
        {
            if (null == img)
                return null;
            ArrayList root = GetBucket<ArrayList>(ref img, EXECUTED_FILTERS);
            if (null == root)
                return null;

            // Replicate structure
            Hashtable ret = new Hashtable();
            Hashtable executed = new Hashtable();
            ArrayList filters_executed = new ArrayList();
            ret.Add(PROJECT_FRAMEWORK_ROOT_DATA, executed);
            executed.Add(EXECUTED_FILTERS, filters_executed);

            foreach (object o in root)
            {
                filters_executed.Add(o);
            }

            return (object)ret;
        }


        /// <summary>
        /// Adds a filter execution to the System.Drawing.Image.Tag property.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="filter"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public static bool AddFilterExecution(
                ref Image img, FilterCore filter, SortedDictionary<string, object> configs)
        {
            if (img == null || filter == null)
                return false;
            ArrayList bucket = GetBucket<ArrayList>(ref img, EXECUTED_FILTERS);

            string dummy1, dummy2;
            bucket.Add(Facilities.FilterExecToString(filter.GetType(), configs, out dummy1, out dummy2));

            return true;
        }

        // Image custom information
        // Attach to an instance of image custom information

        /// <summary>
        /// Returns a bucket for storing Framework custom data in an System.Drawing.Image instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="img"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetBucket<T>(ref Image img, String key)
            where T : class, new()
        {
            Hashtable root = GetRootData(ref img, PROJECT_FRAMEWORK_ROOT_DATA);

            if (null != root)
            {
                if (!root.ContainsKey(key))
                {
                    root.Add(key, new T());
                }

                return root[key] as T;
            }
            return null;
        }

        /// <summary>
        /// Returns the "root" hashtable of the parameter img.
        /// </summary>
        /// <remarks>If the System.Drawing.Image.Tag property
        /// is set to null, it will create and return it. If the tag is already set to some other unexpected
        /// value, it will be ignored and will return true.</remarks>
        /// <param name="img"></param>
        /// <returns>true if the System.Drawing.Image.Tag property respects the assumed structure; null otherwise.</returns>
        private static Hashtable GetRoot(ref Image img)
        {
            if (null == img)
                return null;

            if (null == img.Tag)
            {
                Hashtable root = new Hashtable();
                Hashtable root_data = new Hashtable();

                root.Add(PROJECT_FRAMEWORK_ROOT_DATA, root_data);

                img.Tag = root;

                return root;
            }

            return img.Tag as Hashtable;
        }

        /// <summary>
        /// Returns the hashtable stored inside the root of the System.Drawing.Image.Tag property by key.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="key"></param>
        /// <returns>The instance of the hashtable if found; null otherwise.</returns>
        public static Hashtable GetRootData(ref Image img, String key)
        {
            Hashtable root = GetRoot(ref img);

            if (root == null)
                return null;

            if (root.ContainsKey(key))
                return root[key] as Hashtable;

            return null;
        }

        #endregion

        /// <summary>
        /// Auxiliary function to format a filter with it's parameters.
        /// </summary>
        /// <param name="filterType">The filter's class.</param>
        /// <param name="configs">The configurations of the filter.</param>
        /// <param name="filterStr">Will be overwritten with the filter's name.</param>
        /// <param name="configsStr">Will be overwritten with the filter's configurations, in { key1 = value1, ...} format.</param>
        /// <returns>Filter's name and Filter's configurations, in { key1 = value1, ...} format.</returns>
        public static string FilterExecToString(
                Type filterType, SortedDictionary<string, object> configs,
                out string filterStr, out string configsStr)
        {
            // Force the . as decimal separator
            NumberFormatInfo dblFormat = new NumberFormatInfo();
            dblFormat.NumberDecimalSeparator = ".";

            // Get filter
            filterStr = ""; configsStr = "";

            if (null != filterType)
            {
                filterStr = filterType.FullName;
            }

            // Get parameters to string ...
            if (null == configs)
            {
                configsStr = "";
            }
            else
            {
                if (configs.Count > 0)
                {
                    StringBuilder parBuff = new StringBuilder();
                    IEnumerator<KeyValuePair<string, object>> enumerator = configs.GetEnumerator();

                    enumerator.MoveNext();

                    parBuff.Append("{ ");
                    parBuff.Append(enumerator.Current.Key);
                    parBuff.Append(" = ");
                    parBuff.Append(double.Parse(enumerator.Current.Value.ToString()).ToString(dblFormat));

                    while (enumerator.MoveNext())
                    {
                        parBuff.Append("; ");
                        parBuff.Append(enumerator.Current.Key);
                        parBuff.Append(" = ");
                        parBuff.Append(double.Parse(enumerator.Current.Value.ToString()).ToString(dblFormat));
                    }

                    parBuff.Append(" }");

                    configsStr = parBuff.ToString();
                }
            }

            return (filterStr + " " + configsStr);
        }

    }
}
