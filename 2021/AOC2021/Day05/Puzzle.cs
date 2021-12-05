using System.Text.RegularExpressions;
using MoreLinq;
using AOC2021.Common;

namespace AOC2021.Day05
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            _lines = lines.Select(l => ParseLine(l)).ToArray();
        }

        private readonly Line[] _lines;

        private static Line ParseLine(string l)
        {
            var ints = l.Split(new[] { "->", "," }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .ParseInts()
                .ToArray();

            return new Line(new Point(ints[0], ints[1]), new Point(ints[2], ints[3]));
        }

        private static long CountOverlapping(IEnumerable<Line> lines)
        {
            var dict = new Dictionary<Point, int>();
            foreach (var p in lines.SelectMany(l => l.GetPoints()))
            {
                if (dict.TryGetValue(p, out var i))
                {
                    dict[p] = i + 1;
                }
                else
                {
                    dict[p] = 1;
                }
            }

            return dict.Count(p => p.Value > 1);
        }

        public override long Solution1() => CountOverlapping(_lines.Where(l => l.IsHorOrVert()));

        public override long Solution2() => CountOverlapping(_lines);

        private class Line
        {
            public Line(Point start, Point end)
            {
                _start = start;
                _end = end;
            }

            private readonly Point _start;
            private readonly Point _end;

            public bool IsHorOrVert() => _start.X == _end.X || _start.Y == _end.Y;

            public IEnumerable<Point> GetPoints()
            {
                if (_start == _end)
                {
                    yield return _start;
                    yield break;
                }

                var dx = Math.Sign(_end.X - _start.X);
                var dy = Math.Sign(_end.Y - _start.Y);

                var s = _start;
                while (s != _end)
                {
                    yield return s;
                    s += new Point(dx, dy);
                }
                yield return s;
            }
        }
    }
}
