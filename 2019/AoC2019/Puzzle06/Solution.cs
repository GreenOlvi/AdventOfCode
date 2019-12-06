using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2019.Puzzle06
{
    public class Solution : IPuzzle
    {
        public Solution(IEnumerable<string> lines)
        {
            _nodes = ParseInput(lines);
        }

        public static IDictionary<string, string> ParseInput(IEnumerable<string> lines) =>
            lines.Select(l => ParseLine(l)).ToDictionary(n => n.Node, n => n.Parent);

        private static (string Parent, string Node) ParseLine(string l)
        {
            var n = l.Split(')', StringSplitOptions.RemoveEmptyEntries);
            return (n[0], n[1]);
        }

        private readonly IDictionary<string, string> _nodes;

        public static int CountOrbits(string node, IDictionary<string, string> nodes) =>
            nodes.TryGetValue(node, out var parent) ? 1 + CountOrbits(parent, nodes) : 0;

        public static int Solve1(IDictionary<string, string> nodes)
        {
            return nodes.Keys.Sum(n => CountOrbits(n, nodes));
        }

        public static IEnumerable<string> GetAncestors(string node, IDictionary<string, string> nodes)
        {
            var n = node;
            while (nodes.ContainsKey(n))
            {
                yield return nodes[n];
                n = nodes[n];
            }
        }

        public static int Solve2(IDictionary<string, string> nodes)
        {
            var ancestors1 = GetAncestors("YOU", nodes).ToList();
            var ancestors2 = GetAncestors("SAN", nodes).ToList();

            var common = ancestors1.First(a => ancestors2.Contains(a));

            return ancestors1.IndexOf(common) + ancestors2.IndexOf(common);
        }

        public Task<string> Solve1Async() => 
            Task.Run(() => Solve1(_nodes).ToString());

        public Task<string> Solve2Async() =>
            Task.Run(() => Solve2(_nodes).ToString());
    }
}
