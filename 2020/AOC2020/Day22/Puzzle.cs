using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Day22
{
    public class Puzzle : PuzzleBase<int, int>
    {
        public Puzzle(IEnumerable<string> input)
        {
            var groups = input.SplitGroups();
            _playerCards = groups.Select(c => c.Skip(1).ParseInts().ToArray()).ToArray();
        }

        private readonly int[][] _playerCards;

        private static int CountScore(IEnumerable<int> deck)
        {
            var sum = 0;
            var value = deck.Count();
            foreach (var card in deck)
            {
                sum += card * value;
                value--;
            }
            return sum;
        }

        public override int Solution1()
        {
            var q1 = new Queue<int>(_playerCards[0]);
            var q2 = new Queue<int>(_playerCards[1]);

            while (q1.Any() && q2.Any())
            {
                var c1 = q1.Dequeue();
                var c2 = q2.Dequeue();
                if (c1 > c2)
                {
                    q1.Enqueue(c1);
                    q1.Enqueue(c2);
                }
                else
                {
                    q2.Enqueue(c2);
                    q2.Enqueue(c1);
                }
            }

            return CountScore(q1.Any() ? q1 : q2);
        }

        public override int Solution2()
        {
            return 0;
        }
    }
}
