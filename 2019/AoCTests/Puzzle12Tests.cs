using NUnit.Framework;
using FluentAssertions;
using AoC2019.Puzzle12;
using System.Linq;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle12Tests
    {
        private static readonly Vector3[] _testPos1 = new[]
        {
            new Vector3(-1, 0, 2),
            new Vector3(2, -10, -7),
            new Vector3(4, -8, 8),
            new Vector3(3, 5, -1),
        };

        [Test]
        public void StepTest()
        {
            var pos = _testPos1;
            var vel = new[] { Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero };

            var (p2, v2) = Solution.Step(pos, vel);

            p2.Should().BeEquivalentTo(new[]
            {
                new Vector3(2, -1, 1),
                new Vector3(3, -7, -4),
                new Vector3(1, -7, 5),
                new Vector3(2, 2, 0),
            });

            v2.Should().BeEquivalentTo(new[]
            {
                new Vector3(3, -1, -1),
                new Vector3(1, 3, 3),
                new Vector3(-3, 1, -3),
                new Vector3(-1, -3, 1),
            });
        }

        [Test]
        public void StepsTest()
        {
            var pos = _testPos1;
            var vel = new[] { Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero };

            var (p2, v2) = Solution.Steps(pos, vel, 10);

            p2.Should().BeEquivalentTo(new[]
            {
                new Vector3(2, 1, -3),
                new Vector3(1, -8, 0),
                new Vector3(3, -6, 1),
                new Vector3(2, 0, 4),
            });

            v2.Should().BeEquivalentTo(new[]
            {
                new Vector3(-3, -2, 1),
                new Vector3(-1, 1, 3),
                new Vector3(3, 2, -3),
                new Vector3(1, -1, -1),
            });
        }

        [TestCase(2, 1, -3, 6)]
        [TestCase(-3, -2, 1, 6)]
        public void EnergyTests(int x, int y, int z, int expected)
        {
            Solution.Energy(new Vector3(x, y, z)).Should().Be(expected);
        }

        [TestCase(new[] { 0, 0, 0, 0, 0, 0, 0, 0 }, 0x00UL)]
        [TestCase(new[] { 0, 0, 0, 0, 0, 0, 0, 1 }, 0x01UL)]
        [TestCase(new[] { 0, 0, 0, 0, 0, 0, 1, 1 }, 0x0101UL)]
        [TestCase(new[] { 0, 0, 0, 0, 0, 255, 0, 0 }, 0xff0000UL)]
        [TestCase(new[] { 0, 0, 0, 0, 255, 0, 0, 0 }, 0xff000000UL)]
        [TestCase(new[] { 0, 0, 0, 255, 0, 0, 0, 0 }, 0xff00000000UL)]
        [TestCase(new[] { 0, 0, 255, 0, 0, 0, 0, 0 }, 0xff0000000000UL)]
        [TestCase(new[] { 0, 255, 0, 0, 0, 0, 0, 0 }, 0xff000000000000UL)]
        [TestCase(new[] { 255, 0, 0, 0, 0, 0, 0, 0 }, 0xff00000000000000UL)]
        public void HashCodeTests(int[] i, ulong expected)
        {
            Solution.HashCode(i).Should().Be(expected);
        }

        [TestCase(new[] {
            "<x=-1, y=0, z=2>",
            "<x=2, y=-10, z=-7>",
            "<x=4, y=-8, z=8>",
            "<x=3, y=5, z=-1>",
        }, 2772)]
        [TestCase(new[] {
            "<x=-8, y=-10, z=0>",
            "<x=5, y=5, z=10>",
            "<x=2, y=-7, z=3>",
            "<x=9, y=-8, z=-3>",
        }, 4_686_774_924)]
        public void Solve2Tests(string[] moons, long expected)
        {
            var pos = moons.Select(m => Solution.ParseVector(m)).ToArray();
            Solution.Solve2(pos).Should().Be(expected);
        }
    }
}
