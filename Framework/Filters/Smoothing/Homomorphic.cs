using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Core.Filters.Spatial;
using Framework.Core.Filters.Frequency;
using Framework.Transforms;
using Framework.Filters;
using Framework.Core.Filters.Base;
using Framework.Core.Metrics;
using Framework.Range;

namespace Framework.Filters.Smoothing
{
    /// <summary>
    /// Apply the homomorphic filter to an image.
    /// </summary>
    /// <remarks>
    /// This filter is implemented as a SpatialDomainFilter because it needs to 
    /// preprocess the data before performing the fourier transform
    /// </remarks>
    [Filter("Homomorphic", "Homomorphic Filter",
        "Apply the Homomorphic filter to a image. It uses a high and a low pass butterworth filters to separate " +
        "the reflectance and illuminance, respectively.",
        new string[] { "Smoothing" }, true)]
    [SmoothMetric]
    public class Homomorphic : SpatialDomainFilter
    {

        private Sharpening.Butterworth reflectGetter;
        private Smoothing.Frequency.Butterworth illuminGetter;
        
        /// <summary>
        /// Generate default configurations for the filter.
        /// </summary>
        /// <returns>Dictionary used to pass configuration though the 
        /// configs parameter of the method ApplyFilter. 
        /// null if there is the filter doesn't use configurations.</returns>
        public override SortedDictionary<string, object> GetDefaultConfigs()
        {
            SortedDictionary<string, object> ret = new SortedDictionary<string, object>();

            SortedDictionary<string, object> butter = reflectGetter.GetDefaultConfigs();

            ret.Add("Butterworth-D0", butter["D0"]);
            ret.Add("Butterworth-n", butter["n"]);
            ret.Add("Alpha", new Rangeable(1.0, 1.0, 2.0, 0.0001));
            ret.Add("Beta", new Rangeable(0.0001, 0.0, 1.0, 0.0001));


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
            byte[,] ret = null; FourierTransform ft = new FourierTransform();

            // Preprocess image.
            int width = img.GetLength(0), height = img.GetLength(1), x, y;
            double[,] imgProcessed = new double[width, height];

            const double magic = 0.5;

            //double min = double.MaxValue;
            //// Find min.
            //for (x = 0; x < width; ++x)
            //{
            //    for (y = 0; y < height; ++y)
            //    {
            //        min = Math.Min(img[x, y], min);
            //    }
            //}

            // "normalize"
            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    imgProcessed[x, y] = Math.Log(img[x, y] + magic, Math.E);
                }
            }

            // Take fourier transform.
            ComplexImage homomorph = ft.ApplyTransformBase(imgProcessed);


            // (x)(a) Perform butterworth high pass to select reflectance;
            // perform butterworth low pass to select illuminance.
            // **** OR ****
            // (b) Apply butterworth-generated weigth to select reflectance (1.0 in center)
            // Apply (1.0 - butterworth-generated weigth) to select illuminance (0.0 in center)
            // **** OR ****
            // (c) Apply high pass buttwerworth. only.


            SortedDictionary<string, object> hConfigs = reflectGetter.GetDefaultConfigs();
            hConfigs["D0"] = configs["Butterworth-D0"];
            hConfigs["n"] = configs["Butterworth-n"];

            ComplexImage reflect = reflectGetter.ApplyFilter(homomorph, hConfigs);
            ComplexImage illumin = illuminGetter.ApplyFilter(homomorph, hConfigs);

            // (1, 1) = imagem original
            // (1, 0) = high pass
            // (0, 1) = low pass
            double alpha = double.Parse(configs["Alpha"].ToString());
            double beta = double.Parse(configs["Beta"].ToString());


            // Generate new, Homomorphic-treated image

            //(c)
            //homomorph = illumin;

            // Split into reflectance and illuminance
            // - Sharp reflectance, smooth illuminance
            // - sum
            for (x = 0; x < homomorph.Width; ++x)
            {
                for (y = 0; y < homomorph.Height; ++y)
                {
                    //(a)
                    homomorph[x, y] = new Complex(
                            reflect[x, y].real * beta + // Reflectance part
                            illumin[x, y].real * alpha  // Illuminance part
                            ,
                            reflect[x, y].imag * beta + // Reflectance part
                            illumin[x, y].imag * alpha  // Illuminance part
                            );
                }
            }

            // ... back to spatial domain.
            imgProcessed = ft.ApplyReverseTransformBase(homomorph);

            // Pos-process image.
            ret = new byte[width, height];

            for (x = 0; x < width; ++x)
            {
                for (y = 0; y < height; ++y)
                {
                    ret[x, y] = (byte)Math.Max(byte.MinValue, Math.Min(byte.MaxValue, Math.Exp(imgProcessed[x, y]) - magic));
                }
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        public Homomorphic()
        {
            reflectGetter = new Sharpening.Butterworth();
            illuminGetter = new Frequency.Butterworth();
        }
    }
}
