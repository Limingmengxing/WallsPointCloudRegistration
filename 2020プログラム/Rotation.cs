using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2020プログラム
{
    class Rotation
    {
        public void Calculation(Points[] point, double[] Before, double[] After)　//Z軸回りを回転、XOZ平面を平行ように
        {
            double x = After[0] - Before[0];
            double y = After[1] - Before[1];
            double r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            double cos = x / r;
            double sin = y / r;

            for (int i = 0; i < point.Length; i++) 
            {

                double p_x = point[i].X;
                double p_y = point[i].Y;
                //壁面の厚さと長さの分だけで回転
                point[i].X = Before[0] + (p_x - Before[0]) * cos + (p_y - Before[1]) * sin;
                point[i].Y = Before[1] + (p_y - Before[1]) * cos - (p_x - Before[0]) * sin;

            }

        }

        public void Calculation_both(Points[] point, double[] other, double[] Before, double[] After)　//Z軸回りを回転、二つの壁面を位置合わせように
        {
            double x;
            double y;
            double r;
            double cos;
            double sin;
            if (Math.Abs(After[1]) > Math.Abs(Before[1]))
            {
                x = Math.Sqrt(Math.Pow(Before[0] - other[0], 2) + Math.Pow(Before[1] - other[1], 2));
                y = After[1] - Before[1];
                r = Math.Sqrt(Math.Pow(After[1] - other[1], 2) + Math.Pow(After[0] - other[0], 2));
            }
            else 
            {
                x = Math.Sqrt(Math.Pow(After[0] - other[0], 2) + Math.Pow(After[1] - other[1], 2));
                y = Before[1] - After[1];
                r = Math.Sqrt(Math.Pow(Before[1] - other[1], 2) + Math.Pow(Before[0] - other[0], 2));
            }
            cos = x / r;
            sin = y / r;
            for (int i = 0; i < point.Length; i++)
            {

                double p_x = point[i].X;
                double p_y = point[i].Y;
                //壁面の厚さと長さの分だけで回転
                point[i].X = other[0] + (p_x - other[0]) * cos + (p_y - other[1]) * sin;
                point[i].Y = other[1] + (p_y - other[1]) * cos - (p_x - other[0]) * sin;

            }

        }

    }
}
