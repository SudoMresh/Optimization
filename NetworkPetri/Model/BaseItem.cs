using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkPetri.Model
{
    public class BaseItem
    {
        internal int _id;

        public int ID => _id;

        public BaseItem(int id)
        {
            _id = id;
        }
    }
}
