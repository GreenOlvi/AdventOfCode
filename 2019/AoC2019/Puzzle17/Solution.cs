using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoC2019.Common;

namespace AoC2019.Puzzle17
{
    public class Solution : IPuzzle
    {
        public Solution(string input)
        {
            _input = IntcodeMachine.ParseInput(input).ToArray();
        }

        private readonly long[] _input;

        public static long Solve1(IEnumerable<long> input)
        {
            var m = new IntcodeMachine(input);
            m.Run();

            var grid = new Grid(m.GetAllOutput().Select(o => (char)o));
            return grid.GetIntersections().Select(p => p.X * p.Y).Sum();
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_input).ToString());

        public Task<string> Solve2Async() =>
            throw new NotImplementedException();
    }
}
