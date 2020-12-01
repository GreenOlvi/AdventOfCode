using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Puzzle01
{
    public class P01 : PuzzleBase<int, int>
    {
        public P01(IEnumerable<int> input)
        {
            _input = input.ToHashSet();
        }

        private readonly HashSet<int> _input;

        private IEnumerable<(int, int)> GetPairs(int sum)
        {
            return _input.Where(a => _input.Contains(sum - a)).Select(a => (a, sum - a));
        }

        public override int Solution1()
        {
            var (a, b) = GetPairs(2020).First();
            return a * b;
        }

        private IEnumerable<(int, int, int)> GetThrees(int sum)
        {
            foreach (var a in _input)
            {
                foreach (var (b, c) in GetPairs(sum - a))
                {
                    yield return (a, b, c);
                }
            }
        }

        public override int Solution2()
        {
            var (a, b, c) = GetThrees(2020).First();
            return a * b * c;
        }
    }
}
