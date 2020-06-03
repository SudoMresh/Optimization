using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkPetri.Model
{
    public class Edge : BaseItem
    {
        internal Item _input;
        internal Item _output;
        internal int _requirement;

        public Item Input => _input;
        public Item Output => _output;
        public int Requirement => _requirement;

        public Item GetOppositeItem(int id) => _input.ID == id ? _output : _input;
        public Item Transition => _input.Type == ItemType.Transition ? _input : _output;
        public Item Position => _input.Type == ItemType.Position ? _input : _output;

        public Edge(int id, Item input, Item output, int requirement)
            : base(id)
        {
            _input = input;
            _output = output;
            _requirement = requirement;
            _input.Edges.Add(id);
            _output.Edges.Add(id);
        }

        public override string ToString()
        {
            return $"[E{ID}] {_input} --({_requirement})-> {_output}";
        }
    }
}
