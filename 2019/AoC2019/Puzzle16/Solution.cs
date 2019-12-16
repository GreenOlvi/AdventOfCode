using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2019.Puzzle16
{
    public class Solution : IPuzzle
    {
        public Solution(string input)
        {
            _input = input;
        }

        private static IEnumerable<int> ParseInput(string input) => input.Trim().Select(ch => ch - '0');

        private readonly string _input;

        private static readonly int[] _initialPattern = new[] { 0, 1, 0, -1 };

        private static IEnumerable<int> InitialPattern(int n)
        {
            while (true)
            {
                foreach (var i in _initialPattern)
                {
                    foreach (var e in Enumerable.Repeat(i, n))
                    {
                        yield return e;
                    }
                }
            }
        }

        public static IEnumerable<int> Pattern(int n) => InitialPattern(n).Skip(1);

        public static IEnumerable<int> FFT(IEnumerable<int> input)
        {
            var inp = input.ToArray();
            foreach (var i in Enumerable.Range(0, inp.Length))
            {
                var r = inp.Zip(Pattern(i + 1)).Select(p => p.First * p.Second).Sum();
                yield return Math.Abs(r) % 10;
            }
        }

        public static string Solve1(string input)
        {
            var signal = ParseInput(input);
            foreach (var _ in Enumerable.Range(0, 100))
            {
                signal = FFT(signal);
            }
            return string.Join("", signal.Take(8));
        }

        public static string Solve2(string input)
        {
            var signal = Enumerable.Repeat(ParseInput(input), 10000).SelectMany(s => s);
            var arr = signal.ToArray();
            foreach (var i in Enumerable.Range(1, 100))
            {
                arr = FFT(arr).ToArray();
                Console.WriteLine($"Pass {i}");
            }

            var n = int.Parse(string.Join("", arr.Take(7)));
            return string.Join("", signal.Skip(n - 7).Take(8));
        }

        public Task<string> Solve1Async() => Task.Run(() => Solve1(_input));

        public Task<string> Solve2Async() => Task.Run(() => Solve2(_input));
    }
}
