using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Day10
{
    public class Puzzle : PuzzleBase<int, long>
    {
        public Puzzle(IEnumerable<int> input)
        {
            _input = input.OrderBy(i => i).ToArray();
        }

        private readonly int[] _input;

        public override int Solution1()
        {
            var goal = _input.Last() + 3;
            var oneDiff = 0;
            var threeDiff = 0;
            var modInput = _input.Distinct().Prepend(0).Append(goal).ToArray();

            for (var i = 0; i < modInput.Length - 1; i++)
            {
                var diff = modInput[i + 1] - modInput[i];
                if (diff > 3)
                {
                    throw new PuzzleException("Found break");
                }

                switch (diff)
                {
                    case 1:
                        oneDiff++;
                        break;
                    case 3:
                        threeDiff++;
                        break;
                }
            }

            return oneDiff * threeDiff;
        }

        public override long Solution2()
        {
            var goal = _input.Last() + 3;
            var modInput = _input.Append(0).Append(goal).ToHashSet();
            var count = GetSolutions(modInput, goal);
            return count;
        }

        private readonly Dictionary<string, long> _cache = new Dictionary<string, long>();

        private static string GetHash(HashSet<int> rest)
        {
            var arr = new byte[20];
            foreach (var i in rest)
            {
                arr[i / 8] |= (byte)(1 << (i % 8));
            }
            return string.Join("-", arr);
        }

        private long GetSolutions(HashSet<int> rest, int goal, int start = 0)
        {
            var hash = GetHash(rest);
            if (_cache.TryGetValue(hash, out var solution))
            {
                return solution;
            }

            var options = Enumerable.Range(start + 1, 3).Where(rest.Contains).ToArray();
            var solutions = 0L;
            foreach (var o in options)
            {
                if (o == goal)
                {
                    solutions++;
                }
                else
                {
                    var newSet = rest.Where(e => e > o).ToHashSet();
                    solutions += GetSolutions(newSet, goal, o);
                }
            }

            _cache.Add(hash, solutions);

            return solutions;
        }
    }
}
