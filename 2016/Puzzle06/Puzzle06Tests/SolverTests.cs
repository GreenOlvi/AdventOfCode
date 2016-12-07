using FluentAssertions;
using NUnit.Framework;
using Puzzle06;

namespace Puzzle06Tests
{
    [TestFixture]
    public class SolverTests
    {
        private static readonly string[] Input = {
            "eedadn",
            "drvtee",
            "eandsr",
            "raavrd",
            "atevrs",
            "tsrnev",
            "sdttsa",
            "rasrtv",
            "nssdts",
            "ntnada",
            "svetve",
            "tesnvt",
            "vntsnd",
            "vrdear",
            "dvrsen",
            "enarar",
        };

        [Test]
        public void Solve1ExampleTest()
        {
            new Solver(Input).Solve1().Should().Be("easter");
        }

        [Test]
        public void Solve2ExampleTest()
        {
            new Solver(Input).Solve2().Should().Be("advent");
        }
    }
}