using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Day15
{
    public class Puzzle : PuzzleBase<int, int>
    {
        public Puzzle(IEnumerable<int> input)
        {
            _input = input.ToArray();
        }

        private readonly int[] _input;
        
        private IEnumerable<int> Numbers()
        {
            var lastSeen = new Dictionary<int, (int, int)>();

            var turn = 1;
            foreach (var num in _input)
            {
                lastSeen.Add(num, (turn, 0));
                yield return num;
                turn++;
            }

            void store(int number, int t)
            {
                lastSeen[number] = lastSeen.TryGetValue(number, out var l) ? (t, l.Item1) : (t, 0);
            }

            var lastNumber = _input.Last();
            while (true)
            {
                var (p1, p2) = lastSeen[lastNumber];
                lastNumber = p2 == 0 ? 0 : p1 - p2;

                store(lastNumber, turn);

                yield return lastNumber;
                turn++;
            }
        }

        public override int Solution1() => Numbers().Skip(2020 - 1).First();

        public override int Solution2() => Numbers().Skip(30000000 - 1).First();
    }
}
