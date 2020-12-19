using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2020.Day19
{
    public class RuleSet
    {
        public RuleSet(Dictionary<int, string> unparsedRules)
        {
            _unparsedRules = unparsedRules;
        }

        private readonly Dictionary<int, string> _unparsedRules;
        private readonly Dictionary<int, Rule> _parsedRules = new Dictionary<int, Rule>();

        public Rule GetRule(int id) => GetRule(id, Enumerable.Empty<int>());

        private Rule GetRule(int id, IEnumerable<int> currentlyParsing)
        {
            if (!_parsedRules.ContainsKey(id))
            {
                _parsedRules[id] = ParseRule(_unparsedRules[id], currentlyParsing.Append(id));
            }
            return _parsedRules[id];
        }

        private static readonly Regex _concatRuleRegex = new Regex(@"\d+\s\d+", RegexOptions.Compiled);
        private static readonly Regex _letterRuleRegex = new Regex(@"""(?<text>\w+)""", RegexOptions.Compiled);

        private Rule ParseRule(string unparsed, IEnumerable<int> currentlyParsing)
        {
            if (int.TryParse(unparsed, out var id))
            {
                if (currentlyParsing.Contains(id))
                {
                    return new RuleLink(id, () => GetRule(id));
                }
                else
                {
                    return GetRule(id);
                }
            }

            if (unparsed.Contains("|"))
            {
                var rules = unparsed.Split("|", StringSplitOptions.TrimEntries)
                    .Select(s => ParseRule(s, currentlyParsing))
                    .ToArray();

                return new OrRule(rules);
            }

            if (_concatRuleRegex.IsMatch(unparsed))
            {
                var rules = unparsed.Split(" ", StringSplitOptions.TrimEntries)
                    .Select(s => ParseRule(s, currentlyParsing))
                    .ToArray();

                if (rules.All(r => r is TextRule))
                {
                    var text = string.Join(string.Empty, rules.Cast<TextRule>().Select(t => t.Text));
                    return new TextRule(text);
                }

                return new ConcatRule(rules);
            }

            if (_letterRuleRegex.TryMatch(unparsed, out var match))
            {
                return new TextRule(match.Groups["text"].Value);
            }

            throw new PuzzleException($"Invalid rule: [{unparsed}]");
        }

        public Regex BuildRegex(int startingRule = 0) =>
            new Regex("^" + GetRule(startingRule).ToRegex() + "$", RegexOptions.Compiled);

        public bool IsMatch(string text, int rule = 0) =>
            GetRule(rule).Match(text) is Matched m && m.Rests.Any(r => r == string.Empty);

    }
}
