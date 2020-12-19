using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day19;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class Day19Tests
    {
        private static readonly string[] ExampleData =
        {
            "0: 4 1 5",
            "1: 2 3 | 3 2",
            "2: 4 4 | 5 5",
            "3: 4 5 | 5 4",
            "4: \"a\"",
            "5: \"b\"",
            "",
            "ababbb",
            "bababa",
            "abbbab",
            "aaabbb",
            "aaaabbb",
        };

        private static readonly string[] ExampleData2 =
        {
            "42: 9 14 | 10 1",
            "9: 14 27 | 1 26",
            "10: 23 14 | 28 1",
            "1: \"a\"",
            "11: 42 31",
            "5: 1 14 | 15 1",
            "19: 14 1 | 14 14",
            "12: 24 14 | 19 1",
            "16: 15 1 | 14 14",
            "31: 14 17 | 1 13",
            "6: 14 14 | 1 14",
            "2: 1 24 | 14 4",
            "0: 8 11",
            "13: 14 3 | 1 12",
            "15: 1 | 14",
            "17: 14 2 | 1 7",
            "23: 25 1 | 22 14",
            "28: 16 1",
            "4: 1 1",
            "20: 14 14 | 1 15",
            "3: 5 14 | 16 1",
            "27: 1 6 | 14 18",
            "14: \"b\"",
            "21: 14 1 | 1 14",
            "25: 1 1 | 1 14",
            "22: 14 14",
            "8: 42",
            "26: 14 22 | 1 20",
            "18: 15 15",
            "7: 14 5 | 1 21",
            "24: 14 1",
            "",
            "abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa",
            "bbabbbbaabaabba",
            "babbbbaabbbbbabbbbbbaabaaabaaa",
            "aaabbbbbbaaaabaababaabababbabaaabbababababaaa",
            "bbbbbbbaaaabbbbaaabbabaaa",
            "bbbababbbbaaaaaaaabbababaaababaabab",
            "ababaaaaaabaaab",
            "ababaaaaabbbaba",
            "baabbaaaabbaaaababbaababb",
            "abbbbabbbbaaaababbbbbbaaaababb",
            "aaaaabbaabaaaaababaa",
            "aaaabbaaaabbaaa",
            "aaaabbaabbaaaaaaabbbabbbaaabbaabaaa",
            "babaaabbbaaabaababbaabababaaab",
            "aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba",
        };

        private static readonly TestCaseData[] Solution1Examples = new[]
        {
            new TestCaseData(ExampleData, 2),
            new TestCaseData(ExampleData2, 3),
        };

        [TestCaseSource(nameof(Solution1Examples))]
        public void Solution1Test(string[] input, int expected)
        {
            new Puzzle(input).Solution1().Should().Be(expected);
        }

        private readonly Puzzle _example2 = new Puzzle(ExampleData2);

        [Test]
        public void Solution2Test()
        {
            _example2.Solution2().Should().Be(12);
        }

        private static readonly RuleSet LoopingRuleSet = new RuleSet(new Dictionary<int, string>() {
            { 0, "8 11" },
            { 1, "\"a\"" },
            { 2, "1 24 | 14 4" },
            { 3, "5 14 | 16 1" },
            { 4, "1 1" },
            { 5, "1 14 | 15 1" },
            { 6, "14 14 | 1 14" },
            { 7, "14 5 | 1 21" },
            { 8, "42 | 42 8" },
            { 9, "14 27 | 1 26" },
            { 10, "23 14 | 28 1" },
            { 11, "42 31 | 42 11 31" },
            { 12, "24 14 | 19 1" },
            { 13, "14 3 | 1 12" },
            { 14, "\"b\"" },
            { 15, "1 | 14" },
            { 16, "15 1 | 14 14" },
            { 17, "14 2 | 1 7" },
            { 18, "15 15" },
            { 19, "14 1 | 14 14" },
            { 20, "14 14 | 1 15" },
            { 21, "14 1 | 1 14" },
            { 22, "14 14" },
            { 23, "25 1 | 22 14" },
            { 24, "14 1" },
            { 25, "1 1 | 1 14" },
            { 26, "14 22 | 1 20" },
            { 27, "1 6 | 14 18" },
            { 28, "16 1" },
            { 31, "14 17 | 1 13" },
            { 42, "9 14 | 10 1" },
        });

        [TestCase("abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa", false)]
        [TestCase("bbabbbbaabaabba", true)]
        [TestCase("babbbbaabbbbbabbbbbbaabaaabaaa", true)]
        [TestCase("aaabbbbbbaaaabaababaabababbabaaabbababababaaa", true)]
        [TestCase("bbbbbbbaaaabbbbaaabbabaaa", true)]
        [TestCase("bbbababbbbaaaaaaaabbababaaababaabab", true)]
        [TestCase("ababaaaaaabaaab", true)]
        [TestCase("ababaaaaabbbaba", true)]
        [TestCase("baabbaaaabbaaaababbaababb", true)]
        [TestCase("abbbbabbbbaaaababbbbbbaaaababb", true)]
        [TestCase("aaaaabbaabaaaaababaa", true)]
        [TestCase("aaaabbaaaabbaaa", false)]
        [TestCase("aaaabbaabbaaaaaaabbbabbbaaabbaabaaa", true)]
        [TestCase("babaaabbbaaabaababbaabababaaab", false)]
        [TestCase("aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba", true)]
        public void MatchingTests(string line, bool expected)
        {
            LoopingRuleSet.IsMatch(line).Should().Be(expected);
        }

        [Test]
        public void TextRuleMatchTests()
        {
            var rule = new TextRule("ab");

            {
                var m = rule.Match("abcd");
                m.Should().BeOfType<Matched>();
                ((Matched)m).Rests.Should().BeEquivalentTo("cd");
            }

            {
                var m = rule.Match("ab");
                m.Should().BeOfType<Matched>();
                ((Matched)m).Rests.Should().BeEquivalentTo(string.Empty);
            }

            rule.Match("def").Should().BeOfType<NotMatched>();
        }

        [Test]
        public void ConcatRuleMatchTests()
        {
            var rule = new ConcatRule(new TextRule("ab"), new TextRule("cd"));

            {
                var m = rule.Match("abcd");
                m.Should().BeOfType<Matched>();
                ((Matched)m).Rests.Should().BeEquivalentTo(string.Empty);
            }

            {
                var m = rule.Match("abcdef");
                m.Should().BeOfType<Matched>();
                ((Matched)m).Rests.Should().BeEquivalentTo("ef");
            }

            rule.Match("ab").Should().BeOfType<NotMatched>();
            rule.Match("cd").Should().BeOfType<NotMatched>();
            rule.Match("cdab").Should().BeOfType<NotMatched>();
        }

        [Test]
        public void OrRuleMatchTests()
        {
            var rule = new OrRule(new TextRule("ab"), new TextRule("cd"));

            {
                var m = rule.Match("abcd");
                m.Should().BeOfType<Matched>();
                ((Matched)m).Rests.Should().BeEquivalentTo("cd");
            }

            {
                var m = rule.Match("cd");
                m.Should().BeOfType<Matched>();
                ((Matched)m).Rests.Should().BeEquivalentTo(string.Empty);
            }

            rule.Match("aab").Should().BeOfType<NotMatched>();
        }

        [Test]
        public void ConcatRuleWithBacktrackingMatchTests()
        {
            var rule = new ConcatRule(
                new OrRule(new TextRule("a"), new TextRule("ab")),
                new OrRule(new TextRule("b"), new TextRule("c")));

            {
                var m = rule.Match("abcd");
                m.Should().BeOfType<Matched>();
                ((Matched)m).Rests.Should().BeEquivalentTo("cd", "d");
            }

            rule.Match("cab").Should().BeOfType<NotMatched>();
        }
    }
}
