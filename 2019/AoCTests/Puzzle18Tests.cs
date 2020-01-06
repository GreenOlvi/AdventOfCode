using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using AoC2019.Puzzle18;
using AoC2019.Common;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle18Tests
    {
        #region Examples
        private static readonly string[] _smallMap = new[]
        {
            "#########",
            "#b.A.@.a#",
            "#########",
        };

        private static readonly string[] _map2 = new[]
        {
            "########################",
            "#f.D.E.e.C.b.A.@.a.B.c.#",
            "######################.#",
            "#d.....................#",
            "########################",
        };

        private static readonly string[] _map3 = new[]
        {
            "########################",
            "#...............b.C.D.f#",
            "#.######################",
            "#.....@.a.B.c.d.A.e.F.g#",
            "########################",
        };

        private static readonly string[] _map4 = new[]
        {
            "#################",
            "#i.G..c...e..H.p#",
            "########.########",
            "#j.A..b...f..D.o#",
            "########@########",
            "#k.E..a...g..B.n#",
            "########.########",
            "#l.F..d...h..C.m#",
            "#################",
        };

        private static readonly string[] _map5 = new[]
        {
            "########################",
            "#@..............ac.GI.b#",
            "###d#e#f################",
            "###A#B#C################",
            "###g#h#i################",
            "########################",
        };

        private static readonly string[] _multi1 = new[]
        {
            "#######",
            "#a.#Cd#",
            "##...##",
            "##.@.##",
            "##...##",
            "#cB#Ab#",
            "#######",
        };

        private static readonly string[] _multi1mod = new[]
        {
            "#######",
            "#a.#Cd#",
            "##@#@##",
            "#######",
            "##@#@##",
            "#cB#Ab#",
            "#######",
        };

        private static readonly string[] _multi2 = new[]
        {
            "###############",
            "#d.ABC.#.....a#",
            "######@#@######",
            "###############",
            "######@#@######",
            "#b.....#.....c#",
            "###############",
        };

        private static readonly string[] _multi3 = new[]
        {
            "#############",
            "#DcBa.#.GhKl#",
            "#.###@#@#I###",
            "#e#d#####j#k#",
            "###C#@#@###J#",
            "#fEbA.#.FgHi#",
            "#############",
        };

        private static readonly string[] _multi4 = new[]
        {
            "#############",
            "#g#f.D#..h#l#",
            "#F###e#E###.#",
            "#dCba@#@BcIJ#",
            "#############",
            "#nK.L@#@G...#",
            "#M###N#H###.#",
            "#o#m..#i#jk.#",
            "#############",
        };
        #endregion

        [Test]
        public void MapAllKeysTest()
        {
            var m = Map.Parse(_smallMap);
            m.Start.Should().BeEquivalentTo(new Position(5, 1));
            m.AllKeys.Should().BeEquivalentTo(new[] { new Position(7, 1), new Position(1, 1) });
            m.AllDoors.Should().BeEquivalentTo(new[] { new Position(3, 1) });
        }

        private static IEnumerable<TestCaseData> AvailableKeysTestCases() =>
            new[]
            {
                new TestCaseData("", _smallMap, "a"),
                new TestCaseData("b", _smallMap, "a"),
                new TestCaseData("a", _smallMap, "b"),
                new TestCaseData("ba", _smallMap, ""),
                new TestCaseData("", _map4, "cebfagdh"),
                new TestCaseData("abge", _map4, "cdfhijkn"),
                new TestCaseData("", _multi3, "a"),
                new TestCaseData("abcdefghi", _multi3, "j"),
            };

        [TestCaseSource(nameof(AvailableKeysTestCases))]
        public void AvailableKeysFastTests(string hasKeys, string[] map, string expected)
        {
            var m = Map.Parse(map);
            m.AvailableKeysFast(hasKeys.ToCharArray())
                .Should().BeEquivalentTo(expected.ToCharArray(), o => o.WithoutStrictOrdering());
        }

        private static IEnumerable<TestCaseData> GetDistanceTestCases() =>
            new[]
            {
                new TestCaseData('@', 'a', 2, _smallMap),
                new TestCaseData('a', 'b', 6, _smallMap),
                new TestCaseData('b', 'a', 6, _smallMap),
                new TestCaseData('i', 'j', 16, _map4),
                new TestCaseData('@', 'a', 3, _map4),
            };

        [TestCaseSource(nameof(GetDistanceTestCases))]
        public void GetDistancesTests(char from, char to, int dist, string[] map)
        {
            Map.Parse(map).GetDistance(from, to).Should().Be(dist);
        }

        private static IEnumerable<TestCaseData> Solve1TestCases() =>
            new[]
            {
                new TestCaseData(_smallMap, 8),
                new TestCaseData(_map2, 86),
                new TestCaseData(_map3, 132),
                new TestCaseData(_map4, 136),
                new TestCaseData(_map5, 81),
            };

        [TestCaseSource(nameof(Solve1TestCases))]
        public void Solve1Tests(string[] map, int steps)
        {
            Solution.Solve1(map).Should().Be(steps);
        }

        private static IEnumerable<TestCaseData> ShortestPathTestCases() =>
            new[]
            {
                new TestCaseData(_smallMap, "ab"),
                new TestCaseData(_map2, "abcdef"),
                new TestCaseData(_map3, "bacdfeg"),

                // does not match examples because there are many shortest paths
                //new TestCaseData(_map4, "afbjgnhdloepcikm"),
                //new TestCaseData(_map5, "acfidgbeh"),
            };

        [TestCaseSource(nameof(ShortestPathTestCases))]
        public void ShortestPathTest(string[] map, string expected)
        {
            var (path, _) = Map.Parse(map).FindShortestWay('@', Array.Empty<char>());
            string.Join("", path).Should().Be("@" + expected);
        }

        private static IEnumerable<TestCaseData> Solve2TestCases() =>
            new[]
            {
                new TestCaseData(_multi1, 8),
                new TestCaseData(_multi1mod, 8),
                new TestCaseData(_multi2, 24),
                new TestCaseData(_multi3, 32),
                new TestCaseData(_multi4, 72),
            };

        [TestCaseSource(nameof(Solve2TestCases))]
        public void Solve2Test(string[] map, int steps)
        {
            Solution.Solve2(map).Should().Be(steps);
        }


        [TestCase("abcde", 0x1fu)]
        [TestCase("AbCdE", 0x1fu)]
        [TestCase("edcba", 0x1fu)]
        [TestCase("a", 0x01u)]
        [TestCase("f", 0x20u)]
        public void KeyHashTests(string keys, uint expected)
        {
            Map.KeyHash(keys).Should().Be(expected);
        }

        [TestCase("abc", 'a', 0x6100000007u)]
        [TestCase("abcde", 'z', 0x7a0000001fu)]
        [TestCase("abcdefghijklmnopqrstuvwxyz", '@', 0x4003ffffffu)]
        [TestCase("abcde", '@', 0x400000001fu)]
        public void CombinedHashTests(string keys, char pos, ulong expected)
        {
            Map.CombinedHash(keys, pos).Should().Be(expected);
        }
    }
}
