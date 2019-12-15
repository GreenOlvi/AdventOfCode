using AoC2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC2019.Puzzle13
{
    public class Screen
    {
        public Screen(IEnumerable<Tile> initial)
        {
            var tiles = initial.ToArray();
            _width = tiles.Max(t => t.X) + 1;
            _height = tiles.Max(t => t.Y) + 1;
            _tiles = new int[_width * _height];
            Score = 0;

            Update(tiles);
        }

        public Position Paddle { get; private set; }
        public Position Ball { get; private set; }
        public long Score { get; private set; }
        private readonly int[] _tiles;
        private readonly int _width;
        private readonly int _height;

        public void Update(IEnumerable<Tile> tiles)
        {
            foreach (var tile in tiles)
            {
                if (tile.X == -1 && tile.Y == 0)
                {
                    if (tile.Type > 0)
                    {
                        Score = tile.Type;
                    }
                }
                else
                {
                    _tiles[tile.X + tile.Y * _width] = tile.Type;

                    if (tile.Type == 3)
                    {
                        Paddle = new Position(tile.X, tile.Y);
                    }

                    if (tile.Type == 4)
                    {
                        Ball = new Position(tile.X, tile.Y);
                    }
                }
            }
        }

        public string Draw()
        {
            var sb = new StringBuilder((_width + 1) * _height);
            sb.Append("Score: ");
            sb.Append(Score);
            sb.Append(Environment.NewLine);
            for (var y = 0; y < _height; y++)
            {
                for (var x = 0; x < _width; x++)
                {
                    sb.Append(DrawTile(_tiles[x + y * _width]));
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        private static string DrawTile(int tile) =>
            tile switch
            {
                0 => "  ",
                1 => "##",
                2 => "[]",
                3 => "--",
                4 => "**",
                _ => throw new ArgumentException("Invalid type value"),
            };

    }
}
