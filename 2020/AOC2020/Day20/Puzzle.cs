using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2020.Day20
{
    public class Puzzle : PuzzleBase<long, int>
    {
        public Puzzle(IEnumerable<string> input)
        {
            var tiles = input.SplitGroups();
            _tiles = tiles.Select(ParseTile).ToDictionary();
        }

        private const int TileSize = 10;

        private static readonly Regex TileHeaderRegex = new Regex(@"Tile (?<id>\d+):", RegexOptions.Compiled);

        private (int, Tile) ParseTile(string[] tileLines)
        {
            if (!TileHeaderRegex.TryMatch(tileLines[0], out var match))
            {
                throw new PuzzleException($"Invalid tile header: [{tileLines[0]}]");
            }

            var id = int.Parse(match.Groups["id"].Value);

            var content = Enumerable.Range(1, TileSize)
                .Select(i => tileLines[i])
                .Select(line => (ushort)line.Select((c, o) => c == '#' ? (1 << (TileSize - o - 1)) : 0).Sum())
                .ToArray();

            return (id, new Tile(id, content));
        }

        private readonly Dictionary<int, Tile> _tiles;

        private record Tile
        {
            public Tile(int id, ushort[] content)
            {
                Id = id;
                Content = content;
                _text = string.Join("\n", Content.Select(Unpack));
            }

            public int Id { get; }
            public ushort[] Content { get; }

            private readonly string _text;

            public override string ToString() => $"Tile {Id}:\n" + _text;

            private static string Unpack(ushort n) =>
                new string(Enumerable.Range(0, TileSize)
                    .Select(i => (n & (1 << (TileSize - i - 1))) > 0 ? '#' : '.')
                    .ToArray());

        };

        private static ushort ReverseBits(ushort n)
        {
            ushort result = 0;
            for (int i = 0; i < TileSize; i++)
            {
                if ((n & (1 << (TileSize - i - 1))) > 0)
                {
                    result += (ushort)(1 << i);
                }
            }

            return result;
        }

        private static ushort[] GetEdges(Tile tile)
        {
            var top = tile.Content[0];
            var bottom = tile.Content.Last();
            var right = (ushort)tile.Content.Select((l, i) => (l & 1) > 0 ? 1 << i : 0).Sum();
            var left = (ushort)tile.Content.Select((l, i) => (l & (1 << (TileSize - 1))) > 0 ? 1 << i : 0).Sum();

            return new[] { top, right, bottom, left }
                .Select(b => Math.Min(b, ReverseBits(b)))
                .ToArray();
        }

        private Dictionary<ushort, List<Tile>> GetEdgeMatches()
        {
            var edgeMatch = new Dictionary<ushort, List<Tile>>();

            var edges = _tiles.Values.SelectMany(t => GetEdges(t).Select(e => (e, t)));
            foreach (var (edge, tile) in edges)
            {
                if (edgeMatch.TryGetValue(edge, out var list))
                {
                    list.Add(tile);
                }
                else
                {
                    edgeMatch[edge] = new List<Tile> { tile };
                }
            }

            return edgeMatch;
        }

        private static IEnumerable<Tile> FindCorners(Dictionary<ushort, List<Tile>> edgeMatch) =>
            edgeMatch.Where(kv => kv.Value.Count == 1)
                .Select(kv => (kv.Value.Single(), kv.Key))
                .GroupBy(p => p.Item1.Id)
                .Where(g => g.Count() == 2)
                .Select(g => g.First().Item1);

        public override long Solution1()
        {
            var edgeMatch = GetEdgeMatches();
            var twoSingleEdges = FindCorners(edgeMatch);

            return twoSingleEdges.Select(t => t.Id).Product();
        }

        public override int Solution2()
        {
            return 0;
        }
    }
}
