namespace AOC2021
{
    public abstract class PuzzleBase<T1, T2> : IPuzzle
    {
        public abstract T1 Solution1();
        public abstract T2 Solution2();

        public string Solve1() => Solution1()?.ToString() ?? "[null]";
        public string Solve2() => Solution2()?.ToString() ?? "[null]";
    }
}
