

using System.Reflection.Metadata.Ecma335;

namespace AOC2023.Puzzles;
public partial class Day19 : CustomBaseDay
{
    private readonly FlowDefinition[] _flows;
    private readonly Part[] _parts;

    public Day19()
    {
        (_flows, _parts) = ParseFlowsAndParts(ReadLinesFromFile());
    }

    public Day19(IEnumerable<string> lines)
    {
        (_flows, _parts) = ParseFlowsAndParts(lines);
    }

    private static (FlowDefinition[] Flows, Part[] Parts) ParseFlowsAndParts(IEnumerable<string> lines)
    {
        var groups = lines.SplitGroups().ToArray();
        var flows = groups[0].Select(ParseFlow).ToArray();
        var parts = groups[1].Select(ParsePart).ToArray();
        return (flows, parts);
    }

    private static readonly Regex FlowPattern = BuildFlowPattern();
    private static readonly Regex StepPattern = BuildStepPattern();

    private static FlowDefinition ParseFlow(string line)
    {
        if (!FlowPattern.TryMatch(line, out var match))
        {
            throw new InvalidDataException(line);
        }

        var name = match.Groups["name"].Value;
        var steps = match.Groups["steps"].Value.Split(",");
        var fallback = steps.Last();

        var parsedSteps = steps[0..(steps.Length - 1)]
            .Select(s =>
            {
                if (!StepPattern.TryMatch(s, out var match))
                {
                    throw new InvalidDataException(s);
                }

                var op = match.Groups["op"].Value switch
                {
                    ">" => Op.GreaterThan,
                    "<" => Op.LessThan,
                    _ => throw new InvalidDataException(match.Groups["op"].Value),
                };

                var val = match.Groups["value"].Value.ToInt();

                return new Step(match.Groups["category"].Value, op, val, match.Groups["destination"].Value);
            });

        return new FlowDefinition(name)
        {
            Steps = parsedSteps.ToList(),
            Fallback = fallback,
        };
    }

    private static readonly Regex PartPattern = BuildPartPattern();

    private static Part ParsePart(string line)
    {
        if (!PartPattern.TryMatch(line, out var match))
        {
            throw new InvalidDataException(line);
        }

        return new Part(
            match.Groups["x"].Value.ToInt(),
            match.Groups["m"].Value.ToInt(),
            match.Groups["a"].Value.ToInt(),
            match.Groups["s"].Value.ToInt());
    }

    public override ValueTask<string> Solve_1()
    {
        var flows = _flows.Select(f => new Flow(f))
            .ToDictionary(f => f.Name);

        var buckets = _flows.Select(f => (f.Name, new Queue<Part>()))
            .ToDictionary();

        buckets["in"] = new Queue<Part>(_parts);
        var accepted = new List<Part>();
        var rejected = new List<Part>();

        while (buckets.Any(kv => kv.Value.Count > 0))
        {
            var currentBucket = buckets.First(kv => kv.Value.Count > 0);
            while (currentBucket.Value.Count > 0)
            {
                var part = currentBucket.Value.Dequeue();
                var result = flows[currentBucket.Key].Process(part);
                if (result == "A" || result == "R")
                {
                    if (result == "A")
                    {
                        accepted.Add(part);
                    }
                    else
                    {
                        rejected.Add(part);
                    }
                }
                else
                {
                    buckets[result].Enqueue(part);
                }
            }
        }

        return accepted.Sum(p => p.Values).ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        return "result 2".ToResult();
    }

    private sealed class Flow(FlowDefinition definition)
    {
        public string Name { get; } = definition.Name;

        private readonly string _fallback = definition.Fallback;
        private readonly List<(string, Func<Part, bool>)> _steps = BuildSteps(definition.Steps).ToList();

        private static IEnumerable<(string, Func<Part, bool>)> BuildSteps(IEnumerable<Step> steps)
        {
            bool Gt(int pv, int v) => pv > v;
            bool Lt(int pv, int v) => pv < v;
            int X(Part part) => part.X;
            int M(Part part) => part.M;
            int A(Part part) => part.A;
            int S(Part part) => part.S;

            foreach (var step in steps)
            {
                Func<int, int, bool> opFunc = step.Op switch
                {
                    Op.LessThan => Lt,
                    Op.GreaterThan => Gt,
                    _ => throw new InvalidOperationException(),
                };

                Func<Part, int> catFunc = step.Category switch
                {
                    "x" => X,
                    "m" => M,
                    "a" => A,
                    "s" => S,
                    _ => throw new InvalidOperationException(),
                };

                bool func(Part p) => opFunc(catFunc(p), step.Value);
                yield return (step.Destination, func);
            }
        }

        public string Process(Part part)
        {
            for (var i = 0; i < _steps.Count; i++)
            {
                var (next, pred) = _steps[i];
                if (pred(part))
                {
                    return next;
                }
            }
            return _fallback;
        }
    }

    private readonly record struct Part(int X, int M, int A, int S)
    {
        public override string ToString() => $"{{x={X},m={M},a={A},s={S}}}";
        public int Values => X + M + A + S;
    }

    private class FlowDefinition(string Name)
    {
        public string Name { get; } = Name;
        public required IReadOnlyList<Step> Steps { get; init; }
        public required string Fallback { get; init; }

        public override string ToString() =>
            $"{Name}{{{string.Join(',', Steps.Select(s => s.ToString()))},{Fallback}}}";
    }

    private readonly record struct Step(string Category, Op Op, int Value, string Destination)
    {
        public override string ToString()
        {
            var op = Op switch
            {
                Op.GreaterThan => ">",
                Op.LessThan => "<",
                _ => "?",
            };
            return $"{Category}{op}{Value}:{Destination}";
        }
    }

    private enum Op
    {
        None = 0,
        LessThan,
        GreaterThan,
    }

    [GeneratedRegex(@"^(?<name>\w+)\{(?<steps>.+)\}$")]
    private static partial Regex BuildFlowPattern();
    [GeneratedRegex(@"^(?<category>[xmas])(?<op>[<>])(?<value>\d+):(?<destination>\w+)$")]
    private static partial Regex BuildStepPattern();
    [GeneratedRegex(@"^\{x=(?<x>\d+),m=(?<m>\d+),a=(?<a>\d+),s=(?<s>\d+)\}$")]
    private static partial Regex BuildPartPattern();
}
