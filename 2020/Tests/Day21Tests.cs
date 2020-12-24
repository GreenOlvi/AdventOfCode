using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day21;

namespace Tests
{
    [TestFixture]
    public class Day21Tests
    {
        private static readonly string[] ExampleData =
        {
            "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)",
            "trh fvjkl sbzzf mxmxvkd (contains dairy)",
            "sqjhc fvjkl (contains soy)",
            "sqjhc mxmxvkd sbzzf (contains fish)",
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
            _example.Solution2().Should().Be("mxmxvkd,sqjhc,fvjkl");
        }
    }
}
