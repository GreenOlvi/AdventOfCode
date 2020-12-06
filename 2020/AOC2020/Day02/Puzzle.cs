using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Line = System.ValueTuple<int, int, char, string>;

namespace AOC2020.Day02
{
    public class Puzzle : PuzzleBase<int, int>
    {
        public Puzzle(IEnumerable<string> input)
        {
            _input = input.Select(SplitLine).ToArray();
        }

        private readonly Line[] _input;

        private readonly static Regex LineRegex = new Regex(@"(?<i1>\d+)-(?<i2>\d+)\s(?<letter>\w):\s(?<pass>\w+)", RegexOptions.Compiled);

        private static Line SplitLine(string line)
        {
            var m = LineRegex.Match(line);
            if (!m.Success)
            {
                throw new PuzzleException("Invalid input line format");
            }

            var i1 = int.Parse(m.Groups["i1"].Value);
            var i2 = int.Parse(m.Groups["i2"].Value);
            var letter = m.Groups["letter"].Value[0];
            var pass = m.Groups["pass"].Value;
            return (i1, i2, letter, pass);
        }

        public static bool Rule1(Line input)
        {
            var (from, to, letter, password) = input;
            var l = password.Count(l => l == letter);
            return l >= from && l <= to;
        }

        public override int Solution1() => _input.Count(Rule1);

        public static bool Rule2(Line input)
        {
            var (p1, p2, letter, password) = input;
            return (password[p1 - 1] == letter) ^ (password[p2 - 1] == letter);
        }

        public override int Solution2() => _input.Count(Rule2);
    }
}
