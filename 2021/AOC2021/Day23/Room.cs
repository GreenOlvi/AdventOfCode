namespace AOC2021.Day23
{
    public enum Room
    {
        H1, H2, H3, H4, H5, H6, H7,
        A1, A2, A3, A4,
        B1, B2, B3, B4,
        C1, C2, C3, C4,
        D1, D2, D3, D4,
    };

    public static class RoomExtensions
    {
        public static bool IsHallway(this Room r) =>
            r is Room.H1 or Room.H2 or Room.H3 or Room.H4 or Room.H5 or Room.H6 or Room.H7;
    }
}
