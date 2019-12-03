namespace AoC2019.Puzzle03
{
    public enum Direction
    {
        None = 0,
        Up,
        Down,
        Left,
        Right,
    }

    public static class DirectionExtensions
    {
        public static bool TryParseDirection(char c, out Direction direction)
        {
            switch (c)
            {
                case 'U': direction = Direction.Up; return true;
                case 'D': direction = Direction.Down; return true;
                case 'R': direction = Direction.Right; return true;
                case 'L': direction = Direction.Left; return true;
                default: direction = Direction.None; return false;
            }
        }
    }
}
