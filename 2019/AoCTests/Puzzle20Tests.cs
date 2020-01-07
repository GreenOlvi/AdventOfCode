using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using AoC2019.Puzzle20;
using AoC2019.Common;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle20Tests
    {
        #region Examples
        private static readonly string[] _map1 = new[]
        {
        //             11111111112
        //   012345678901234567890
            "         A",            // 0
            "         A",            // 1
            "  #######.#########  ", // 2
            "  #######.........#  ", // 3
            "  #######.#######.#  ", // 4
            "  #######.#######.#  ", // 5
            "  #######.#######.#  ", // 6
            "  #####  B    ###.#  ", // 7
            "BC...##  C    ###.#  ", // 8
            "  ##.##       ###.#  ", // 9
            "  ##...DE  F  ###.#  ", // 10
            "  #####    G  ###.#  ", // 11
            "  #########.#####.#  ", // 12
            "DE..#######...###.#  ", // 13
            "  #.#########.###.#  ", // 14
            "FG..#########.....#  ", // 15
            "  ###########.#####  ", // 16
            "             Z",        // 17
            "             Z",        // 18
        };

        private static readonly string[] _map2 = new[]
        {
            "                   A",
            "                   A",
            "  #################.#############  ",
            "  #.#...#...................#.#.#  ",
            "  #.#.#.###.###.###.#########.#.#  ",
            "  #.#.#.......#...#.....#.#.#...#  ",
            "  #.#########.###.#####.#.#.###.#  ",
            "  #.............#.#.....#.......#  ",
            "  ###.###########.###.#####.#.#.#  ",
            "  #.....#        A   C    #.#.#.#  ",
            "  #######        S   P    #####.#  ",
            "  #.#...#                 #......VT",
            "  #.#.#.#                 #.#####  ",
            "  #...#.#               YN....#.#  ",
            "  #.###.#                 #####.#  ",
            "DI....#.#                 #.....#  ",
            "  #####.#                 #.###.#  ",
            "ZZ......#               QG....#..AS",
            "  ###.###                 #######  ",
            "JO..#.#.#                 #.....#  ",
            "  #.#.#.#                 ###.#.#  ",
            "  #...#..DI             BU....#..LF",
            "  #####.#                 #.#####  ",
            "YN......#               VT..#....QG",
            "  #.###.#                 #.###.#  ",
            "  #.#...#                 #.....#  ",
            "  ###.###    J L     J    #.#.###  ",
            "  #.....#    O F     P    #.#...#  ",
            "  #.###.#####.#.#####.#####.###.#  ",
            "  #...#.#.#...#.....#.....#.#...#  ",
            "  #.#####.###.###.#.#.#########.#  ",
            "  #...#.#.....#...#.#.#.#.....#.#  ",
            "  #.###.#####.###.###.#.#.#######  ",
            "  #.#.........#...#.............#  ",
            "  #########.###.###.#############  ",
            "           B   J   C",
            "           U   P   P",
        };
        #endregion

        [Test]
        public void MapParseTest()
        {
            var m = Map.Parse(_map1);

            m.Labels.Should().BeEquivalentTo(
                ("AA", new Position(9, 2), Direction.Up),
                ("BC", new Position(2, 8), Direction.Left),
                ("BC", new Position(9, 6), Direction.Down),
                ("DE", new Position(6, 10), Direction.Right),
                ("FG", new Position(11, 12), Direction.Up),
                ("DE", new Position(2, 13), Direction.Left),
                ("FG", new Position(2, 15), Direction.Left),
                ("ZZ", new Position(13, 16), Direction.Down));

            m.Start.Should().Be(new Position(9, 2));
            m.End.Should().Be(new Position(13, 16));
        }

        private static readonly IEnumerable<TestCaseData> Solve1TestCases = new[]
        {
            new TestCaseData(23, _map1),
            new TestCaseData(58, _map2),
        };

        [TestCaseSource(nameof(Solve1TestCases))]
        public void Solve1Tests(int expected, string[] map)
        {
            Solution.Solve1(map).Should().Be(expected);
        }
    }
}
