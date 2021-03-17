using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2020プログラム
{
    class Points
    {
        public double X;
        public double Y;
        public double Z;
        public double N;
        public double C;　//傾斜度

        public Points()
        {
        }
        public Points(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Points(double x, double y, double z, double n)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.N = n;
        }
        //選択された平面のX Y,Z座標の最大値と最小値を求める
        public double[] PointExtract(List<double[]> list)
        {
            double[] XYZ = new double[6];
            for (int i = 0; i <= 2; i++)
            {

                double max = list[0][i], min = list[0][i];
                foreach (double[] a in list)
                {
                    if (max < a[i])
                    {
                        max = a[i];
                    }
                    else if (min > a[i])
                    {
                        min = a[i];
                    }
                }
                switch (i)
                {
                    case 0:
                        XYZ[0] = max;
                        XYZ[1] = min;
                        break;
                    case 1:
                        XYZ[2] = max;
                        XYZ[3] = min;
                        break;
                    case 2:
                        XYZ[4] = max;
                        XYZ[5] = min;
                        break;
                }

            }
            return XYZ;
        }

    }
}
