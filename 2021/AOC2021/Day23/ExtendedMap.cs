namespace AOC2021.Day23
{
    public class ExtendedMap : Map
    {
        public ExtendedMap() : base(ExtendedEdges)
        {
        }

        private static readonly Dictionary<(Room A, Room B), int> ExtendedEdges = new()
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
            [(Room.A2, Room.A3)] = 1,
            [(Room.A3, Room.A4)] = 1,
            [(Room.B1, Room.B2)] = 1,
            [(Room.B2, Room.B3)] = 1,
            [(Room.B3, Room.B4)] = 1,
            [(Room.C1, Room.C2)] = 1,
            [(Room.C2, Room.C3)] = 1,
            [(Room.C3, Room.C4)] = 1,
            [(Room.D1, Room.D2)] = 1,
            [(Room.D2, Room.D3)] = 1,
            [(Room.D3, Room.D4)] = 1,
        };

        /*
         * H1 - H2 - * - H3 - * - H4 - * - H5 - * - H6 - H7
         *           |        |        |        |
         *           A1       B1       C1       D1
         *           |        |        |        |
         *           A2       B2       C2       D2
         *           |        |        |        |
         *           A3       B3       C3       D3
         *           |        |        |        |
         *           A4       B4       C4       D4
         */


        private readonly Dictionary<Amphipod, Room[]> _homes = new()
        {
            [Amphipod.A] = new[] { Room.A1, Room.A2, Room.A3, Room.A4 },
            [Amphipod.B] = new[] { Room.B1, Room.B2, Room.B3, Room.B4 },
            [Amphipod.C] = new[] { Room.C1, Room.C2, Room.C3, Room.C4 },
            [Amphipod.D] = new[] { Room.D1, Room.D2, Room.D3, Room.D4 },
        };

        public override Room[] GetHomes(Amphipod a) => _homes[a];
    }
}
