using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day07;

namespace Tests
{
    [TestFixture]
    public class Day07Tests
    {
        private static readonly string[] ExampleData =
        {
            "light red bags contain 1 bright white bag, 2 muted yellow bags.",
            "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
            "bright white bags contain 1 shiny gold bag.",
            "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
            "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
            "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
            "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
            "faded blue bags contain no other bags.",
            "dotted black bags contain no other bags.",
        };

        private readonly Puzzle _example = new Puzzle(ExampleData);

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be(4);
        }

        [Test]
        public void Solution2Test()
        {
            _example.Solution2().Should().Be(32);
        }
    }
}
