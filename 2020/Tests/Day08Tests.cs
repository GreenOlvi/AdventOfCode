using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day08;

namespace Tests
{
    [TestFixture]
    public class Day08Tests
    {
        private static readonly string[] ExampleData =
        {
            "nop +0",
            "acc +1",
            "jmp +4",
            "acc +3",
            "jmp -3",
            "acc -99",
            "acc +1",
            "jmp -4",
            "acc +6",
        };

        private readonly Puzzle _example = new Puzzle(ExampleData);

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be(5);
        }

        [Test]
        public void Solution2Test()
        {
            _example.Solution2().Should().Be(8);
        }
    }
}
