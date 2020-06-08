namespace Dijkstra
{
    /// <summary>
    /// 
    /// </summary>
    public class GraphEdge
    {
        /// <summary>
        /// 
        /// </summary>
        public GraphVertex ConnectedVertex { get; }

        /// <summary>
        /// 
        /// </summary>
        public double EdgeWeight { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectedVertex"></param>
        /// <param name="weight"></param>
        public GraphEdge(GraphVertex connectedVertex, double weight)
        {
            ConnectedVertex = connectedVertex;
            EdgeWeight = weight;
        }
    }
}
