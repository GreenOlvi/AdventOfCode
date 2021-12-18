namespace AOC2021.Day18
{
    public class CharStream
    {
        public CharStream(IEnumerable<char> chars)
        {
            _chars = chars.ToArray();
        }

        private readonly char[] _chars;

        public int Length => _chars.Length;
        public int Position { get; private set; }
        public bool IsEmpty => Position >= _chars.Length;

        public IEnumerable<char> ReadChars(int n)
        {
            var v = _chars.Skip(Position).Take(n);
            Position += n;
            return v;
        }

        public char ReadChar() => _chars[Position++];
        public char PeekChar() => IsEmpty ? '\0' : _chars[Position];
        public void Skip(int n = 1) => Position += n;
    }

}
