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

        public Rule GetRule(int id)
        {
            if (!_parsedRules.ContainsKey(id))
            {
                _parsedRules[id] = ParseRule(_unparsedRules[id]);
            }
            return _parsedRules[id];
        }

        private static readonly Regex _concatRuleRegex = new Regex(@"\d+\s\d+", RegexOptions.Compiled);
        private static readonly Regex _letterRuleRegex = new Regex(@"""(?<char>\w)""", RegexOptions.Compiled);

        private Rule ParseRule(string unparsed)
        {
            Console.WriteLine($"Parsing rule [{unparsed}]...");

            if (int.TryParse(unparsed, out var id))
            {
                return GetRule(id);
            }

            if (unparsed.Contains("|"))
            {
                var parts = unparsed.Split("|", StringSplitOptions.TrimEntries);
                var left = ParseRule(parts[0]);
                var right = ParseRule(parts[1]);
                return new OrRule(left, right);
            }

            if (_concatRuleRegex.IsMatch(unparsed))
            {
                var rules = unparsed.Split(" ", StringSplitOptions.TrimEntries)
                    .ParseInts()
                    .Select(GetRule)
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
                var l = match.Groups["char"].Value;
                return new TextRule(l);
            }

            throw new PuzzleException($"Invalid rule: [{unparsed}]");
        }

        public Regex BuildRegex(int startingRule = 0) =>
            new Regex("^" + GetRule(startingRule).ToRegex() + "$", RegexOptions.Compiled);
    }
}
