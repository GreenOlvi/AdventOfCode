using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2019.Puzzle01
{
    public class Solution : IPuzzle
    {
        public Solution(string input)
        {
            _input = ParseInput(input);
        }

        public Solution(IEnumerable<string> input)
        {
            _input = ParseInput(input);
        }

        private readonly IEnumerable<int> _input;

        public static int Solve1(IEnumerable<int> input)
        {
            return input.Select(i => (i / 3) - 2).Sum();
        }

        public static int Solve2(IEnumerable<int> input)
        {
            return input.Select(i => CalculateFuel(i)).Sum();
        }

        private IEnumerable<int> ParseInput(IEnumerable<string> input) =>
            input.Select(i => int.Parse(i));

        private IEnumerable<int> ParseInput(string input) =>
            ParseInput(input.Split('\n', StringSplitOptions.RemoveEmptyEntries));

        private static int CalculateFuel(int mass)
        {
            var fuel = (mass / 3) - 2;
            return fuel > 0 ? fuel + CalculateFuel(fuel) : 0;
        }

        public async Task<string> Solve1Async()
        {
            return await Task.Run(() => Solve1(_input).ToString());
        }

        public async Task<string> Solve2Async()
        {
            return await Task.Run(() => Solve2(_input).ToString());
        }
    }
}
