namespace AOC2021.Day23
{
    public class Map
    {
        public Map()
        {
        }

        protected Map(Dictionary<(Room A, Room B), int> edges) : this()
        {
            _edges = edges;
        }

        protected readonly Dictionary<(Room A, Room B), int> _edges = new()
        {
            [(Room.H1, Room.H2)] = 1,
            [(Room.H2, Room.H3)] = 2,
            [(Room.H3, Room.H4)] = 2,
            [(Room.H4, Room.H5)] = 2,
            [(Room.H5, Room.H6)] = 2,
            [(Room.H6, Room.H7)] = 1,
            [(Room.H2, Room.A1)] = 2,
            [(Room.H3, Room.A1)] = 2,
            [(Room.H3, Room.B1)] = 2,
            [(Room.H4, Room.B1)] = 2,
            [(Room.H4, Room.C1)] = 2,
            [(Room.H5, Room.C1)] = 2,
            [(Room.H5, Room.D1)] = 2,
            [(Room.H6, Room.D1)] = 2,
            [(Room.A1, Room.A2)] = 1,
            [(Room.B1, Room.B2)] = 1,
            [(Room.C1, Room.C2)] = 1,
            [(Room.D1, Room.D2)] = 1,
        };

        /*
         * H1 - H2 - * - H3 - * - H4 - * - H5 - * - H6 - H7
         *           |        |        |        |
         *           A1       B1       C1       D1
         *           |        |        |        |
         *           A2       B2       C2       D2
         */

        private readonly Dictionary<(Room A, Room B), int> _distances = new();
        private readonly Dictionary<(Room A, Room B), Room[]> _routes = new();

        public int GetDistance(Room a, Room b)
        {
            if (_distances.TryGetValue((a, b), out var distance))
            {
                return distance;
            }

            if (_edges.TryGetValue((a, b), out var dist))
            {
                _distances[(a, b)] = dist;
                return dist;
            }

            if (_edges.TryGetValue((b, a), out var distRev))
            {
                _distances[(a, b)] = distRev;
                return distRev;
            }

            foreach (var p in FindDistances(a))
            {
                if (!_distances.ContainsKey((a, p.Key)))
                {
                    _distances[(a, p.Key)] = p.Value;
                }

                if (!_distances.ContainsKey((p.Key, a)))
                {
                    _distances[(p.Key, a)] = p.Value;
                }
            }

            return _distances[(a, b)];
        }

        public IEnumerable<Room> GetRoute(Room a, Room b)
        {
            if (_routes.TryGetValue((a, b), out var route))
            {
                return route;
            }

            foreach (var kv in FindRoutes(a))
            {
                if (!_routes.ContainsKey((a, kv.Key)))
                {
                    _routes[(a, kv.Key)] = kv.Value.Route;
                }
            }

            return _routes[(a, b)];
        }

        private Dictionary<Room, int> FindDistances(Room a)
        {
            var todo = new Queue<(Room, int)>();
            todo.Enqueue((a, 0));

            var distances = new Dictionary<Room, int>();
            while (todo.Count > 0)
            {
                var (r, d) = todo.Dequeue();
                if (distances.TryGetValue(r, out var current) && current <= d)
                {
                    continue;
                }

                distances[r] = d;

                foreach (var edge in _edges.Where(e => e.Key.A == r || e.Key.B == r))
                {
                    var (r1, r2) = edge.Key;
                    var other = r1 == r ? r2 : r1;
                    todo.Enqueue((other, d + edge.Value));
                }
            }
            return distances;
        }

        private Dictionary<Room, (int Distance, Room[] Route)> FindRoutes(Room a)
        {
            var todo = new Queue<(Room, int, Room[])>();
            todo.Enqueue((a, 0, Array.Empty<Room>()));

            var routes = new Dictionary<Room, (int Distance, Room[] Route)>();
            while (todo.Count > 0)
            {
                var (r, d, path) = todo.Dequeue();
                if (routes.TryGetValue(r, out var current) && current.Distance <= d)
                {
                    continue;
                }

                routes[r] = (d, path);

                foreach (var edge in _edges.Where(e => e.Key.A == r || e.Key.B == r))
                {
                    var (r1, r2) = edge.Key;
                    var other = r1 == r ? r2 : r1;

                    todo.Enqueue((other, d + edge.Value, path.Append(other).ToArray()));
                }
            }
            return routes;
        }

        public IEnumerable<Room> GetAllRooms() => new[]
        {
            Room.H1, Room.H2, Room.H3, Room.H4, Room.H5, Room.H6, Room.H7,
        }
            .Concat(GetHomes(Amphipod.A))
            .Concat(GetHomes(Amphipod.B))
            .Concat(GetHomes(Amphipod.C))
            .Concat(GetHomes(Amphipod.D));

        private readonly Dictionary<Amphipod, Room[]> _homes = new()
        {
            [Amphipod.A] = new[] { Room.A1, Room.A2 },
            [Amphipod.B] = new[] { Room.B1, Room.B2 },
            [Amphipod.C] = new[] { Room.C1, Room.C2 },
            [Amphipod.D] = new[] { Room.D1, Room.D2 },
        };

        public static Room[] GetHallways() => new[]
        {
            Room.H1, Room.H2, Room.H3, Room.H4, Room.H5, Room.H6, Room.H7,
        };

        public virtual Room[] GetHomes(Amphipod a) => _homes[a];
    }
}
