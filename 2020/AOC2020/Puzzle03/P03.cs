using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Puzzle03
{
    public class P03 : PuzzleBase<int, long>
    {
        public P03(IEnumerable<string> input)
        {
            _input = input.ToArray();
        }

        private readonly string[] _input;

        public override int Solution1()
        {
            var trees = 0;
            var x = 0;

            foreach (var line in _input.Skip(1))
            {
                x += 3;
                if (x >= line.Length)
                {
                    x -= line.Length;
                }
                
                if (line[x] == '#')
                {
                    trees++;
                }
            }

            return trees;
        }

        private long CheckSlope(int dx, int dy)
        {
            var trees = 0;
            var x = 0;
            var y = 0;

            while (y < _input.Length)
            {
                if (_input[y][x] == '#')
                {
                    trees++;
                }

                y += dy;
                x += dx;

                if (x >= _input[0].Length)
                {
                    x -= _input[0].Length;
                }

            }

            return trees;
        }

        public override long Solution2()
        {
            return CheckSlope(1, 1)
                * CheckSlope(3, 1)
                * CheckSlope(5, 1)
                * CheckSlope(7, 1)
                * CheckSlope(1, 2);
        }
    }
}
