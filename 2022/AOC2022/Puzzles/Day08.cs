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
}
