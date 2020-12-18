using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day17;
using System.Linq;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class Day17Tests
    {
        private static readonly string[] ExampleData =
        {
            ".#.",
            "..#",
            "###",
        };

        private readonly Puzzle _example = new Puzzle(ExampleData);

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be(112);
        }

        [Test]
        public void Solution2Test()
        {
            _example.Solution2().Should().Be(848);
        }

        [Test]
        public void EnumeratePointsTest()
        {
            var min = new Vector(-3, -2, -1, 5);
            var max = new Vector(0, 1, 0, 7);

            var points = Puzzle.EnumeratePoints(min, max).ToArray();

            points.Should().HaveCount(96);
            points.All(p => p.N == 4).Should().BeTrue();
        }

        [Test]
        public void VectorEqualityTests()
        {
            var v1 = new Vector(1, 2, 3);

            v1.Should().Be(new Vector(1, 2, 3));
            v1.Should().NotBe(new Vector(2, 2, 2));
            v1.Should().NotBe(new Vector(1, 2));

            var set = new HashSet<Vector>(new[]
            {
                new Vector(1, 2, 3),
                new Vector(3, 2, 1),
            });

            set.Should().Contain(v1);
            set.Should().NotContain(new Vector(2, 1));
        }
    }
}
