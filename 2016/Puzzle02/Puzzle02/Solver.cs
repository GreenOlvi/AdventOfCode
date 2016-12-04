using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzle02
{
    public class Solver
    {
        public Solver(params string[] input)
        {
            Input = input;
        }

        private string[] Input { get; }

        public string Solve1()
        {
            var keypad = new Keypad(
                new[] {"1", "2", "3"},
                new []{"4", "5", "6"},
                new []{"7", "8", "9"});

            return GetCode(keypad);
        }

        public string Solve2()
        {
            var keypad = new Keypad(
                new[] {null, null, "1", null, null},
                new[] {null, "2", "3", "4", null},
                new[] {"5", "6", "7", "8", "9"},
                new[] {null, "A", "B", "C", null},
                new[] {null, null, "D", null, null});

            return GetCode(keypad);
        }

        private string GetCode(Keypad keypad)
        {
            var code = new StringBuilder();
            var position = keypad.GetPosition("5");

            foreach (var line in Input)
            {
                position = position.Move(ParseInputLine(line));
                code.Append(position.GetNumber());
            }

            return code.ToString();
        }

        private IEnumerable<Direction> ParseInputLine(string line)
        {
            return line.Trim()
                .ToCharArray()
                .Select(c => (Direction) Enum.Parse(typeof (Direction), c.ToString()));
        }

        public enum Direction { U, D, L, R }

        public class Position
        {
            public Position(Keypad keypad, int x, int y)
            {
                X = x;
                Y = y;
                Keypad = keypad;
            }

            private int X { get; }
            private int Y { get; }
            private Keypad Keypad { get; }

            public string GetNumber()
            {
                return Keypad.GetNumber(X, Y);
            }

            public Position Move(Direction direction)
            {
                switch (direction)
                {
                    case Direction.U:
                        if (Keypad.CanMoveTo(X, Y - 1))
                        {
                            return new Position(Keypad, X, Y - 1);
                        }
                        break;
                    case Direction.D:
                        if (Keypad.CanMoveTo(X, Y + 1))
                        {
                            return new Position(Keypad, X, Y + 1);
                        }
                        break;
                    case Direction.L:
                        if (Keypad.CanMoveTo(X - 1, Y))
                        {
                            return new Position(Keypad, X - 1, Y);
                        }
                        break;
                    case Direction.R:
                        if (Keypad.CanMoveTo(X + 1, Y))
                        {
                            return new Position(Keypad, X + 1, Y);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("direction");
                }
                return this;
            }

            public Position Move(IEnumerable<Direction> directions)
            {
                return directions.Aggregate(this, (position, direction) => position.Move(direction));
            }
        }

        public class Keypad
        {
            public Keypad(params string[][] layout)
            {
                Layout = layout;
                Height = layout.Length;
                Width = layout.Max(row => row.Length);
            }

            private string[][] Layout { get; }
            private int Width { get; }
            private int Height { get; }

            public bool CanMoveTo(int x, int y)
            {
                return x >= 0 && y >= 0 && x < Width && y < Height && !String.IsNullOrEmpty(Layout[y][x]);
            }

            public string GetNumber(int x, int y)
            {
                return !CanMoveTo(x, y) ? null : Layout[y][x];
            }

            public Position GetPosition(string number)
            {
                var y = 0;
                foreach (var row in Layout)
                {
                    var x = 0;
                    foreach (var digit in row)
                    {
                        if (digit != null && digit == number)
                        {
                            return new Position(this, x, y);
                        }
                        x++;
                    }
                    y++;
                }

                return null;
            }
        }
    }
}
