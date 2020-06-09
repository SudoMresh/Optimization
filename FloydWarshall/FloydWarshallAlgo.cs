using System;
using System.Collections.Generic;
using System.Text;

namespace FloydWarshall
{
    public class FloydWarshallAlgo
    {

        public const int INF = 10000;
        private int[,] distance;
        private int[,] parent;
        private bool isCreated;
        private bool isHaveVertex;

        public FloydWarshallAlgo()
        {

            //this.distance = new int[,] {
            //    { INF,   1,   INF,   INF,   2},
            //{ INF, INF,   INF,   2,   INF},
            //{ 3, INF, INF,  2,   1},
            //{ INF, INF, INF, INF,   INF},
            //{ INF, 3, INF, INF,   INF}
            ////{ INF, INF, INF, INF, INF, INF}
            //};
        }

        public bool IsCreated()
        {
            return isCreated;
        }

        public bool IsHaveVertex()
        {
            return isHaveVertex;
        }

        public int GetMatrixSize()
        {
            return this.distance.GetLength(0);
        }

        public void CreateMatrix(int size)
        {
            this.distance = new int[size, size];
            this.parent = new int[size, size];

            for (int i = 0; i < size; ++i)
                for (int j = 0; j < size; ++j)
                    this.distance[i, j] = INF;

            this.isCreated = true;
        }

        public void AddVertex(int i, int j, int weight)
        {
            this.distance[i, j] = weight;

            this.isHaveVertex = true;
        }

        private static String MatrixToString(int[,] matrix)
        {
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

                if (point == -1) { break; }

                path.Add(point);
            }

            path.Reverse();

            if (path.Count == 0 || path[0] != from) { return null; }

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
                        if ((distance[i, k] + distance[k, j] < distance[i, j]) && (i != j))
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

    }
}