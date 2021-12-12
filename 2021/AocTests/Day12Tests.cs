using NUnit.Framework;
using AOC2021.Day12;
using FluentAssertions;

namespace AocTests
{
    public class Day12Tests
    {
        private static readonly string[] _smallTestInput = new string[]
        {
            "start-A",
            "start-b",
            "A-c",
            "A-b",
            "b-d",
            "A-end",
            "b-end",
        };

        private static readonly string[] _slightlyLargerTestInput = new string[]
        {
            "dc-end",
            "HN-start",
            "start-kj",
            "dc-start",
            "dc-HN",
            "LN-dc",
            "HN-end",
            "kj-sa",
            "kj-HN",
            "kj-dc",
        };

        private static readonly string[] _evenLargerTestInput = new string[]
        {
            "fs-end",
            "he-DX",
            "fs-he",
            "start-DX",
            "pj-DX",
            "end-zg",
            "zg-sl",
            "zg-pj",
            "pj-he",
            "RW-he",
            "fs-DX",
            "pj-RW",
            "zg-RW",
            "start-pj",
            "he-WI",
            "zg-he",
            "pj-fs",
            "start-RW",
        };

        private static readonly TestCaseData[] _testData1 = new[]
        {
            new TestCaseData(_smallTestInput, 10),
            new TestCaseData(_slightlyLargerTestInput, 19),
            new TestCaseData(_evenLargerTestInput, 226),
        };

        private static readonly TestCaseData[] _testData2 = new[]
        {
            new TestCaseData(_smallTestInput, 36),
            new TestCaseData(_slightlyLargerTestInput, 103),
            new TestCaseData(_evenLargerTestInput, 3509),
        };

        [TestCaseSource(nameof(_testData1))]
        public void Solution1Test(string[] input, long expected)
        {
            var puzzle = new Puzzle(input);
            puzzle.Solution1().Should().Be(expected);
        }

        [TestCaseSource(nameof(_testData2))]
        public void Solution2Test(string[] input, long expected)
        {
            var puzzle = new Puzzle(input);
            puzzle.Solution2().Should().Be(expected);
        }
    }
}