using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Puzzle22
{
    public class Deck
    {
        public Deck(long size)
        {
            _size = size;
            Cards = NewDeck(size);
        }

        public Deck(IEnumerable<long> cards)
        {
            Cards = cards;
            _size = cards.Count();
        }

        public Deck(IEnumerable<long> cards, long size)
        {
            Cards = cards;
            _size = size;
        }

        private readonly long _size;
        public IEnumerable<long> Cards { get; private set; }

        private static IEnumerable<long> NewDeck(long size)
        {
            for (var i = 0L; i < size; i++)
            {
                yield return i;
            }
        }

        public Deck DealIntoNewStack() =>
            new Deck(Cards.Reverse(), _size);

        public Deck Cut(long n)
        {
            var mn = Mod(n);
            var cut = Cards.TakeWhile((e, i) => i < mn);
            var cards = Cards.SkipWhile((e, i) => i < mn).Concat(cut);
            return new Deck(cards, _size);
        }

        private long Mod(long n)
        {
            while (n < 0)
            {
                n += _size;
            }

            while (n >= _size)
            {
                n -= _size;
            }

            return n;
        }

        public Deck DealWithIncrement(long n)
        {
            var c = Cards as List<long> ?? Cards.ToList();
            var arr = new long[_size];

            for (var i = 0; i < _size; i++)
            {
                arr[i * n % _size] = c[i];
            }

            return new Deck(arr, _size);
        }

        public long FindIndex(long n)
        {
            Cards = Cards as List<long> ?? Cards.ToList();
            return ((List<long>)Cards).IndexOf(n);
        }
    }
}
