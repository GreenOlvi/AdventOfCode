using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using AoC2019.Puzzle18;
using AoC2019.Common;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle18Tests
    {
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

        [Test]
        public void MapAllKeysTest()
        {
            var m = Map.Parse(_smallMap);
            m.Start.Should().Be(new Position(5, 1));
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
            };

        [TestCaseSource(nameof(AvailableKeysTestCases))]
        public void AvailableKeysTests(string hasKeys, string[] map, string expected)
        {
            Map.Parse(map).AvailableKeys(hasKeys.ToCharArray())
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
