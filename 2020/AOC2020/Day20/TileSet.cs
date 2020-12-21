using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Day20
{
    public class TileSet
    {
        public TileSet(IEnumerable<Tile> tiles)
        {
            _tiles = tiles.ToDictionary(t => t.Id);
            _borderMatches = BuildBorderMatches(_tiles.Values);
            _corners = new Lazy<int[]>(() => FindCornerIds().ToArray());
        }

        public int[] CornerIds => _corners.Value;

        private readonly Dictionary<int, Tile> _tiles;
        private readonly Dictionary<ushort, Tile[]> _borderMatches;
        private readonly Lazy<int[]> _corners;

        public Tile GetTile(int id) => _tiles[id];

        public Image RebuildImage()
        {
            var corner = GetTile(CornerIds.First());
            corner = OrientTopLeftCorner(corner);

            var list = new List<List<Tile>>
            {
                new List<Tile> { corner }
            };
            list[0].AddRange(FillLineToRight(corner));

            foreach (var tile in FillLineToBottom(corner))
            {
                var line = new List<Tile> { tile };
                line.AddRange(FillLineToRight(tile));
                list.Add(line);
            }

            return new Image(list);
        }

        private static ushort GetBorderRepresentative(ushort edge) => Math.Min(edge, Puzzle.ReverseBits(edge));

        private static ushort[] GetBorders(Tile tile) =>
            new[] { tile.TopBorder, tile.RightBorder, tile.BottomBorder, tile.LeftBorder }
                .Select(GetBorderRepresentative)
                .ToArray();

        private static Dictionary<ushort, Tile[]> BuildBorderMatches(IEnumerable<Tile> tiles)
        {
            var edgeMatch = new Dictionary<ushort, Tile[]>();

            var edges = tiles.SelectMany(t => GetBorders(t).Select(e => (e, t)));
            foreach (var (edge, tile) in edges)
            {
                edgeMatch[edge] = edgeMatch.TryGetValue(edge, out var list)
                    ? list.Append(tile).ToArray()
                    : (new Tile[] { tile });
            }

            return edgeMatch;
        }

        private IEnumerable<int> FindCornerIds() =>
            _borderMatches.Where(kv => kv.Value.Length == 1)
                .Select(kv => (kv.Value.Single(), kv.Key))
                .GroupBy(p => p.Item1.Id)
                .Where(g => g.Count() == 2)
                .Select(g => g.First().Item1.Id);

        private bool IsEdge(ushort border) =>
            _borderMatches[GetBorderRepresentative(border)].Length == 1;

        private bool TryFindMatching(Tile to, ushort border, out Tile found)
        {
            var matches = _borderMatches[GetBorderRepresentative(border)].Where(t => t.Id != to.Id);
            if (matches.Any())
            {
                found = matches.First();
                return true;
            }

            found = default;
            return false;
        }

        private IEnumerable<Tile> FillLineToRight(Tile starting)
        {
            var matchTo = starting;
            while (true)
            {
                if (!TryFindMatching(matchTo, matchTo.RightBorder, out var matching))
                {
                    break;
                }
                var orientedMatching = TransformToMatchLeftWith(matching, matchTo.RightBorder);
                yield return orientedMatching;

                matchTo = orientedMatching;
            }
        }

        private IEnumerable<Tile> FillLineToBottom(Tile starting)
        {
            var matchTo = starting;
            while (true)
            {
                if (!TryFindMatching(matchTo, matchTo.BottomBorder, out var matching))
                {
                    break;
                }
                var orientedMatching = TransformToMatchTopWith(matching, matchTo.BottomBorder);
                yield return orientedMatching;

                matchTo = orientedMatching;
            }
        }

        private Tile OrientTopLeftCorner(Tile corner)
        {
            var orientation = (IsEdge(corner.TopBorder) ? 1 : 0)
                + (IsEdge(corner.RightBorder) ? 2 : 0)
                + (IsEdge(corner.BottomBorder) ? 4 : 0)
                + (IsEdge(corner.LeftBorder) ? 8 : 0);

            var oriented = orientation switch
            {
                3 => corner.RotateLeft(),
                6 => corner.Rotate180(),
                12 => corner.RotateRight(),
                9 => corner,
                _ => throw new InvalidOperationException("Tile must be a corner"),
            };

            return oriented;
        }

        private static Tile TransformToMatchLeftWith(Tile tile, ushort border)
        {
            var b = GetBorderRepresentative(border);
            Tile rotated;
            if (GetBorderRepresentative(tile.TopBorder) == b)
            {
                rotated = tile.RotateLeft();
            }
            else if (GetBorderRepresentative(tile.RightBorder) == b)
            {
                rotated = tile.Rotate180();
            }
            else if (GetBorderRepresentative(tile.BottomBorder) == b)
            {
                rotated = tile.RotateRight();
            }
            else if (GetBorderRepresentative(tile.LeftBorder) == b)
            {
                rotated = tile;
            }
            else
            {
                throw new InvalidOperationException("Tile is not matching");
            }

            return rotated.LeftBorder == border ? rotated.FlipVertical() : rotated;
        }

        private static Tile TransformToMatchTopWith(Tile tile, ushort border)
        {
            var b = GetBorderRepresentative(border);
            Tile rotated;
            if (GetBorderRepresentative(tile.TopBorder) == b)
            {
                rotated = tile;
            }
            else if (GetBorderRepresentative(tile.RightBorder) == b)
            {
                rotated = tile.RotateLeft();
            }
            else if (GetBorderRepresentative(tile.BottomBorder) == b)
            {
                rotated = tile.Rotate180();
            }
            else if (GetBorderRepresentative(tile.LeftBorder) == b)
            {
                rotated = tile.RotateRight();
            }
            else
            {
                throw new InvalidOperationException("Tile is not matching");
            }

            return rotated.TopBorder == border ? rotated.FlipHorizontal() : rotated;
        }
    }
}
