using System.Text;
using System.Text.RegularExpressions;
using AOC2021.Common;
using MoreLinq;

namespace AOC2021.Day14
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            _template = lines.First();
            _rules = lines.Skip(2).Select(ParseLine).ToDictionary(p => p.Item1, p => p.Item2);
        }

        private readonly string _template;
        private readonly Dictionary<(char, char), char> _rules;

        private static readonly Regex _rulePattern = new(@"^(?<a>\w)(?<b>\w)\s+->\s+(?<c>\w)$", RegexOptions.Compiled);

        private static ((char, char), char) ParseLine(string line)
        {
            var (a, b, c) = _rulePattern.Parse(line, ("a", s => s[0]), ("b", s => s[0]), ("c", s => s[0]));
            return ((a, b), c);
        }

        public static Counts ToCounts(string template)
        {
            var pairs = template.Pairwise((a, b) => (a, b))
                .GroupBy(p => p)
                .ToDictionary(g => g.Key, g => (long)g.Count());

            var ingredients = template.GroupBy(c => c).ToDictionary(g => g.Key, g => (long)g.Count());

            return new Counts { PairCount = pairs, IngredientCount = ingredients };
        }

        public Counts ProcessCounts(Counts state)
        {
            var ing = new Dictionary<char, long>(state.IngredientCount);
            var result = new Dictionary<(char, char), long>();
            foreach (var pair in state.PairCount)
            {
                var (a, b) = pair.Key;
                var r = _rules[pair.Key];
                var c = pair.Value;

                result.TryUpdate((a, r), v => v + c, c);
                result.TryUpdate((r, b), v => v + c, c);
                ing.TryUpdate(r, v => v + c, c);
            }
            return new Counts { PairCount = result, IngredientCount = ing };
        }

        public static string CountsToString(Dictionary<(char, char), long> counts) =>
            string.Join(", ", counts.Select(kv => ($"{kv.Key.Item1}{kv.Key.Item2}", kv.Value)).OrderBy(p => p.Item1).Select(p => $"{p.Item1}:{p.Item2}"));

        private Counts Process(Counts input, int n) =>
            Enumerable.Range(0, n).Aggregate(input, (value, i) => ProcessCounts(value));//var value = input;//for (var i = 0; i < n; i++)//{//    value = ProcessCounts(value);//}//return value;

        public override long Solution1()
        {
            var counts = Process(ToCounts(_template), 10);
            return counts.IngredientCount.Values.Max() - counts.IngredientCount.Values.Min();
        }

        public override long Solution2()
        {
            var counts = Process(ToCounts(_template), 40);
            return counts.IngredientCount.Values.Max() - counts.IngredientCount.Values.Min();
        }

        public class Counts
        {
            public Dictionary<(char, char), long> PairCount { get; init; }
            public Dictionary<char, long> IngredientCount { get; init; }
        }
    }
}
