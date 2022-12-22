using System.Security.Principal;

namespace AOC2022.Puzzles;

public class Day21 : CustomBaseDay
{
    private readonly Dictionary<string, Monkey> _monkeys;
    private readonly Dictionary<string, long> _values;

    public Day21()
    {
        _monkeys = ParseInput(ReadLinesFromFile()).ToDictionary(m => m.Name);
        _values = SolveMonkeys(_monkeys);
    }

    public Day21(IEnumerable<string> lines)
    {
        _monkeys = ParseInput(lines).ToDictionary(m => m.Name);
        _values = SolveMonkeys(_monkeys);
    }

    private static IEnumerable<Monkey> ParseInput(IEnumerable<string> lines) =>
        lines.Select(ParseLine);

    private static readonly Regex LiteralMonkeyPattern = new(@"^(?<name>\w+): (?<value>-?\d+)$");
    private static readonly Regex AggregateMonkeyPattern = new(@"^(?<name>\w+): (?<monkey_a>\w+) (?<op>[\+\-\*\/]) (?<monkey_b>\w+)$");

    private static readonly Dictionary<string, Func<long, long, long>> Operations = new()
    {
        ["+"] = (a, b) => a + b,
        ["-"] = (a, b) => a - b,
        ["*"] = (a, b) => a * b,
        ["/"] = (a, b) => a / b,
    };

    private static Monkey ParseLine(string line)
    {
        if (LiteralMonkeyPattern.TryMatch(line, out var literalMatch))
        {
            return new LiteralMonkey(literalMatch.Groups["name"].Value, int.Parse(literalMatch.Groups["value"].Value));
        }

        if (AggregateMonkeyPattern.TryMatch(line, out var aggregateMatch))
        {
            return new AggregateMonkey(
                aggregateMatch.Groups["name"].Value,
                aggregateMatch.Groups["monkey_a"].Value,
                aggregateMatch.Groups["monkey_b"].Value,
                aggregateMatch.Groups["op"].Value); 
        }

        throw new InvalidDataException();
    }

    private static List<T> TopologicalSort<T>(HashSet<T> nodes, IEnumerable<(T From, T To)> edges) where T : IEquatable<T>
    {
        var ordered = new List<T>();
        var noIncoming = new HashSet<T>(nodes.Where(n => edges.All(e => e.To.Equals(n) == false)));
        var edgeSet = edges.ToHashSet();

        while (noIncoming.Any())
        {
            var n = noIncoming.First();
            noIncoming.Remove(n);
            ordered.Add(n);

            foreach (var e in edgeSet.Where(e => e.From.Equals(n)))
            {
                var m = e.To;
                edgeSet.Remove(e);

                if (edgeSet.All(me => me.To.Equals(m) == false))
                {
                    noIncoming.Add(m);
                }
            }
        }

        if (edgeSet.Any())
        {
            throw new InvalidOperationException("Graph has cycle");
        }
        return ordered;
    }

    private static IEnumerable<(string Name, char Side)> MonkeyPathToRoot(IEnumerable<(string From, string To, char Side)> edges, string monkey)
    {
        var edgeSet = edges.ToDictionary(m => m.From, m => (m.To, m.Side));
        var node = (monkey, edgeSet[monkey].Side);
        while (node.monkey != "root")
        {
            yield return node;
            node = edgeSet[node.monkey];
        }
    }

    private static Dictionary<string, long> SolveMonkeys(Dictionary<string, Monkey> monkeys)
    {
        var nodes = monkeys.Keys.ToHashSet();
        var edges = monkeys.Values.OfType<AggregateMonkey>()
            .SelectMany(m => new[] { (m.MonkeyA, m.Name), (m.MonkeyB, m.Name) })
            .ToArray();

        var sorted = TopologicalSort(nodes, edges);
        var values = new Dictionary<string, long>();
        foreach (var monkey in sorted)
        {
            var m = monkeys[monkey];
            if (m is LiteralMonkey lm)
            {
                values[monkey] = lm.Value;
            }
            else
            {
                var am = (AggregateMonkey)m;
                values[monkey] = am.Operation(values[am.MonkeyA], values[am.MonkeyB]);
            }
        }

        return values;
    }

    public override ValueTask<string> Solve_1() => _values["root"].ToResult();

    public override ValueTask<string> Solve_2()
    {
        var edges = _monkeys.Values.OfType<AggregateMonkey>()
            .SelectMany(m => new[] { (m.MonkeyA, m.Name, 'A'), (m.MonkeyB, m.Name, 'B') })
            .ToArray();

        var humnPath = MonkeyPathToRoot(edges, "humn").Reverse().ToArray();

        var opMonkeys = _monkeys.Where(kv => kv.Value is AggregateMonkey)
            .Select(kv => (kv.Key, (AggregateMonkey)kv.Value))
            .ToDictionary();

        var node = opMonkeys["root"] with { Op = "-" };
        var result = 0L;
        var side = node.MonkeyA == humnPath[0].Name ? 'A' : 'B';
        foreach (var (child, newSide) in humnPath)
        {
            var other = _values[side == 'A' ? node.MonkeyB : node.MonkeyA];
            result = (side, node.Op) switch
            {
                ('A', "+") => result - other,
                ('A', "-") => result + other,
                ('A', "*") => result / other,
                ('A', "/") => result * other,
                ('B', "+") => result - other,
                ('B', "-") => other - result,
                ('B', "*") => result / other,
                ('B', "/") => other / result,
                _ => throw new ArgumentOutOfRangeException(nameof(node.Op)),
            };

            if (child != "humn")
            {
                node = opMonkeys[child];
                side = newSide;
            }
        }

        return result.ToResult();
    }

    private interface Monkey
    {
        string Name { get; }
    }

    private readonly record struct LiteralMonkey : Monkey
    {
        public string Name { get; init; }

        public long Value { get; init; }

        public LiteralMonkey(string name, long value)
        {
            Name = name;
            Value = value;
        }
    }

    private readonly record struct AggregateMonkey : Monkey
    {
        public string Name { get; init; }

        public string MonkeyA { get; init; }
        public string MonkeyB { get; init; }

        public string Op { get; init; }
        public Func<long, long, long> Operation => Operations[Op];

        public AggregateMonkey(string name, string monkeyA, string monkeyB, string op)
        {
            Name = name;
            MonkeyA = monkeyA;
            MonkeyB = monkeyB;
            Op = op;
        }
    }
}
