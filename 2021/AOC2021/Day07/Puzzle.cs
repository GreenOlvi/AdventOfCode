using AOC2021.Common;

namespace AOC2021.Day07
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            _positions = lines.First()
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ParseInts()
                .ToArray();
        }

        private readonly int[] _positions;

        private static long Cost1(int pos, int dest) => Math.Abs(pos - dest);

        private static long Cost2(int pos, int dest)
        {
            var d = Math.Abs(pos - dest);
            return (1 + d) * d / 2;
        }

        private static (int Position, long Cost) FindLowestCostPosition(int[] positions, Func<int, int, long> costFunc)
        {
            var min = positions.Min();
            var max = positions.Max();

            long GetCost(int i) => positions.Sum(p => costFunc(p, i));

            return Enumerable.Range(min, max - min)
                .Select(i => (i, GetCost(i)))
                .MinBy(p => p.Item2);
        }

        public override long Solution1() => FindLowestCostPosition(_positions, Cost1).Cost;

        public override long Solution2() => FindLowestCostPosition(_positions, Cost2).Cost;
    }
}
