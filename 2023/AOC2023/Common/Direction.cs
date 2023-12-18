namespace AOC2023.Common;

public enum Direction
{
    None = 0,
    Up,
    Down,
    Left,
    Right,
};

public static class DirectionExtensions
{
    private static readonly Dictionary<Direction, Direction> _opposite = new()
    {
        [Direction.Up] = Direction.Down,
        [Direction.Down] = Direction.Up,
        [Direction.Left] = Direction.Right,
        [Direction.Right] = Direction.Left,
    };

    public static Direction Opposite(this Direction direction) => _opposite[direction];

    public static IReadOnlyCollection<Direction> AllExceptNone =>
        [Direction.Up, Direction.Right, Direction.Down, Direction.Left];
}
