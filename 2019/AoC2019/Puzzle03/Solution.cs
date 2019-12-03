using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2019.Puzzle03
{
    public class Solution : IPuzzle
    {
        public Solution(IEnumerable<string> input)
        {
            var wires = input.ToArray();
            _wire1 = ParseWire(wires[0]).ToArray();
            _wire2 = ParseWire(wires[1]).ToArray();
        }

        private readonly Wire[] _wire1;
        private readonly Wire[] _wire2;

        private static readonly IDictionary<Direction, Func<int, int, (int, int)>> DirectionChange =
            new Dictionary<Direction, Func<int, int, (int, int)>>()
            {
                { Direction.Up, (xo, yo) => (xo, yo - 1) },
                { Direction.Down, (xo, yo) => (xo, yo + 1) },
                { Direction.Left, (xo, yo) => (xo - 1, yo) },
                { Direction.Right, (xo, yo) => (xo + 1, yo) },
            };

        private static IEnumerable<(int, int)> Unwind(IEnumerable<Wire> wire)
        {
            var x = 0;
            var y = 0;

            foreach (var w in wire)
            {
                if (!DirectionChange.TryGetValue(w.Direction, out var next))
                {
                    throw new ArgumentOutOfRangeException(nameof(w.Direction));
                }

                foreach (var _ in Enumerable.Range(0, w.Distance))
                {
                    (x, y) = next(x, y);
                    yield return (x, y);
                }
            }
        }

        private static int ManhattanDistance((int, int) x, (int, int) y) =>
            Math.Abs(x.Item1 - y.Item1) + Math.Abs(x.Item2 - y.Item2);

        public static int Solve1(IEnumerable<Wire> w1, IEnumerable<Wire> w2)
        {
            var p1 = Unwind(w1).ToList();
            var p2 = Unwind(w2).ToList();
            var intersections = p1.Intersect(p2);

            return intersections.Min(i => ManhattanDistance((0, 0), i));
        }

        public static int Solve2(IEnumerable<Wire> w1, IEnumerable<Wire> w2)
        {
            var p1 = Unwind(w1).ToList();
            var p2 = Unwind(w2).ToList();
            var intersections = p1.Intersect(p2);

            return intersections.Min(i =>
            {
                var d1 = p1.FindIndex(p => i == p) + 1;
                var d2 = p2.FindIndex(p => i == p) + 1;
                return d1 + d2;
            });
        }

        public async Task<string> Solve1Async() =>
            await Task.Run(() => Solve1(_wire1, _wire2).ToString());

        public async Task<string> Solve2Async() =>
            await Task.Run(() => Solve2(_wire1, _wire2).ToString());

        public static IEnumerable<Wire> ParseWire(string input)
        {
            return input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(w => Wire.TryParse(w, out var wire) ? wire : throw new ArgumentException($"Not a proper Wire definition '{w}'"));
        }
    }
}
