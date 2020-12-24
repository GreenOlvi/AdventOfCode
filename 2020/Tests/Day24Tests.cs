using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day24;

namespace Tests
{
    [TestFixture]
    public class Day24Tests
    {
        private static readonly string[] ExampleData =
        {
            "sesenwnenenewseeswwswswwnenewsewsw",
            "neeenesenwnwwswnenewnwwsewnenwseswesw",
            "seswneswswsenwwnwse",
            "nwnwneseeswswnenewneswwnewseswneseene",
            "swweswneswnenwsewnwneneseenw",
            "eesenwseswswnenwswnwnwsewwnwsene",
            "sewnenenenesenwsewnenwwwse",
            "wenwwweseeeweswwwnwwe",
            "wsweesenenewnwwnwsenewsenwwsesesenwne",
            "neeswseenwwswnwswswnw",
            "nenwswwsewswnenenewsenwsenwnesesenew",
            "enewnwewneswsewnwswenweswnenwsenwsw",
            "sweneswneswneneenwnewenewwneswswnese",
            "swwesenesewenwneswnwwneseswwne",
            "enesenwswwswneneswsenwnewswseenwsese",
            "wnwnesenesenenwwnenwsewesewsesesew",
            "nenewswnwewswnenesenwnesewesw",
            "eneswnwswnwsenenwnwnwwseeswneewsenese",
            "neswnwewnwnwseenwseesewsenwsweewe",
            "wseweeenwnesenwwwswnew",
        };

        private readonly Puzzle _example = new Puzzle(ExampleData);

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be(10);
        }

        [Test]
        public void ParseLineTest()
        {
            Puzzle.ParseLine("nwwswee").Should().BeEquivalentTo(
                Puzzle.Direction.NW,
                Puzzle.Direction.W,
                Puzzle.Direction.SW,
                Puzzle.Direction.E,
                Puzzle.Direction.E);
        }

        [Test]
        public void Solution2Test()
        {
            _example.Solution2().Should().Be(2208);
        }
    }
}
