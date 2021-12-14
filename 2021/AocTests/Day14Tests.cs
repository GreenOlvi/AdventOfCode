using NUnit.Framework;
using AOC2021.Day14;
using FluentAssertions;
using System.Collections.Generic;

namespace AocTests
{
    public class Day14Tests
    {
        private static readonly string[] _testInput = new string[]
        {
            "NNCB",
            "",
            "CH -> B",
            "HH -> N",
            "CB -> H",
            "NH -> C",
            "HB -> C",
            "HC -> B",
            "HN -> C",
            "NN -> C",
            "BH -> H",
            "NC -> B",
            "NB -> B",
            "BN -> B",
            "BB -> N",
            "BC -> B",
            "CC -> N",
            "CN -> C",
        };

        private readonly Puzzle _puzzle = new(_testInput);

        [Test]
        public void Solution1Test()
        {
            _puzzle.Solution1().Should().Be(1588);
        }

        [Test]
        public void Solution2Test()
        {
            _puzzle.Solution2().Should().Be(2188189693529);
        }

        [Test]
        public void ToCountsTest()
        {
            Puzzle.ToCounts("NNCB").Should().BeEquivalentTo(new Puzzle.Counts
            {
                PairCount = new Dictionary<(char, char), long>
                {
                    { ('N', 'N'), 1 },
                    { ('N', 'C'), 1 },
                    { ('C', 'B'), 1 },
                },
                IngredientCount = new Dictionary<char, long>
                {
                    { 'N', 2 },
                    { 'B', 1 },
                    { 'C', 1 },
                }
            });
        }

        [TestCase("NNCB", "NCNBCHB")]
        [TestCase("NCNBCHB", "NBCCNBBBCBHCB")]
        [TestCase("NBCCNBBBCBHCB", "NBBBCNCCNBBNBNBBCHBHHBCHB")]
        [TestCase("NBBBCNCCNBBNBNBBCHBHHBCHB", "NBBNBNBBCCNBCNCCNBBNBBNBBBNBBNBBCBHCBHHNHCBBCBHCB")]
        public void ProcessCounts2Test(string input, string expected)
        {
            var inputCounts = Puzzle.ToCounts(input);
            var expectedCounts = Puzzle.ToCounts(expected);
            _puzzle.ProcessCounts(inputCounts).Should().BeEquivalentTo(expectedCounts);
        }
    }
}