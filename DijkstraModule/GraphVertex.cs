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
    public class GraphVertex
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<GraphEdge> Edges { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertexName"></param>
        public GraphVertex(string vertexName)
        {
            Name = vertexName;
            Edges = new List<GraphEdge>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddEdge(GraphEdge newEdge)
        {
            Edges.Add(newEdge);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="weight"></param>
        public void AddEdge(GraphVertex vertex, double weight)
        {
            AddEdge(new GraphEdge(vertex, weight));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;
    }
}
