using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Day09
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<long> input, int preambleLength = 25)
        {
            _input = input.ToArray();
            _preamble = preambleLength;
        }

        private readonly long[] _input;
        private readonly int _preamble;

        public override long Solution1()
        {
            for (var i = 0; i < _input.Length - _preamble; i++)
            {
                var prev = _input.Skip(i).Take(_preamble).ToHashSet();
                var number = _input[i + _preamble];

                var found = false;
                foreach (var a in prev)
                {
                    var b = number - a;
                    if (a != b && prev.Contains(b))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return number;
                }
            }

            throw new PuzzleException("Result not found");
        }

        public override long Solution2()
        {
            var number = Solution1();
            for (var a = 0; a < _input.Length; a++)
            {
                if (_input[a] > number)
                {
                    continue;
                }

                var numbers = new List<long>() { _input[a] };
                var sum = _input[a];
                var b = a + 1;
                while (sum < number)
                {
                    sum += _input[b];
                    numbers.Add(_input[b]);
                    b++;
                }

                if (sum > number)
                {
                    continue;
                }
                
                return numbers.Min() + numbers.Max();
            }

            throw new PuzzleException("Result not found");
        }
    }
}
