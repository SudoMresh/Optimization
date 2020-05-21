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
    public class GraphVertexInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public GraphVertex Vertex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsUnvisited { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double EdgesWeightSum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GraphVertex PreviousVertex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertex"></param>
        public GraphVertexInfo(GraphVertex vertex)
        {
            Vertex = vertex;
            IsUnvisited = true;
            EdgesWeightSum = double.MaxValue;
            PreviousVertex = null;
        }
    }
}
