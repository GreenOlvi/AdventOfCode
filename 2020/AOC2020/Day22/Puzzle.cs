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
        private enum Player { One, Two };

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

        private static (Player Winner, IEnumerable<int> Deck) Play(IEnumerable<int> p1, IEnumerable<int> p2)
        {
            var q1 = new Queue<int>(p1);
            var q2 = new Queue<int>(p2);

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

            return q1.Any() ? (Player.One, q1) : (Player.Two, q2);
        }

        private static string ToHash(IEnumerable<int> p1, IEnumerable<int> p2) =>
            $"P1[" + string.Join(",", p1) + "];P2[" + string.Join(",", p2) + "]";

        private static (Player Winner, IEnumerable<int> Deck) PlayRecursive(IEnumerable<int> p1, IEnumerable<int> p2)
        {
            var q1 = new Queue<int>(p1);
            var q2 = new Queue<int>(p2);

            var seen = new HashSet<string>();

            while (q1.Any() && q2.Any())
            {
                var h = ToHash(q1, q2);
                if (seen.Contains(h))
                {
                    return (Player.One, q1);
                }
                else
                {
                    seen.Add(h);
                }

                var c1 = q1.Dequeue();
                var c2 = q2.Dequeue();

                Player winner;

                if (c1 <= q1.Count && c2 <= q2.Count)
                {
                    (winner, _) = PlayRecursive(q1.Take(c1), q2.Take(c2));
                }
                else
                {
                    winner = c1 > c2 ? Player.One : Player.Two;
                }

                if (winner == Player.One)
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

            return q1.Any() ? (Player.One, q1) : (Player.Two, q2);
        }

        public override int Solution1()
        {
            var (_, deck) = Play(_playerCards[0], _playerCards[1]);
            return CountScore(deck);
        }

        public override int Solution2()
        {
            var (_, deck) = PlayRecursive(_playerCards[0], _playerCards[1]);
            return CountScore(deck);
        }
    }
}
