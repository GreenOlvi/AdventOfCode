using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day19;

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

        private readonly Puzzle _example = new Puzzle(ExampleData);

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be(2);
        }

        [Test]
        [Ignore("Not implemented yet")]
        public void Solution2Test()
        {
            _example.Solution2().Should().Be(-1);
        }
    }
}
