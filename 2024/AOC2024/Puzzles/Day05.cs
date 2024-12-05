namespace AOC2024.Puzzles;

public class Day05 : CustomBaseProblem<long>
{
    private readonly Rule[] _rules;
    private readonly long[][] _updates;

    private readonly Dictionary<long, Dictionary<long, Rule>> _expandedRules;

    public Day05()
    {
        (_rules, _updates) = ParseInput(ReadLinesFromFile());
        _expandedRules = ExpandRules(_rules);
    }

    public Day05(IEnumerable<string> lines)
    {
        (_rules, _updates) = ParseInput(lines);
        _expandedRules = ExpandRules(_rules);
    }

    private static (Rule[] Rules, long[][] Updates) ParseInput(IEnumerable<string> lines)
    {
        var rules = lines.TakeWhile(static l => !string.IsNullOrWhiteSpace(l))
            .Select(static l =>
            {
                var s = l.Split('|', StringSplitOptions.TrimEntries);
                return new Rule(s[0].ToLong(), s[1].ToLong());
            }).ToArray();

        var updates = lines.SkipWhile(static l => !string.IsNullOrWhiteSpace(l))
            .Skip(1)
            .Select(static l => l.Split(',', StringSplitOptions.TrimEntries).Select(static e => e.ToLong()).ToArray())
            .ToArray();

        return (rules, updates);
    }

    private static Dictionary<long, Dictionary<long, Rule>> ExpandRules(Rule[] simpleRules)
    {
        var rules = new Dictionary<long, Dictionary<long, Rule>>();
        foreach (var rule in simpleRules)
        {
            if (!rules.TryGetValue(rule.A, out var aRules))
            {
                aRules = [];
                rules[rule.A] = aRules;
            }
            aRules[rule.B] = rule;

            if (!rules.TryGetValue(rule.B, out var bRules))
            {
                bRules = [];
                rules[rule.B] = bRules;
            }
            bRules[rule.A] = rule;
        }
        return rules;
    }

    private static bool IsCorrect(Dictionary<long, Dictionary<long, Rule>> rules, long[] update)
    {
        var order = update.Select(static (p, i) => (p, i)).ToDictionary();

        var relevantPages = update.Where(rules.ContainsKey);

        foreach (var (a, b) in relevantPages.EachPair())
        {
            var rule = rules[a][b];
            var orderA = order[a];
            var orderB = order[b];
            if (a == rule.A)
            {
                if (orderA > orderB)
                {
                    return false;
                }
            }
            else
            {
                if (orderB > orderA)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static long[] Reorder(Dictionary<long, Dictionary<long, Rule>> rules, long[] update)
    {
        var ordered = new List<long>() { update[0] };

        for (var i = 1; i < update.Length; i++)
        {
            var newElement = update[i];
            var newElementRules = rules[newElement];

            var insertIndex = 0;
            for (var j = 0; j < i; j++)
            {
                var el = ordered[j];
                var rule = newElementRules[el];

                if (rule.A == newElement)
                {
                    break;
                }
                else
                {
                    insertIndex++;
                }
            }
            ordered.Insert(insertIndex, newElement);
        }

        return [.. ordered];
    }

    private static long GetMiddle(long[] update) => update[update.Length / 2];

    public override long Solve1() =>
        _updates.Where(u => IsCorrect(_expandedRules, u))
            .Sum(GetMiddle);

    public override long Solve2() =>
        _updates.Where(u => !IsCorrect(_expandedRules, u))
            .Select(u => Reorder(_expandedRules, u))
            .Sum(GetMiddle);


    private readonly record struct Rule(long A, long B);
}
