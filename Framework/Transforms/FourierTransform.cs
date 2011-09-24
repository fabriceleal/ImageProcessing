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
    /// Implementation of the fourier transform, for use by implemented filters or external use.
    /// </summary>
    /// <remarks>
    /// Set to use the FastFourierTransform implementation.
    /// </remarks>
    /// <see cref="FastFourierTransform"/>
    public class FourierTransform : TransformCoreBase<ComplexImage>
    {

        #region Attributes

        private TransformCoreBase<ComplexImage> fourierImpl;

        public FourierTransform()
        {
            fourierImpl = new FastFourierTransform();
        }

        #endregion

        #region TransformCoreBase ( double[,] )

        /// <summary>
        /// Apply the fourier transform on dataSpatialDomain.
        /// </summary>
        /// <param name="dataSpatialDomain">The data to transform</param>
        /// <returns>The fourier transfrom of dataSpatialDomain.</returns>
        public override ComplexImage ApplyTransformBase(double[,] dataSpatialDomain)
        {
            return fourierImpl.ApplyTransformBase(dataSpatialDomain);
        }

        /// <summary>
        /// Apply the inverse fourier transform on an image on the frequency domain.
        /// </summary>
        /// <param name="dataFrequencyDomain">The image in frequency domain.</param>
        /// <returns>The inverse fourier transform of dataFrequencyDomain.</returns>
        public override double[,] ApplyReverseTransformBase(ComplexImage dataFrequencyDomain)
        {
            return fourierImpl.ApplyReverseTransformBase(dataFrequencyDomain);
        }

        #endregion

        #region TransformCoreBase ( byte[,] )

        /// <summary>
        /// Apply the fourier transform on imageSpatialDomain.
        /// </summary>
        /// <param name="imageSpatialDomain">The image (greyscale) to transform.</param>
        /// <returns>The fourier transfrom of imageSpatialDomain.</returns>
        public override ComplexImage ApplyTransform(byte[,] imageSpatialDomain)
        {
            return fourierImpl.ApplyTransform(imageSpatialDomain);
        }

        /// <summary>
        /// Apply the inverse fourier transform on an image on the frequency domain.
        /// </summary>
        /// <param name="imageFrequencyDomain">The image in frequency domain.</param>
        /// <returns>The inverse fourier transform of imageFrequencyDomain.</returns>
        public override byte[,] ApplyReverseTransform(ComplexImage imageFrequencyDomain)
        {
            return fourierImpl.ApplyReverseTransform(imageFrequencyDomain);
        }

        #endregion

    }
}
