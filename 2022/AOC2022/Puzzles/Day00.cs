namespace AOC2022.Puzzles;

public class Day00 : CustomBaseDay
{
    private readonly string[] _lines;

    public Day00()
    {
        _lines = ReadLinesFromFile();
    }

    public Day00(IEnumerable<string> lines)
    {
        _lines = lines;
    }

    public override ValueTask<string> Solve_1()
    {
        return "result1".ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        return "result2".ToResult();
    }
}
