using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzle01
{
    public class Solver
    {
        public Solver(string input)
        {
            Input = input;
        }

        public string Input { get; private set; }

        public int Solve1()
        {
            var movements = ParseInput(Input);

            var direction = 0;
            var position = new Position(0, 0);
            foreach (var move in movements)
            {
                direction = Rotate(direction, move.Direction);
                position = position.Move(direction, move.Distance);
            }

            return DistanceFromStart(position);
        }

        public int Solve2()
        {
            var movements = ParseInput(Input);

            var visited = new HashSet<string>();
            var direction = 0;
            var position = new Position(0, 0);
            visited.Add(position.ToString());

            foreach (var move in movements)
            {
                direction = Rotate(direction, move.Direction);
                for (var i = 0; i < move.Distance; i++)
                {
                    position = position.Move(direction, 1);
                    if (visited.Contains(position.ToString()))
                    {
                        return DistanceFromStart(position);
                    }
                    visited.Add(position.ToString());
                }
            }

            throw new Exception("No position was visited twice.");
        }

        private static readonly Regex MovementRegex = new Regex(@"^(?<direction>L|R)(?<distance>\d+)$", RegexOptions.Compiled);
        private IEnumerable<Movement> ParseInput(string input)
        {
            var list = input.Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var element in list)
            {
                var match = MovementRegex.Match(element);
                var direction = match.Groups["direction"].Value == "L" ? Direction.L : Direction.R;
                var distance = int.Parse(match.Groups["distance"].Value);
                yield return new Movement(direction, distance);
            }
        }

        private int DistanceFromStart(Position position)
        {
            return Math.Abs(position.X) + Math.Abs(position.Y);
        }

        private int Rotate(int current, Direction direction)
        {
            var newDir = current;

            if (direction == Direction.R) newDir += 1;
            if (direction == Direction.L) newDir -= 1;

            if (newDir < 0)
                newDir += 4;

            if (newDir >= 4)
                newDir -= 4;

            return newDir;
        }

        private struct Position
        {
            public Position(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; }
            public int Y { get; }

            public Position Move(int direction, int distance)
            {
                switch (direction)
                {
                    case 0:
                        return new Position(X, Y + distance);
                    case 1:
                        return new Position(X + distance, Y);
                    case 2:
                        return new Position(X, Y - distance);
                    case 3:
                        return new Position(X - distance, Y);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction));
                }
            }

            public override string ToString()
            {
                return $"({X},{Y})";
            }
        }

        private struct Movement
        {
            public Direction Direction { get; }
            public int Distance { get; }

            public Movement(Direction direction, int distance)
            {
                Direction = direction;
                Distance = distance;
            }
        }

        enum Direction { L, R }
    }
}