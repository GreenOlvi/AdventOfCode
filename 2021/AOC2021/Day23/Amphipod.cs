namespace AOC2021.Day23
{
    public enum Amphipod { None, A, B, C, D };

    public static class AmphipodExtensions
    {
        private static readonly Dictionary<Amphipod, char> _chars = new()
        {
            [Amphipod.None] = ' ',
            [Amphipod.A] = 'A',
            [Amphipod.B] = 'B',
            [Amphipod.C] = 'C',
            [Amphipod.D] = 'D',
        };

        public static char ToChar(this Amphipod a) => _chars[a];
    }
}
