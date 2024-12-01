namespace AOC2024;

public abstract class CustomBaseProblem<U> : CustomBaseProblem<U, U> { }

public abstract class CustomBaseProblem<U, V> : BaseProblem
{
    protected IEnumerable<string> ReadLinesFromFile() => File.ReadLines(InputFilePath);

    protected override string ClassPrefix => "Day";

    public abstract U Solve1();

    public override ValueTask<string> Solve_1() => new(Solve1()?.ToString() ?? "[null]");

    public abstract V Solve2();

    public override ValueTask<string> Solve_2() => new(Solve2()?.ToString() ?? "[null]");
}
