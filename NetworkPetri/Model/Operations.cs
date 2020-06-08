using System.Collections.Generic;
using System.Linq;

namespace NetworkPetri.Model
{
    public static class Operations
    {
        public static bool CanAddEdge(Item inputItem, Item outputItem, out string msg)
        {
            if (inputItem.Type == outputItem.Type)
            {
                msg = "Input and output are the same type. This is not allowed";
                return false;
            }

            msg = "OK";
            return true;
        }

        public static bool CanTransitionWork(this NetworkPetri network, Item transition, out string msg)
        {
            if (!network.Transitions.ContainsKey(transition.ID))
            {
                msg = $"Network doesn't contain transition with id = {transition.ID}";
                return false;
            }

            foreach (var edgeId in transition.Edges)
            {
                var edge = network.Edges[edgeId];
                if (edge.Input.Type == ItemType.Transition)
                    continue;
                else if (edge.Requirement > edge.Position.Value)
                {
                    msg = $"Input on Edge[{edgeId}] can not work. Not enough points ({edge.Requirement} > {edge.Position.Value})";
                    return false;
                }
            }

            msg = "OK";
            return true;
        }

        public static bool TryDoTransition(this NetworkPetri network, Item transition, out string msg)
        {
            if (!network.CanTransitionWork(transition, out msg))
                return false;

            foreach (var edgeId in transition.Edges)
            {
                var edge = network.Edges[edgeId];
                var item = edge.GetOppositeItem(transition.ID);

                if (item == edge.Input) // input in transition, withdraw marks
                    item._value -= edge.Requirement;
                else // output from transaction, add marks
                    item._value += edge.Requirement;
            }

            msg = "OK";
            return true;
        }

        public static bool TryDoTransition(this NetworkPetri network, int transitionId, out string msg)
        {
            if (network.Transitions.ContainsKey(transitionId))
            {
                var transition = network.Transitions[transitionId];
                return network.TryDoTransition(transition, out msg);
            }
            else
            {
                msg = $"Network does not contain transition [ID = {transitionId}]";
                return false;
            }
        }

        public static bool TryDoAllTransitions(this NetworkPetri network, out string msg)
        {
            Dictionary<int, int> positionNewValues = new Dictionary<int, int>();

            foreach (var transition in network.Transitions)
            {
                if (!network.CanTransitionWork(transition.Value, out msg))
                    return false;

                foreach (var edgeId in transition.Value.Edges)
                {
                    var edge = network.Edges[edgeId];
                    var item = edge.GetOppositeItem(transition.Value.ID);
                    if (!positionNewValues.ContainsKey(item.ID))
                        positionNewValues.Add(item.ID, item.Value);

                    if (item == edge.Input) // input in transition, withdraw marks
                        positionNewValues[item.ID] -= edge.Requirement;
                    else // output from transition, withdraw marks
                        positionNewValues[item.ID] += edge.Requirement;
                }
            }

            foreach (var positionDelta in positionNewValues)
                network.Positions[positionDelta.Key]._value = positionDelta.Value;

            msg = "OK";
            return true;
        }

        public static int GetItemId(string item)
        {
            int id = 0;
            if (item.Contains('('))
            {
                int endId = item.IndexOf('(');
                id = int.Parse(new string(item.Skip(1).Take(endId - 1).ToArray()));
            }
            else
            {
                id = int.Parse(new string(item.Skip(1).ToArray()));
            }

            return id;
        }

        public static int GetEdgeId(string edge)
        {
            int startId = edge.IndexOf('[') + 1;
            int endId = edge.IndexOf(']');
            int id = int.Parse(new string(edge.Skip(startId).Take(endId - startId).ToArray()));
            return id;
        }
    }
}
