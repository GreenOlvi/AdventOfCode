using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Day19
{
    public class OrRule : Rule
    {
        public OrRule(IEnumerable<Rule> rules)
            : this(rules.ToArray())
        {
        }

        public OrRule(params Rule[] rules)
            : base($"( {string.Join(" | ", rules.Select(r => r.ToString()))} )")
        {
            _rules = rules;
        }

        private readonly Rule[] _rules;

        public override MatchResult Match(string text)
        {
            var matches = _rules.Select(r => r.Match(text))
                .Where(m => m is Matched)
                .Cast<Matched>()
                .ToArray();

            if (!matches.Any())
            {
                return NotMatched;
            }
            return Matched(matches.SelectMany(m => m.Rests));
        }

        public override string ToRegex() => $"({string.Join("|", _rules.Select(r => r.ToRegex()))})";
    }
}
