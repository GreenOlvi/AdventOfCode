using System.Threading.Tasks;

namespace AOC2020
{
    public abstract class PuzzleBase<T1, T2> : IPuzzle
    {
        public abstract T1 Solution1();
        public abstract T2 Solution2();

        public Task<string> Solve1() => Task.Run(() => Solution1()?.ToString() ?? "[null]");
        public Task<string> Solve2() => Task.Run(() => Solution2()?.ToString() ?? "[null]");

        public virtual string? GetProgress1() => null;
        public virtual string? GetProgress2() => null;
    }
}
