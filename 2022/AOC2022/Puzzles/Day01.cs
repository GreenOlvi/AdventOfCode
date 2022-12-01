namespace AOC2022.Puzzles;

public class Day01 : CustomBaseDay
{
    private readonly int[][] _numbers;

    public Day01(IEnumerable<string> lines)
    {
        _numbers = lines.SplitGroups(int.Parse).ToArray();
    }

    public Day01()
    {
        _numbers = ReadLinesFromFile().SplitGroups(int.Parse).ToArray();
    }

    public override ValueTask<string> Solve_1() =>
        _numbers.Select(e => e.Sum()).Max().ToResult();

    public override ValueTask<string> Solve_2() =>
        _numbers.Select(e => e.Sum()).OrderByDescending(s => s).Take(3).Sum().ToResult();
}
