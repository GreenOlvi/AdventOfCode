using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day23;

namespace Tests
{
    [TestFixture]
    public class Day23Tests
    {
        private const string ExampleData = "389125467";

        private readonly Puzzle _example = new Puzzle(ExampleData);

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be("67384529");
        }

        [Test]
        [Ignore("Not solved yet")]
        public void Solution2Test()
        {
            _example.Solution2().Should().Be(-1);
        }
    }
}
