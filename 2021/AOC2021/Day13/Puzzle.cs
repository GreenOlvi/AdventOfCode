using System.Text.RegularExpressions;
using AOC2021.Common;

namespace AOC2021.Day13
{
    public class Puzzle : PuzzleBase<long, string>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            _input = lines.TakeWhile(l => l != string.Empty)
                .Select(l => _pointPattern.Parse(l, ("x", int.Parse), ("y", int.Parse)))
                .Select(p => new Point(p))
                .ToArray();

            _folds = lines.Skip(_input.Length + 1)
                .Select(line => _foldPattern.Parse(line, ("axis", x => x), ("value", int.Parse)))
                .ToArray();
        }

        private readonly Regex _pointPattern = new(@"^(?<x>\d+),(?<y>\d+)$", RegexOptions.Compiled);
        private readonly Regex _foldPattern = new(@"^fold along (?<axis>\w)=(?<value>\d+)$", RegexOptions.Compiled);

        private readonly Point[] _input;
        private readonly (string Axis, int Value)[] _folds;

        public static Point FoldX(Point p, int value) => p.X < value ? p : new Point(2 * value - p.X, p.Y);
        public static Point FoldY(Point p, int value) => p.Y < value ? p : new Point(p.X, 2 * value - p.Y);

        private static IEnumerable<Point> FoldX(IEnumerable<Point> points, int value) => points.Select(p => FoldX(p, value));
        private static IEnumerable<Point> FoldY(IEnumerable<Point> points, int value) => points.Select(p => FoldY(p, value));

        private static IEnumerable<Point> Fold(IEnumerable<Point> points, (string, int) fold) =>
            fold.Item1 == "x" ? FoldX(points, fold.Item2) : FoldY(points, fold.Item2);

        public override long Solution1() => Fold(_input, _folds[0]).Distinct().Count();

        public override string Solution2() => _folds.Aggregate((IEnumerable<Point>)_input, (set, f) => Fold(set, f).Distinct()).Print();
    }
}
