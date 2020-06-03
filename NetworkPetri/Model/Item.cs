using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkPetri.Model
{
    public class Item : BaseItem
    {
        internal HashSet<int> _edges;
        internal ItemType _type;
        internal int _value;

        public ItemType Type => _type;
        public HashSet<int> Edges => _edges;
        public int Value => _value;

        private Item(int id, ItemType type, int value)
            : base(id)
        {
            _type = type;
            _edges = new HashSet<int>();
            _value = value;
        }

        internal static Item CreatePosition(int id, int value)
        {
            return new Item(id, ItemType.Position, value);
        }

        internal static Item CreateTransition(int id)
        {
            return new Item(id, ItemType.Transition, 1);
        }

        public override string ToString()
        {
            switch (Type)
            {
                case ItemType.Transition:
                    return $"T{ID}";
                case ItemType.Position:
                    return $"P{ID}({_value})";
                default:
                    return "NaN";
            }
        }
    }
}
