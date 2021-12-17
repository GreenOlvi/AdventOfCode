using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using AOC2021.Common;

namespace AOC2021.Day17
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            if (!InputPattern.TryParseMany(lines.First(),
                ("x1", int.Parse),
                ("x2", int.Parse),
                ("y1", int.Parse),
                ("y2", int.Parse),
                out var input))
            {
                throw new InvalidDataException();
            }

            _target = new Rectangle
            {
                Left = input.Item1,
                Right = input.Item2,
                Bottom = input.Item3,
                Top = input.Item4,
            };
        }

        private static readonly Regex InputPattern = new(@"^target area: x=(?<x1>-?\d+)..(?<x2>-?\d+), y=(?<y1>-?\d+)..(?<y2>-?\d+)$");

        private readonly Point _start = Point.Zero;
        private readonly Rectangle _target;

        private static IEnumerable<Point> GetTrajectory(Point start, Point v)
        {
            var p = start;
            var currentV = v;
            while (true)
            {
                p += currentV;
                yield return p;

                currentV += new Point(currentV.X > 0 ? -1 : 0, -1);
            }
        }

        private static bool Hits(Point start, Point v, Rectangle target)
        {
            foreach (var p in GetTrajectory(start, v))
            {
                if (target.IsInside(p))
                {
                    return true;
                }

                if (p.X > target.Right || p.Y < target.Bottom)
                {
                    return false;
                }
            }
            return false;
        }

        private static long GetMaxY(Point start, Point v) =>
            v.Y > start.Y ? v.Y * (v.Y + 1) / 2 : v.Y;

        private static IEnumerable<Point> FindAllHits(Point start, Rectangle target)
        {
            for (var dx = 1; dx <= target.Right; dx++)
            {
                for (var dy = target.Bottom; dy <= target.Right; dy++)
                {
                    var v = new Point(dx, dy);
                    if (Hits(start, v, target))
                    {
                        yield return v;
                    }
                }
            }
        }

        public IEnumerable<Point> FindAllHits() => FindAllHits(_start, _target);

        public override long Solution1() => FindAllHits().Max(v => GetMaxY(_start, v));

        public override long Solution2() => FindAllHits().Count();

        private static void Print(Point start, Rectangle target, IEnumerable<Point> trajectory = default)
        {
            if (OperatingSystem.IsWindows())
            {
                var points = trajectory?.ToArray() ?? Array.Empty<Point>();
                var minX = points.Select(p => p.X).Concat(new[] { start.X, target.Left, target.Right }).Min();
                var maxX = points.Select(p => p.X).Concat(new[] { start.X, target.Left, target.Right }).Max();
                var minY = points.Select(p => p.Y).Concat(new[] { start.Y, target.Top, target.Bottom }).Min();
                var maxY = points.Select(p => p.Y).Concat(new[] { start.Y, target.Top, target.Bottom }).Max();

                Console.SetWindowPosition(0, 0);
                Console.SetWindowSize(200, 84);
                Console.Clear();

                Console.SetCursorPosition((int)(start.X - minX), (int)(maxY - start.Y));
                Console.Write("S");

                for (var y = target.Top; y >= target.Bottom; y--)
                {
                    Console.SetCursorPosition((int)(target.Left - minX), (int)(maxY - y));
                    for (var x = 0; x < target.Width; x++)
                    {
                        Console.Write("T");
                    }
                }

                foreach (var p in points)
                {
                    Console.SetCursorPosition((int)(p.X - minX), (int)(maxY - p.Y));
                    Console.Write("#");
                }

                Console.ReadLine();
            }
        }

    }

    public record Rectangle
    {
        public int Left { get; init; }
        public int Right { get; init; }
        public int Top { get; init; }
        public int Bottom { get; init; }
        public int Width => Right - Left + 1;
        public int Height => Top - Bottom + 1;

        public override string ToString() => $"[{Left}, {Top}, {Right}, {Bottom}]";

        public bool IsInside(Point p) =>
            p.X >= Left && p.X <= Right && p.Y <= Top && p.Y >= Bottom;
    }
}
