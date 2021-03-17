using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2020プログラム
{
    class Surface
    {
        public double[] newJiaoPoint;
       
        public void Vector_surface(double[] p1, double[] p2, double[] p3)
        {
            newJiaoPoint = new double[4];
            //平面方程Ax+BY+CZ+d=0 行列式计算
            double a = (p3[1] - p1[1]) * (p3[2] - p1[2]) - ((p2[2] - p1[2]) * (p3[1] - p1[1]));

            double b = (p2[2] - p1[2]) * (p3[0] - p1[0]) - ((p2[0] - p1[0]) * (p3[2] - p1[2]));

            double c = (p2[0] - p1[0]) * (p3[1] - p1[1]) - ((p2[1] - p1[1]) * (p3[0] - p1[0]));

            double d = -(a * p1[0] + b * p1[1] + c * p1[2]);

            newJiaoPoint[0] = a;
            newJiaoPoint[1] = b;
            newJiaoPoint[2] = c;
            newJiaoPoint[3] = d;

        }

        //点と面の距離
        public void Distance(Points[] point) 
        {
            for (int i = 0; i < point.Length; i++)
            {
                double S = Math.Abs(newJiaoPoint[0] * point[i].X + newJiaoPoint[1] * point[i].Y + newJiaoPoint[2] * point[i].Z + newJiaoPoint[3]) / Math.Sqrt(newJiaoPoint[0] * newJiaoPoint[0] + newJiaoPoint[1] * newJiaoPoint[1] + newJiaoPoint[2] * newJiaoPoint[2]);
                point[i].N = S;
            }
           
        }
        //Y座標と面の距離
        public void Distance_Z(Points[] point)
        {
            for (int i = 0; i < point.Length; i++) 
            {
                double S = (-newJiaoPoint[3] - point[i].X * newJiaoPoint[0] - point[i].Z * newJiaoPoint[2]) / newJiaoPoint[1];
                point[i].N = Math.Abs(point[i].Y) - Math.Abs(S);
            }
            
        }

        public void regi(Voxel[,] voxel) 
        {
            double S;
            for (int m = 0; m < voxel.GetLength(0); m++)
            {
                for (int n = 0; n < voxel.GetLength(1); n++)
                {
                    if (voxel[m, n].voxel_points.Count() != 0)
                    {
                        S = (-newJiaoPoint[3] - voxel[m, n].voxel_points[0].X * newJiaoPoint[0] - voxel[m, n].voxel_points[0].Z * newJiaoPoint[2]) / newJiaoPoint[1];
                        voxel[m, n].voxel_points[0].Y -= (voxel[m, n].voxel_points[0].Y - S); 
                    }
                }
            }
            
        }

        public void heikinn(Points[] points, Points p) 
        {
            double X = 0, Y = 0, Z = 0;
            for (int i = 0; i < points.Length; i++) 
            {
                X += points[i].X;
                Y += points[i].Y;
                Z += points[i].Z;
            }
            p.X = X / points.Length;
            p.Y = Y / points.Length;
            p.Z = Z / points.Length;
        }
    }
}
