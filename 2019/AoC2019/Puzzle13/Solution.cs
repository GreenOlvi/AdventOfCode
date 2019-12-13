using AoC2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2019.Puzzle13
{
    public partial class Solution : IPuzzle
    {
        public Solution(string input)
        {
            _input = ParseInput(input);
        }

        private readonly long[] _input;

        public static long[] ParseInput(string input) =>
            input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray();

        private static IEnumerable<Tile> ToTiles(IEnumerable<long> input)
        {
            var e = input.GetEnumerator();
            while (e.MoveNext())
            {
                var x = e.Current;
                e.MoveNext();
                var y = e.Current;
                e.MoveNext();
                var b = e.Current;
                yield return new Tile((int)x, (int)y, (int)b);
            }
        }

        private static int Solve1(IEnumerable<long> input)
        {
            var m = new IntcodeMachine(input);
            m.Run();

            var blocks = ToTiles(m.GetAllOutput()).ToList();

            return blocks.Count(b => b.Type == 2);
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_input).ToString());

        public Task<string> Solve2Async() =>
            throw new NotImplementedException();
    }
}
