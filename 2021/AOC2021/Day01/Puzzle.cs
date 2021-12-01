using AOC2021.Common;

namespace AOC2021.Day01
{
    public class Puzzle : PuzzleBase<int, int>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            _input = lines.ParseInts().ToArray();
        }

        private readonly int[] _input;

        public override int Solution1() =>
            _input.SlidingWindow(2).Select(a => a.ToArray()).Count(a => a[1] > a[0]);

        public override int Solution2() =>
            _input.SlidingWindow(3).Select(a => a.Sum()).SlidingWindow(2).Select(a => a.ToArray()).Count(a => a[1] > a[0]);
    }
}
