using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Day19
{
    public class ConcatRule : Rule
    {
        public ConcatRule(IEnumerable<Rule> rules) : base($"( {string.Join(" ", rules.Select(r => r.ToString()))} )")
        {
            _rules = rules.ToArray();
        }

        private readonly Rule[] _rules;
        public IReadOnlyCollection<Rule> Rules => Array.AsReadOnly(_rules);

        public override string ToRegex() => $"({string.Join("", _rules.Select(r => r.ToRegex()))})";
    }
}
