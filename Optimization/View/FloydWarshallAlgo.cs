using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Security.Policy;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Floyd_WarshallAlg
{
    class FloydWarshallAlgo
    {

        public const int INF = 10000;
        int[,] distance;
        int[,] parent;

        public FloydWarshallAlgo()
        {
            this.distance = new int[6, 6]
            {
                { INF,   2,   1,   INF,   INF,   INF},
                { INF, INF,   3,   3,   INF,   INF},
                { INF, INF, INF,   INF,   1,   INF},
                { INF, INF, INF, INF,   INF, 2},
                { INF, INF, INF, 2,   INF,   5},
                { INF, INF, INF, INF, INF, INF}
            };
        }

        public void CreateMatrix(int size)
        {
            this.distance = new int[size, size];
            this.parent = new int[size, size];

            for (int i = 0; i < size; ++i)
                for (int j = 0; j < size; ++j)
                    this.distance[i, j] = INF;
        }

        public void AddVertex(int i, int j, int weight)
        {
            this.distance[i, j] = weight;
        }

        private static String MatrixToString(int[,] matrix) {
            int verticesCount = matrix.GetLength(0);
            StringBuilder sb = new StringBuilder();


            for (int i = 0; i < verticesCount; ++i)
            {
                for (int j = 0; j < verticesCount; ++j)
                {
                    if (matrix[i, j] == INF)
                        sb.Append("INF".PadLeft(6));
                    else
                        sb.Append(matrix[i, j].ToString().PadLeft(7));
                }

                sb.Append("\n");
            }

            return sb.ToString();
        }

        public String ToStringDistances()
        {
            return FloydWarshallAlgo.MatrixToString(this.distance);
        }

        public String ToStringParents()
        {
            return FloydWarshallAlgo.MatrixToString(this.parent);
        }

        public List<int> FindPath(int from, int to)
        {
            List<int> path = new List<int>();

            path.Add(to);
            int point = to;

            while (point != from)
            {
                point = parent[from, point];
                path.Add(point);
            }

            path.Reverse();

            return path;
        }

        public void Solve()
        {
            int size = this.distance.GetLength(0);
            this.parent = new int[size, size];

            for (int i = 0; i < size; ++i)
                for (int j = 0; j < size; ++j)
                    this.parent[i, j] = i;

            for (int k = 0; k < size; ++k)
            {
                for (int i = 0; i < size; ++i)
                {
                    for (int j = 0; j < size; ++j)
                    {
                        if (distance[i, k] + distance[k, j] < distance[i, j])
                        {
                            distance[i, j] = distance[i, k] + distance[k, j];
                            parent[i, j] = parent[k, j];
                        }
                    }
                }
            }

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    if (distance[i, j] == INF)
                    {
                        parent[i, j] = -1;
                    }
                    else
                    {
                        parent[i, j] = parent[i, j];
                    }
                }
            }

        }

        public static void Run()
        {
            int[,] graph = {
                         { INF,   2,   1,   INF,   INF,   INF},
                         { INF, INF,   3,   3,   INF,   INF},
                         { INF, INF, INF,   INF,   1,   INF},
                         { INF, INF, INF, INF,   INF, 2},
                         { INF, INF, INF, 2,   INF,   5},
                         { INF, INF, INF, INF, INF, INF}
                         };


        }

    }
}