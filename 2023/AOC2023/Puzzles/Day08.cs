namespace AOC2023.Puzzles;
public partial class Day08 : CustomBaseDay
{
    private readonly string _instructions;
    private readonly Dictionary<string, string[]> _network;

    public Day08()
    {
        (_instructions, _network) = ParseInput(ReadLinesFromFile());
    }

    public Day08(IEnumerable<string> lines)
    {
        (_instructions, _network) = ParseInput(lines);
    }

    private static readonly Regex LinePattern = BuildLinePattern();
    private static (string, Dictionary<string, string[]>) ParseInput(IEnumerable<string> lines)
    {
        var instructions = lines.First().Trim();
        var net = new Dictionary<string, string[]>();
        foreach (var line in lines.Skip(2))
        {
            if (!LinePattern.TryMatch(line, out var match))
            {
                throw new InvalidDataException(line);
            }

            net.Add(match.Groups["name"].Value, [match.Groups["left"].Value, match.Groups["right"].Value]);
        }

        return (instructions, net);
    }

    private static long FollowNetwork(Dictionary<string, string[]> network, string instructions, string start, string end)
    {
        long i = 0;
        var node = start;
        while (node != end)
        {
            var dir = instructions[(int)(i % instructions.Length)] == 'L' ? 0 : 1;
            node = network[node][dir];
            i++;
        }
        return i;
    }

    private static long FindLoop(Dictionary<string, string[]> network, string instructions, string start)
    {
        var i = 0L;
        var node = start;
        var seen = new Dictionary<(string, int), long>();
        do
        {
            var l = (int)(i % instructions.Length);
            var dir = instructions[l] == 'L' ? 0 : 1;
            node = network[node][dir];
            if (node.EndsWith('Z'))
            {
                if (seen.TryGetValue((node, l), out var value))
                {
                    return i - value;
                }
                seen.Add((node, l), i);
            }
            i++;
        }
        while (true);
    }

    private static long FollowManyNetworks(Dictionary<string, string[]> network, string instructions) =>
        network.Keys
            .Where(k => k.EndsWith('A'))
            .Select(n => FindLoop(network, instructions, n))
            .Aggregate(Utils.LeastCommonMultiple);

    public override ValueTask<string> Solve_1() =>
        FollowNetwork(_network, _instructions, "AAA", "ZZZ").ToResult();

    public override ValueTask<string> Solve_2() =>
        FollowManyNetworks(_network, _instructions).ToResult();

    [GeneratedRegex(@"(?<name>\w+)\s=\s\((?<left>\w+),\s(?<right>\w+)\)")]
    private static partial Regex BuildLinePattern();
}
