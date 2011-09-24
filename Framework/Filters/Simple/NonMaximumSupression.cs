using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Core.Filters.Spatial;
using Framework.Transforms;
using Framework.Filters;
using Framework.Core.Filters.Base;
using Framework.Core.Metrics;
using Framework.Range;
using System.Drawing;

namespace Framework.Filters.Simple
{

    /// <summary>
    /// Apply non maximum supression to an image.
    /// </summary>
    /// <remarks>
    /// Set to 0 any pixel that is not a local maximum in a certain neighborhood. Uses Sobel kernels to calculate
    /// edge gradient in the x and y axes; it then performs the non maximum supression in the neighborhood
    /// of the edge direction.
    /// </remarks>
    [Filter("Non-Maximum Supression", "Non-Maximum Supression Filter",
        "Set to 0 any pixel that is not a local maximum in a certain neighborhood. " +
        "Uses Sobel kernels to calculate edge gradient in the x and y axes; " +
        "it then performs the non maximum supression in the neighborhood " +
        "of the edge direction.",
        new string[] { "Simple" }, true)]
    public class NonMaximumSupression : SpatialDomainFilter
    {

        /// <summary>
        /// Apply filter to a byte[,].
        /// </summary>
        /// <param name="img"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public override byte[,] ApplyFilter(byte[,] img, SortedDictionary<string, object> configs)
        {
            // (1) Take gradient of image - Sobel operator. 
            // (Compute and take separately the x and y componentes into Gx and Gy)            
            Kernel2D kernel_x = new Kernel2D(0, (1 / 4.0),
                    new double[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } });
            Kernel2D kernel_y = new Kernel2D(0, (1 / 4.0),
                    new double[,] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } });

            int width = img.GetLength(0), height = img.GetLength(1);
            double[,] edges_dir = new double[width, height];
            byte[,] step2 = new byte[width, height];

            const double degree_factor = 180.0 / Math.PI;

            Tuple<Point, Point>[] mappings; double g_x, g_y, dummy;
            int i;

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    // Get neighbors to parse
                    mappings = kernel_x.ResolvePositions(x, y, width, height);
                    g_x = 0; g_y = 0;

                    // Calculate convolution
                    for (i = 0; i < mappings.Length; ++i)
                    {
                        if (mappings[i].Item2.X != -1 && mappings[i].Item2.Y != -1)
                        {
                            g_x += img[mappings[i].Item2.X, mappings[i].Item2.Y] * kernel_x.Matrix[mappings[i].Item1.X, mappings[i].Item1.Y];
                            g_y += img[mappings[i].Item2.X, mappings[i].Item2.Y] * kernel_y.Matrix[mappings[i].Item1.X, mappings[i].Item1.Y];
                        }
                    }

                    g_x = g_x * kernel_x.Multiplier + kernel_x.Threeshold;
                    g_y = g_y * kernel_y.Multiplier + kernel_y.Threeshold;

                    step2[x, y] = (byte)Math.Sqrt(g_x * g_x + g_y * g_y);

                    // (2) Calculate edge direction, using Gx and Gy

                    if (0 == g_x)
                    {
                        edges_dir[x, y] = (0 == g_y ? 0 : 90);
                    }
                    else
                    {
                        // need to convert in degree
                        edges_dir[x, y] = Math.Atan(g_y / g_x) * degree_factor;
                    }

                    // (3) Relate edge direction to a direction that can be traced in the image (0º, 45º, 90º, 135º)
                    dummy = edges_dir[x, y];

                    // [0;22.5] e [157.5;180]  = 0
                    // [22.5;67.5] = 45
                    // [67.5;112.5] = 90
                    // [112.5;157.5] = 135

                    //if (dummy > 180)
                    //    throw new Exception(string.Format("Theta = {0}", dummy));

                    if (dummy >= 157.5 || dummy < 22.5)
                    {
                        edges_dir[x, y] = 0;
                    }
                    else if (dummy >= 112.5)
                    {
                        edges_dir[x, y] = 135;
                    }
                    else if (dummy >= 67.5)
                    {
                        edges_dir[x, y] = 90;
                    }
                    else if (dummy >= 22.5)
                    {
                        edges_dir[x, y] = 45;
                    }
                }
            }
            mappings = null;

            // (4) Apply non maximum suppression. Set to 0 any pixel that is not a local maximum;
            // compare with neighbourhood using edge direction ...
            //
            // for point (x,y) test:
            // dir = 0, (x, y-1) and (x, y+1)
            // dir = 45, (x+1, y+1) and (x-1, y-1)
            // dir = 90, (x-1, y) and (x+1, y)
            // dir = 135, (x+1, y-1) and (x-1, y+1)

            Point[] neighborhood;

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    dummy = edges_dir[x, y];

                    // Ignore, PLEASE, out of bound values ...
                    neighborhood = GetNeighborhood(x, y, width, height, dummy);

                    i = 0;
                    while (step2[x, y] != 0 && i < neighborhood.Length)
                    {
                        if (step2[x, y] < step2[neighborhood[i].X, neighborhood[i].Y])
                        {
                            step2[x, y] = 0;
                        }
                        ++i;
                    }

                }
            }

            return step2;
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
