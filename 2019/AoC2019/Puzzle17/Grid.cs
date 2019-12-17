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

            var width = o.IndexOf('\n');
            var height = o.Count() / width;

            var scaffolding = o.Select((c, i) => (c, i))
                .Where(p => p.c == '#' || p.c == '^')
                .Select(p => new Position(p.i % (width + 1), p.i / (width + 1)));

            _set = scaffolding.ToHashSet();
            _width = _set.Max(p => p.X) + 1;
            _height = _set.Max(p => p.Y) + 1;
            _output = new string(o.ToArray());
        }

        private readonly HashSet<Position> _set;
        private readonly int _width;
        private readonly int _height;
        private readonly string _output;

        public IEnumerable<Position> GetIntersections() =>
            _set.Where(p => p.X > 0 && p.Y > 0 && p.X < _width && p.Y < _height)
                .Where(p => _set.Contains(p.Move(Direction.Up))
                        && _set.Contains(p.Move(Direction.Down))
                        && _set.Contains(p.Move(Direction.Left))
                        && _set.Contains(p.Move(Direction.Right)));

        public string Draw() => _output;
    }
}
