using System;

namespace Puzzle13
{
    public struct Point
    {
        public Point(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Point && Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X*397) ^ Y;
            }
        }

        public override string ToString()
        {
            return String.Format("[{0}, {1}]", X, Y);
        }

        public string ToShortString()
        {
            return X + "," + Y;
        }
    }
}