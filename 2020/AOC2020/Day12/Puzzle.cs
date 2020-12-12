using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AOC2020.Common;

namespace AOC2020.Day12
{
    public class Puzzle : PuzzleBase<uint, uint>
    {
        public Puzzle(IEnumerable<string> input)
        {
            _input = input.Select(ParseLine).ToArray();
        }

        private readonly Instruction[] _input;

        private static readonly Regex InstructionRegex = new Regex(@"^(?<dir>[NSEWLRF])(?<val>\d+)$", RegexOptions.Compiled);

        public static Instruction ParseLine(string line)
        {
            var m = InstructionRegex.Match(line);
            if (!m.Success)
            {
                throw new PuzzleException($"Invalid input: {line}");
            }

            var dir = m.Groups["dir"].Value;
            var val = uint.Parse(m.Groups["val"].Value);
            return dir switch
            {
                "N" => new MoveAbsolute('N', val),
                "S" => new MoveAbsolute('S', val),
                "E" => new MoveAbsolute('E', val),
                "W" => new MoveAbsolute('W', val),
                "L" => new Rotation('L', val),
                "R" => new Rotation('R', val),
                "F" => new MoveRelative(val),
                _ => throw new InvalidOperationException(),
            };
        }

        private record Ship(Point Position, AbsoluteDirection Direction);

        private static Ship Run(Ship ship, Instruction instruction) =>
            instruction switch
            {
                MoveAbsolute m => ship with { Position = m.Move(ship.Position) },
                MoveRelative m => ship with { Position = m.Move(ship.Position, ship.Direction) },
                Rotation r => ship with { Direction = r.Rotate(ship.Direction) },
                _ => throw new InvalidOperationException(),
            };

        private record ShipWithWaypoint(Point Position, Point WaypointRelative);

        private static ShipWithWaypoint Run(ShipWithWaypoint ship, Instruction instruction) =>
            instruction switch
            {
                MoveAbsolute m => ship with { WaypointRelative = m.Move(ship.WaypointRelative) },
                MoveRelative m => ship with { Position = ship.Position + ship.WaypointRelative * (int)m.Value },
                Rotation r => ship with { WaypointRelative = r.RotateAroundOrigin(ship.WaypointRelative) },
                _ => throw new InvalidOperationException(),
            };

        private static uint ManhattanDistance(Point position) =>
            (uint)(Math.Abs(position.X) + Math.Abs(position.Y));

        public override uint Solution1()
        {
            var ship = new Ship(new Point(0, 0), AbsoluteDirection.East);
            var end = _input.Aggregate(ship, Run);
            return ManhattanDistance(end.Position);
        }

        public override uint Solution2()
        {
            var ship = new ShipWithWaypoint(new Point(0, 0), new Point(10, 1));
            var end = _input.Aggregate(ship, Run);
            return ManhattanDistance(end.Position);
        }
    }

    public record Instruction
    {
        public char Letter { get; init; }
        public uint Value { get; init; }

        protected Instruction(char letter, uint value) => (Letter, Value) = (letter, value);
        public void Deconstruct(out char letter, out uint value) => (letter, value) = (Letter, Value);

        public override string ToString() => $"{Letter}{Value}";
    }

    public record MoveAbsolute : Instruction
    {
        public MoveAbsolute(char letter, uint value) : base(letter, value)
        {
            Vector = letter switch
            {
                'N' => new Point(0, 1) * (int)value,
                'S' => new Point(0, -1) * (int)value,
                'E' => new Point(1, 0) * (int)value,
                'W' => new Point(-1, 0) * (int)value,
                _ => throw new InvalidOperationException("Invalid absolute direction"),
            };
        }

        public MoveAbsolute(AbsoluteDirection direction, uint value) : this(direction.ToString()[0], value)
        {
        }

        public Point Vector { get; }

        public Point Move(Point from) => from + Vector;
    }

    public record Rotation : Instruction
    {
        public Rotation(char letter, uint value) : base(letter, value)
        {
            Direction = letter switch
            {
                'L' => RotationDirection.Left,
                'R' => RotationDirection.Right,
                _ => throw new InvalidOperationException("Invalid rotation direction"),
            };
            Turns = value / 90;
        }

        public RotationDirection Direction { get; }
        public uint Turns { get; }

        public AbsoluteDirection Rotate(AbsoluteDirection startingDirection)
        {
            var d = startingDirection switch
            {
                AbsoluteDirection.North => 0,
                AbsoluteDirection.East => 1,
                AbsoluteDirection.South => 2,
                AbsoluteDirection.West => 3,
                _ => throw new InvalidOperationException(),
            };

            var dt = Direction switch
            {
                RotationDirection.Left => d - Turns,
                RotationDirection.Right => d + Turns,
                _ => throw new InvalidOperationException(),
            };

            while (dt < 0)
            {
                dt += 4;
            }
            dt %= 4;

            return dt switch
            {
                0 => AbsoluteDirection.North,
                1 => AbsoluteDirection.East,
                2 => AbsoluteDirection.South,
                3 => AbsoluteDirection.West,
                _ => throw new InvalidOperationException(),
            };
        }

        public Point RotateAroundOrigin(Point waypointRelative)
        {
            Func<Point, Point> rot = Direction switch
            {
                RotationDirection.Left => RotateLeft,
                RotationDirection.Right => RotateRight,
                _ => throw new InvalidOperationException(),
            };
            return Enumerable.Range(0, (int)Turns)
                .Aggregate(waypointRelative, (p, i) => rot(p));
        }

        private static Point RotateRight(Point p) => new Point(p.Y, -p.X);
        private static Point RotateLeft(Point p) => new Point(-p.Y, p.X);
    }

    public record MoveRelative : Instruction
    {
        public MoveRelative(uint value) : base('F', value)
        {
        }

        public Point Move(Point from, AbsoluteDirection direction) => new MoveAbsolute(direction, Value).Move(from);
    }

    public enum AbsoluteDirection { North, South, East, West }
    public enum RotationDirection { Left, Right }

}
