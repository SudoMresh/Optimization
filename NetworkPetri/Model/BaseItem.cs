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
