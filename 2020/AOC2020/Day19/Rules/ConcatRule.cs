using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Day19
{
    public class ConcatRule : Rule
    {
        public ConcatRule(IEnumerable<Rule> rules)
            : this(rules.ToArray())
        {
        }

        public ConcatRule(params Rule[] rules)
            : base($"( {string.Join(" ", rules.Select(r => r.ToString()))} )")
        {
            _rules = rules;
        }

        private readonly Rule[] _rules;
        public IReadOnlyCollection<Rule> Rules => Array.AsReadOnly(_rules);

        public override string ToRegex() => $"({string.Join("", _rules.Select(r => r.ToRegex()))})";

        public override MatchResult Match(string text) => MatchRecursive(_rules, text);

        private MatchResult MatchRecursive(Rule[] rules, string text)
        {
            var rule = rules[0];

            var result = rule.Match(text);
            if (result is NotMatched)
            {
                return result;
            }

            var match = (Matched)result;

            if (rules.Length == 1)
            {
                return match;
            }
            else
            {
                var newRules = rules.Skip(1).ToArray();
                var rests = match.Rests
                    .Select(rest => MatchRecursive(newRules, rest))
                    .Where(m => m is Matched)
                    .Cast<Matched>()
                    .SelectMany(m => m.Rests)
                    .ToArray();

                return rests.Any() ? Matched(rests) : NotMatched;
            }
        }
    }
}
