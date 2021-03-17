using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2020プログラム
{
    class Voxel
    {
        double[] range_X = new double[2];
        double[] range_Y = new double[2];
        double[] range_Z = new double[2];

        //メッシュのサイズ
        double stride_X = 0.02;
        double stride_Y = 0.02;
        double stride_Z = 0.02;

        double x_min, y_min, z_min;
        
        //面方向判定
        int mesh_direct;
        //点群をボクセルごとに記録
        public List<Points> voxel_points = new List<Points>();

        public Voxel()
        {
        }

        public Voxel(double[] X, double[] Y)
        {
            range_X = X;
            range_Y = Y;
        }
        
        //メッシュの作成
        public Voxel[,] Mesh(double[] XYZ)
        {
            //xyz方向の長さ
            double length_X = XYZ[0] - XYZ[1];
            double length_Y = XYZ[2] - XYZ[3];
            double length_Z = XYZ[4] - XYZ[5];

            x_min = XYZ[1];
            y_min = XYZ[3];
            z_min = XYZ[5];

            //面方向判定
            //Mesh_Direct(XYZ);
            mesh_direct = 1;
            //xyz方向メッシュの個数
            int num_X, num_Y, num_Z;
            if (length_X % stride_X != 0)
                num_X = (int)(length_X / stride_X) + 1;
            else
                num_X = (int)(length_X / stride_X);
            if (length_Y % stride_Y != 0)
                num_Y = (int)(length_Y / stride_Y) + 1;
            else
                num_Y = (int)(length_Y / stride_Y);
            if (length_Z % stride_Z != 0)
                num_Z = (int)(length_Z / stride_Z) + 1;
            else
                num_Z = (int)(length_Z / stride_Z);

            int index, index_X, index_Y, index_Z;
            index = index_X = index_Y = index_Z = 0;

            Voxel[,] voxels = new Voxel[0, 0];

            switch (mesh_direct)
            {
                case 0:
                    {
                        //YZメッシュ分割
                        voxels = new Voxel[num_Y, num_Z];

                        for (double i = XYZ[2]; i > y_min; i -= stride_Y)
                        {
                            for (double k = z_min; k < XYZ[4]; k += stride_Z)
                            {
                                range_Y = new double[2] { i - stride_Y, i };
                                range_Z = new double[2] { k, k + stride_Z };
                                voxels[index_Y, index_Z] = new Voxel(range_Y, range_Z);

                                index++;
                                index_Z++;
                            }

                            index_Y++;
                            index_Z = 0;
                        }
                    }
                    break;

                case 1:
                    {
                        //XZメッシュ分割
                        voxels = new Voxel[num_X, num_Z];

                        for (double i = XYZ[0]; i > x_min; i -= stride_X)
                        {
                            for (double k = z_min; k < XYZ[4]; k += stride_Z)
                            {
                                range_X = new double[2] { i - stride_X, i };
                                range_Z = new double[2] { k, k + stride_Z };
                                voxels[index_X, index_Z] = new Voxel(range_X, range_Z);

                                index++;
                                index_Z++;
                            }

                            index_X++;
                            index_Z = 0;
                        }
                    }
                    break;

                case 2:
                    {
                        //XYメッシュ分割
                        voxels = new Voxel[num_X, num_Y];

                        for (double i = XYZ[0]; i > x_min; i -= stride_X)
                        {
                            for (double k = y_min; k < XYZ[2]; k += stride_Y)
                            {
                                range_X = new double[2] { i - stride_X, i };
                                range_Y = new double[2] { k, k + stride_Y };
                                voxels[index_X, index_Y] = new Voxel(range_X, range_Y);

                                index++;
                                index_Y++;
                            }

                            index_X++;
                            index_Y = 0;
                        }
                        break;
                    }

            }
            
            return voxels;
        }

        //メッシュに点をいれる
        public void MakeVoxel(Voxel[,] voxels, Points point)
        {
            switch (mesh_direct) 
            {
                case 0:
                    {
                        double p_y = point.Y - y_min;
                        double p_z = point.Z - z_min;

                        int index_y = (int)(p_y / stride_X);
                        int index_z = (int)(p_z / stride_Y);

                        voxels[index_y, index_z].voxel_points.Add(point);
                    }
                    break;
                case 1:
                    {
                        double p_x = point.X - x_min;
                        double p_z = point.Z - z_min;

                        int index_x = (int)(p_x / stride_X);
                        int index_z = (int)(p_z / stride_Y);

                        voxels[index_x, index_z].voxel_points.Add(point);
                    }
                    break;
                case 2:
                    {
                        double p_x = point.X - x_min;
                        double p_y = point.Y - y_min;

                        int index_x = (int)(p_x / stride_X);
                        int index_y = (int)(p_y / stride_Y);

                        voxels[index_x, index_y].voxel_points.Add(point);
                    }
                    break;
            }
        }

        //メッシュの平均値を求め
        public void Voxel_aver(Voxel[,] voxels, File_Operation file) 
        {
            for (int i = 0; i < voxels.GetLength(0); i++) 
            {
                for (int j = 0; j < voxels.GetLength(1); j++) 
                {
                    if (voxels[i, j].voxel_points.Count() != 0) 
                    {
                        switch (mesh_direct)
                        {
                            case 0:
                                {
                                    double x_sum = 0;

                                    foreach (var n in voxels[i, j].voxel_points)
                                    {
                                        x_sum += n.X;
                                    }

                                    double x_aver = x_sum / voxels[i, j].voxel_points.Count();

                                    voxels[i, j].voxel_points.Clear();
                                    Points point_aver = new Points(x_aver, file.XYZ[3] + i * stride_Y + stride_Y/2, file.XYZ[5] + j * stride_Z + stride_Z / 2);
                                    voxels[i, j].voxel_points.Add(point_aver);
                                }
                                break;
                            case 1:
                                {
                                    double y_sum = 0;

                                    foreach (var n in voxels[i, j].voxel_points)
                                    {
                                        y_sum += n.Y;
                                    }

                                    double y_aver = y_sum / voxels[i, j].voxel_points.Count();

                                    voxels[i, j].voxel_points.Clear();
                                    Points point_aver = new Points(file.XYZ[1] + i * stride_X + stride_X / 2, y_aver, file.XYZ[5] + j * stride_Z + stride_Z / 2);
                                    voxels[i, j].voxel_points.Add(point_aver);
                                }
                                break;

                            case 2:
                                {
                                    double z_sum = 0;

                                    foreach (var n in voxels[i, j].voxel_points)
                                    {
                                        z_sum += n.Z;
                                    }

                                    double z_aver = z_sum / voxels[i, j].voxel_points.Count();

                                    voxels[i, j].voxel_points.Clear();
                                    Points point_aver = new Points(file.XYZ[1] + i * stride_X + stride_X / 2, file.XYZ[3] + j * stride_Y + stride_Y / 2, z_aver);
                                    voxels[i, j].voxel_points.Add(point_aver);
                                }
                                break;

                        }
                    }
                       
                }
            }
        }

        //メッシュの最大点数と最小点数及び平均点数
        public int point_min , point_max, point_aver, voxel_quantity;
        
        public void Voxel_point(Voxel[,] voxels) 
        {
            point_min = 0; point_max = 0; point_aver = 0; voxel_quantity = 0;
            int voxel_aver = 0;

            for (int i = 0; i < voxels.GetLength(0); i++)
            {
                for (int j = 0; j < voxels.GetLength(1); j++) 
                {
                    if(voxels[i,j].voxel_points.Count() != 0) 
                    {
                        if (voxel_quantity == 0) 
                        {
                            point_min = voxels[i, j].voxel_points.Count();
                            point_max = voxels[i, j].voxel_points.Count();
                        }
                        voxel_aver += voxels[i, j].voxel_points.Count();
                        voxel_quantity++;
                        if (point_min > voxels[i, j].voxel_points.Count())
                            point_min = voxels[i, j].voxel_points.Count();
                        else if (point_max < voxels[i, j].voxel_points.Count())
                            point_max = voxels[i, j].voxel_points.Count();
                    }
                }
            }
            point_aver = voxel_aver / voxel_quantity; 
        }

        private void Mesh_Direct(double[] XYZ) 
        {
            mesh_direct = 0; //面方向判定
            double X = XYZ[0] - XYZ[1];
            double Y = XYZ[2] - XYZ[3];
            double Z = XYZ[4] - XYZ[5];
            if (X < Y && X < Z)
            {
                mesh_direct = 0;
                Console.WriteLine("YZ平面");
            }
            else if (Y < X && Y < Z)
            {
                mesh_direct = 1;
                Console.WriteLine("XZ平面");
            }
            else if (Z < X && Z < Y)
            {
                mesh_direct = 2;
                Console.WriteLine("XY平面");
            }
        }
    }
}
