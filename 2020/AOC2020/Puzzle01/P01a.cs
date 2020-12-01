using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Puzzle01
{
    public class P01a : PuzzleBase<int, int>
    {
        public P01a(IEnumerable<int> input)
        {
            _input = input.ToArray();
        }

        private readonly int[] _input;

        public IEnumerable<(int, int)> GetPairs()
        {
            foreach (var a in _input)
            {
                foreach (var b in _input.Where(i => a + i == 2020))
                {
                    yield return (a, b);
                }
            }
        }

        public override int Solution1()
        {
            var (a, b) = GetPairs().First();
            return a * b;
        }

        public IEnumerable<(int, int, int)> GetThrees()
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

        public override int Solution2()
        {
            var (a, b, c) = GetThrees().First();
            return a * b * c;
        }
    }
}
