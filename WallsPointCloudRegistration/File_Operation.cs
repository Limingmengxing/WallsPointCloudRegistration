using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2020プログラム
{
    class File_Operation
    {
        public List<string> FileOpen() //ファイル選択
        {
            List<string> FileNames = new List<string>();
            OpenFileDialog ofd = new OpenFileDialog();

            DialogResult dr = ofd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < ofd.FileNames.Length; i++)
                {
                    FileNames.Add(String.Copy(ofd.FileNames[i]));

                    //if (ofd.FileNames.Length == 1)
                    //filepass = System.IO.Path.GetDirectoryName(FileNames[0]);

                }
            }
            ofd.Dispose();
            return FileNames;
        }

        public List<string> FileOpen(string title) //ファイル選択
        {
            List<string> FileNames = new List<string>();
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = title
            };

            DialogResult dr = ofd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < ofd.FileNames.Length; i++)
                {
                    FileNames.Add(String.Copy(ofd.FileNames[i]));

                    //if (ofd.FileNames.Length == 1)
                    //filepass = System.IO.Path.GetDirectoryName(FileNames[0]);

                }
            }
            ofd.Dispose();
            return FileNames;
        }

        //最大最小を外部から参照するための変数
        public double[] XYZ = new double[6];
        public Points[] FileRead(List<string> FileNames) //点群データ読み込み
        {
            Points[] point_double;
            double[] x = new double[2];
            double[] y = new double[2];
            double[] z = new double[2];
            int readcount = 0;
            int DataCount = 0;

            for (int i = 0; i < FileNames.Count; i++)
            {
                using (Stream stream = File.OpenRead(FileNames[i]))
                {

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        //拡張子がtxt,xyz,csvだった場合、要素数を数える
                        if (string.Compare(System.IO.Path.GetExtension(FileNames[i]), ".txt", true) == 0
                            || string.Compare(System.IO.Path.GetExtension(FileNames[i]), ".xyz", true) == 0
                            || string.Compare(System.IO.Path.GetExtension(FileNames[i]), ".csv", true) == 0)
                        {
                            DataCount += TxtDataCount(FileNames[i]);
                            Console.Write("{0}\n", DataCount);
                        }

                        //拡張子がOFFのときのみ2行分読み飛ばす
                        else if (string.Compare(System.IO.Path.GetExtension(FileNames[i]), ".off", true) == 0)
                        {
                            ////OFFは1行目に"OFF"という記述があるため、読み飛ばす
                            reader.ReadLine();

                            //OFFは2行目に（点数　線数　面数）の順に記述されている
                            DataCount += int.Parse(reader.ReadLine().Split(new char[] { ',', ' ' })[0]);
                            Console.Write("{0}\n", DataCount);
                        }
                    }
                }
            }

            //データを格納するための配列の宣言
            point_double = new Points[DataCount];

            //配列の初期化
            for (int a = 0; a < DataCount; a++)
            {
                //point[a] = new Vertex(new Vector3(0, 0, 0));
                point_double[a] = new Points(0, 0, 0);
            }

            for (int i = 0; i < FileNames.Count; i++)
            {
                using (Stream stream = File.OpenRead(FileNames[i]))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        //データの読み込み
                        while (reader.Peek() >= 0 || readcount > DataCount)
                        {
                            string line = reader.ReadLine();
                            string[] split_line = line.Split(new char[] { ',', ' ' }); //デリミタの設定

                            //データの一行から各座標を抜き出す
                            double X = double.Parse(split_line[0]);
                            double Y = double.Parse(split_line[1]);
                            double Z = double.Parse(split_line[2]);

                            point_double[readcount].X = X;
                            point_double[readcount].Y = Y;
                            point_double[readcount].Z = Z;

                            //各座標の最大・最小を求める
                            if (readcount == 0)
                            {
                                x[0] = X;
                                x[1] = X;
                                y[0] = Y;
                                y[1] = Y;
                                z[0] = Z;
                                z[1] = Z;
                            }

                            else
                            {

                                x[0] = max_value_double(x[0], X);
                                x[1] = min_value_double(x[1], X);
                                y[0] = max_value_double(y[0], Y);
                                y[1] = min_value_double(y[1], Y);
                                z[0] = max_value_double(z[0], Z);
                                z[1] = min_value_double(z[1], Z);
                            }

                            //配列に格納した要素数のカウント
                            readcount++;
                        }
                        reader.Close();

                        Console.WriteLine("x 最大: {0} , 最小: {1}\n", x[0], x[1]);
                        Console.WriteLine("y 最大: {0} , 最小: {1}\n", y[0], y[1]);
                        Console.WriteLine("z 最大: {0} , 最小: {1}\n", z[0], z[1]);

                        XYZ[0] = x[0];
                        XYZ[1] = x[1];
                        XYZ[2] = y[0];
                        XYZ[3] = y[1];
                        XYZ[4] = z[0];
                        XYZ[5] = z[1];

                    }
                    stream.Close();


                    //string datapass;
                    //datapass = System.IO.Path.GetDirectoryName(FileNames[0]);

                    //file(x, y, z, datapass); //データ切り出し
                }
            }

            return point_double;
        }
        public Points[] FileRead4(List<string> FileNames) //点群データ読み込み

        {
            Points[] point_double;
            double[] x = new double[2];
            double[] y = new double[2];
            double[] z = new double[2];
            int readcount = 0;
            int DataCount = 0;

            for (int i = 0; i < FileNames.Count; i++)
            {
                using (Stream stream = File.OpenRead(FileNames[i]))
                {

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        //拡張子がtxt,xyz,csvだった場合、要素数を数える
                        if (string.Compare(System.IO.Path.GetExtension(FileNames[i]), ".txt", true) == 0
                            || string.Compare(System.IO.Path.GetExtension(FileNames[i]), ".xyz", true) == 0
                            || string.Compare(System.IO.Path.GetExtension(FileNames[i]), ".csv", true) == 0)
                        {
                            DataCount += TxtDataCount(FileNames[i]);
                            Console.Write("{0}\n", DataCount);
                        }

                        //拡張子がOFFのときのみ2行分読み飛ばす
                        else if (string.Compare(System.IO.Path.GetExtension(FileNames[i]), ".off", true) == 0)
                        {
                            ////OFFは1行目に"OFF"という記述があるため、読み飛ばす
                            reader.ReadLine();

                            //OFFは2行目に（点数　線数　面数）の順に記述されている
                            DataCount += int.Parse(reader.ReadLine().Split(new char[] { ',', ' ' })[0]);
                            Console.Write("{0}\n", DataCount);
                        }
                    }
                }
            }

            //データを格納するための配列の宣言
            point_double = new Points[DataCount];

            //配列の初期化
            for (int a = 0; a < DataCount; a++)
            {
                //point[a] = new Vertex(new Vector3(0, 0, 0));
                point_double[a] = new Points(0, 0, 0, 0);
            }

            for (int i = 0; i < FileNames.Count; i++)
            {
                using (Stream stream = File.OpenRead(FileNames[i]))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        //データの読み込み
                        while (reader.Peek() >= 0 || readcount > DataCount)
                        {
                            string line = reader.ReadLine();
                            string[] split_line = line.Split(new char[] { ',', ' ' }); //デリミタの設定

                            //データの一行から各座標を抜き出す
                            double X = double.Parse(split_line[0]);
                            double Y = double.Parse(split_line[1]);
                            double Z = double.Parse(split_line[2]);
                            //double N = double.Parse(split_line[3]);

                            point_double[readcount].X = X;
                            point_double[readcount].Y = Y;
                            point_double[readcount].Z = Z;
                            if (split_line.Length >= 4) 
                            {
                                double N = double.Parse(split_line[3]);
                                point_double[readcount].N = N;
                            }
                            //point_double[readcount].N = 0;

                            //各座標の最大・最小を求める
                            if (readcount == 0)
                            {
                                x[0] = X;
                                x[1] = X;
                                y[0] = Y;
                                y[1] = Y;
                                z[0] = Z;
                                z[1] = Z;
                            }

                            else
                            {

                                x[0] = max_value_double(x[0], X);
                                x[1] = min_value_double(x[1], X);
                                y[0] = max_value_double(y[0], Y);
                                y[1] = min_value_double(y[1], Y);
                                z[0] = max_value_double(z[0], Z);
                                z[1] = min_value_double(z[1], Z);
                            }

                            //配列に格納した要素数のカウント
                            readcount++;
                        }
                        reader.Close();

                        Console.WriteLine("x 最大: {0} , 最小: {1}\n", x[0], x[1]);
                        Console.WriteLine("y 最大: {0} , 最小: {1}\n", y[0], y[1]);
                        Console.WriteLine("z 最大: {0} , 最小: {1}\n", z[0], z[1]);

                        XYZ[0] = x[0];
                        XYZ[1] = x[1];
                        XYZ[2] = y[0];
                        XYZ[3] = y[1];
                        XYZ[4] = z[0];
                        XYZ[5] = z[1];

                    }
                    stream.Close();


                    //string datapass;
                    //datapass = System.IO.Path.GetDirectoryName(FileNames[0]);

                    //file(x, y, z, datapass); //データ切り出し
                }
            }

            return point_double;
        }

        public int TxtDataCount(string FileName) //txtデータの要素数カウント
        {
            int count = 0; //要素数

            using (Stream stream = File.OpenRead(FileName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (reader.Peek() >= 0)
                    {
                        reader.ReadLine();
                        count++;
                    }
                    reader.Close();
                }
                stream.Close();
            }

            return count; //要素数を返す
        }

        public double max_value_double(double max_value, double value) //最大値比較
        {
            if (max_value < value)
                return value;
            else
                return max_value;
        }
    
        public double min_value_double(double min_value, double value) //最小値比較
        {
            if (min_value > value)
                return value;
            else
                return min_value;
        }

        public void Filewriter_mesh(Voxel[,] voxel, string foldpath, string name) //点群データ書き出し
        {
            StreamWriter writer_txt = new StreamWriter(foldpath + name + ".txt", false);

            for (int i = 0; i < voxel.GetLength(0); i++)
            {
                for (int j = 0; j < voxel.GetLength(1); j++)
                {
                    if (voxel[i, j].voxel_points.Count() != 0)
                    {
                        writer_txt.WriteLine("{0:F5} {1:F5} {2:F5}", voxel[i, j].voxel_points[0].X, voxel[i, j].voxel_points[0].Y, voxel[i, j].voxel_points[0].Z);
                    }
                }
            }
            writer_txt.Close();

            StreamWriter writer_voxel = new StreamWriter(foldpath + "voxel_aver.txt", false);
            Voxel VV = new Voxel();
            VV.Voxel_point(voxel);
            writer_voxel.WriteLine("メッシュの最大点数：" +　"{0:F5}", VV.point_max);
            writer_voxel.WriteLine("メッシュの最小点数：{0:F5}", VV.point_min);
            writer_voxel.WriteLine("メッシュの平均点数：{0:F5}", VV.point_aver);
            writer_voxel.WriteLine("メッシュの総計点数：{0:F5}", VV.voxel_quantity);
            writer_voxel.Close();
        }
        public void Filewriter_meshcount(Voxel VV, string foldpath) 
        {
            StreamWriter writer_voxel = new StreamWriter(foldpath + "voxel_aver.txt", false);
            writer_voxel.WriteLine("メッシュの最大点数：" + "{0:F5}", VV.point_max);
            writer_voxel.WriteLine("メッシュの最小点数：{0:F5}", VV.point_min);
            writer_voxel.WriteLine("メッシュの平均点数：{0:F5}", VV.point_aver);
            writer_voxel.WriteLine("メッシュの総計点数：{0:F5}", VV.voxel_quantity);
            writer_voxel.Close();
        }
        public void Filewriter_point(Points[] point, string foldpath, string name, int count) //点群データ書き出し
        {
            StreamWriter writer_txt = new StreamWriter(foldpath + System.DateTime.Now.ToString("MM" + "dd_H時mm分ss秒_") + name + ".txt", false);

            switch (count) 
            {
                case 3:
                    {
                        for (int i = 0; i < point.Length; i++)
                        {
                            writer_txt.WriteLine("{0:F5} {1:F5} {2:F5}", point[i].X, point[i].Y, point[i].Z);
                        }
                    }
                    break;
                case 4:
                    {
                        for (int i = 0; i < point.Length; i++)
                        {
                            writer_txt.WriteLine("{0:F5} {1:F5} {2:F5} {3:F5}", point[i].X, point[i].Y, point[i].Z, point[i].N);
                        }
                    }
                    break;
                case 5:
                    {
                        for (int i = 0; i < point.Length; i++)
                        {
                            writer_txt.WriteLine("{0:F5} {1:F5} {2:F5} {3:F5} {4:F5}", point[i].X, point[i].Y, point[i].Z, point[i].N, point[i].C);
                        }
                    }
                    break;
            }
            //for (int i = 0; i < point.Length; i++)
            //{
            //     writer_txt.WriteLine("{0:F5} {1:F5} {2:F5} {3:F5}", point[i].X, point[i].Y, point[i].Z, point[i].N);
            //}
            writer_txt.Close();
        }
        public void Filewriter_clustering(Voxel[,] voxel, string foldpath, int pcl) //点群データ書き出し
        {
            StreamWriter writer_txt = new StreamWriter(foldpath + System.DateTime.Now.ToString("MM" + "dd_H時mm分ss秒_") + "cluster" + pcl + ".txt", false);

            for (int i = 0; i < voxel.GetLength(0); i++)
            {
                for (int j = 0; j < voxel.GetLength(1); j++)
                {
                    if (voxel[i, j].voxel_points.Count() != 0 && voxel[i, j].voxel_points[0].N == pcl)
                    {
                        for (int n = 0; n < voxel[i, j].voxel_points.Count(); n++)
                        {

                            writer_txt.WriteLine("{0:F5} {1:F5} {2:F5} {3:F5}", voxel[i, j].voxel_points[n].X, voxel[i, j].voxel_points[n].Y, voxel[i, j].voxel_points[n].Z, voxel[i, j].voxel_points[n].N);

                        }
                    }
                }
            }
            writer_txt.Close();
        }

        public List<Points> FileReadlist(List<string> FileNames) //点群データ読み込み
        {
            //List<Points> list; 
            double[] x = new double[2];
            double[] y = new double[2];
            double[] z = new double[2];
            int readcount = 0;
            int DataCount = 0;

            for (int i = 0; i < FileNames.Count; i++)
            {
                using (Stream stream = File.OpenRead(FileNames[i]))
                {

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        //拡張子がtxt,xyz,csvだった場合、要素数を数える
                        if (string.Compare(System.IO.Path.GetExtension(FileNames[i]), ".txt", true) == 0
                            || string.Compare(System.IO.Path.GetExtension(FileNames[i]), ".xyz", true) == 0
                            || string.Compare(System.IO.Path.GetExtension(FileNames[i]), ".csv", true) == 0)
                        {
                            DataCount += TxtDataCount(FileNames[i]);
                            Console.Write("{0}\n", DataCount);
                        }

                        //拡張子がOFFのときのみ2行分読み飛ばす
                        else if (string.Compare(System.IO.Path.GetExtension(FileNames[i]), ".off", true) == 0)
                        {
                            ////OFFは1行目に"OFF"という記述があるため、読み飛ばす
                            reader.ReadLine();

                            //OFFは2行目に（点数　線数　面数）の順に記述されている
                            DataCount += int.Parse(reader.ReadLine().Split(new char[] { ',', ' ' })[0]);
                            Console.Write("{0}\n", DataCount);
                        }
                    }
                }
            }

            //データを格納するための配列の宣言
            //point_double = new Points[DataCount];
          
            List<Points> list = new List<Points>();

            //配列の初期化
            for (int a = 0; a < DataCount; a++)
            {
                //point[a] = new Vertex(new Vector3(0, 0, 0));
                list = new List<Points>(a) { new Points(0, 0, 0)};
            }

            for (int i = 0; i < FileNames.Count; i++)
            {
                using (Stream stream = File.OpenRead(FileNames[i]))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        //データの読み込み
                        while (reader.Peek() >= 0 || readcount > DataCount)
                        {
                            string line = reader.ReadLine();
                            string[] split_line = line.Split(new char[] { ',', ' ' }); //デリミタの設定

                            //データの一行から各座標を抜き出す
                            double X = double.Parse(split_line[0]);
                            double Y = double.Parse(split_line[1]);
                            double Z = double.Parse(split_line[2]);

                            list[readcount].X = X;
                            list[readcount].Y = Y;
                            list[readcount].Z = Z;

                            //各座標の最大・最小を求める
                            if (readcount == 0)
                            {
                                x[0] = X;
                                x[1] = X;
                                y[0] = Y;
                                y[1] = Y;
                                z[0] = Z;
                                z[1] = Z;
                            }

                            else
                            {

                                x[0] = max_value_double(x[0], X);
                                x[1] = min_value_double(x[1], X);
                                y[0] = max_value_double(y[0], Y);
                                y[1] = min_value_double(y[1], Y);
                                z[0] = max_value_double(z[0], Z);
                                z[1] = min_value_double(z[1], Z);
                            }

                            //配列に格納した要素数のカウント
                            readcount++;
                        }
                        reader.Close();

                        Console.WriteLine("x 最大: {0} , 最小: {1}\n", x[0], x[1]);
                        Console.WriteLine("y 最大: {0} , 最小: {1}\n", y[0], y[1]);
                        Console.WriteLine("z 最大: {0} , 最小: {1}\n", z[0], z[1]);

                        XYZ[0] = x[0];
                        XYZ[1] = x[1];
                        XYZ[2] = y[0];
                        XYZ[3] = y[1];
                        XYZ[4] = z[0];
                        XYZ[5] = z[1];

                    }
                    stream.Close();


                    //string datapass;
                    //datapass = System.IO.Path.GetDirectoryName(FileNames[0]);

                    //file(x, y, z, datapass); //データ切り出し
                }
            }

            return list;
        }
    }
}
