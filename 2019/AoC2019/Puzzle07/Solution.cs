using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2019.Puzzle07
{
    public class Solution : IPuzzle
    {
        public Solution(string input)
        {
            _input = ParseInput(input);
        }

        private readonly IEnumerable<long> _input;

        public static IEnumerable<long> ParseInput(string input)
        {
            return input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => long.Parse(s));
        }

        public static long RunAmp(long[] acs, int setting, long input)
        {
            var mem = new long[acs.Length];
            Array.Copy(acs, 0, mem, 0, acs.Length);
            var amp = new IntcodeMachine(ref mem, new long[] { setting, input });
            amp.Run();

            return amp.Output.First();
        }

        private static IntcodeMachine NewAmp(long[] acs, int setting)
        {
            var mem = new long[acs.Length];
            Array.Copy(acs, 0, mem, 0, acs.Length);
            var amp = new IntcodeMachine(ref mem);
            amp.AddInput(setting);
            return amp;
        }

        public static long RunAmps(long[] acs, int[] settings)
        {
            var input = 0L;
            foreach (var s in settings)
            {
                input = RunAmp(acs, s, input);
            }
            return input;
        }

        public static long RunLoopedAmps(long[] acs, int[] settings)
        {
            var amps = settings.Select(s => NewAmp(acs, s)).ToArray();

            var run = true;
            var input = 0L;
            while (run)
            {
                foreach (var amp in amps)
                {
                    amp.AddInput(input);
                    amp.Run();
                    input = amp.Output.Dequeue();
                }
                run = amps.All(a => !a.IsHalted);
            }

            return input;
        }

        public static long Solve1(IEnumerable<long> input)
        {
            var acs = input.ToArray();
            return Permutate(new[] { 0, 1, 2, 3, 4 })
                .Select(s => RunAmps(acs, s))
                .Max();
        }

        public static long Solve2(IEnumerable<long> input)
        {
            var acs = input.ToArray();
            return Permutate(new[] { 5, 6, 7, 8, 9 })
                .Select(s => RunLoopedAmps(acs, s))
                .Max();
        }

        public static IEnumerable<T[]> Permutate<T>(T[] elements)
        {
            var n = elements.Length;
            for (var i = 0L; i < Factorial(n); i++)
            {
                yield return PermLexUnrank(n, i).Select(p => elements[p]).ToArray();
            }
        }

        public static int[] PermLexUnrank(int n, long r)
        {
            var p = new int[n];
            p[n - 1] = 0;
            var r2 = r;
            for (var j = 1; j <= n - 1; j++)
            {
                var fj = Factorial(j);
                var d = (r % Factorial(j + 1)) / fj;
                r2 -= d * fj;
                p[n - j - 1] = (int)d;

                for (var i = n - j + 1; i <= n; i++)
                {
                    if (p[i - 1] >= d)
                    {
                        p[i - 1] = p[i - 1] + 1;
                    }
                }
            }
            return p;
        }

        private static long Factorial(int n) =>
            n <= 1 ? 1 : n * Factorial(n - 1);

        public Task<string> Solve1Async()
            => Task.Run(() => Solve1(_input).ToString());

        public Task<string> Solve2Async()
            => Task.Run(() => Solve2(_input).ToString());
    }
}
