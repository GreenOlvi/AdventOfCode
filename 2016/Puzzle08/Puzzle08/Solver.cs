using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Puzzle08
{
    public class Solver
    {
        public Solver(params string[] input)
        {
            var commands = input.Select(line => ParseLine(line));
            Display = new Display(50, 6);
            foreach (var command in commands)
            {
                command.Run(Display);
            }
        }

        public Display Display { get; }

        public int Solve1()
        {
            return Display.Lights.Count(l => l);
        }

        public string Solve2()
        {
            return Display.ToString();
        }

        private static Regex RectRegex = new Regex(@"rect (?<width>\d+)x(?<height>\d+)", RegexOptions.Compiled);
        private static Regex RotateColumnRegex = new Regex(@"rotate column x=(?<index>\d+) by (?<count>\d+)", RegexOptions.Compiled);
        private static Regex RotateRowRegex = new Regex(@"rotate row y=(?<index>\d+) by (?<count>\d+)", RegexOptions.Compiled);
        public ICommand ParseLine(string line)
        {
            if (RectRegex.IsMatch(line))
            {
                var match = RectRegex.Match(line);
                var width = int.Parse(match.Groups["width"].Value);
                var height = int.Parse(match.Groups["height"].Value);
                return new RectCommand(width, height);
            }

            if (RotateColumnRegex.IsMatch(line))
            {
                var match = RotateColumnRegex.Match(line);
                var index = int.Parse(match.Groups["index"].Value);
                var count = int.Parse(match.Groups["count"].Value);
                return new RotateColumnCommand(index, count);
            }

            if (RotateRowRegex.IsMatch(line))
            {
                var match = RotateRowRegex.Match(line);
                var index = int.Parse(match.Groups["index"].Value);
                var count = int.Parse(match.Groups["count"].Value);
                return new RotateRowCommand(index, count);
            }

            throw new ArgumentOutOfRangeException("Unknown command '" + line + "'");
        }

        public interface ICommand
        {
            void Run(Display display);
        }

        public class RectCommand : ICommand
        {
            public RectCommand(int width, int height)
            {
                Width = width;
                Height = height;
            }

            public int Width { get; }
            public int Height { get; }

            public void Run(Display display)
            {
                display.Rect(Width, Height);
            }
        }

        public class RotateColumnCommand : ICommand
        {
            public RotateColumnCommand(int column, int count)
            {
                Column = column;
                Count = count;
            }

            private int Column { get; }
            private int Count { get; }

            public void Run(Display display)
            {
                display.RotateColumn(Column, Count);
            }
        }

        public class RotateRowCommand : ICommand
        {
            public RotateRowCommand(int row, int count)
            {
                Row = row;
                Count = count;
            }

            private int Row { get; }
            private int Count { get; }

            public void Run(Display display)
            {
                display.RotateRow(Row, Count);
            }
        }
    }

    public class Display
    {
        public Display(int width, int height)
        {
            Width = width;
            Height = height;
            _lights = new bool[width, height];
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        private readonly bool[,] _lights;
        public IEnumerable<bool> Lights {
            get
            {
                foreach (var light in _lights)
                {
                    yield return light;
                }
            }
        }

        public bool GetLight(int x, int y)
        {
            return _lights[x, y];
        }

        public void SetLight(int x, int y, bool state)
        {
            _lights[x, y] = state;
        }

        public void Rect(int width, int height)
        {
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    SetLight(x, y, true);
                }
            }
        }

        public void RotateColumn(int column, int count)
        {
            var c = count%Height;
            var newCol = new bool[Height];
            for (var y = 0; y < Height; y++)
            {
                newCol[(y + c)%Height] = GetLight(column, y);
            }
            for (var y = 0; y < Height; y++)
            {
                SetLight(column, y, newCol[y]);
            }
        }

        public void RotateRow(int row, int count)
        {
            var c = count%Width;
            var newRow = new bool[Width];
            for (var x = 0; x < Width; x++)
            {
                newRow[(x + c)%Width] = GetLight(x, row);
            }
            for (var x = 0; x < Width; x++)
            {
                SetLight(x, row, newRow[x]);
            }
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            for (var y = 0; y < Height; y++)
            {
                var line = Enumerable.Range(0, Width)
                    .Select(x => _lights[x, y] ? "#" : ".")
                    .Aggregate(String.Empty, (s1, s2) => s1 + s2);
                s.AppendLine(line);
            }

            return s.ToString();
        }
    }
}
