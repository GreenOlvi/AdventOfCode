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
        public void Solution2Test()
        {
            _example.Solution2().Should().Be(149245887792);
        }
        
        [Test]
        public void BuildSuccessorListTest()
        {
            Puzzle.BuildSuccessorList(new[] { 3, 8, 9, 1, 2, 5, 4, 6, 7 })
                .Should().BeEquivalentTo(-1, 2, 5, 8, 6, 4, 7, 3, 9, 1);
        }
    }
}
