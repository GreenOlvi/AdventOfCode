using System;

namespace AOC2020.Day19
{
    public class RuleLink : Rule
    {
        public RuleLink(int id, Func<Rule> ruleFactory) : base($"Rule({id})")
        {
            Id = id;
            _factory = ruleFactory;
        }

        public int Id { get; }
        private readonly Func<Rule> _factory;

        public override string ToRegex() => _factory().ToRegex();

        public override MatchResult Match(string text) => _factory().Match(text);
    }
}
