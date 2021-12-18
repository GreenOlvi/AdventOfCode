namespace AOC2021.Day18
{
    public record Number : INode
    {
        public long Value { get; set; }
        public Pair Parent { get; set; }

        public Number(long value)
        {
            Value = value;
        }

        public long GetMagnitude() => Value;

        public override string ToString() => Value.ToString();

        public INode Copy() => new Number(Value);

        public int GetDepth() => 0;
    }

}
