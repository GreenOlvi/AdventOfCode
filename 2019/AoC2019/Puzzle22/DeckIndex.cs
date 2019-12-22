namespace AoC2019.Puzzle22
{
    public class DeckByIndex
    {
        public DeckByIndex(long size)
        {
            _size = size;
        }

        private readonly long _size;

        public long DealIntoNewStack(long i) => Mod(-i - 1);

        public long Cut(long i, long n) => Mod(i + n);

        public long DealWithIncrement(long i, long n)
        {
            var ni = i;
            while (ni % n != 0)
            {
                ni += _size;
            }
            return ni / n;
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

    }
}
