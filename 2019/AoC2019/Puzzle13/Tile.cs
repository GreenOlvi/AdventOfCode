namespace AoC2019.Puzzle13
{
    public struct Tile
    {
        public Tile(int x, int y, int type)
        {
            X = x;
            Y = y;
            Type = type;
        }

        public int X { get; }
        public int Y { get; }
        public int Type { get; }
    }
}
