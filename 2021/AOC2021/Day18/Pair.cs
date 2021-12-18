namespace AOC2021.Day18
{
    public record Pair : INode
    {
        private INode _left;
        private INode _right;

        public INode Left
        {
            get => _left;
            set
            {
                value.Parent = this;
                _left = value;
            }
        }

        public INode Right
        {
            get => _right;
            set
            {
                value.Parent = this;
                _right = value;
            }
        }

        public Pair Parent { get; set; }

        public Pair(INode left, INode right)
        {
            Left = left;
            Right = right;
        }

        public int GetDepth()
        {
            return Math.Max(Left.GetDepth(), Right.GetDepth()) + 1;
        }

        public long GetMagnitude() => 3 * Left.GetMagnitude() + 2 * Right.GetMagnitude();

        public override string ToString() => $"[{Left},{Right}]";

        public INode Copy() => new Pair(Left.Copy(), Right.Copy());

        public Side GetSide(INode node)
        {
            if (ReferenceEquals(Left, node))
            {
                return Side.Left;
            }
            if (ReferenceEquals(Right, node))
            {
                return Side.Right;
            }
            return Side.None;
        }
    }

}
