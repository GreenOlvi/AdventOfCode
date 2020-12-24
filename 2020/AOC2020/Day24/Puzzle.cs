using AOC2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Day24
{
    public class Puzzle : PuzzleBase<int, int>
    {
        public Puzzle(IEnumerable<string> input)
        {
            _input = input.Select(line => ParseLine(line).ToArray()).ToArray();
        }

        private readonly Direction[][] _input;

        public enum Direction { E, SE, SW, W, NW, NE };

        public static IEnumerable<Direction> ParseLine(string line)
        {
            var i = 0;
            while (i < line.Length)
            {
                yield return line[i] switch
                {
                    'e' => Direction.E,
                    'w' => Direction.W,
                    's' => line[++i] switch
                        {
                            'e' => Direction.SE,
                            'w' => Direction.SW,
                            _ => throw new PuzzleException($"Invalid direction: [{line[i - 1]}{line[i]}]"),
                        },
                    'n' => line[++i] switch
                        {
                            'e' => Direction.NE,
                            'w' => Direction.NW,
                            _ => throw new PuzzleException($"Invalid direction: [{line[i - 1]}{line[i]}]"),
                        },
                    _ => throw new PuzzleException($"Invalid direction: [{line[i]}]"),
                };
                i++;
            }
        }

        private static readonly Dictionary<Direction, Point> DirectionOffset = new Dictionary<Direction, Point>
        {
            { Direction.E,  new Point(1, 0) },
            { Direction.SE, new Point(0, 1) },
            { Direction.SW, new Point(-1, 1) },
            { Direction.W,  new Point(-1, 0) },
            { Direction.NW, new Point(0, -1) },
            { Direction.NE, new Point(1, -1) },
        };

        private static Point Move(Point p, Direction d) => p + DirectionOffset[d];

        private static Point Move(Point p, IEnumerable<Direction> path) =>
            path.Aggregate(p, (p, d) => Move(p, d));

        public override int Solution1()
        {
            var start = new Point(0, 0);
            var black = new HashSet<Point>();
            foreach (var path in _input)
            {
                var dest = Move(start, path);
                if (black.Contains(dest))
                {
                    black.Remove(dest);
                }
                else
                {
                    black.Add(dest);
                }
            }

            return black.Count;
        }

        public override int Solution2()
        {
            return 0;
        }
    }
}
