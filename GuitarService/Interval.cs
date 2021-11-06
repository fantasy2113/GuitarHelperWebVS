namespace GuitarService
{
    public sealed class Interval
    {
        private readonly bool[] Structure;

        public Interval(bool root, bool t1, bool t2, bool t3, bool t4, bool t5, bool t6, bool t7, bool t8, bool t9, bool t10, bool t11)
        {
            Structure = new bool[] { root, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11 };
        }

        public bool[] GetStructure()
        {
            return Structure;
        }
    }
}
