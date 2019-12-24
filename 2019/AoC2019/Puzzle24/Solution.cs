using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2019.Puzzle24
{
    public class Solution : IPuzzle
    {
        public Solution(IEnumerable<string> input)
        {
            _input = ParseInput(input).ToArray();
        }

        private readonly bool[] _input;

        public static IEnumerable<bool> ParseInput(IEnumerable<string> input) =>
            input.SelectMany(line => line.Where(c => c == '.' || c == '#').Select(c => c == '#'));

        public static uint GetGridHash(bool[] grid)
        {
            var bit = 1u;
            var hash = 0u;
            for (var i = 0; i < 25; i++)
            {
                if (grid[i])
                {
                    hash |= bit;
                }
                bit <<= 1;
            }
            return hash;
        }

        public static uint Step(uint grid)
        {
            var result = 0u;
            for (var i = 0; i < 25; i++)
            {
                var n = 0;
                if (i > 4 && ((1 << (i - 5)) & grid) > 0)
                {
                    n++;
                }
                if (i < 20 && ((1 << (i + 5)) & grid) > 0)
                {
                    n++;
                }
                var m = i % 5;
                if (m != 0 && ((1 << (i - 1)) & grid) > 0)
                {
                    n++;
                }
                if (m != 4 && ((1 << (i + 1)) & grid) > 0)
                {
                    n++;
                }

                var cur = (grid & (1 << i)) > 0;
                if ((cur && n == 1) || (!cur && (n == 1 || n == 2)))
                {
                    result |= (1u << i);
                }
            }
            return result;
        }

        public static string Draw(uint grid)
        {
            var sb = new StringBuilder();
            var bit = 1u;
            for (var i = 0; i < 25; i++)
            {
                sb.Append((grid & bit) == 0 ? '.' : '#');
                bit <<= 1;
                if (i % 5 == 4)
                {
                    sb.Append(Environment.NewLine);
                }
            }
            return sb.ToString();
        }

        public static uint FindDuplicate(uint first)
        {
            var hash = first;
            var set = new HashSet<uint>();
            set.Add(hash);

            while (true)
            {
                var next = Step(hash);
                if (set.Contains(next))
                {
                    return next;
                }
                set.Add(next);
                hash = next;
            }
        }

        public static uint Solve1(bool[] input)
        {
            var hash = GetGridHash(input);
            return FindDuplicate(hash);
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_input).ToString());

        public Task<string> Solve2Async()
        {
            throw new NotImplementedException();
        }
    }
}
