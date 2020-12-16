using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Day16
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> input, Func<string, bool> ruleSelector)
        {
            var groups = input.SplitGroups().ToArray();
            _rules = groups[0].Select(ParseRule).ToArray();
            _myTicket = ParseTicket(groups[1].Skip(1).First());
            _nearbyTickets = groups[2].Skip(1).Select(ParseTicket).ToArray();
            _bigSet = SumSets(_rules.Select(ToHashSet));
            _ruleSelector = ruleSelector;
        }

        public Puzzle(IEnumerable<string> input) : this(input, r => r.StartsWith("departure"))
        {
        }

        private static Rule ParseRule(string line)
        {
            var parts = line.Split(":", StringSplitOptions.TrimEntries);
            var ranges = parts[1].Split("or", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var r = ranges.Select(r => r.Split("-", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(int.Parse).ToArray())
                .Select(r => (r[0], r[1]));
            return new Rule(parts[0], r.ToArray());
        }

        private static Ticket ParseTicket(string line) =>
            new Ticket(line.Split(",", StringSplitOptions.TrimEntries).Select(int.Parse).ToArray());

        private readonly Rule[] _rules;
        private readonly Ticket _myTicket;
        private readonly Ticket[] _nearbyTickets;
        private readonly HashSet<int> _bigSet;

        private readonly Func<string, bool> _ruleSelector;

        private int Columns => _myTicket.Values.Length;

        private record Rule(string Name, (int, int)[] Ranges)
        {
            public override string ToString() => Name;
        };

        private record Ticket(int[] Values);

        private static HashSet<int> ToHashSet(Rule rule) =>
            SumSets(rule.Ranges.Select(r => Enumerable.Range(r.Item1, r.Item2 - r.Item1 + 1).ToHashSet()));

        private static HashSet<int> SumSets(IEnumerable<HashSet<int>> sets) =>
            sets.Aggregate(new HashSet<int>(), (a, b) => a.Union(b).ToHashSet());

        private bool IsValid(Ticket ticket) => ticket.Values.All(_bigSet.Contains);

        private static IEnumerable<int> TakeColumn(IEnumerable<Ticket> tickets, int column) =>
            tickets.Select(t => t.Values[column]);

        private Dictionary<int, Rule> MatchRulesToColumns(int[][] columns)
        {
            var matching = new List<(Rule Rule, int[] Columns)>();
            foreach (var rule in _rules)
            {
                var ruleHash = ToHashSet(rule);
                var cols = columns.Select((c, i) => (i, c))
                    .Where(p => p.c.All(ruleHash.Contains))
                    .Select(p => p.i)
                    .ToArray();

                matching.Add((rule, cols));
            }

            var found = new Dictionary<int, Rule>();
            while (matching.Any())
            {
                var newMatches = matching.Select(m => (m.Rule, m.Columns.Where(c => !found.ContainsKey(c)).ToArray()))
                    .Where(p => p.Item2.Length == 1)
                    .ToArray();

                if (!newMatches.Any())
                {
                    throw new PuzzleException("Could not match rules to columns");
                }

                foreach (var (rule, cols) in newMatches)
                {
                    found.Add(cols.First(), rule);
                    matching.RemoveAll(m => m.Rule == rule);
                }
            }

            return found;
        }

        public override long Solution1() =>
            _nearbyTickets.SelectMany(t => t.Values.Where(v => !_bigSet.Contains(v))).Sum();

        public override long Solution2()
        {
            var validTickets = _nearbyTickets.Where(IsValid).Append(_myTicket).ToArray();
            var columns = Enumerable.Range(0, Columns)
                .Select(i => TakeColumn(validTickets, i).ToArray())
                .ToArray();

            var matched = MatchRulesToColumns(columns);

            return matched.Where(kv => _ruleSelector(kv.Value.Name))
                .Select(kv => kv.Key)
                .Select(i => _myTicket.Values[i])
                .Product();
        }
    }
}
