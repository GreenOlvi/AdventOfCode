using System.Text;

namespace AOC2022.Puzzles;

public partial class Day16 : CustomBaseDay
{
    private readonly string[] _lines;

    public Day16()
    {
        _lines = ReadLinesFromFile().ToArray();
    }

    public Day16(IEnumerable<string> lines)
    {
        _lines = lines.ToArray();
    }


    [GeneratedRegex("^Valve (?<name>\\w+) has flow rate=(?<flow>\\d+); tunnels? leads? to valves? (?<links>.+)$", RegexOptions.Compiled)]
    private static partial Regex MakeValvePattern();
    private static readonly Regex ValvePattern = MakeValvePattern();

    private static IEnumerable<Node> ParseInput(IEnumerable<string> lines)
    {
        return lines.Select(ParseNode);
    }

    private static Node ParseNode(string line)
    {
        if (!ValvePattern.TryParseMany<string, int, string[]>(line,
            ("name", s => s),
            ("flow", int.Parse),
            ("links", s => s.Split(", ")), out var result))
        {
            throw new InvalidDataException();
        }

        return new Node
        {
            Name = result.Item1,
            FlowRate = result.Item2,
            Neighbours = result.Item3,
        };
    }

    public override ValueTask<string> Solve_1()
    {
        var nodes = ParseInput(_lines).ToArray();

        return "result1".ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        return "result2".ToResult();
    }

    private static string ToMermaid(IEnumerable<Node> nodes)
    {
        var sb = new StringBuilder();

        foreach (var node in nodes)
        {
            sb.AppendLine($"{node.Name}[{node.Name} {node.FlowRate}]");
        }

        foreach (var node in nodes)
        {
            foreach (var link in node.Neighbours)
            {
                sb.AppendLine($"{node.Name} --> {link}");
            }
        }

        return sb.ToString();
    }

    private record Node
    {
        public string Name { get; init; } = string.Empty;
        public int FlowRate { get; init; }
        public string[] Neighbours { get; init; } = Array.Empty<string>();
    }
}
