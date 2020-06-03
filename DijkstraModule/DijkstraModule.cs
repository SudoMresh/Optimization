using DijkstraModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra
{
    /// <summary>
    /// 
    /// </summary>
    public class DijkstraModule
    {
        private Graph graph;
        private List<GraphVertexInfo> infos;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        public DijkstraModule(Graph graph)
        {
            this.graph = graph;
        }

        /// <summary>
        /// 
        /// </summary>
        void InitInfo()
        {
            infos = new List<GraphVertexInfo>();

            foreach (var item in graph.Vertices)
            {
                infos.Add(new GraphVertexInfo(item));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        GraphVertexInfo GetGraphVertexInfo(GraphVertex vertex)
        {
            foreach (var item in infos)
            {
                if (item.Vertex.Equals(vertex))
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GraphVertexInfo FindUnvisitedVertexWithSum()
        {
            var minValue = double.MaxValue;
            GraphVertexInfo minVertexInfo = null;

            foreach (var item in infos)
            {
                if (item.IsUnvisited && item.EdgesWeightSum < minValue)
                {
                    minVertexInfo = item;
                    minValue = item.EdgesWeightSum;
                }
            }

            return minVertexInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startname"></param>
        /// <param name="finishName"></param>
        /// <returns></returns>
        public string FindShortestPath(string startname, string finishName)
        {
            var foundStart = graph.FindVertex(startname);
            var foundEnd = graph.FindVertex(finishName);

            if (foundStart == null || foundEnd == null)
            {
                NotFoundEdgeExeption exeption = new NotFoundEdgeExeption();

                if (foundStart == null)
                    exeption.NotFoundFirstEdge = startname;

                if (foundEnd == null)
                    exeption.NotFoundSecondEdge = finishName;

                throw exeption;
            }

            return FindShortestPath(graph.FindVertex(startname), graph.FindVertex(finishName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startVertex"></param>
        /// <param name="finishVertex"></param>
        /// <returns></returns>
        public string FindShortestPath(GraphVertex startVertex, GraphVertex finishVertex)
        {
            InitInfo();

            var first = GetGraphVertexInfo(startVertex);
            first.EdgesWeightSum = 0;

            var current = FindUnvisitedVertexWithSum();

            while (current != null)
            {
                SetSumToNextVertex(current);
                current = FindUnvisitedVertexWithSum();
            }

            return GetPath(startVertex, finishVertex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        void SetSumToNextVertex(GraphVertexInfo info)
        {
            info.IsUnvisited = false;

            foreach (var item in info.Vertex.Edges)
            {
                var nextInfo = GetGraphVertexInfo(item.ConnectedVertex);
                var sum = info.EdgesWeightSum + item.EdgeWeight;

                if (sum < nextInfo.EdgesWeightSum)
                {
                    nextInfo.EdgesWeightSum = sum;
                    nextInfo.PreviousVertex = info.Vertex;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startVertex"></param>
        /// <param name="endVertex"></param>
        /// <returns></returns>
        string GetPath(GraphVertex startVertex, GraphVertex endVertex)
        {
            var path = endVertex.ToString();

            while (startVertex != endVertex)
            {
                endVertex = GetGraphVertexInfo(endVertex).PreviousVertex;
                path = endVertex.ToString() + "->" + path;
            }

            return path;
        }
    }
}
