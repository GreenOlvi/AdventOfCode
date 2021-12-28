using System.Diagnostics;
using System.Text;

namespace AOC2021.Day23
{
    public class MapState
    {
        private MapState(Map map, Dictionary<Room, Amphipod> rooms)
        {
            _map = map;
            _rooms = rooms;
            _shortString = ToShort();
        }

        private readonly Map _map;
        private readonly Dictionary<Room, Amphipod> _rooms;
        private readonly string _shortString;

        public Amphipod GetRoom(Room r) => _rooms.TryGetValue(r, out var amphipod) ? amphipod : Amphipod.None;

        private IEnumerable<(Amphipod, Room)> GetAmphipods() =>
            _rooms.Where(kv => !IsDone(kv.Value, kv.Key))
                .Select(kv => (kv.Value, kv.Key));

        public IEnumerable<Room> GetEmptyRooms() => _map.GetAllRooms().Where(r => GetRoom(r) == Amphipod.None);

        private static readonly IReadOnlyDictionary<Amphipod, int> Costs = new Dictionary<Amphipod, int>()
        {
            { Amphipod.A, 1 },
            { Amphipod.B, 10 },
            { Amphipod.C, 100 },
            { Amphipod.D, 1000 },
        };

        public MapState Move(Amphipod a, Room from, Room to)
        {
            Debug.Assert(GetRoom(from) == a, "Amphipod Room mismatch");

            var rooms = new Dictionary<Room, Amphipod>(_rooms);
            rooms.Remove(from);
            rooms[to] = a;
            return new MapState(_map, rooms);
        }

        public IEnumerable<(Amphipod A, Room From, Room To)> GetPossibleMoves()
        {
            foreach (var (a, from) in GetAmphipods())
            {
                foreach (var to in GetEmptyRooms())
                {
                    if (CanGo(a, from, to))
                    {
                        yield return (a, from, to);
                    }
                }
            }
        }

        private bool IsDone(Amphipod a, Room r)
        {
            var homes = _map.GetHomes(a);
            var index = Array.IndexOf(homes, r);
            if (index == -1)
            {
                return false;
            }

            for (var i = index; i < homes.Length; i++)
            {
                if (GetRoom(homes[i]) != a)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CanGo(Amphipod a, Room from, Room to)
        {
            Debug.Assert(GetRoom(from) == a, "Amphipod Room mismatch");

            if (GetRoom(to) != Amphipod.None)
            {
                return false;
            }

            if (from.IsHallway() && to.IsHallway())
            {
                return false;
            }

            var route = _map.GetRoute(from, to);
            if (!route.All(r => GetRoom(r) == Amphipod.None))
            {
                return false;
            }

            if (to.IsHallway())
            {
                return true;
            }

            var home = _map.GetHomes(a);
            if (!to.IsHallway() && !home.Contains(to))
            {
                return false;
            }

            var index = Array.IndexOf(home, to);
            for (var i = index + 1; i < home.Length; i++)
            {
                if (GetRoom(home[i]) != a)
                {
                    return false;
                }
            }

            return true;
        }

        public long GetCost(Amphipod a, Room from, Room to) => _map.GetDistance(from, to) * Costs[a];

        public static MapState FromRooms(Map map, Amphipod[] roomA, Amphipod[] roomB, Amphipod[] roomC, Amphipod[] roomD)
        {
            var rooms = new Dictionary<Room, Amphipod>
            {
                [Room.A1] = roomA[0],
                [Room.A2] = roomA[1],
                [Room.B1] = roomB[0],
                [Room.B2] = roomB[1],
                [Room.C1] = roomC[0],
                [Room.C2] = roomC[1],
                [Room.D1] = roomD[0],
                [Room.D2] = roomD[1]
            };
            return new MapState(map, rooms);
        }

        public static MapState FromRoomsExtended(ExtendedMap map, Amphipod[] roomA, Amphipod[] roomB, Amphipod[] roomC, Amphipod[] roomD)
        {
            var rooms = new Dictionary<Room, Amphipod>
            {
                [Room.A1] = roomA[0],
                [Room.A2] = Amphipod.D,
                [Room.A3] = Amphipod.D,
                [Room.A4] = roomA[1],
                [Room.B1] = roomB[0],
                [Room.B2] = Amphipod.C,
                [Room.B3] = Amphipod.B,
                [Room.B4] = roomB[1],
                [Room.C1] = roomC[0],
                [Room.C2] = Amphipod.B,
                [Room.C3] = Amphipod.A,
                [Room.C4] = roomC[1],
                [Room.D1] = roomD[0],
                [Room.D2] = Amphipod.A,
                [Room.D3] = Amphipod.C,
                [Room.D4] = roomD[1]
            };
            return new MapState(map, rooms);
        }

        public static MapState FromPositions(Map map, Dictionary<Amphipod, Room[]> positions)
        {
            var rooms = new Dictionary<Room, Amphipod>();
            foreach (var kv in positions)
            {
                foreach (var r in kv.Value)
                {
                    rooms[r] = kv.Key;
                }
            }
            return new MapState(map, rooms);
        }

        public string Print()
        {
            var sb = new StringBuilder();
            sb.AppendLine("#############");
            sb.Append('#');
            sb.Append(GetRoom(Room.H1).ToChar());
            sb.Append(GetRoom(Room.H2).ToChar());
            sb.Append(' ');
            sb.Append(GetRoom(Room.H3).ToChar());
            sb.Append(' ');
            sb.Append(GetRoom(Room.H4).ToChar());
            sb.Append(' ');
            sb.Append(GetRoom(Room.H5).ToChar());
            sb.Append(' ');
            sb.Append(GetRoom(Room.H6).ToChar());
            sb.Append(GetRoom(Room.H7).ToChar());
            sb.AppendLine("#");
            sb.AppendLine($"###{GetRoom(Room.A1).ToChar()}#{GetRoom(Room.B1).ToChar()}#{GetRoom(Room.C1).ToChar()}#{GetRoom(Room.D1).ToChar()}###");
            sb.AppendLine($"  #{GetRoom(Room.A2).ToChar()}#{GetRoom(Room.B2).ToChar()}#{GetRoom(Room.C2).ToChar()}#{GetRoom(Room.D2).ToChar()}#");
            sb.AppendLine("  #########");
            return sb.ToString();
        }

        public override string ToString() => _shortString;
        private string ToShort() => new(_map.GetAllRooms().Select(r => GetRoom(r).ToChar()).ToArray());

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((MapState)obj);
        }

        private bool Equals(MapState other) => _shortString == other.ToString();

        public override int GetHashCode() => _shortString.GetHashCode();
    }
}
