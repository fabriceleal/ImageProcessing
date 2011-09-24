using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Core;
using Framework.Core.Filters.Base;
using Framework.Core.Filters.Spatial;
using Framework.Filters.Smoothing;
using Framework.Core.Metrics;
using Framework.Range;

namespace Framework.Filters.Simple
{

    /// <summary>
    /// Apply the hystersis operation to an image.
    /// </summary>
    /// <remarks>
    /// The image intensities will be splitted in "strong" (intensities higher than a Threeshoold-High) 
    /// and "weak" edges (intensities between Threeshoold-Low and Threeshoold-High); any "strong" edge will be included
    /// in the final image, but a "weak" edge will only be included if it links with a "strong" edge.
    /// </remarks>
    [Filter("Hystersis", "Hystersis filter",
        "The image intensities will be splitted in \"strong\" (intensities higher than a Threeshoold-High) " +
        "and \"weak\" edges (intensities between Threeshoold-Low and Threeshoold-High); any \"strong\" edge will be included" +
        "in the final image, but a \"weak\" edge will only be included if it links with a \"strong\" edge.",
        new string[] { "Simple" }, true)]
    [AllMetricAttribute()]
    public class Hystersis : SpatialDomainFilter
    {

        /// <summary>
        /// Generate default configurations for the filter.
        /// </summary>
        /// <returns>Dictionary used to pass configuration though the 
        /// configs parameter of the method ApplyFilter. 
        /// null if there is the filter doesn't use configurations.</returns>
        public override SortedDictionary<string, object> GetDefaultConfigs()
        {
            SortedDictionary<string, object> ret = new SortedDictionary<string, object>();

            ret.Add("Threeshoold-Low", Rangeable.ForByte(5));
            ret.Add("Threeshoold-High", Rangeable.ForByte(15));

            return ret;
        }

        /// <summary>
        /// Apply the filter to a byte[,].
        /// </summary>
        /// <param name="img"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public override byte[,] ApplyFilter(byte[,] img, SortedDictionary<string, object> configs)
        {
            // (1) Double threshoold
            byte t_low = byte.Parse(configs["Threeshoold-Low"].ToString());
            byte t_high = byte.Parse(configs["Threeshoold-High"].ToString());
            int width = img.GetLength(0), height = img.GetLength(1), i;

            byte[,] strong = new byte[width, height];

            Stack<Point> strong_points = new Stack<Point>();

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    if (img[x, y] >= t_high)
                    {
                        img[x, y] = 255;
                        strong[x, y] = 255;

                        strong_points.Push(new Point(x, y));
                    }
                    else if (img[x, y] >= t_low)
                    {
                        img[x, y] = 124;
                    }
                    else
                    {
                        img[x, y] = 0;
                    }
                }
            }


            // (2) Edge tracking by hystersis.
            // Strong edges will be interpreted as "certain edges", and will be included in the final image.
            // Weak edges will be included if they are connected to a strong edge ...

            // From a strong point, try to find weak points ...
            Point wrk; Point[] ngb;
            while (strong_points.Count > 0)
            {
                wrk = strong_points.Pop();

                ngb = GetNeighborhood(wrk.X, wrk.Y, width, height, 360);
                for (i = 0; i < ngb.Length; ++i)
                {
                    // if neighbor already is 255, is already on strong_points,
                    // if neighbor has 124, set to 255 and had to strong_points
                    if (img[wrk.X, wrk.Y] == 124)
                    {
                        img[wrk.X, wrk.Y] = 255; // so is not processed again ...
                        strong[wrk.X, wrk.Y] = 255;
                        strong_points.Push(new Point(wrk.X, wrk.Y));
                    }
                }
            }
            // --

            return strong;
        }

        /// <summary>
        /// Generates list of points reachable in a 3x3 neighborhood. Ignores out of bounds points.
        /// </summary>
        /// <param name="x">The center x coordinate.</param>
        /// <param name="y">The center y coordinate.</param>
        /// <param name="w">The width of the image.</param>
        /// <param name="h">The height of the image.</param>
        /// <param name="dir"></param>
        /// <returns>3x3 Neighboord of the pixel (x,y)</returns>
        private Point[] GetNeighborhood(int x, int y, int w, int h, double dir)
        {
            List<Point> neigh = new List<Point>();

            if (0 == dir)
            {
                if (y - 1 >= 0)
                {
                    neigh.Add(new Point(x, y - 1));
                }
                if (y + 1 < h)
                {
                    neigh.Add(new Point(x, y + 1));
                }
            }
            else if (45 == dir)
            {
                if (y - 1 >= 0 && x - 1 >= 0)
                {
                    neigh.Add(new Point(x - 1, y - 1));
                }
                if (y + 1 < h && x + 1 < w)
                {
                    neigh.Add(new Point(x + 1, y + 1));
                }
            }
            else if (90 == dir)
            {
                if (x - 1 >= 0)
                {
                    neigh.Add(new Point(x - 1, y));
                }
                if (x + 1 < w)
                {
                    neigh.Add(new Point(x + 1, y));
                }
            }
            else if (135 == dir)
            {
                if (x - 1 >= 0 && y + 1 < w)
                {
                    neigh.Add(new Point(x - 1, y + 1));
                }
                if (x + 1 < h && y - 1 >= 0)
                {
                    neigh.Add(new Point(x + 1, y - 1));
                }
            }
            else if (360 == dir)
            {
                // ALL DIRECTIONS ...
                if (x - 1 >= 0)
                {
                    neigh.Add(new Point(x - 1, y));
                    if (y - 1 >= 0)
                    {
                        neigh.Add(new Point(x - 1, y - 1));
                    }
                    if (y < h)
                    {
                        neigh.Add(new Point(x - 1, y + 1));
                    }
                }
                if (x + 1 < h)
                {
                    neigh.Add(new Point(x + 1, y));
                    if (y - 1 >= 0)
                    {
                        neigh.Add(new Point(x + 1, y - 1));
                    }
                    if (y < h)
                    {
                        neigh.Add(new Point(x + 1, y + 1));
                    }
                }
                if (y - 1 >= 0)
                {
                    neigh.Add(new Point(x, y - 1));
                }
                if (y < h)
                {
                    neigh.Add(new Point(x, y + 1));

                }
            }

            return neigh.ToArray();
        }


    }

}
