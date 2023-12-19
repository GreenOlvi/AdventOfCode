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

                var category = match.Groups["category"].Value switch
                {
                    "x" => Category.X,
                    "m" => Category.M,
                    "a" => Category.A,
                    "s" => Category.S,
                    _ => throw new InvalidDataException(),
                };

                var op = match.Groups["op"].Value switch
                {
                    ">" => Op.GreaterThan,
                    "<" => Op.LessThan,
                    _ => throw new InvalidDataException(match.Groups["op"].Value),
                };

                var val = match.Groups["value"].Value.ToInt();

                return new Step(category, op, val, match.Groups["destination"].Value);
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
        var flows = _flows.ToDictionary(f => f.Name);

        var queue = new Queue<(string, PartRange)>();

        var fullRange = new Range(1, 4000);
        queue.Enqueue(("in", new PartRange(fullRange, fullRange, fullRange, fullRange)));

        var accepted = new List<PartRange>();

        while (queue.Count > 0)
        {
            var (flowName, range) = queue.Dequeue();
            if (flowName == "A" || flowName == "R")
            {
                if (flowName == "A")
                {
                    accepted.Add(range);
                }
                continue;
            }

            var flow = flows[flowName];
            foreach (var step in flow.Steps)
            {
                PartRange match;
                (match, range) = range.Split(step);
                if (match.Count > 0)
                {
                    queue.Enqueue((step.Destination, match));
                }

                if (range.Count == 0)
                {
                    break;
                }
            }

            if (range.Count > 0)
            {
                queue.Enqueue((flow.Fallback, range));
            }
        }

        var t = new PartRange(new Range(1, 4000), new Range(2090, 4000), new Range(2006, 4000), new Range(1, 1351));

        return accepted.Sum(r => r.Count).ToResult();
    }

    private readonly record struct Range(int From, int To)
    {
        public static readonly Range Empty = new(0, -1);
        public long Count => To - From + 1;

        public (Range Match, Range NoMatch) Split(Op op, int x)
        {
            if (op == Op.GreaterThan)
            {
                if (x < From)
                {
                    return (Empty, this);
                }
                else if (x < To)
                {
                    return (new Range(x + 1, To), new Range(From, x));
                }
                else
                {
                    return (this, Empty);
                }
            }
            else
            {
                if (x <= From)
                {
                    return (Empty, this);
                }
                else if (x <= To)
                {
                    return (new Range(From, x - 1), new Range(x, To));
                }
                else
                {
                    return (this, Empty);
                }
            }
        }

        public override string ToString() => $"{From}-{To}";
    }

    private readonly record struct PartRange(Range X, Range M, Range A, Range S)
    {
        public long Count => X.Count * M.Count * A.Count * S.Count;
        public (PartRange Match, PartRange NoMatch) Split(Step step)
        {
            switch (step.Category)
            {
                case Category.X:
                    {
                        var (y, n) = X.Split(step.Op, step.Value);
                        return (new PartRange(y, M, A, S), new PartRange(n, M, A, S));
                    }

                case Category.M:
                    {
                        var (y, n) = M.Split(step.Op, step.Value);
                        return (new PartRange(X, y, A, S), new PartRange(X, n, A, S));
                    }

                case Category.A:
                    {
                        var (y, n) = A.Split(step.Op, step.Value);
                        return (new PartRange(X, M, y, S), new PartRange(X, M, n, S));
                    }

                case Category.S:
                    {
                        var (y, n) = S.Split(step.Op, step.Value);
                        return (new PartRange(X, M, A, y), new PartRange(X, M, A, n));
                    }
                default:
                    throw new InvalidOperationException();
            }
        }
        public override string ToString() => $"{{x={X},m={M},a={A},s={S}}}";
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
                    Category.X => X,
                    Category.M => M,
                    Category.A => A,
                    Category.S => S,
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

    private readonly record struct Step(Category Category, Op Op, int Value, string Destination)
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

    private enum Category
    {
        None = 0,
        X,
        M,
        A,
        S
    }

    [GeneratedRegex(@"^(?<name>\w+)\{(?<steps>.+)\}$")]
    private static partial Regex BuildFlowPattern();
    [GeneratedRegex(@"^(?<category>[xmas])(?<op>[<>])(?<value>\d+):(?<destination>\w+)$")]
    private static partial Regex BuildStepPattern();
    [GeneratedRegex(@"^\{x=(?<x>\d+),m=(?<m>\d+),a=(?<a>\d+),s=(?<s>\d+)\}$")]
    private static partial Regex BuildPartPattern();
}
