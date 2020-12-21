using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AOC2020.Day20
{
    public partial class Puzzle : PuzzleBase<long, int>
    {
        public Puzzle(IEnumerable<string> input)
        {
            var tiles = input.SplitGroups();
            _image = new TileSet(tiles.Select(ParseTile).ToArray());
        }

        public const int TileSize = 10;

        private static readonly Regex TileHeaderRegex = new Regex(@"Tile (?<id>\d+):", RegexOptions.Compiled);

        public static Tile ParseTile(params string[] tileLines)
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

            return new Tile(id, content);
        }

        private readonly TileSet _image;

        public static ushort ReverseBits(ushort n)
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

        public override long Solution1() => _image.CornerIds.Product();

        public static void Draw(Image image)
        {
            var defaultColor = Console.ForegroundColor;
            foreach (var line in image.Lines)
            {
                foreach (var c in line)
                {
                    var color = c switch
                    {
                        'O' => ConsoleColor.Green,
                        '#' => ConsoleColor.Gray,
                        '.' => ConsoleColor.DarkGray,
                        _ => defaultColor,
                    };

                    Console.ForegroundColor = color;
                    Console.Write(c);
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = defaultColor;
        }

        private static readonly string[] Nessie = new[] {
            "                  # ",
            "#    ##    ##    ###",
            " #  #  #  #  #  #   ",
        };

        public static int FindNessie(Image image, out Image marked)
        {
            var nessieOffsets = Nessie.Select(s => s.Select((c, i) => (c, i)).Where(p => p.c == '#').Select(p => p.i).ToArray());
            var width = image.Lines.First().Length;
            var combined = string.Join(string.Empty, image.Lines);
            var offsets = nessieOffsets.SelectMany((a, i) => a.Select(o => o + (i * width))).ToArray();
            var patternWidth = 20;

            var sb = new StringBuilder(combined);

            var found = 0;
            for (var y = 0; y < width - 2; y++)
            {
                for (var x = 0; x < width - patternWidth; x++)
                {
                    var index = y * width + x;
                    var matched = offsets.Select(o => combined[o + index]).All(c => c == '#');

                    if (matched)
                    {
                        found++;
                        foreach (var o in offsets)
                        {
                            sb[o + index] = 'O';
                        }
                    }
                }
            }

            if (found > 0)
            {
                var markedLines = sb.ToString()
                    .Select((c, i) => (c, i))
                    .GroupBy(p => p.i / width)
                    .Select(g => new string(g.Select(p => p.c).ToArray()))
                    .ToArray();
                marked = new Image(markedLines);
            }
            else
            {
                marked = image;
            }

            return found;
        }

        public override int Solution2()
        {
            var img = _image.RebuildImage();

            int found;
            Image marked;
            var r = 0;
            while (true)
            {
                found = FindNessie(img, out marked);
                if (found != 0)
                {
                    break;
                }

                img = img.RotateRight();
                r++;

                if (r == 4)
                {
                    img = img.FlipVertical();
                }

                if (r == 8)
                {
                    throw new PuzzleException("Nessie not found");
                }
            }

            Draw(marked);
            return marked.CountChars('#');
        }
    }
}
