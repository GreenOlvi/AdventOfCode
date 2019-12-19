using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoC2019.Common;

namespace AoC2019.Puzzle19
{
    public class Solution : IPuzzle
    {
        public Solution(string input)
        {
            _input = IntcodeMachine.ParseInput(input).ToArray();
        }

        private readonly long[] _input;

        private static bool CheckPoint(IEnumerable<long> arr, long x, long y)
        {
            if (x < 0 || y < 0)
            {
                return false;
            }
            var m = new IntcodeMachine(arr);
            m.AddInput(x, y);
            m.Run();
            return m.GetOutput() == 1;
        }

        private static Func<long, long, bool> Checker(IEnumerable<long> arr) => (x, y) => CheckPoint(arr, x, y);

        private static IEnumerable<long> IntsUp(long start = 0)
        {
            var n = start;
            while (true)
            {
                yield return n++;
            }
        }

        public static bool CheckFits(long x, long y, int n, Func<long, long, bool> checker) =>
            checker(x, y) && checker(x - n + 1, y + n - 1);

        public static long Solve1(IEnumerable<long> input, int width, int height)
        {
            var checker = Checker(input);
            var grid = new Grid(width, height);
            foreach (var y in Enumerable.Range(0, height))
            {
                foreach (var x in Enumerable.Range(0, width))
                {
                    var f = checker(x, y) ? Field.Affected : Field.Stationary;
                    grid.Set(x, y, f);
                }
            }

            return grid.GetAffected();
        }

        public static long Solve2(IEnumerable<long> input, int n)
        {
            var checker = Checker(input);

            var x = 100L;
            var y = 0L;
            while (true)
            {
                y = IntsUp(y).First(yp => checker(x, yp));
                if (CheckFits(x, y, n, checker))
                {
                    break;
                }
                x++;
            }

            var x0 = x - n + 1;
            var y0 = y;

            return x0 * 10_000 + y0;
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_input, 50, 50).ToString());

        public Task<string> Solve2Async() =>
            Task.Run(() => Solve2(_input, 100).ToString());
    }
}
