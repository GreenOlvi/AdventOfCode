using AoC2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Puzzle20
{
    public class Map
    {
        public Map(IEnumerable<char> map, int width)
        {
            _map = map.ToArray();
            _width = width;
            _height = _map.Length / width;
            Labels = ExtractLabels(_map, _width).ToList();
            Links = GenerateLinks(Labels);
            Start = Labels.First(l => l.Label == "AA").Position;
            End = Labels.First(l => l.Label == "ZZ").Position;
        }

        private readonly int _width;
        private readonly int _height;
        private readonly char[] _map;

        public IList<(string Label, Position Position, Direction Direction)> Labels { get; }
        public IDictionary<(Position Position, Direction Direction), Position> Links { get; }
        public Position Start { get; }
        public Position End { get; }

        private static IEnumerable<(string Label, Position Position, Direction Direction)> ExtractLabels(char[] map, int width)
        {
            for (var i = 0; i < map.Length; i++)
            {
                var x = i % width;
                var y = i / width;
                var c = map[i];

                if (!IsLetter(c))
                {
                    continue;
                }

                if (x > 0 && IsLetter(map[i - 1]))
                {
                    var label = string.Join(string.Empty, map[i - 1], c);
                    if (x > 1 && map[i - 2] == '.')
                    {
                        yield return (label, new Position(x - 2, y), Direction.Right);
                    }
                    else
                    {
                        yield return (label, new Position(x + 1, y), Direction.Left);
                    }
                }

                if (y > 0 && IsLetter(map[i - width]))
                {
                    var str = string.Join(string.Empty, map[i - width], c);
                    if (y > 1 && map[i - 2 * width] == '.')
                    {
                        yield return (str, new Position(x, y - 2), Direction.Down);
                    }
                    else
                    {
                        yield return (str, new Position(x, y + 1), Direction.Up);
                    }
                }
            }
        }

        private IDictionary<(Position Position, Direction Direction), Position> GenerateLinks(IEnumerable<(string Label, Position Position, Direction Direction)> labels)
        {
            var dict = new Dictionary<(Position, Direction), Position>();
            foreach (var link in labels.GroupBy(l => l.Label).Where(g => g.Count() == 2))
            {
                var first = link.ElementAt(0);
                var second = link.ElementAt(1);
                dict.Add((first.Position, first.Direction), second.Position);
                dict.Add((second.Position, second.Direction), first.Position);
            }
            return dict;
        }

        public int ShortestPath()
        {
            var dist = new Dictionary<Position, int>
            {
                [Start] = 0
            };

            var queue = new Queue<Position>();
            queue.Enqueue(Start);

            while (queue.Any())
            {
                var p = queue.Dequeue();
                var d = dist[p];

                foreach (var dir in DirectionExtensions.Directions)
                {
                    var newPos = MoveOrTeleport(p, dir);
                    var c = Get(newPos);

                    if (c == '#')
                    {
                        continue;
                    }

                    if (c == '.')
                    {
                        if (dist.TryGetValue(newPos, out var newPDist))
                        {
                            if (newPDist > d + 1)
                            {
                                dist[newPos] = d + 1;
                                queue.Enqueue(newPos);
                            }
                        }
                        else
                        {
                            dist[newPos] = d + 1;
                            queue.Enqueue(newPos);
                        }
                    }
                }
            }

            return dist[End];
        }

        private Position MoveOrTeleport(Position position, Direction direction) =>
            Links.TryGetValue((position, direction), out var destination) ? destination : position.Move(direction);

        private char Get(Position position) => _map[position.X + position.Y * _width];

        private static bool IsLetter(char c) => c >= 'A' && c <= 'Z';

        public static Map Parse(IEnumerable<string> input)
        {
            var lines = input as string[] ?? input.ToArray();
            var width = lines.Max(l => l.Length);

            var normLines = lines.SelectMany(l => l.ToCharArray().Concat(Enumerable.Repeat(' ', width - l.Length)));
            return new Map(normLines, width);
        }
    }
}
