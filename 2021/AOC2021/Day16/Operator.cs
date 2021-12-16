namespace AOC2021.Day16
{
    public record Operator : PacketBase
    {
        public byte LengthType { get; init; }
        public PacketBase[] Packets { get; init; }
        public override string ToString() =>
            $"{Type}({string.Join(", ", Packets.Select(p => p.ToString()))})";
    }
}
