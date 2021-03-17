using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2020プログラム
{
    public enum Axis2
    {
        X, Z
    }

    class Node
    {
        public Points Point { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public Axis2 Axis { get; set; }

        public Node()
        {
            Point = null;
            Left = null;
            Right = null;
        }
    }

    class KDTree
    {
        public Node Root { get; }

        /// <summary>
        /// 点データの配列をもとにk-d木を作成する
        /// </summary>
        /// <param name="points">点データの配列(点群データ)</param>
        public KDTree(Points[] points)
        {
            Console.WriteLine("Building KDTree...");
            Root = BuildRecursive(points, 0);
            Console.WriteLine("Built");
        }

        // k-d木の構築
        private Node BuildRecursive(Points[] points, int depth)
        {
            if (points.Length == 0)
            {
                return null;
            }

            Axis2 axis = (Axis2)(depth % 2);
            if (axis == Axis2.X)
            {
                Array.Sort(points, (p1, p2) => p1.X.CompareTo(p2.X));
            }
            else
            {
                Array.Sort(points, (p1, p2) => p1.Z.CompareTo(p2.Z));
            }

            int median = points.Length / 2;
            Node node = new Node();
            node.Point = points[median];
            node.Axis = axis;
            node.Left = BuildRecursive(points.Take(median).ToArray(), depth++);
            if (points.Length % 2 == 0)
            {
                node.Right = BuildRecursive(points.Skip(median + 1).Take(median - 1).ToArray(), depth++);
            }
            else
            {
                node.Right = BuildRecursive(points.Skip(median + 1).Take(median).ToArray(), depth++);
            }

            return node;
        }

        public double Distance(Points p1, Points p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Z - p2.Z) * (p1.Z - p2.Z));
        }

        // centerから最近傍の点を返す
        public Points NNSearch(Points center)
        {
            Points guess = new Points(0, 0, 0);
            double _minDist = double.MaxValue;

            NNSearchRecursive(center, Root, guess, ref _minDist);

            return guess;
        }

        public Points NNSearch4(Points center)
        {
            Points guess = new Points(0, 0, 0, 0);
            double _minDist = double.MaxValue;

            NNSearchRecursive4(center, Root, guess, ref _minDist);

            return guess;
        }
        private void NNSearchRecursive4(Points center, Node node, Points guess, ref double minDist)
        {
            if (node == null)
            {
                return;
            }

            Points point = node.Point;

            double dist = Distance(center, point);
            if (dist <= minDist)
            {
                minDist = dist;
                guess.X = node.Point.X;
                guess.Y = node.Point.Y;
                guess.Z = node.Point.Z;
                guess.N = node.Point.N;
            }

            Axis2 axis = node.Axis;
            int dir;
            double diff;
            if (axis == Axis2.X)
            {
                dir = center.X < point.X ? 0 : 1;
                diff = Math.Abs(center.X - point.X);
            }
            else
            {
                dir = center.Z < point.Z ? 0 : 1;
                diff = Math.Abs(center.Z - point.Z);
            }

            if (dir == 0)
            {
                NNSearchRecursive4(center, node.Left, guess, ref minDist);
            }
            else
            {
                NNSearchRecursive4(center, node.Right, guess, ref minDist);
            }

            if (diff <= minDist)
            {
                if (dir == 0)
                {
                    NNSearchRecursive4(center, node.Right, guess, ref minDist);
                }
                else
                {
                    NNSearchRecursive4(center, node.Left, guess, ref minDist);
                }
            }
        }

        private void NNSearchRecursive(Points center, Node node, Points guess, ref double minDist)
        {
            if (node == null)
            {
                return;
            }

            Points point = node.Point;

            double dist = Distance(center, point);
            if (dist <= minDist)
            {
                minDist = dist;
                guess.X = node.Point.X;
                guess.Y = node.Point.Y;
                guess.Z = node.Point.Z;
            }

            Axis2 axis = node.Axis;
            int dir;
            double diff;
            if (axis == Axis2.X)
            {
                dir = center.X < point.X ? 0 : 1;
                diff = Math.Abs(center.X - point.X);
            }
            else
            {
                dir = center.Z < point.Z ? 0 : 1;
                diff = Math.Abs(center.Z - point.Z);
            }

            if (dir == 0)
            {
                NNSearchRecursive(center, node.Left, guess, ref minDist);
            }
            else
            {
                NNSearchRecursive(center, node.Right, guess, ref minDist);
            }

            if (diff <= minDist)
            {
                if (dir == 0)
                {
                    NNSearchRecursive(center, node.Right, guess, ref minDist);
                }
                else
                {
                    NNSearchRecursive(center, node.Left, guess, ref minDist);
                }
            }
        }
    }
}
