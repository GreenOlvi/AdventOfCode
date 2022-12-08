namespace AOC2022.Puzzles;

public class Day08 : CustomBaseDay
{
    private readonly Grid<byte> _grid;

    public Day08()
    {
        _grid = ParseInput(ReadLinesFromFile());
    }

    public Day08(IEnumerable<string> lines)
    {
        _grid = ParseInput(lines);
    }

    public static Grid<byte> ParseInput(IEnumerable<string> lines)
    {
        var rows = new List<byte[]>();
        foreach (var line in lines)
        {
            var row = line.Select(c => (byte)(c - '0')).ToArray();
            rows.Add(row);
        }

        return new Grid<byte>(rows.ToArray());
    }

    private static IEnumerable<byte> TreesToEdge(Grid<byte> grid, Point2 tree, Direction direction)
    {
        var next = tree.Move(direction);
        while (grid.IsInside(next))
        {
            yield return grid.Get(next);
            next = next.Move(direction);
        }
    }

    private static bool IsVisible(Grid<byte> grid, Point2 tree)
    {
        var h = grid.Get(tree);
        return Enum.GetValues<Direction>().Any(d => TreesToEdge(grid, tree, d).All(t => t < h));
    }

    public static int CountTrees(Grid<byte> grid, Point2 tree, Direction direction)
    {
        var h = grid.Get(tree);
        var i = 0;
        foreach (var t in TreesToEdge(grid, tree, direction))
        {
            i++;
            if (t >= h)
            {
                return i;
            }
        }
        return i;
    }

    private static long GetScore(Grid<byte> grid, Point2 p) =>
        Enum.GetValues<Direction>()
            .Select(d => CountTrees(grid, p, d))
            .Product();

    public override ValueTask<string> Solve_1() =>
        _grid.EnumeratePoints().Count(p => IsVisible(_grid, p)).ToResult();

    public override ValueTask<string> Solve_2() =>
        _grid.EnumeratePoints().Select(p => GetScore(_grid, p)).Max().ToResult();

    public readonly record struct Grid<T>
    {
        private readonly T[][] _items;

        public readonly int Width;
        public readonly int Height;

        public Grid(T[][] items)
        {
            _items = items;
            Width = _items[0].Length;
            Height = _items.Length;
        }

        public T this[Point2 p] => _items[p.Y][p.X];

        public T Get(Point2 p) => _items[p.Y][p.X];

        public IEnumerable<Point2> EnumeratePoints()
        {
            int w = Width;
            return Enumerable.Range(0, Height)
                .SelectMany(y => Enumerable.Range(0, w)
                    .Select(x => new Point2(x, y)));
        }

        public bool IsInside(Point2 p) => p.X >= 0 && p.Y >= 0 && p.X < Width && p.Y < Height;
    }
}
