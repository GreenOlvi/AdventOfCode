using AoC2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC2019.Puzzle11
{
    public class Grid
    {
        private readonly Dictionary<Position, GridColor> _grid = new Dictionary<Position, GridColor>();

        public void SetColor(Position position, GridColor color)
        {
            _grid[position] = color;
        }

        public GridColor GetColor(Position position)
        {
            if (_grid.TryGetValue(position, out var color))
            {
                return color;
            }
            return GridColor.Black;
        }

        public string Draw(Robot? robot)
        {
            if (robot == null)
            {
                throw new ArgumentNullException(nameof(robot));
            }

            var allPos = _grid.Keys.Concat(new[] { robot.Position }).ToArray();
            var minX = allPos.Min(p => p.X) - 2;
            var minY = allPos.Min(p => p.Y) - 2;
            var maxX = allPos.Max(p => p.X) + 2;
            var maxY = allPos.Max(p => p.Y) + 2;

            var sb = new StringBuilder();
            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    var pos = new Position(x, y);
                    if (robot.Position == pos)
                    {
                        sb.Append(DrawDirection(robot.Direction));
                    }
                    else if (_grid.TryGetValue(pos, out var value))
                    {
                        sb.Append(value == GridColor.White ? '#' : '.');
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                }
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        private static char DrawDirection(Direction direction) =>
            direction switch
            {
                Direction.Up => '^',
                Direction.Down => 'v',
                Direction.Left => '<',
                Direction.Right => '>',
                _ => throw new ArgumentException("Invalid direction"),
            };

    }
}
