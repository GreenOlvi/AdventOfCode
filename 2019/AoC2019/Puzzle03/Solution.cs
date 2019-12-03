using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2019.Puzzle03
{
    public partial class Solution : IPuzzle
    {
        public Solution(IEnumerable<string> input)
        {
            _input = input.Select(line => ParseWire(line).ToList()).ToList();
        }

        private readonly List<List<Wire>> _input;

        private static IEnumerable<(int, int)> Unwind(IEnumerable<Wire> wire)
        {
            var x = 0;
            var y = 0;

            foreach (var w in wire)
            {
                switch (w.Direction)
                {
                    case Direction.Up:
                        foreach (var i in Enumerable.Range(0, w.Distance))
                        {
                            y--;
                            yield return (x, y);
                        }
                        break;
                    case Direction.Down:
                        foreach (var i in Enumerable.Range(0, w.Distance))
                        {
                            y++;
                            yield return (x, y);
                        }
                        break;
                    case Direction.Left:
                        foreach (var i in Enumerable.Range(0, w.Distance))
                        {
                            x--;
                            yield return (x, y);
                        }
                        break;
                    case Direction.Right:
                        foreach (var i in Enumerable.Range(0, w.Distance))
                        {
                            x++;
                            yield return (x, y);
                        }
                        break;
                }
            }
        }

        public static int ManhattanDistance((int, int) x, (int, int) y) =>
            Math.Abs(x.Item1 - y.Item1) + Math.Abs(x.Item2 - y.Item2);

        public static int Solve1(IEnumerable<IEnumerable<Wire>> wires)
        {
            var positions = wires.Select(Unwind).ToArray();
            var intersections = positions[0].Intersect(positions[1]);

            return intersections.Min(i => ManhattanDistance((0, 0), i));
        }

        public static int Solve2(IEnumerable<IEnumerable<Wire>> wires)
        {
            var positions = wires.Select(w => Unwind(w).ToList()).ToArray();
            var intersections = positions[0].Intersect(positions[1]);

            return intersections.Select(i =>
            {
                var w1 = positions[0].FindIndex(p => i == p) + 1;
                var w2 = positions[1].FindIndex(p => i == p) + 1;
                return w1 + w2;
            }).Min();
        }

        public async Task<string> Solve1Async() =>
            await Task.Run(() => Solve1(_input).ToString());

        public async Task<string> Solve2Async() =>
            await Task.Run(() => Solve2(_input).ToString());

        public static IEnumerable<Wire> ParseWire(string input)
        {
            return input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(Wire.Parse);
        }
    }
}
