namespace AOC2021.Day24
{
    public class NumStream
    {
        public NumStream(IEnumerable<long> nums)
        {
            _nums = nums.ToArray();
        }

        public NumStream(long[] nums)
        {
            _nums = nums;
        }

        private readonly long[] _nums;

        public int Length => _nums.Length;
        public int Position { get; private set; }
        public bool IsEmpty => Position >= _nums.Length;

        public long Read() => _nums[Position++];

        public long Peek() => IsEmpty
            ? throw new InvalidOperationException()
            : _nums[Position];

        public bool TryRead(out long num)
        {
            if (IsEmpty)
            {
                num = default;
                return false;
            }
            num = _nums[Position++];
            return true;
        }

        public bool TryPeek(out long num)
        {
            if (IsEmpty)
            {
                num = default;
                return false;
            }
            num = _nums[Position];
            return true;
        }

        public void Skip(int n = 1) => Position += n;
    }

}
