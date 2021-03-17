using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2020プログラム
{
    class Clustering
    {
        public bool Clustering1(int i, int j, Voxel[,] voxels)
        {
            double a = 0, b = 0, c = 0, d = 0;
            try
            {
                if (voxels[i + 1, j - 1].voxel_points.Count != 0 )
                {
                    a = voxels[i + 1, j - 1].voxel_points[0].N;
                }
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                if (voxels[i, j + 1].voxel_points.Count != 0 )
                {
                    b = voxels[i, j + 1].voxel_points[0].N;
                }
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                if (voxels[i + 1, j + 1].voxel_points.Count != 0 )
                {
                    c = voxels[i + 1, j + 1].voxel_points[0].N;
                }
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                if (voxels[i + 1, j].voxel_points.Count != 0 )
                {
                    d = voxels[i + 1, j].voxel_points[0].N;
                }
            }
            catch (IndexOutOfRangeException)
            {
            }
            if (a == b && b == c && c == d)
            {
                return true;
            }
            return false;
        }



        public void Clustering2(int i, int j, Voxel[,] voxels, List<double> ppccll)
        {
            List<double> point = new List<double>();
            try
            {
                if (voxels[i + 1, j - 1].voxel_points.Count != 0 && voxels[i + 1, j - 1].voxel_points[0].N != 0)
                {
                    point.Add(voxels[i + 1, j - 1].voxel_points[0].N);
                }
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                if (voxels[i, j + 1].voxel_points.Count != 0 && voxels[i, j + 1].voxel_points[0].N != 0)
                {
                    point.Add(voxels[i, j + 1].voxel_points[0].N);
                }
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                if (voxels[i + 1, j + 1].voxel_points.Count != 0  && voxels[i + 1, j + 1].voxel_points[0].N != 0)
                {
                    point.Add(voxels[i + 1, j + 1].voxel_points[0].N);
                }
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                if (voxels[i + 1, j].voxel_points.Count != 0 && voxels[i + 1, j].voxel_points[0].N != 0)
                {
                    point.Add(voxels[i + 1, j].voxel_points[0].N);
                }
            }
            catch (IndexOutOfRangeException)
            {
            }
            if (point.Count != 0)
            {
                point.Sort();
                voxels[i, j].voxel_points[0].N = point[0];
                for (int pl = 1; pl < point.Count; pl++)
                {
                    for (int pll = 0; pll < ppccll.Count; pll++)
                    {
                        if (ppccll[pll] == point[pl])
                        {
                            ppccll[pll] = point[0];
                        }
                    }
                }
            }

        }
    }
}
