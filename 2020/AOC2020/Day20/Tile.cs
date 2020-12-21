using System;
using System.Linq;

namespace AOC2020.Day20
{
    public struct Tile
    {
        private static readonly ushort BorderMask = ((1 << Puzzle.TileSize - 2) - 1) << 1;

        public Tile(int id, ushort[] content)
        {
            Id = id;
            Content = content;
            _text = string.Join("\n", Content.Select(Unpack));
            TopBorder = Content[0];
            BottomBorder = Puzzle.ReverseBits(Content.Last());
            RightBorder = (ushort)Content.Select((l, i) => (l & 1) > 0 ? 1 << (Puzzle.TileSize - 1 - i) : 0).Sum();
            LeftBorder = (ushort)Content.Select((l, i) => (l & (1 << (Puzzle.TileSize - 1))) > 0 ? 1 << i : 0).Sum();
        }

        public int Id { get; }
        public ushort[] Content { get; }

        public ushort TopBorder { get; }
        public ushort RightBorder { get; }
        public ushort BottomBorder { get; }
        public ushort LeftBorder { get; }

        private readonly string _text;

        public override string ToString() => $"Tile {Id}:\n" + _text;

        private static string Unpack(ushort n) =>
            new string(Enumerable.Range(0, Puzzle.TileSize)
                .Select(i => (n & (1 << (Puzzle.TileSize - i - 1))) > 0 ? '#' : '.')
                .ToArray());

        public Tile FlipHorizontal() =>
            new Tile(Id, Content.Select(l => Puzzle.ReverseBits(l)).ToArray());

        public Tile FlipVertical() =>
            new Tile(Id, Content.Reverse().ToArray());

        private static ushort GetBit(ushort val, int bit) =>
            (ushort)((val >> (bit)) & 1);

        public Tile RotateRight()
        {
            var c = new ushort[Puzzle.TileSize];
            for (var y = 0; y < Puzzle.TileSize; y++)
            {
                for (var x = 0; x < Puzzle.TileSize; x++)
                {
                    c[Puzzle.TileSize - x - 1] |= (ushort)(GetBit(Content[y], x) << y);
                }
            }
            return new Tile(Id, c);
        }

        public Tile RotateLeft()
        {
            var c = new ushort[Puzzle.TileSize];
            for (var y = 0; y < Puzzle.TileSize; y++)
            {
                for (var x = 0; x < Puzzle.TileSize; x++)
                {
                    c[x] |= (ushort)(GetBit(Content[y], x) << (Puzzle.TileSize - y - 1));
                }
            }
            return new Tile(Id, c);
        }

        public Tile Rotate180() => 
            new Tile(Id, Content.Select(l => Puzzle.ReverseBits(l)).Reverse().ToArray());

        public ushort[] RemoveBorder()
        {
            return Content.Skip(1)
                .Take(Puzzle.TileSize - 2)
                .Select(l => (ushort)((l & BorderMask) >> 1))
                .ToArray();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(Tile))
            {
                return false;
            }

            var other = (Tile)obj;
            return Id == other.Id && Content.SequenceEqual(other.Content);
        }

        public override int GetHashCode() =>
            Content.Aggregate(Id.GetHashCode(), (a, b) => HashCode.Combine(a, b));

        public static bool operator ==(Tile left, Tile right) => left.Equals(right);

        public static bool operator !=(Tile left, Tile right) => !(left == right);
    }
}
