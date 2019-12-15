using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoC2019.Common;

namespace AoC2019.Puzzle15
{
    public class Solution : IPuzzle
    {
        public Solution(string input)
        {
            _input = IntcodeMachine.ParseInput(input).ToArray();
        }

        private readonly long[] _input;

        private static Dictionary<Position, long> FillDistances(Map map, Position start)
        {
            var distances = new Dictionary<Position, long>
            {
                { start, 0 },
            };

            var directions = new[]
            {
                Direction.Up,
                Direction.Down,
                Direction.Left,
                Direction.Right,
            };

            var queue = new Queue<Position>();
            queue.Enqueue(start);

            while (queue.Any())
            {
                var p = queue.Dequeue();
                var i = distances[p];

                foreach (var d in directions)
                {
                    var newPos = p.Move(d);
                    if (map.GetTile(newPos) != Tile.Wall)
                    {
                        if (distances.TryGetValue(newPos, out var dist))
                        {
                            if (dist > i + 1)
                            {
                                distances[newPos] = i + 1;
                                queue.Enqueue(newPos);
                            }
                        }
                        else
                        {
                            queue.Enqueue(newPos);
                            distances.Add(newPos, i + 1);
                        }
                    }
                }
            }
            return distances;
        }

        public static long Solve1(IEnumerable<long> input)
        {
            var droid = new Droid(new IntcodeMachine(input));
            droid.Run();

            var map = droid.Map;
            var start = map.FindTiles(Tile.Start).First();
            var end = map.FindTiles(Tile.System).First();
            var distances = FillDistances(map, start);

            return distances[end];
        }

        public static long Solve2(IEnumerable<long> input)
        {
            var droid = new Droid(new IntcodeMachine(input));
            droid.Run();

            var map = droid.Map;
            var system = map.FindTiles(Tile.System).First();
            var dist = FillDistances(droid.Map, system);

            return dist.Values.Max();
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_input).ToString());

        public Task<string> Solve2Async() =>
            Task.Run(() => Solve2(_input).ToString());
    }
}
