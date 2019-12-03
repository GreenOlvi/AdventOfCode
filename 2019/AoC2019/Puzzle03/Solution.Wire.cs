using System;

namespace AoC2019.Puzzle03
{
    public partial class Solution
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

            public static Wire Parse(string piece)
            {
                var d = ParseDirection(piece[0]);
                var len = int.Parse(piece.Substring(1));
                return new Wire(d, len);
            }

            public static Direction ParseDirection(char d)
            {
                switch (d)
                {
                    case 'U': return Direction.Up;
                    case 'D': return Direction.Down;
                    case 'R': return Direction.Right;
                    case 'L': return Direction.Left;
                    default: throw new ArgumentOutOfRangeException(nameof(d));
                }
            }
        }
    }
}
