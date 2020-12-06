using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Day06
{
    public class Puzzle : PuzzleBase<int, int>
    {
        public Puzzle(IEnumerable<string> input)
        {
            _input = input.SplitGroups(ToBitMap).ToArray();
        }

        private readonly uint[][] _input;

        private static uint ToBitMap(string line)
        {
            var map = 0u;
            foreach (var c in line)
            {
                if (c < 'a' || c > 'z')
                {
                    throw new PuzzleException("Invalid input");
                }

                map |= 1u << (c - 'a');
            }
            return map;
        }

        private static int CountBits(uint i)
        {
            unchecked
            {
                var b = i - ((i >> 1) & (0x55555555u));
                b = (b & 0x33333333) + ((b >> 2) & 0x33333333);
                b = (((b + (b >> 4)) & 0x0f0f0f0f) * 0x01010101) >> 24;
                return (int)b;
            }
        }

        public override int Solution1() => _input.Select(g => CountBits(g.Aggregate(0u, (a, b) => a | b))).Sum();

        public override int Solution2() => _input.Select(g => CountBits(g.Aggregate(0xffffffffu, (a, b) => a & b))).Sum();
    }
}
