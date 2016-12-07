using System.Linq;
using FluentAssertions;
using Puzzle05;
using NUnit.Framework;

namespace Puzzle05Tests
{
    [TestFixture]
    public class SolverTests
    {
        [Test]
        public void Solve1ExampleTest()
        {
            new Solver("abc").Solve1().Should().Be("18f47a30");
        }

        [Test]
        public void HashGeneratorTest()
        {
            new Solver.HashGenerator("abc", 3231929)
                .Select(b => Solver.HashGenerator.HashString(b))
                .First()
                .Should()
                .Be("00000155F8105DFF7F56EE10FA9B9ABD");
        }

        [Test]
        public void HashGeneratorWith5ZerosTest()
        {
            new Solver.HashGenerator("abc")
                .Where(h => Solver.StartsWith5Zeros(h))
                .Select(b => Solver.HashGenerator.HashString(b))
                .First()
                .Should()
                .Be("00000155F8105DFF7F56EE10FA9B9ABD");
        }

        [Test]
        public void Solve2ExampleTest()
        {
            new Solver("abc").Solve2().Should().Be("05ace8e3");
        }

    }
}
