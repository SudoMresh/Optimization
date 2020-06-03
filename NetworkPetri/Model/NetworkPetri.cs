using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NetworkPetri.Model
{
    public class NetworkPetri
    {
        internal Dictionary<int, Item> _transitions;
        internal Dictionary<int, Item> _positions;
        internal Dictionary<int, Edge> _edges;

        public Dictionary<int, Item> Transitions => _transitions;
        public Dictionary<int, Item> Positions => _positions;
        public Dictionary<int, Edge> Edges => _edges;

        public NetworkPetri()
        {
            _transitions = new Dictionary<int, Item>();
            _positions = new Dictionary<int, Item>();
            _edges = new Dictionary<int, Edge>();
        }

        public int NextIdTransitions => _transitions.Count > 0 ? _transitions.Keys.Max() + 1 : 1;
        public int NextIdPositions => _positions.Count > 0 ? _positions.Keys.Max() + 1 : 1;
        public int NextIdEdges => _edges.Count > 0 ? _edges.Keys.Max() + 1 : 1;

        public bool ContainsEdge(Item input, Item output)
        {
            foreach (var edgeId in input.Edges)
            {
                var edge = Edges[edgeId];
                if (edge.Output.Type == output.Type && edge.Output.ID == output.ID)
                    return true;
            }
            return false;
        }

        public Item AddPosition(int value)
        {
            int id = NextIdPositions;
            var position = Item.CreatePosition(id, value);
            _positions.Add(id, position);
            return position;
        }

        public Item AddTransition()
        {
            int id = NextIdTransitions;
            var transition = Item.CreateTransition(id);
            _transitions.Add(id, transition);
            return transition;
        }

        public Edge AddEdge(Item input, Item output, int requirement)
        {
            if (!Operations.CanAddEdge(input, output, out string msg))
                throw new InvalidOperationException(msg);

            int id = NextIdEdges;
            var edge = new Edge(id, input, output, requirement);
            _edges.Add(id, edge);
            return edge;
        }

        public void RemovePosition(int id)
        {
            if (!_positions.ContainsKey(id))
                return;
            var position = _positions[id];
            foreach (var edgeId in position.Edges)
            {
                var edge = _edges[edgeId];
                edge.GetOppositeItem(id)._edges.Remove(edgeId);
                _edges.Remove(edgeId);
            }
            _positions.Remove(id);
        }

        public void RemoveTransition(int id)
        {
            if (!_transitions.ContainsKey(id))
                return;
            var transition = _transitions[id];
            foreach (var edgeId in transition.Edges)
            {
                var edge = _edges[edgeId];
                edge.GetOppositeItem(id)._edges.Remove(edgeId);
                _edges.Remove(edgeId);
            }
            _transitions.Remove(id);
        }

        public void RemoveEdge(int id)
        {
            if (!_edges.ContainsKey(id))
                return;
            var edge = _edges[id];
            edge.Input._edges.Remove(id);
            edge.Output._edges.Remove(id);
            _edges.Remove(id);
        }
    }
}
