using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2020プログラム
{
    class Tool
    {
        // ボクセルに柱の切り取る
        public double[] Pillar_Vox_Avenger(double[] point, Voxel[,] voxel, int down, int up, int left, int right) 
        {
            List<Points> p = new List<Points>();
            for (int n = down; n < up; n++)
            {
                for (int i = left; i < right; i++)
                {
                    if (voxel[i, n].voxel_points.Count() != 0)
                    {
                        p.Add(voxel[i, n].voxel_points[0]);
                    }
                }
                
            }
            for (int i = 0; i < p.Count(); i++)
            {
                point[0] += p[i].X;
                point[1] += p[i].Y;
                point[2] += p[i].Z;
            }
            for (int i = 0; i < 3; i++)
            {
                point[i] /= p.Count();
            }
            return point;
        }
        //点で柱の切り取る
        public double[] Pillar_Avenger(double[] point, Points[] vs, double down, double up, double left, double right)
        {
            List<Points> p = new List<Points>();
            for (int n = 0; n < vs.Length; n++)
            {
                if (vs[n].Z > down && vs[n].Z < up && (vs[n].X > left && vs[n].X < right))
                {
                    p.Add(vs[n]);
                }

            }
            for (int i = 0; i < p.Count(); i++)
            {
                point[0] += p[i].X;
                point[1] += p[i].Y;
                point[2] += p[i].Z;
            }
            for (int i = 0; i < 3; i++)
            {
                point[i] /= p.Count();
            }
            return point;
        }
    }
}
