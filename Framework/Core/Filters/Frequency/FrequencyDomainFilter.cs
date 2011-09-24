using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;
using Framework.Core.Transforms;

namespace Framework.Core.Filters.Frequency
{
    /// <summary>
    /// Abstract class for implementing a filter in the frequency domain.
    /// </summary>
    /// <remarks>A frequency domain filter that
    /// inherits from this class should override the ApplyFilter(ComplexImage img, Dictionary configs)
    /// method. For further documentation, go to FilterCore.
    /// </remarks>
    /// <typeparam name="T">The type of the transform to use for converting the image to frequency domain 
    /// and back to spatial domain.</typeparam>
    public abstract class FrequencyDomainFilter<T> : FilterCore
        where T : TransformCoreBase<ComplexImage>
    {
        
        // Force conversion from byte[,] to ComplexImage

        /// <summary>
        /// Apply filter to a byte[,].
        /// </summary>
        /// <param name="img"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public override byte[,] ApplyFilter(byte[,] img, SortedDictionary<string, object> configs)
        {
            // Force transform 

            T transformer = Activator.CreateInstance<T>();

            ComplexImage complex = transformer.ApplyTransform(img);

            complex = ApplyFilter(complex, configs);

            return transformer.ApplyReverseTransform(complex);
        }

        //
        // ComplexImage ApplyFilter(ComplexImage img, Dictionary<string, object> configs)
        //  should be implemented (override) in the inherited class ...
        
    }
}
