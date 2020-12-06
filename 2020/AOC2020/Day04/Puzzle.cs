using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2020.Day04
{
    public class Puzzle : PuzzleBase<int, int>
    {
        public Puzzle(IEnumerable<string> input)
        {
            _input = input.ToArray();
        }

        private readonly string[] _input;

        private static Dictionary<string, string> SplitFields(IEnumerable<string> lines) =>
            lines.SelectMany(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(e => e.Split(':')))
                .ToDictionary(a => a[0], a => a[1]);

        private static readonly string[] requiredFields = new[]
        {
            "byr",
            "iyr",
            "eyr",
            "hgt",
            "hcl",
            "ecl",
            "pid",
            //"cid",
        };

        private static readonly Regex hairColorRegex = new Regex(@"^#[0-9a-f]{6}$", RegexOptions.Compiled);

        private static readonly HashSet<string> eyeColors = new HashSet<string>(new[]
        {
            "amb", "blu", "brn", "gry", "grn", "hzl", "oth",
        });

        private static readonly Regex passId = new Regex(@"^\d{9}$", RegexOptions.Compiled);

        private static readonly Regex heightRegex = new Regex(@"(?<value>\d+)(?<units>cm|in)", RegexOptions.Compiled);

        private static bool ValidateHeight(string s)
        {
            var m = heightRegex.Match(s);
            if (!m.Success)
            {
                return false;
            }

            if (!int.TryParse(m.Groups["value"].Value, out var val))
            {
                return false;
            }

            var units = m.Groups["units"].Value;
            return units switch
            {
                "cm" => val is (>= 150 and <= 193),
                "in" => val is (>= 59 and <= 76),
                _ => throw new NotImplementedException(),
            };
        }

        private static readonly IDictionary<string, Func<string, bool>> fieldValidators = new Dictionary<string, Func<string, bool>>()
        {
            { "byr", s => int.TryParse(s, out var i) && i >= 1920 && i <= 2002 },
            { "iyr", s => int.TryParse(s, out var i) && i >= 2010 && i <= 2020 },
            { "eyr", s => int.TryParse(s, out var i) && i >= 2020 && i <= 2030 },
            { "hgt", s => ValidateHeight(s) },
            { "hcl", s => hairColorRegex.IsMatch(s) },
            { "ecl", s => eyeColors.Contains(s) },
            { "pid", s => passId.IsMatch(s) },
            { "cid", s => true },
        };

        public static bool IsFieldValid(string name, string value) => fieldValidators[name](value);

        private static bool HasValidFields(Dictionary<string, string> p) => p.All(f => IsFieldValid(f.Key, f.Value));

        private static bool HasRequiredFields(Dictionary<string, string> p) => requiredFields.All(r => p.ContainsKey(r));

        public override int Solution1() => _input.SplitGroups().Select(SplitFields).Count(p => HasRequiredFields(p));

        public override int Solution2() => _input.SplitGroups().Select(SplitFields).Count(p => HasRequiredFields(p) && HasValidFields(p));
    }
}
