using System.Collections.Generic;
using AoC2019.Puzzle06;
using NUnit.Framework;
using FluentAssertions;
using System.Linq;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle06Tests
    {
        private static readonly string[] _testInput1 = new[]
        {
            "COM)B",
            "B)C",
            "C)D",
            "D)E",
            "E)F",
            "B)G",
            "G)H",
            "D)I",
            "E)J",
            "J)K",
            "K)L",
        };

        private static readonly IDictionary<string, string> _nodes1 = Solution.ParseInput(_testInput1);
        private static readonly IDictionary<string, string> _nodes2 =
            Solution.ParseInput(_testInput1.Concat(new[] { "K)YOU", "I)SAN" }));

        private static IEnumerable<TestCaseData> CountOrbitsTestCases()
        {
            yield return new TestCaseData("D", 3, _nodes1);
            yield return new TestCaseData("L", 7, _nodes1);
            yield return new TestCaseData("COM", 0, _nodes1);
        }

        [Test]
        [TestCaseSource(nameof(CountOrbitsTestCases))]
        public void CountOrbitsTests(string node, int result, IDictionary<string, string> nodes)
        {
            Solution.CountOrbits(node, nodes).Should().Be(result);
        }

        [Test]
        public void Solve1Test()
        {
            Solution.Solve1(_nodes1).Should().Be(42);
        }

        private static IEnumerable<TestCaseData> GetAncestorsTestCases()
        {
            yield return new TestCaseData(_nodes2, "YOU", new[] { "K", "J", "E", "D", "C", "B", "COM" });
            yield return new TestCaseData(_nodes2, "SAN", new[] { "I", "D", "C", "B", "COM" });
        }

        [Test]
        [TestCaseSource(nameof(GetAncestorsTestCases))]
        public void GetAncestorsTests(IDictionary<string, string> nodes, string node, string[] ancestors)
        {
            Solution.GetAncestors(node, nodes).Should().BeEquivalentTo(ancestors);
        }

        [Test]
        public void Solve2Test()
        {
            Solution.Solve2(_nodes2).Should().Be(4);
        }
    }
}
