using System;
using System.Collections.Generic;
using System.Linq;
using AoC2019.Common;

namespace AoC2019.Puzzle17
{
    public class Grid
    {
        public Grid(IEnumerable<char> display)
        {
            var o = display.ToList();

            var width = o.IndexOf('\n') + 1;
            var scaffolding = o.Select((c, i) => (c, i))
                .Where(p => p.c == '#' || p.c == '^')
                .Select(p => new Position(p.i % width, p.i / width));

            _set = scaffolding.ToHashSet();
            _width = _set.Max(p => p.X) + 1;
            _height = _set.Max(p => p.Y) + 1;
            _output = new string(o.ToArray());

            var robot = o.Select((c, i) => (c, i))
                .First(p => p.c == '^' || p.c == 'v' || p.c == '>' || p.c == '<');
            RobotPosition = new Position(robot.i % width, robot.i / width);
            RobotDirection = robot.c switch
            {
                '^' => Direction.Up,
                'v' => Direction.Down,
                '>' => Direction.Right,
                '<' => Direction.Down,
                _ => throw new ArgumentOutOfRangeException(nameof(robot.c)),
            };
        }

        private readonly HashSet<Position> _set;
        private readonly int _width;
        private readonly int _height;
        private readonly string _output;

        public Position RobotPosition { get;  }
        public Direction RobotDirection { get;  }

        public bool IsScaffold(Position position) => _set.Contains(position) ? true : false;

        public IEnumerable<Position> GetIntersections() =>
            _set.Where(p => p.X > 0 && p.Y > 0 && p.X < _width && p.Y < _height)
                .Where(p => _set.Contains(p.Move(Direction.Up))
                        && _set.Contains(p.Move(Direction.Down))
                        && _set.Contains(p.Move(Direction.Left))
                        && _set.Contains(p.Move(Direction.Right)));

        public string Draw() => _output;
    }
}
