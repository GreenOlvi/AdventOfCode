using AoC2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2019.Puzzle11
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

        public static int Solve1(IEnumerable<long> input)
        {
            var grid = new Grid();

            var robot = new Robot(new IntcodeMachine(input), grid);
            robot.Run();

            return robot.Visited;
        }

        public static string Solve2(IEnumerable<long> input)
        {
            var grid = new Grid();
            grid.SetColor(new Position(0, 0), GridColor.White);

            var robot = new Robot(new IntcodeMachine(input), grid);
            robot.Run();

            return grid.Draw(robot);
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_input).ToString());

        public Task<string> Solve2Async() =>
            Task.Run(() => Solve2(_input));
    }
}
