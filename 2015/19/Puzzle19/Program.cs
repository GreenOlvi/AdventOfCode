using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

internal partial class Program
{
    private const string InputFile = "input.txt";
    private static void Main(string[] args)
    {
        var (rules, molecule) = ParseInput(File.ReadLines(InputFile));

        var result1 = Solve1(rules, molecule);
        Assert.AreEqual(509, result1);
        Console.WriteLine("Result 1 = " + result1);
    }

    private static ((string, string)[], string) ParseInput(IEnumerable<string> input)
    {
        var rules = input.TakeWhile(i => i != string.Empty).Select(ParseRule).ToArray();
        var mol = input.SkipWhile(l => l.Contains("=>") || l == string.Empty).First();

        return (rules, mol);
    }

    private static Dictionary<string, string[]> ToDictionary((string, string)[] rules)
    {
        var dict = new Dictionary<string, string[]>();
        foreach (var left in rules.Select(r => r.Item1).Distinct())
        {
            var right = rules.Where(r => r.Item1 == left).Select(r => r.Item2).ToArray();
            dict.Add(left, right);
        }

        return dict;
    }

    [GeneratedRegex(@"^(?<from>\w+) => (?<to>\w+)$", RegexOptions.Compiled)]
    private static partial Regex MakeRulePattern();
    private static readonly Regex RulePattern = MakeRulePattern();

    private static (string, string) ParseRule(string line)
    {
        var match = RulePattern.Match(line);
        if (!match.Success)
        {
            throw new InvalidDataException();
        }

        return (match.Groups["from"].Value, match.Groups["to"].Value);
    }

    private static int Solve1((string, string)[] rules, string molecule)
    {
        var molecules = new HashSet<string>();
        var dict = ToDictionary(rules);
        foreach (var kv in dict)
        {
            var pattern = new Regex(kv.Key);
            foreach (var match in pattern.EnumerateMatches(molecule))
            {
                foreach (var r in kv.Value)
                {
                    var newMolecule = ReplaceSubstring(molecule, r, match.Index, match.Length);
                    molecules.Add(newMolecule);
                }
            }
        }

        return molecules.Count;
    }

    private static string ReplaceSubstring(string text, string replacement, int start, int length)
    {
        var part1 = text.Substring(0, start);
        var part2 = text.Substring(start + length, text.Length - (start + length));
        return part1 + replacement + part2;
    }
}