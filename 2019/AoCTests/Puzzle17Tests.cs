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

        [Test]
        public void CompressorTest()
        {
            //var input = "R,8,R,8,R,4,R,4,R,8,L,6,L,2,R,4,R,4,R,8,R,8,R,8,L,6,L,2".Split(",");
            var input = "R,8,L,10,R,8,R,12,R,8,L,8,L,12,R,8,L,10,R,8,L,12,L,10,L,8,R,8,L,10,R,8,R,12,R,8,L,8,L,12,L,12,L,10,L,8,L,12,L,10,L,8,R,8,L,10,R,8,R,12,R,8,L,8,L,12".Split(",");
            var c = new Compressor();
            c.Compress(input);
        }
    }
}
