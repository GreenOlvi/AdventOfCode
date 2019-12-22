using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Puzzle22
{
    public static class DeckExtensions
    {
        public static IEnumerable<long> NewDeck(long size)
        {
            for (var i = 0L; i < size; i++)
            {
                yield return i;
            }
        }

        public static IEnumerable<long> DealIntoNewStack(this IEnumerable<long> deck) =>
            deck.Reverse();

        public static IEnumerable<long> Cut(this IEnumerable<long> deck, long n)
        {
            var mn = Mod(n, deck.Count());
            var cut = deck.TakeWhile((e, i) => i < mn);
            return deck.SkipWhile((e, i) => i < mn).Concat(cut);
        }

        private static long Mod(long n, long m)
        {
            while (n < 0)
            {
                n += m;
            }

            while (n >= m)
            {
                n -= m;
            }

            return n;
        }

        public static IEnumerable<long> DealWithIncrement(this IEnumerable<long> deck, long n)
        {
            var d = deck.ToArray();
            var l = d.Length;
            var arr = new long[d.Length];

            for (var i = 0; i < d.Length; i++)
            {
                arr[i * n % l] = d[i];
            }

            return arr;
        }
    }
}
