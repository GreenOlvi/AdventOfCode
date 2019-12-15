using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoC2019.Common;

namespace AoC2019.Puzzle15
{
    public class Map
    {
        private Dictionary<Position, Tile> _map = new Dictionary<Position, Tile>();

        public void AddTile(Position position, Tile tile)
        {
            if (!_map.ContainsKey(position))
            {
                _map[position] = tile;
            }
        }
        
        public Tile GetTile(Position position)
        {
            if (_map.TryGetValue(position, out var tile))
            {
                return tile;
            }
            return Tile.Unknown;
        }

        public IEnumerable<Position> FindTiles(Tile tile) =>
            _map.Where(kv => kv.Value == tile).Select(kv => kv.Key);

        public string Draw(Position droid)
        {
            var minX = _map.Keys.Select(p => p.X).Min() - 2;
            var minY = _map.Keys.Select(p => p.Y).Min() - 2;
            var maxX = _map.Keys.Select(p => p.X).Max() + 2;
            var maxY = _map.Keys.Select(p => p.Y).Max() + 2;

            var sb = new StringBuilder();
            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    var p = new Position(x, y);
                    if (p == droid)
                    {
                        sb.Append(DrawTile(Tile.Droid));
                    }
                    else
                    {
                        if (_map.TryGetValue(p, out var tile))
                        {
                            sb.Append(DrawTile(tile));
                        }
                        else
                        {
                            sb.Append(DrawTile(Tile.Unknown));
                        }
                    }
                }
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public void ConsoleDraw(Position droid)
        {
            Console.Clear();

            var minX = _map.Keys.Select(p => p.X).Min() - 2;
            var minY = _map.Keys.Select(p => p.Y).Min() - 2;
            var maxX = _map.Keys.Select(p => p.X).Max() + 2;
            var maxY = _map.Keys.Select(p => p.Y).Max() + 2;

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    var p = new Position(x, y);
                    var tile = p == droid
                        ? Tile.Droid
                        : _map.ContainsKey(p)
                            ? _map[p]
                            : Tile.Unknown;

                    Console.ForegroundColor = Color(tile);
                    Console.Write(DrawTile(tile));
                }
                Console.WriteLine();
            }
        }

        private static ConsoleColor Color(Tile tile) =>
            tile switch
            {
                Tile.Empty => ConsoleColor.DarkGray,
                Tile.Wall => ConsoleColor.White,
                Tile.System => ConsoleColor.Red,
                Tile.Droid => ConsoleColor.Green,
                Tile.Start => ConsoleColor.Green,
                _ => ConsoleColor.White,
            };

        private static string DrawTile(Tile tile) =>
            tile switch
            {
                Tile.Unknown => "  ",
                Tile.Empty => "..",
                Tile.Wall => "##",
                Tile.System => "@@",
                Tile.Droid => "D ",
                Tile.Start => "**",
                _ => throw new ArgumentOutOfRangeException(nameof(tile)),
            };
    }
}
