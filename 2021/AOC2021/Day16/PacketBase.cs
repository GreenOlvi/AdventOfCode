namespace AOC2021.Day16
{
    public abstract record PacketBase
    {
        public byte Version { get; init; }
        public PacketType Type { get; init; }
    }
}
