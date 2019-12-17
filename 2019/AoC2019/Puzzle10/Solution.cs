using AoC2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2019.Puzzle10
{
    public class Solution : IPuzzle
    {
        public Solution(IEnumerable<string> input)
        {
            _input = new Grid(ParseInput(input));
        }

        private readonly Grid _input;

        public static IEnumerable<Position> ParseInput(IEnumerable<string> input)
        {
            var y = 0;
            foreach (var line in input)
            {
                var x = 0;
                foreach (var f in line)
                {
                    if (f == '#')
                    {
                        yield return new Position(x, y);
                    }
                    x++;
                }
                y++;
            }
        }

        private static IEnumerable<(T1 First, T2 Second)> Cartesian<T1, T2>(IEnumerable<T1> a, IEnumerable<T2> b)
        {
            foreach (var i1 in a)
            {
                foreach (var i2 in b)
                {
                    yield return (i1, i2);
                }
            }
        }

        private static Position BestAsteroid(Grid grid)
        {
            return Cartesian(grid.Asteroids, grid.Asteroids)
                .Select(t => (t.First, t.Second, grid.IsVisible(t.First, t.Second)))
                .GroupBy(t => t.First)
                .Select(g => (Position: g.Key, Count: g.Where(t => t.Item3 == true).Count()))
                .OrderByDescending(p => p.Count)
                .First().Position;
        }

        public static int Solve1(Grid grid)
        {
            return Cartesian(grid.Asteroids, grid.Asteroids)
                .Select(t => (t.First, t.Second, grid.IsVisible(t.First, t.Second)))
                .GroupBy(t => t.First)
                .Select(g => (g.Key, g.Where(t => t.Item3 == true).Count()))
                .Max(i => i.Item2);
        }

        public static int Solve2(Grid grid, int n)
        {
            var p = BestAsteroid(grid);
            var a = grid.AsteroidsInShootingOrder(p).Skip(n - 1).First();
            return a.X * 100 + a.Y;
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_input).ToString());

        public Task<string> Solve2Async() =>
            Task.Run(() => Solve2(_input, 200).ToString());
    }
}
