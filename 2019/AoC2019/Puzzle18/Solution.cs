using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2019.Puzzle18
{
    public class Solution : IPuzzle
    {
        public Solution(IEnumerable<string> input)
        {
            _input = input.ToArray();
        }

        private readonly string[] _input;

        public static int Solve1(string[] input)
        {
            var map = Map.Parse(input.ToArray());
            var (_, steps) = map.FindShortestWay('@', Array.Empty<char>());
            return steps;
        }

        public static int Solve2(string[] input)
        {
            return 0;
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_input).ToString());

        public Task<string> Solve2Async() =>
            Task.Run(() => Solve2(_input).ToString());
    }
}
