using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;

namespace Framework.Filters.Simple
{
    /// <summary>
    /// Converts an image from a "colored-RGB" model to a "greyscale-RGB" model.
    /// </summary>
    /// <remarks>
    /// </remarks>
    [Filter("Greyscale", "Greyscale filter",
            "Converts a RGB color image to RGB greyscale", new string[] { "Simple" }, true)]
    public class Greyscale : SpatialDomainFilter
    {

        #region ImageFilterCore

        /// <summary>
        /// Apply filter to an Image.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public override Image ApplyFilter(Image img, SortedDictionary<string, object> configs)
        {
            byte[,] greyscales = Facilities.To8bppGreyScale(
                   Facilities.ToBitmap(img));

            Bitmap tmp_ret2 = Facilities.ToBitmap(
                    Facilities.ToRGBGreyScale(greyscales));

            return Facilities.ToImage(tmp_ret2);
        }

        /// <summary>
        /// Apply filter to a Color[,].
        /// </summary>
        /// <param name="img"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public override Color[,] ApplyFilter(Color[,] img, SortedDictionary<string, object> configs)
        {
            return Facilities.ToRGBGreyScale(Facilities.To8bppGreyScale(img));
        }

        /// <summary>
        /// Apply filter to a byte[,].
        /// </summary>
        /// <param name="img"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public override byte[,] ApplyFilter(byte[,] img, SortedDictionary<string, object> configs)
        {
            return img;
        }

        #endregion

    }
}
