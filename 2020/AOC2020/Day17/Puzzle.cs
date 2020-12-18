using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AOC2020.Common;

namespace AOC2020.Day17
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> input)
        {
            _input = input.SelectMany((line, i) => ParseLine(line, i)).ToArray();
        }

        private readonly Point[] _input;

        public record CubeN
        {
            public CubeN(HashSet<Vector> points)
            {
                N = points.First().N;
                if (!points.All(p => p.N == N))
                    throw new InvalidOperationException("Points must have same dimensions");

                Points = points;
            }

            public CubeN(IEnumerable<Vector> points) : this(points.ToHashSet())
            {
            }

            public CubeN(int n, IEnumerable<Point> points) : this(points.Select(p => new Vector(n, p)))
            {
            }

            public int N { get; }
            public HashSet<Vector> Points { get; }
        }

        private static IEnumerable<Point> ParseLine(string line, int y) =>
            line.Select((c, i) => (c, i))
                .Where(p => p.c == '#')
                .Select(p => new Point(p.i, y));

        private static long CountActive(CubeN cube) => cube.Points.Count;

        public static IEnumerable<Vector> EnumeratePoints(Vector min, Vector max)
        {
            var n = min.N;
            if (n == 1)
            {
                for (var i = min[0]; i <= max[0]; i++)
                {
                    yield return new Vector(i);
                }
            }
            else
            {
                for (var i = min[n - 1]; i <= max[n - 1]; i++)
                {
                    var min1 = new Vector(min.Coords.Take(n - 1));
                    var max1 = new Vector(max.Coords.Take(n - 1));
                    foreach (var v1 in EnumeratePoints(min1, max1))
                    {
                        yield return new Vector(v1.Coords.Append(i));
                    }
                }
            }
        }

        private static (Vector Min, Vector Max) GetRange(CubeN cube)
        {
            var min = new Vector(Enumerable.Range(0, cube.N).Select(i => cube.Points.Min(p => p[i])));
            var max = new Vector(Enumerable.Range(0, cube.N).Select(i => cube.Points.Max(p => p[i])));
            return (min, max);
        }
 
        private static int CountNeighbours(CubeN cube, Vector point) =>
            EnumeratePoints(point - Vector.One(point.N), point + Vector.One(point.N))
                .Where(p => p != point)
                .Count(p => cube.Points.Contains(p));

        private static bool TransformPoint(CubeN cube, Vector point)
        {
            var isActive = cube.Points.Contains(point);
            var n = CountNeighbours(cube, point);

            return (isActive && (n == 2 || n == 3)) || (!isActive && n == 3);
        }


        private static CubeN Transform(CubeN cube)
        {
            var (min, max) = GetRange(cube);
            var points = EnumeratePoints(min - Vector.One(cube.N), max + Vector.One(cube.N))
                .Where(p => TransformPoint(cube, p))
                .ToArray();
            return new CubeN(points);
        }

        public static string PrintCube(CubeN cube)
        {
            if (cube.N != 3) throw new InvalidOperationException();

            var sb = new StringBuilder();
            var (min, max) = GetRange(cube);

            for (var z = min[2]; z <= max[2]; z++)
            {
                sb.AppendLine($"z={z}");
                for (var y = min[1]; y <= max[1]; y++)
                {
                    for (var x = min[0]; x <= max[0]; x++)
                    {
                        sb.Append(cube.Points.Contains(new Vector(x, y, z)) ? '#' : '.');
                    }
                    sb.AppendLine();
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public override long Solution1()
        {
            var initN = new CubeN(3, _input);
            var cubeN = Enumerable.Range(0, 6).Aggregate(initN, (a, b) => Transform(a));
            return CountActive(cubeN);
        }

        public override long Solution2()
        {
            var initN = new CubeN(4, _input);
            var cubeN = Enumerable.Range(0, 6).Aggregate(initN, (a, b) => Transform(a));
            return CountActive(cubeN);
        }
    }
}
