using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC2020.Day20
{
    public class Image
    {
        public Image(IEnumerable<IEnumerable<Tile>> tiles)
        {
            _content = StripImageData(tiles).ToArray();
            _width = _content[0].Length;
            _height = _content.Length;
        }

        public Image(IEnumerable<string> content)
        {
            _content = content.ToArray();
            _width = _content[0].Length;
            _height = _content.Length;
        }

        private readonly string[] _content;
        private readonly int _width;
        private readonly int _height;

        public IReadOnlyCollection<string> Lines => Array.AsReadOnly(_content);

        public int CountChars(char c) => _content.Select(line => line.Count(s => s == c)).Sum();

        private static IEnumerable<string> StripImageData(IEnumerable<IEnumerable<Tile>> tiles)
        {
            var sb = new StringBuilder();
            foreach (var line in tiles)
            {
                var imageLine = line.ToArray();
                for (var l = 0; l < Puzzle.TileSize - 2; l++)
                {
                    sb.Clear();
                    foreach (var i in imageLine.Select(i => i.RemoveBorder()))
                    {
                        for (var x = Puzzle.TileSize - 3; x >= 0; x--)
                        {
                            if ((i[l] & (1 << x)) == 0)
                            {
                                sb.Append('.');
                            }
                            else
                            {
                                sb.Append('#');
                            }
                        }
                    }
                    yield return sb.ToString();
                }
            }
        }

        public Image FlipVertical() => new Image(_content.Reverse());

        public Image RotateRight()
        {
            var newC = Enumerable.Range(0, _width)
                .Select(i => new char[_height])
                .ToArray();

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    newC[x][y] = _content[_height - y - 1][x];
                }
            }

            return new Image(newC.Select(l => new string(l)));
        }

        public Image RotateLeft()
        {
            var newC = Enumerable.Range(0, _width)
                .Select(i => new char[_height])
                .ToArray();

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    newC[_width - x - 1][y] = _content[y][x];
                }
            }

            return new Image(newC.Select(l => new string(l)));
        }
    }
}
