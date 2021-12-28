using AOC2021.Common;
using System.Text.RegularExpressions;

namespace AOC2021.Day23
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            (_roomA, _roomB, _roomC, _roomD) = ParseRooms(lines);
        }

        private static readonly Regex _roomPattern = new(@"^\s*#*(?<ra>[ABCD])#(?<rb>[ABCD])#(?<rc>[ABCD])#(?<rd>[ABCD])#*\s*$", RegexOptions.Compiled);

        private readonly Amphipod[] _roomA;
        private readonly Amphipod[] _roomB;
        private readonly Amphipod[] _roomC;
        private readonly Amphipod[] _roomD;

        public readonly Map Map = new();
        public readonly ExtendedMap Map2 = new();

        private static (Amphipod[], Amphipod[], Amphipod[], Amphipod[]) ParseRooms(IEnumerable<string> lines)
        {
            var a = new List<Amphipod>();
            var b = new List<Amphipod>();
            var c = new List<Amphipod>();
            var d = new List<Amphipod>();
            foreach (var line in lines)
            {
                if (_roomPattern.TryMatchAll(line, out var m))
                {
                    a.Add(Enum.Parse<Amphipod>(m["ra"]));
                    b.Add(Enum.Parse<Amphipod>(m["rb"]));
                    c.Add(Enum.Parse<Amphipod>(m["rc"]));
                    d.Add(Enum.Parse<Amphipod>(m["rd"]));
                }
            }
            return (a.ToArray(), b.ToArray(), c.ToArray(), d.ToArray());
        }


        private static long FindLowestCostSolution(MapState initial, MapState ending)
        {
            var states = new Dictionary<MapState, long>();

            var todo = new PriorityQueue<(MapState State, long Cost), double>();
            todo.Enqueue((initial, 0), 0);

            var costLimit = long.MaxValue;

            while (todo.Count > 0)
            {
                var (state, cost) = todo.Dequeue();
                if (states.TryGetValue(state, out long existing) && existing <= cost)
                {
                    continue;
                }

                states[state] = cost;
                if (state.Equals(ending))
                {
                    costLimit = cost;
                }

                var moves = state.GetPossibleMoves();
                foreach (var (a, from, to) in moves)
                {
                    var completeCost = cost + state.GetCost(a, from, to);
                    var newState = state.Move(a, from, to);
                    if (cost < costLimit && (!states.TryGetValue(newState, out var currentCost) || currentCost > completeCost))
                    {
                        todo.Enqueue((newState, completeCost), completeCost);
                    }
                }
            }

            return states[ending];
        }

        public override long Solution1()
        {
            var initial = MapState.FromRooms(Map, _roomA, _roomB, _roomC, _roomD);
            var ending = MapState.FromPositions(Map, new Dictionary<Amphipod, Room[]> {
                { Amphipod.A, new[] { Room.A1, Room.A2 } },
                { Amphipod.B, new[] { Room.B1, Room.B2 } },
                { Amphipod.C, new[] { Room.C1, Room.C2 } },
                { Amphipod.D, new[] { Room.D1, Room.D2 } },
            });

            return FindLowestCostSolution(initial, ending);
        }

        public override long Solution2()
        {
            var initial = MapState.FromRoomsExtended(Map2, _roomA, _roomB, _roomC, _roomD);
            var ending = MapState.FromPositions(Map2, new Dictionary<Amphipod, Room[]> {
                { Amphipod.A, new[] { Room.A1, Room.A2, Room.A3, Room.A4 } },
                { Amphipod.B, new[] { Room.B1, Room.B2, Room.B3, Room.B4 } },
                { Amphipod.C, new[] { Room.C1, Room.C2, Room.C3, Room.C4 } },
                { Amphipod.D, new[] { Room.D1, Room.D2, Room.D3, Room.D4 } },
            });

            return FindLowestCostSolution(initial, ending);
        }
    }
}
