using System.Text;
using AOC2021.Common;

namespace AOC2021.Day25
{
    public record Map
    {
        public Map(int width, int height, IEnumerable<Cucumber> cucumbers)
        {
            _width = width;
            _height = height;
            _map = cucumbers as Cucumber[] ?? cucumbers.ToArray();
        }

        private readonly int _width;
        private readonly int _height;
        private readonly Cucumber[] _map;

        private int GetIndex(Point p) => GetIndex((int)p.X, (int)p.Y);
        private int GetIndex(int x, int y) => (y % _height) * _width + (x % _width);

        private Point GetPoint(int i) => new(i % _width, i / _width);

        public Cucumber GetCucumber(Point p) => _map[GetIndex(p)];
        public Cucumber GetCucumber(int x, int y) => _map[GetIndex(x, y)];

        public (bool, Map) Step()
        {
            var newMap = new Cucumber[_map.Length];
            var changed = false;

            var east = _map.Select((c, i) => (c, i))
                .Where(p => p.c == Cucumber.East)
                .Select(p => p.i);
            foreach (var i in east)
            {
                var dest = GetIndex(GetPoint(i).Move(Direction.Right));
                if (_map[dest] == Cucumber.Empty)
                {
                    newMap[dest] = Cucumber.East;
                    changed = true;
                }
                else
                {
                    newMap[i] = Cucumber.East;
                }
            }

            var south = _map.Select((c, i) => (c, i))
                .Where(p => p.c == Cucumber.South)
                .Select(p => p.i);
            foreach (var i in south)
            {
                var dest = GetIndex(GetPoint(i).Move(Direction.Down));
                if (_map[dest] is Cucumber.Empty or Cucumber.East && newMap[dest] == Cucumber.Empty)
                {
                    newMap[dest] = Cucumber.South;
                    changed = true;
                }
                else
                {
                    newMap[i] = Cucumber.South;
                }
            }

            return (changed, new Map(_width, _height, newMap));
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var y = 0; y < _height; y++)
            {
                sb.AppendLine();
                for (var x = 0; x < _width; x++)
                {
                    var c = GetCucumber(x, y);
                    sb.Append(c switch
                    {
                        Cucumber.Empty => '.',
                        Cucumber.East => '>',
                        Cucumber.South => 'v',
                        _ => throw new ArgumentOutOfRangeException(),
                    });
                }
            }
            return sb.ToString();
        }

        public static Map FromInput(IEnumerable<string> lines)
        {
            var input = lines.ToArray();
            var width = input[0].Length;
            var height = input.Length;
            return new Map(width, height, input.SelectMany(ParseCucumbers));
        }

        private static IEnumerable<Cucumber> ParseCucumbers(IEnumerable<char> chars) =>
            chars.Select(c => c switch
            {
                '.' => Cucumber.Empty,
                '>' => Cucumber.East,
                'v' => Cucumber.South,
                _ => throw new InvalidDataException(),
            });
    }
}
