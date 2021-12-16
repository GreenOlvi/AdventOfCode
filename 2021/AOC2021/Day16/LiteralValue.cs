namespace AOC2021.Day16
{
    public record LiteralValue : PacketBase
    {
        public LiteralValue() : base()
        {
            Type = PacketType.LiteralValue;
        }

        public long Value { get; init; }
        public override string ToString() => Value.ToString();
    }
}
