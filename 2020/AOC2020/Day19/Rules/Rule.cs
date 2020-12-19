using System.Collections.Generic;

namespace AOC2020.Day19
{
    public abstract class Rule
    {
        protected static readonly NotMatched NotMatched = new NotMatched();
        protected static Matched Matched(params string[] rests) => new Matched(rests);
        protected static Matched Matched(IEnumerable<string> rests) => new Matched(rests);

        protected Rule(string text)
        {
            _text = text;
        }

        private readonly string _text;

        public abstract string ToRegex();

        public abstract MatchResult Match(string text);

        public override string ToString() => _text;
    }

    public abstract record MatchResult();
    public record NotMatched() : MatchResult;
    public record Matched(IEnumerable<string> Rests) : MatchResult;
}
