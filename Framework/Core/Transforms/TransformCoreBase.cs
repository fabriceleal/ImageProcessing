using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Core.Transforms
{
    /// <summary>
    /// Abstract class for implementing a transform. All transforms must inherit this class.
    /// The "domain" image-type of the transform.
    /// </summary>
    /// <typeparam name="DEST">The "counter-domain" image-type of the transform.</typeparam>
    public abstract class TransformCoreBase<DEST>
    {

        #region Methods


        /// <summary>
        /// Calculate the forward-transform on a byte[,] image.
        /// </summary>
        /// <param name="src"></param>
        /// <returns>The transform of src.</returns>
        public abstract DEST ApplyTransform(byte[,] src);

        /// <summary>
        /// Calculate the reverse-transform to a byte[,] image.
        /// </summary>
        /// <param name="dest"></param>
        /// <returns>The reverse-transform of dest.</returns>
        public abstract byte[,] ApplyReverseTransform(DEST dest);



        /// <summary>
        /// Calculate the forward-transform. This is the mathematical formulation of the transform.
        /// </summary>
        /// <param name="src">The data to perform the transform on.</param>
        /// <returns>The transform of src.</returns>
        public abstract DEST ApplyTransformBase(double[,] src);

        /// <summary>
        /// Calculate the reverse-transform. This is the mathematical formulation of the reverse-transform.
        /// </summary>
        /// <param name="dest"></param>
        /// <returns>The reverse-transform of dest.</returns>
        public abstract double[,] ApplyReverseTransformBase(DEST dest);

        #endregion

    }
}
