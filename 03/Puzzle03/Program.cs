using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Puzzle03
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var filename in args)
            {
                Console.WriteLine(filename);
                var directions = GetDirections(filename).ToList();

                var navi = Navigate(directions);
                var sum = navi.Visited.Count(x => x.Value > 0);
                Console.WriteLine(@"Santa: {0}", sum);

                var lists = directions
                    .Select((x, i) => new {Index = i, Value = x})
                    .GroupBy(x => x.Index%2)
                    .Select(x => x.Select(v => v.Value).ToList()).ToList();

                var santa = Navigate(lists[0]);
                var robot = Navigate(lists[1]);

                var visited = santa.Visited
                    .Union(robot.Visited)
                    .GroupBy(v => v.Key)
                    .ToDictionary(x => x.Key, v => v.Sum(a => a.Value));

                var sumBoth = visited.Count(x => x.Value > 0);
                Console.WriteLine(@"Santa and Robot: {0}", sumBoth);
            }

            Console.ReadLine();
        }

        private static Navi Navigate(IEnumerable<char> directions)
        {
            var navi = new Navi();

            foreach (var direction in directions)
            {
                navi.Go(direction);
            }

            return navi;
        }

        private static IEnumerable<char> GetDirections(string filename)
        {
            char[] list;

            using (var file = new StreamReader(filename))
            {
                list = file.ReadLine()?.ToCharArray();
            }

            return list;
        }

        private class Navi
        {
            public Navi()
            {
                Position = new Point(0, 0);
                Visited = new Dictionary<Point, int> {{Position, 1}};
            }

            public Dictionary<Point, int> Visited { get; }
            public Point Position { get; private set; }

            public void Go(char direction)
            {
                switch (direction)
                {
                    case '<':
                        Position = new Point(Position.X - 1, Position.Y);
                        break;
                    case '>':
                        Position = new Point(Position.X + 1, Position.Y);
                        break;
                    case '^':
                        Position = new Point(Position.X, Position.Y + 1);
                        break;
                    case 'v':
                        Position = new Point(Position.X, Position.Y - 1);
                        break;
                    default:
                        throw new ArgumentException("Invalid input (" + direction + ")", "direction");
                }

                if (Visited.ContainsKey(Position))
                {
                    Visited[Position]++;
                }
                else
                {
                    Visited.Add(Position, 1);
                }
            }

            public void DumpVisited()
            {
                foreach (var v in Visited)
                {
                    Console.WriteLine("{0} -> {1}", v.Key, v.Value);
                }
            }
        }

        private struct Point
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; }
            public int Y { get; }

            public override string ToString()
            {
                return string.Format(@"({0},{1})", X, Y);
            }

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
        }
    }
}