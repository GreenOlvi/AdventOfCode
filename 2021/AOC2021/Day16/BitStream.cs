namespace AOC2021.Day16
{
    public class BitStream
    {
        public BitStream(IEnumerable<byte> bits)
        {
            _bits = bits.ToArray();
        }

        private readonly byte[] _bits;

        public int Length => _bits.Length;
        public int Position { get; private set; }
        public bool IsEmpty => Position == _bits.Length || OnlyZerosLeft();

        public bool OnlyZerosLeft()
        {
            return _bits.Skip(Position).All(b => b == 0);
        }

        public IEnumerable<byte> ReadBits(int n)
        {
            var v = _bits.Skip(Position).Take(n);
            Position += n;
            return v;
        }

        public byte ReadBit() => _bits[Position++];
    }
}
