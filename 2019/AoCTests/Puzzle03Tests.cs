using NUnit.Framework;
using AoC2019.Puzzle03;
using FluentAssertions;
using System.Linq;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle03Tests
    {
        [Test]
        [TestCase("R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83", 610)]
        [TestCase("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7", 410)]
        public void Solve2Test(string w1, string w2, int result)
        {
            var wires = new[] { w1, w2 }.Select(Solution.ParseWire);
            Solution.Solve2(wires).Should().Be(result);
        }
    }
}
