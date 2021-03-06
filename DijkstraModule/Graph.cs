﻿using DijkstraModule;
using System.Collections.Generic;

namespace Dijkstra
{
    /// <summary>
    /// 
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GraphVertex> Vertices { get; }

        /// <summary>
        /// 
        /// </summary>
        public Graph()
        {
            Vertices = new List<GraphVertex>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertexName"></param>
        public void AddVertex(string vertexName)
        {
            Vertices.Add(new GraphVertex(vertexName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertexName"></param>
        /// <returns></returns>
        public GraphVertex FindVertex(string vertexName)
        {
            foreach (var v in Vertices)
            {
                if (v.Name.Equals(vertexName))
                {
                    return v;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="secondName"></param>
        /// <param name="weight"></param>
        public void AddEdge(string firstName, string secondName, double weight)
        {
            var v1 = FindVertex(firstName);
            var v2 = FindVertex(secondName);
            if (v2 != null && v1 != null)
            {
                v1.AddEdge(v2, weight);
                v2.AddEdge(v1, weight);
            }
            else
            {
                NotFoundEdgeExeption exeption = new NotFoundEdgeExeption();

                if (v1 == null)
                    exeption.NotFoundFirstEdge = firstName;

                if (v2 == null)
                    exeption.NotFoundSecondEdge = secondName;

                throw exeption;
            }
        }
    }
}
