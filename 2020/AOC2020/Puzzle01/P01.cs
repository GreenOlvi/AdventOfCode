using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Puzzle01
{
    public class P01 : PuzzleBase<int, int>
    {
        public P01(IEnumerable<int> input)
        {
            _input = input.ToArray();
        }

        private readonly int[] _input;

        private IEnumerable<(int, int)> GetPairs()
        {
            foreach (var a in _input)
            {
                foreach (var b in _input.Where(i => a + i == 2020))
                {
                    yield return (a, b);
                }
            }
        }

        protected override int Solution1()
        {
            var (a, b) = GetPairs().First();
            return a * b;
        }

        private IEnumerable<(int, int, int)> GetThrees()
        {
            foreach (var a in _input)
            {
                foreach (var b in _input.Where(i => a + i < 2020))
                {
                    foreach (var c in _input.Where(i => a + b + i == 2020))
                    {
                        yield return (a, b, c);
                    }
                }
            }
        }

        protected override int Solution2()
        {
            var (a, b, c) = GetThrees().First();
            return a * b * c;
        }
    }
}
