using NUnit.Framework;
using FluentAssertions;
using AoC2019.Puzzle17;
using AoC2019.Common;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle17Tests
    {
        [Test]
        public void GridIntersectionsTest()
        {
            var g = string.Join('\n',
                "..#..........",
                "..#..........",
                "#######...###",
                "#.#...#...#.#",
                "#############",
                "..#...#...#..",
                "..#####...^..");

            var grid = new Grid(g);
            grid.GetIntersections().Should().BeEquivalentTo(new[]
            {
                new Position(2, 2),
                new Position(2, 4),
                new Position(6, 4),
                new Position(10, 4),
            });
        }
    }
}
