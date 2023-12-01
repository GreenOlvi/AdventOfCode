namespace AOC2023.Puzzles;
public class Day00 : CustomBaseDay
{
    private readonly string[] _lines;

    public Day00()
    {
        _lines = ReadLinesFromFile().ToArray();
    }

    public Day00(IEnumerable<string> lines)
    {
        _lines = lines.ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        return "result 1".ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        return "result 2".ToResult();
    }
}
