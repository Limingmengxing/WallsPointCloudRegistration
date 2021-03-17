using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2020プログラム
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

       

        private void 位置合わせ_Click(object sender, EventArgs e)
        {
            Points[] points1; //点群データ
            Points[] points2;
            File_Operation file = new File_Operation();

            List<string> f1 = file.FileOpen("旧一つ年度の点群を選択"); //ファイル名をlistに読み込み
            points1 = file.FileRead(f1); //点群データ読み込み

            List<string> f2 = file.FileOpen("新一つ年度の点群を選択"); //ファイル名をlistに読み込み
            points2 = file.FileRead(f2); //点群データ読み込み

            Console.WriteLine("点群読み込み完了");

            //選択された柱の範囲の値を求め
            string str = Interaction.InputBox("柱上限と下限を入力（スペースやコンマで区割り）", "柱の範囲", "", -1, -1);
            string[] Pillar = str.Split(new char[] { ' ', ',' });
            double Pillar_up = double.Parse(Pillar[0]);
            double Pillar_down = double.Parse(Pillar[1]);

            string str_l = Interaction.InputBox("柱の左側と右側を入力（スペースやコンマで区割り）", "柱の範囲", "", -1, -1);
            string[] Pillar_l = str_l.Split(new char[] { ' ', ',' });
            double Pillar_l_l = double.Parse(Pillar_l[0]);
            double Pillar_l_r = double.Parse(Pillar_l[1]);

            //範囲内の点の平均値を求め、合計で二点
            double[] point1_l = new double[3];
            double[] point2_l = new double[3];

            Tool tl = new Tool();

            tl.Pillar_Avenger(point1_l, points1, Pillar_down, Pillar_up, Pillar_l_l, Pillar_l_r);
            tl.Pillar_Avenger(point2_l, points2, Pillar_down, Pillar_up, Pillar_l_l, Pillar_l_r);

            double dis = point2_l[1] - point1_l[1];
            for (int i = 0; i < points1.Length; i++)
            {
                points1[i].Y += dis;
            }

            string name = "位置合わせ";
            string foldpath = Path.GetDirectoryName(f1[0]) + "\\位置合わせ\\" + Path.GetFileNameWithoutExtension(f1[0]) + "_" + System.DateTime.Now.ToString("MM" + "dd_H時mm分ss秒_") + "\\";
            System.IO.Directory.CreateDirectory(foldpath);
            file.Filewriter_point(points1, foldpath, name, 4);
            Console.WriteLine("終了");
        }

        private void クラスタリング_Click(object sender, EventArgs e)
        {
            Points[] points; //点群データ
            Voxel v = new Voxel();
            File_Operation file = new File_Operation();

            List<string> f = file.FileOpen("一つ年度の点群を選択"); //ファイル名をlistに読み込み
            points = file.FileRead4(f); //点群データ読み込み
            Console.WriteLine("点群読み込み完了");

            Voxel[,] voxel = v.Mesh(file.XYZ);//ボクセル作成
            Console.WriteLine("ボクセル作成完了");

            for (int i = 0; i < points.Count(); i++) //ボクセル分割
            {
                //xyz平面切り替え
                v.MakeVoxel(voxel, points[i]);
            }
            Console.WriteLine("ボクセル分割完了");

            Clustering cl = new Clustering();

            int pcl = 1;
            List<double> ppccll = new List<double>();
            for (int j = voxel.GetLength(1) - 1; j >= 0; j--)
            {
                for (int i = 0; i < voxel.GetLength(0); i++)
                {
                    try
                    {
                        if (voxel[i, j].voxel_points.Count != 0)
                        {
                            //新しい損傷を検出し、番号をつける
                            if (cl.Clustering1(i, j, voxel))
                            {
                                ppccll.Add(pcl);
                                voxel[i, j].voxel_points[0].N = pcl;
                                pcl++;
                            }
                            //周囲にほかの損傷ある損傷が番号をつける
                            else
                            {
                                cl.Clustering2(i, j, voxel, ppccll);
                            }

                        }

                    }
                    catch (IndexOutOfRangeException)
                    {
                    }

                }
            }
            //つけた番号を書き直す
            for (int ix = 0; ix < ppccll.Count; ix++)
            {
                if (ix + 1 != ppccll[ix])
                {
                    for (int i = 0; i < voxel.GetLength(0); i++)
                    {
                        for (int j = 0; j < voxel.GetLength(1); j++)
                        {

                            if (voxel[i, j].voxel_points.Count != 0 && voxel[i, j].voxel_points[0].N == ix + 1)
                            {
                                voxel[i, j].voxel_points[0].N = ppccll[ix];
                            }
                        }
                    }
                }

            }
            string foldpath = Path.GetDirectoryName(f[0]) + "\\クラスタリング\\" + Path.GetFileNameWithoutExtension(f[0]) + "_" + System.DateTime.Now.ToString("MM" + "dd_H時mm分ss秒_") + "\\";
            System.IO.Directory.CreateDirectory(foldpath);
            for (int i = pcl; i > 0; i--)
            {
                Console.WriteLine(i);
                file.Filewriter_clustering(voxel, foldpath, i);　//各種類の点群データ書き出し
            }
            Console.WriteLine("終了");
        }

        private void 傾き位置合わせ_Click(object sender, EventArgs e) //XOZ平面に平行ように
        {
            Points[] points1; //点群データ
            Points[] points2;
            File_Operation file = new File_Operation();

            List<string> f1 = file.FileOpen("旧一つ年度の点群を選択"); //ファイル名をlistに読み込み
            points1 = file.FileRead(f1); //点群データ読み込み

            List<string> f2 = file.FileOpen("新一つ年度の点群を選択"); //ファイル名をlistに読み込み
            points2 = file.FileRead(f2); //点群データ読み込み

            Console.WriteLine("点群読み込み完了");

            string str = Interaction.InputBox("柱上限と下限を入力（スペースやコンマで区割り）", "柱の範囲", "", -1, -1);
            string[] Pillar = str.Split(new char[] { ' ', ',' });
            double Pillar_up = double.Parse(Pillar[0]);
            double Pillar_down = double.Parse(Pillar[1]);

            string str_l = Interaction.InputBox("左柱の左側と右側を入力（スペースやコンマで区割り）", "柱の範囲", "", -1, -1);
            string[] Pillar_l = str_l.Split(new char[] { ' ', ',' });
            double Pillar_l_l = double.Parse(Pillar_l[0]);
            double Pillar_l_r = double.Parse(Pillar_l[1]);

            string str_r = Interaction.InputBox("右柱の左側と右側を入力（スペースやコンマで区割り）", "柱の範囲", "", -1, -1);
            string[] Pillar_r = str_r.Split(new char[] { ' ', ',' });
            double Pillar_r_l = double.Parse(Pillar_r[0]);
            double Pillar_r_r = double.Parse(Pillar_r[1]);
            //各柱の平均点
            double[] point1_l = new double[3];
            double[] point1_r = new double[3];
            double[] point2_l = new double[3];
            double[] point2_r = new double[3];

            Tool tl = new Tool();
            tl.Pillar_Avenger(point1_l, points1, Pillar_down, Pillar_up, Pillar_l_l, Pillar_l_r);
            tl.Pillar_Avenger(point1_r, points1, Pillar_down, Pillar_up, Pillar_r_l, Pillar_r_r);
            tl.Pillar_Avenger(point2_l, points2, Pillar_down, Pillar_up, Pillar_l_l, Pillar_l_r);
            tl.Pillar_Avenger(point2_r, points2, Pillar_down, Pillar_up, Pillar_r_l, Pillar_r_r);

            //左側の柱を移動させる
            double dis = point2_l[1] - point1_l[1];
            for (int i = 0; i < points1.Length; i++)
            {
                points1[i].Y += dis;
            }
            //右側の柱は左側の柱を基準として回転
            Rotation rt = new Rotation();
            point1_r[1] += dis; //前データの右側柱のｙ座標も変わる
            rt.Calculation_both(points1, point2_l, point2_r, point1_r);

            KDTree kt = new KDTree(points2);
            for (int i = 0; i < points1.Length; i++)
            {
                Points nn = kt.NNSearch(points1[i]);
                points1[i].N = points1[i].Y - nn.Y;
                //傾斜の角度
                points1[i].C = points1[i].N / (points1[i].Z - file.XYZ[5]);
            }

            string name = "傾き検出";
            string foldpath = Path.GetDirectoryName(f1[0]) + "\\基部位置合わせ\\" + Path.GetFileNameWithoutExtension(f1[0]) + "_" + System.DateTime.Now.ToString("MM" + "dd_H時mm分ss秒_") + "\\";
            System.IO.Directory.CreateDirectory(foldpath);
            file.Filewriter_point(points1, foldpath, name, 5);
            Console.WriteLine("終了");

        }


        private void 差分計算_Click(object sender, EventArgs e)
        {
            Points[] points1; //点群データ
            Points[] points2;
            File_Operation file = new File_Operation();

            List<string> f1 = file.FileOpen("旧一つ年度の点群を選択"); //ファイル名をlistに読み込み
            points1 = file.FileRead(f1); //点群データ読み込み

            List<string> f2 = file.FileOpen("新一つ年度の点群を選択"); //ファイル名をlistに読み込み
            points2 = file.FileRead(f2); //点群データ読み込み

            Console.WriteLine("点群読み込み完了");

            KDTree kt = new KDTree(points2);
            for (int i = 0; i < points1.Length; i++)
            {
                Points nn = kt.NNSearch(points1[i]);
                points1[i].N = points1[i].Y - nn.Y;
            }

            string name = "差分計算";
            string foldpath = Path.GetDirectoryName(f1[0]) + "\\差分計算\\" + Path.GetFileNameWithoutExtension(f1[0]) + "_" + System.DateTime.Now.ToString("MM" + "dd_H時mm分ss秒_") + "\\";
            System.IO.Directory.CreateDirectory(foldpath);
            file.Filewriter_point(points1, foldpath, name, 4);
            Console.WriteLine("終了");
        }

        private void メッシュの分割_Click(object sender, EventArgs e)
        {
            Points[] points; //点群データ
            Voxel v = new Voxel();
            File_Operation file = new File_Operation();

            List<string> f = file.FileOpen("一つ年度の点群を選択"); //ファイル名をlistに読み込み
            points = file.FileRead(f); //点群データ読み込み

            Console.WriteLine("点群読み込み完了");

            Voxel[,] voxel = v.Mesh(file.XYZ);//ボクセル作成
            Console.WriteLine("ボクセル作成完了");

            for (int i = 0; i < points.Count(); i++) //ボクセル分割
            {
                //xyz平面切り替え
                v.MakeVoxel(voxel, points[i]);
            }
            Console.WriteLine("ボクセル分割完了");

            v.Voxel_point(voxel);//メッシュの評価
            v.Voxel_aver(voxel, file); //メッシュ代表点を求め

            string name = "メッシュ分割";
            string foldpath = Path.GetDirectoryName(f[0]) + "\\メッシュ\\" + Path.GetFileNameWithoutExtension(f[0]) + "_" + System.DateTime.Now.ToString("MM" + "dd_H時mm分ss秒_") + "\\";
            System.IO.Directory.CreateDirectory(foldpath);
            file.Filewriter_mesh(voxel, foldpath, name);　//メッシュの点群データ書き出し
            file.Filewriter_meshcount(v, foldpath);
            Console.WriteLine("終了");
        }

       

        private void 差分の差_Click(object sender, EventArgs e)
        {
            Points[] points1; //点群データ
            Points[] points2;
            File_Operation file = new File_Operation();

            List<string> f1 = file.FileOpen("旧一つ年度の点群を選択"); //ファイル名をlistに読み込み
            points1 = file.FileRead4(f1); //点群データ読み込み

            List<string> f2 = file.FileOpen("新一つ年度の点群を選択"); //ファイル名をlistに読み込み
            points2 = file.FileRead4(f2); //点群データ読み込み

            Console.WriteLine("点群読み込み完了");

            KDTree kt = new KDTree(points2);
            for (int i = 0; i < points1.Length; i++)
            {

                Points nn = kt.NNSearch4(points1[i]);
                if (nn.N > 0)
                {
                    points1[i].N -= nn.N;
                }


            }

            string name = "差分の差";
            string foldpath = Path.GetDirectoryName(f1[0]) + "\\差分の差\\" + Path.GetFileNameWithoutExtension(f1[0]) + "_" + System.DateTime.Now.ToString("MM" + "dd_H時mm分ss秒_") + "\\";
            System.IO.Directory.CreateDirectory(foldpath);
            file.Filewriter_point(points1, foldpath, name, 4);
            Console.WriteLine("終了");
        }

        private void 歪み検出_Click(object sender, EventArgs e)
        {
            Points[] points1; //点群データ
            Points[] points2;
            File_Operation file = new File_Operation();

            List<string> f1 = file.FileOpen("旧一つ年度の点群を選択"); //ファイル名をlistに読み込み
            points1 = file.FileRead(f1); //点群データ読み込み

            List<string> f2 = file.FileOpen("新一つ年度の点群を選択"); //ファイル名をlistに読み込み
            points2 = file.FileRead(f2); //点群データ読み込み

            Console.WriteLine("点群読み込み完了");

            string str = Interaction.InputBox("右柱上限と下限を入力（スペースやコンマで区割り）", "柱の範囲", "", -1, -1);
            string[] Pillar = str.Split(new char[] { ' ', ',' });
            double Pillar_r_up = double.Parse(Pillar[0]);
            double Pillar_r_down = double.Parse(Pillar[1]);

            string str_r = Interaction.InputBox("右柱の左側と右側を入力（スペースやコンマで区割り）", "柱の範囲", "", -1, -1);
            string[] Pillar_r = str_r.Split(new char[] { ' ', ',' });
            double Pillar_r_l = double.Parse(Pillar_r[0]);
            double Pillar_r_r = double.Parse(Pillar_r[1]);

            string str_d = Interaction.InputBox("中柱の上限と下限を入力（スペースやコンマで区割り）", "柱の範囲", "", -1, -1);
            string[] Pillar_d = str_d.Split(new char[] { ' ', ',' });
            double Pillar_d_up = double.Parse(Pillar_d[0]);
            double Pillar_d_down = double.Parse(Pillar_d[1]);

            string str_l = Interaction.InputBox("中柱の左側と右側を入力（スペースやコンマで区割り）", "柱の範囲", "", -1, -1);
            string[] Pillar_l = str_l.Split(new char[] { ' ', ',' });
            double Pillar_d_l = double.Parse(Pillar_l[0]);
            double Pillar_d_r = double.Parse(Pillar_l[1]);

            string str_ld = Interaction.InputBox("左柱の上限と下限を入力（スペースやコンマで区割り）", "柱の範囲", "", -1, -1);
            string[] Pillar_ld = str_ld.Split(new char[] { ' ', ',' });
            double Pillar_l_up = double.Parse(Pillar_ld[0]);
            double Pillar_l_down = double.Parse(Pillar_ld[1]);

            string str_dd = Interaction.InputBox("左柱の左限と右限を入力（スペースやコンマで区割り）", "柱の範囲", "", -1, -1);
            string[] Pillar_dd = str_dd.Split(new char[] { ' ', ',' });
            double Pillar_l_l = double.Parse(Pillar_dd[0]);
            double Pillar_l_r = double.Parse(Pillar_dd[1]);

            Tool tl = new Tool();
            //旧データの各柱の平均点
            double[] point1_l = new double[3];
            double[] point1_r = new double[3];
            double[] point1_d = new double[3];

            tl.Pillar_Avenger(point1_l, points1, Pillar_l_down, Pillar_l_up, Pillar_l_l, Pillar_l_r);
            tl.Pillar_Avenger(point1_r, points1, Pillar_r_down, Pillar_r_up, Pillar_r_l, Pillar_r_r);
            tl.Pillar_Avenger(point1_d, points1, Pillar_d_down, Pillar_d_up, Pillar_d_l, Pillar_d_r);
            //旧データの本年度の歪み
            Surface sf1 = new Surface();
            sf1.Vector_surface(point1_l, point1_r, point1_d);
            sf1.Distance_Z(points1);
            //新データの各柱の平均点
            double[] point2_l = new double[3];
            double[] point2_r = new double[3];
            double[] point2_d = new double[3];

            tl.Pillar_Avenger(point2_l, points2, Pillar_l_down, Pillar_l_up, Pillar_l_l, Pillar_l_r);
            tl.Pillar_Avenger(point2_r, points2, Pillar_r_down, Pillar_r_up, Pillar_r_l, Pillar_r_r);
            tl.Pillar_Avenger(point2_d, points2, Pillar_d_down, Pillar_d_up, Pillar_d_l, Pillar_d_r);
            //新データの本年度の歪み
            Surface sf2 = new Surface();
            sf2.Vector_surface(point2_l, point2_r, point2_d);
            sf2.Distance_Z(points2);
            //二時期データの歪み進行状況を計算
            KDTree kt = new KDTree(points2);
            for (int i = 0; i < points1.Length; i++)
            {
                Points nn = kt.NNSearch4(points1[i]);
                double p_n = points1[i].N;
                points1[i].N = nn.N - p_n;
            }

            string name = "歪み検出";
            string foldpath = Path.GetDirectoryName(f1[0]) + "\\歪み検出\\" + Path.GetFileNameWithoutExtension(f1[0]) + "_" + System.DateTime.Now.ToString("MM" + "dd_H時mm分ss秒_") + "\\";
            System.IO.Directory.CreateDirectory(foldpath);
            file.Filewriter_point(points1, foldpath, name, 4);
            Console.WriteLine("終了");
        }

        private void 中央値で位置合わせ(object sender, EventArgs e)
        {
            Points[] points1; //点群データ
            Points[] points2;
            
            File_Operation file1 = new File_Operation();
            List<string> f1 = file1.FileOpen("旧一つ年度の壁面のメッシュを選択"); //ファイル名をlistに読み込み
            points1 = file1.FileRead(f1); //点群データ読み込み

            File_Operation file2 = new File_Operation();
            List<string> f2 = file2.FileOpen("新一つ年度の壁面のメッシュを選択"); //ファイル名をlistに読み込み
            points2 = file2.FileRead(f2); //点群データ読み込み

            Console.WriteLine("点群読み込み完了");

            //中央値の計算
            double x_median = (file1.XYZ[0] - file1.XYZ[1]) / 2;
            double z_median = (file1.XYZ[4] - file1.XYZ[5]) / 2;

            //中心部位の差分を求め
            double[] p1_median = new double[3];
            double[] p2_median = new double[3];

            Tool tl = new Tool();
            tl.Pillar_Avenger(p1_median, points1, file1.XYZ[5] + z_median - 0.2, file1.XYZ[5] + z_median + 0.2, file1.XYZ[1] + x_median - 0.2, file1.XYZ[1] + x_median + 0.2);
            tl.Pillar_Avenger(p2_median, points2, file1.XYZ[5] + z_median - 0.2, file1.XYZ[5] + z_median + 0.2, file1.XYZ[1] + x_median - 0.2, file1.XYZ[1] + x_median + 0.2);

            //中央点のY座標の差分で位置合わせ
            double y_diff = p1_median[1] - p2_median[1];
            for (int i = 0; i < points1.Length; i++) 
            {
                points1[i].Y -= y_diff;
            }

            //損傷検出
            KDTree kt = new KDTree(points2);
            for (int i = 0; i < points1.Length; i++)
            {
                Points nn = kt.NNSearch(points1[i]);
                points1[i].N = points1[i].Y - nn.Y;
            }

            string name = "中心位置合わせ";
            string foldpath = Path.GetDirectoryName(f1[0]) + "\\" + name + "\\" + Path.GetFileNameWithoutExtension(f1[0]) + "_" + System.DateTime.Now.ToString("MM" + "dd_H時mm分ss秒_") + "\\";
            System.IO.Directory.CreateDirectory(foldpath);
            file1.Filewriter_point(points1, foldpath, name, 4);
            Console.WriteLine("終了");
        }
    }
}
