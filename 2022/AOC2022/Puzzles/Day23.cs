namespace AOC2022.Puzzles;

public class Day23 : CustomBaseDay
{
    private readonly HashGrid<bool> _initialGrid;

    public Day23()
    {
        _initialGrid = ParseInput(ReadLinesFromFile());
    }

    public Day23(IEnumerable<string> lines)
    {
        _initialGrid = ParseInput(lines);
    }

    private static HashGrid<bool> ParseInput(IEnumerable<string> lines)
    {
        var grid = new HashGrid<bool>(new Dictionary<bool, char>()
        {
            [true] = '#',
            [false] = '.',
        });

        var y = 0;
        foreach (var line in lines)
        {
            for (var x = 0; x < line.Length; x++)
            {
                if (line[x] == '#')
                {
                    grid[(x, y)] = true;
                }
            }
            y++;
        }
        return grid;
    }

    public override ValueTask<string> Solve_1()
    {
        var s = _initialGrid.Draw();
        return "result1".ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        return "result2".ToResult();
    }
}
