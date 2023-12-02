using AOC2022.Common;

namespace AOC2023.Puzzles;
public partial class Day02 : CustomBaseDay
{
    private readonly Game[] _games;

    public Day02()
    {
        _games = ReadLinesFromFile().Select(ParseLine).ToArray();
    }

    public Day02(IEnumerable<string> lines)
    {
        _games = lines.Select(ParseLine).ToArray();
    }

    private readonly Regex _lineMatch = BuildLineMatch();
    private Game ParseLine(string line)
    {
        if (!_lineMatch.TryMatch(line, out var match))
        {
            throw new InvalidOperationException();
        }

        var id = int.Parse(match.Groups["game_id"].Value);
        var groups = match.Groups["cubes"].Value.Split(";", StringSplitOptions.TrimEntries);
        var cubeList = new List<Cubes>();
        foreach (var group in groups)
        {
            var cubes = group.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var (r, g, b) = (0, 0, 0);

            foreach (var c in cubes)
            {
                var sp = c.Split(" ", StringSplitOptions.TrimEntries);
                var count = int.Parse(sp[0]);
                switch (sp[1])
                {
                    case "red":
                        r = count;
                        break;
                    case "green":
                        g = count;
                        break;
                    case "blue":
                        b = count;
                        break;
                    default:
                        throw new InvalidDataException();
                }
            }
            cubeList.Add(new Cubes(r, g, b));
        }
        return new Game(id, cubeList);
    }

    private bool IsPossible(Game game)
    {
        var limit = new Cubes(12, 13, 14);
        return game.Cubes.All(c => c.Red <= limit.Red && c.Green <= limit.Green && c.Blue <= limit.Blue);
    }

    public override ValueTask<string> Solve_1()
    {
        var sum = _games.Where(IsPossible).Sum(g => g.Id);
        return sum.ToResult();
    }

    private Cubes Minimum(Game game)
    {
        return game.Cubes.Aggregate(new Cubes(),
            (a, c) => new Cubes(Math.Max(a.Red, c.Red), Math.Max(a.Green, c.Green), Math.Max(a.Blue, c.Blue)));
    }

    private long Power(Cubes cubes) => cubes.Red * cubes.Green * cubes.Blue;

    public override ValueTask<string> Solve_2()
    {
        var sum = _games.Select(Minimum).Select(Power).Sum();
        return sum.ToResult();
    }

    private readonly record struct Game(int Id, List<Cubes> Cubes);
    private readonly record struct Cubes(int Red, int Green, int Blue);

    [GeneratedRegex(@"^Game\s(?<game_id>\d+): (?<cubes>.+)$")]
    private static partial Regex BuildLineMatch();
}
