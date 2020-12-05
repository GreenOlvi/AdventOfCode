using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Puzzle05
{
    public class P05 : PuzzleBase<int, int>
    {
        public P05(IEnumerable<string> input)
        {
            _seats = input.Select(line => GetSeatId(line)).ToHashSet();
        }

        private readonly HashSet<int> _seats;

        public static int GetSeatId(string seat)
        {
            var sum = 0;
            foreach (var l in seat)
            {
                sum *= 2;
                sum += (l == 'B' || l == 'R') ? 1 : 0;
            }
            return sum;
        }

        public override int Solution1() => _seats.Max();

        public override int Solution2() =>
            Enumerable.Range(_seats.Min(), _seats.Max() - _seats.Min())
                .FirstOrDefault(s => !_seats.Contains(s) && _seats.Contains(s - 1) && _seats.Contains(s + 1));

    }
}
