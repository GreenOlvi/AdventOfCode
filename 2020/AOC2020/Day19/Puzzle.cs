using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Day19
{
    public partial class Puzzle : PuzzleBase<int, int>
    {
        public Puzzle(IEnumerable<string> input)
        {
            var parts = input.SplitGroups().ToArray();
            _unparsedRules = parts[0].Select(ParseLine).ToDictionary();
            _input = parts[1];
        }

        private readonly string[] _input;
        private readonly Dictionary<int, string> _unparsedRules;

        private (int, string) ParseLine(string line)
        {
            var parts = line.Split(":", StringSplitOptions.TrimEntries);
            return (int.Parse(parts[0]), parts[1]);
        }

        public override int Solution1()
        {
            var ruleSet = new RuleSet(_unparsedRules);
            return _input.Count(line => ruleSet.IsMatch(line));
        }

        private static Dictionary<int, string> Modify(Dictionary<int, string> source, Func<KeyValuePair<int, string>, KeyValuePair<int, string>> visitor)
            => new Dictionary<int, string>(source.Select(kv => visitor(kv)));

        public override int Solution2()
        {
            var newRules = Modify(_unparsedRules, kv => kv.Key switch {
                8 => KeyValuePair.Create(kv.Key, "42 | 42 8"),
                11 => KeyValuePair.Create(kv.Key, "42 31 | 42 11 31"),
                _ => kv,
            });

            var ruleSet = new RuleSet(newRules);
            return _input.Count(line => ruleSet.IsMatch(line));
        }
    }
}
