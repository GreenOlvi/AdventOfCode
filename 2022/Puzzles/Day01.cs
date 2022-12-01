namespace AOC2022.Puzzles;

public class Day01 : CustomBaseDay
{
    private readonly List<List<int>> _numbers = new();

    protected override void Init()
    {
        var current = new List<int>();
        foreach (var line in Input.Value)
        {
            if (line == string.Empty)
            {
                _numbers.Add(current);
                current = new List<int>();
            }
            else
            {
                var i = int.Parse(line);
                current.Add(i);
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var sums = _numbers.Select(e => e.Sum()).Max();
        return ValueTask.FromResult(sums.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var sums = _numbers.Select(e => e.Sum()).OrderByDescending(s => s).Take(3).Sum();
        return ValueTask.FromResult(sums.ToString());
    }
}
