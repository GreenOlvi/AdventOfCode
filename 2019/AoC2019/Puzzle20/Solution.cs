using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2019.Puzzle20
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
            var m = Map.Parse(input);
            return m.ShortestPath();
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_input).ToString());

        public Task<string> Solve2Async() =>
            throw new NotImplementedException();
    }
}
