namespace AOC2021.Day18
{
    public interface INode
    {
        public Pair Parent { get; set; }
        public int GetDepth();
        public long GetMagnitude();
        public INode Copy();
    }
}
