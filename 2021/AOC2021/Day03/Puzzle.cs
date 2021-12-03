using System.Text.RegularExpressions;
using AOC2021.Common;

namespace AOC2021.Day03
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            _input = lines.ToArray();
        }

        private readonly string[] _input;

        public override long Solution1()
        {
            var length = _input[0].Length;
            var gamma = Enumerable.Range(0, length).Select(i => _input.MostCommon(i)).FromBinary();
            var epsilon = Enumerable.Range(0, length).Select(i => _input.LeastCommon(i)).FromBinary();
            return gamma * epsilon;
        }

        public override long Solution2()
        {
            var oxy = _input.KeepFiltering(Day03Extensions.MostCommon).FromBinary();
            var co2 = _input.KeepFiltering(Day03Extensions.LeastCommon).FromBinary();
            return oxy * co2;
        }
    }

    internal static class Day03Extensions
    {
        public static IEnumerable<string> FilterByBit(this IEnumerable<string> numbers, int bit, char value) =>
            numbers.Where(n => n[bit] == value);

        public static string KeepFiltering(this IEnumerable<string> numbers, Func<IEnumerable<string>, int, char> bitFilter)
        {
            var nums = numbers.ToArray();
            var bit = 0;
            while (nums.Length > 1)
            {
                nums = nums.FilterByBit(bit, bitFilter(nums, bit)).ToArray();
                bit++;
            }

            return nums.Single();
        }

        public static long FromBinary(this IEnumerable<char> text) =>
            text.Aggregate(0L, (s, c) => s * 2 + (c - '0'));

        private static (int, int) BitFrequency(int bit, IEnumerable<string> numbers) =>
            numbers.Select(n => n[bit])
                .Aggregate((0, 0),
                    (s, c) => c == '0'
                        ? (s.Item1 + 1, s.Item2)
                        : (s.Item1, s.Item2 + 1));

        public static char MostCommon(this IEnumerable<string> numbers, int bit)
        {
            var (zero, one) = BitFrequency(bit, numbers);
            return zero > one ? '0' : '1';
        }

        public static char LeastCommon(this IEnumerable<string> numbers, int bit)
        {
            var (zero, one) = BitFrequency(bit, numbers);
            return zero <= one ? '0' : '1';
        }
    }
}
