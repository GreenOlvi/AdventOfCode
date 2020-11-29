using System.Threading.Tasks;

namespace AOC2020
{
    public abstract class PuzzleBase<T1, T2> : IPuzzle
    {
        protected abstract T1 Solution1();
        protected abstract T2 Solution2();

        public Task<string> Solve1() => Task.Run(() => Solution1().ToString());
        public Task<string> Solve2() => Task.Run(() => Solution2().ToString());
    }
}
