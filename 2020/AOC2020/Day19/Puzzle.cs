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
            var regex = ruleSet.BuildRegex();
            return _input.Count(regex.IsMatch);
        }

        public override int Solution2()
        {
            return 0;
        }
    }
}
