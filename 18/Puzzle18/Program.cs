using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Puzzle18
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var filename in args)
            {
                var first = Board.FromFile(filename).GetIterator().Take(100).Last(); 
                Console.WriteLine("Lit after 100: {0}", first.CountLit());

                var second = Board.FromFile(filename, true).GetIterator().Take(100).Last(); 
                Console.WriteLine("Lit after 100 (with corners): {0}", second.CountLit());
            }

            Console.ReadLine();
        }


        private class Board
        {
            private bool[][] Lights { get; }
            public int Width { get; }
            public int Height { get; }
            private bool CornersOn { get; }

            public static Board FromFile(string filename, bool cornersOn = false)
            {
                var height = 0;
                var lightsList = new List<bool[]>();

                using (var reader = new StreamReader(filename))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine().ToCharArray().Select(x => x == '#').ToArray();
                        lightsList.Add(line);
                        height++;
                    }
                }

                var width = lightsList.Count;

                return new Board(lightsList.ToArray(), width, height, cornersOn);
            }

            public Board(bool[][] lights, int width, int height, bool cornersOn = false)
            {
                Lights = lights;
                Width = width;
                Height = height;
                CornersOn = cornersOn;

                if (cornersOn)
                {
                    Lights[0][0] = true;
                    Lights[0][Width - 1] = true;
                    Lights[Height - 1][Width - 1] = true;
                    Lights[Height - 1][0] = true;
                }
            }

            public bool GetLight(int x, int y)
            {
                return Lights[y][x];
            }

            public int CountLit()
            {
                return Lights.Sum(y => y.Count(x => x));
            }

            public void Draw()
            {
                for (var y = 0; y < Height; y++)
                {
                    for (var x = 0; x < Width; x++)
                    {
                        Console.Write(GetLight(x, y) ? "#" : ".");
                    }
                    Console.WriteLine();
                }
            }

            public Board GetNext()
            {
                var next = Enumerable.Range(0, Height).Select(y => Enumerable.Range(0, Width)
                    .Select(x => NextLight(GetLight(x, y), GetNeighbours(x, y).Count(l => l)))
                    .ToArray()).ToArray();

                return new Board(next, Width, Height, CornersOn);
            }

            public IEnumerable<Board> GetIterator()
            {
                var current = this;
                while (true)
                {
                    current = current.GetNext();
                    yield return current;
                }
            } 

            private static bool NextLight(bool isLit, int neighboursLit)
            {
                if (isLit)
                {
                    return neighboursLit == 2 || neighboursLit == 3;
                }

                return neighboursLit == 3;
            }

            private IEnumerable<bool> GetNeighbours(int x, int y)
            {
                var n = new List<bool>();

                if (InBoard(x - 1, y - 1)) n.Add(GetLight(x - 1, y - 1));
                if (InBoard(x, y - 1)) n.Add(GetLight(x, y - 1));
                if (InBoard(x + 1, y - 1)) n.Add(GetLight(x + 1, y - 1));
                if (InBoard(x + 1, y)) n.Add(GetLight(x + 1, y));
                if (InBoard(x + 1, y + 1)) n.Add(GetLight(x + 1, y + 1));
                if (InBoard(x, y + 1)) n.Add(GetLight(x, y + 1));
                if (InBoard(x - 1, y + 1)) n.Add(GetLight(x - 1, y + 1));
                if (InBoard(x - 1, y)) n.Add(GetLight(x - 1, y));

                return n.ToArray();
            }

            private bool InBoard(int x, int y)
            {
                return x >= 0 && x < Width && y >= 0 && y < Height;
            }
        }
    }
}
