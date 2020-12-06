using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Day03
{
    public class Puzzle : PuzzleBase<int, long>
    {
        public Puzzle(IEnumerable<string> input)
        {
            _input = input.ToArray();
            _width = _input[0].Length;
        }

        private readonly string[] _input;
        private readonly int _width;

        private IEnumerable<(int X, int Y)> SlopeTiles(int dx, int dy)
        {
            var a = (.0 + dx) / dy;
            for (var y = 0; y < _width; y += dy)
            {
                var x = (int)(y * a) % _input[0].Length;
                yield return (x, y);
            }
        }

        public int CountTrees(int dx, int dy) =>
            SlopeTiles(dx, dy).Count(p => _input[p.Y][p.X] == '#');

        public override int Solution1() => CountTrees(3, 1);

        public override long Solution2() =>
            new (int X, int Y)[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) }
                .Select(p => CountTrees(p.X, p.Y))
                .Aggregate(1L, (a, b) => a * b);
    }
}
