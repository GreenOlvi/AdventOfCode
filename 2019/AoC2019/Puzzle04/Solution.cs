using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2019.Puzzle04
{
    public class Solution : IPuzzle
    {
        public Solution(string input)
        {
            (_from, _to) = ParseInput(input);
        }

        private readonly int _from;
        private readonly int _to;

        private static (int, int) ParseInput(string input)
        {
            var i = input.Split('-', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.Parse(s))
                .ToArray();
            return (i[0], i[1]);
        }

        public static int[] Split(int number) =>
            number.ToString().Select(c => c - '0').ToArray();

        public static bool IsValid1(int number) => IsValid1(Split(number));

        private static bool IsValid1(int[] n)
        {
            if (n.Length != 6)
            {
                return false;
            }

            if (!Enumerable.Range(0, 5).Any(i => n[i] == n[i + 1]))
            {
                return false;
            }

            if (!Enumerable.Range(0, 5).All(i => n[i] <= n[i + 1]))
            {
                return false;
            }

            return true;
        }

        public static IEnumerable<int> Range(int from, int to) =>
            Enumerable.Range(from, to - from + 1);

        public static IEnumerable<int> RepeatedLengths(int[] n)
        {
            var last = n[0];
            var len = 1;
            foreach (var i in n.Skip(1))
            {
                if (last == i)
                {
                    len++;
                }
                else
                {
                    yield return len;
                    last = i;
                    len = 1;
                }
            }
            yield return len;
        }

        public static bool IsValid2(int number)
        {
            var n = Split(number);
            if (!IsValid1(n))
            {
                return false;
            }

            return RepeatedLengths(n).Any(l => l == 2);
        }

        public static int Solve1(int from, int to) =>
            Range(from, to).Where(i => IsValid1(i)).Count();

        public static int Solve2(int from, int to) =>
            Range(from, to).Where(i => IsValid2(i)).Count();

        public async Task<string> Solve1Async() =>
            await Task.Run(() => Solve1(_from, _to).ToString());

        public async Task<string> Solve2Async() =>
           await Task.Run(() => Solve2(_from, _to).ToString());
    }
}
