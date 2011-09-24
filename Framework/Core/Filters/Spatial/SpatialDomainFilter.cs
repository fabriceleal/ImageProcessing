using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Frequency;

namespace Framework.Core.Filters.Spatial
{
    /// <summary>
    /// Abstract class for implementing a filter in the spatial domain.
    /// </summary>
    /// <remarks>A spatial domain filter that
    /// inherits from this class should override the ApplyFilter(byte[,] img, Dictionary configs)
    /// method. For further documentation, go to FilterCore.
    /// </remarks>
    public abstract class SpatialDomainFilter : FilterCore
    {
        // Dummy override

        /// <summary>
        /// Apply filter to a ComplexImage. This method will throw a NotImplementedException.
        /// </summary>
        /// <param name="complexImg"></param>
        /// <param name="configs"></param>
        /// <exception cref="NotImplementedException">Spatial Domain Filters do not operate on ComplexImage.</exception>
        /// <returns></returns>
        public override ComplexImage ApplyFilter(ComplexImage complexImg, SortedDictionary<string, object> configs)
        {
            // Throw exception ... to turn a ComplexImage into bytes, you have to know 
            // which forward-transform and reverse-transform to perform!
            throw new NotImplementedException("Spatial Domain Filters do not operate on ComplexImage. Apply the inverse transform first!");
        }

    }
}
