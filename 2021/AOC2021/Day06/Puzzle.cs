using AOC2021.Common;

namespace AOC2021.Day06
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            _initial = lines.First()
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .ParseInts()
                .ToArray();
        }

        private readonly int[] _initial;

        private long LoopFishes(int days)
        {
            var fishes = new long[9];
            foreach (var f in _initial)
            {
                fishes[f]++;
            }

            for (int day = 0; day < days; day++)
            {
                var newFishes = fishes[0];
                for (int i = 0; i < 8; i++)
                {
                    fishes[i] = fishes[i + 1];
                }
                fishes[8] = newFishes;
                fishes[6] += newFishes;
            }

            return fishes.Sum();
        }

        public override long Solution1() => LoopFishes(80);

        public override long Solution2() => LoopFishes(256);
    }
}
