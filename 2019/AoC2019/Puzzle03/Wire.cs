using System;

namespace AoC2019.Puzzle03
{
    public struct Wire
    {
        public Wire(Direction direction, int distance)
        {
            Direction = direction;
            Distance = distance;
        }

        public Direction Direction { get; }
        public int Distance { get; }

        public override string ToString()
        {
            return $"{Direction}({Distance})";
        }

        public static bool TryParse(string piece, out Wire wire)
        {
            if (!DirectionExtensions.TryParseDirection(piece[0], out var direction))
            {
                throw new ArgumentException($"Invalid direction '{piece[0]}'", nameof(piece));
            }

            if (!int.TryParse(piece.Substring(1), out var length))
            {
                throw new ArgumentException($"Wire length is not an integer", nameof(piece));
            }

            wire = new Wire(direction, length);
            return true;
        }

        public bool Equals(Wire other)
        {
            return Direction == other.Direction && Distance == other.Distance;
        }

        public override bool Equals(object obj)
        {
            return obj is Wire other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)Direction * 397) ^ Distance;
            }
        }

        public static bool operator ==(Wire left, Wire right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Wire left, Wire right)
        {
            return !(left == right);
        }
    }
}
