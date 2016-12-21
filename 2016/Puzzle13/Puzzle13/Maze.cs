using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzle13
{
    public class Maze
    {
        public Maze(int seed)
        {
            Seed = seed;
        }

        private int Seed { get; }

        public List<Point> LocationsInRadius(Point startLocation, int radius)
        {
            throw new NotImplementedException();
        }

        public bool IsWalkable(Point location)
        {
            var f = location.X*location.X + 3*location.X + 2*location.X*location.Y
                    + location.Y + location.Y*location.Y + Seed;
            var bits = HammingWeight(f);
            return bits%2 == 0;
        }

        private static int HammingWeight(int value)
        {
            var sum = 0;

            while (value > 0)
            {
                sum += value & 0x01;
                value >>= 1;
            }

            return sum;
        }

        public IEnumerable<Point> GetAdjacentLocations(Point location)
        {
            return new[]
            {
                new Point(location.X, location.Y+1), 
                new Point(location.X, location.Y-1), 
                new Point(location.X+1, location.Y), 
                new Point(location.X-1, location.Y), 
            };
        }

        public string ToString(IEnumerable<Point> path)
        {
            var pathList = path.ToList();

            var startx = pathList.Min(p => p.X) - 1;
            var starty = pathList.Min(p => p.Y) - 1;
            var width = (pathList.Max(p => p.X) + 2) - startx;
            var height = (pathList.Max(p => p.Y) + 2) - starty;

            var sb = new StringBuilder();
            for (var y = starty; y < starty + height; y++)
            {
                var tmpY = y;

                sb.AppendLine(String.Concat(Enumerable.Range(startx, width)
                    .Select(x => new Point(x, tmpY))
                    .Select(l =>
                    {
                        return pathList.Contains(l) ? "O" : DrawLocation(l);
                    })));
            }
            return sb.ToString();
        }

        public string ToString(int startx, int starty, int width, int height)
        {
            var sb = new StringBuilder();
            for (var y = starty; y < starty + height; y++)
            {
                var tmpY = y;

                sb.AppendLine(String.Concat(Enumerable.Range(startx, width)
                    .Select(x => DrawLocation(new Point(x, tmpY)))));
            }
            return sb.ToString();
        }

        private string DrawLocation(Point location)
        {
            if (location.X == 1 && location.Y == 1)
            {
                return "s";
            }
            return IsWalkable(location) ? "." : "#";
        }
    }
}