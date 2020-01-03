using System;

namespace AoC2019.Common
{
    public struct Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public static Position From((int x, int y) p) => new Position(p.x, p.y);

        public static Position From(int index, int width) => new Position(index % width, index / width);

        public Position Move(Direction direction) =>
            direction switch
            {
                Direction.Up => new Position(X, Y - 1),
                Direction.Down => new Position(X, Y + 1),
                Direction.Left => new Position(X - 1, Y),
                Direction.Right => new Position(X + 1, Y),
                _ => throw new ArgumentException("Invalid direction", nameof(direction)),
            };

        public override string ToString() => $"({X},{Y})";

        public bool Equals(Position other) =>
            X == other.X && Y == other.Y;

        public override bool Equals(object? obj) =>
            obj is Position other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }
    }
}
