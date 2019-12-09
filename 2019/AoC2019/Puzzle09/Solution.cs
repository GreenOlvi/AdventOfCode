using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2019.Puzzle09
{
    public class Solution : IPuzzle
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

        public static long Solve(IEnumerable<long> code, long input)
        {
            var m = new IntcodeMachine(code);
            m.AddInput(input);
            m.Run();
            return m.Output.Dequeue();
        }

        public Task<string> Solve1Async()
            => Task.Run(() => Solve(_input, 1).ToString());

        public Task<string> Solve2Async()
            => Task.Run(() => Solve(_input, 2).ToString());
    }
}
