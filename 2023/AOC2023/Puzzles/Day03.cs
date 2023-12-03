namespace AOC2023.Puzzles;
public class Day03 : CustomBaseDay
{
    private readonly string[] _lines;

    public Day03()
    {
        _lines = ReadLinesFromFile().ToArray();
    }

    public Day03(IEnumerable<string> lines)
    {
        _lines = lines.ToArray();
    }

    private static IEnumerable<Point2> GetNeighbours(Point2 point)
    {
        yield return point + new Point2(-1, -1);
        yield return point + new Point2(0, -1);
        yield return point + new Point2(+1, -1);
        yield return point + new Point2(-1, 0);
        yield return point + new Point2(+1, 0);
        yield return point + new Point2(-1, +1);
        yield return point + new Point2(0, +1);
        yield return point + new Point2(+1, +1);
    }

    private static bool IsSymbol(char c) => (c < '0' || c > '9') && c != '\0';

    private static bool IsNumber(char c) => char.IsAsciiDigit(c);


    public override ValueTask<string> Solve_1()
    {
        var grid = ParseGrid();

        var numbers = new Queue<Point2>(grid.Where(kv => IsNumber(kv.Item2)).Select(kv => kv.Item1));

        var partNumbers = new List<int>();

        while (numbers.Count > 0)
        {
            var point = numbers.Dequeue();
            var isPartNumber = false;
            var wholeNumber = grid[point] - '0';

            do
            {
                if (GetNeighbours(point).Any(p => IsSymbol(grid[p])))
                {
                    isPartNumber = true;
                }

                var next = point + new Point2(1, 0);
                var nextChar = grid[next];
                if (IsNumber(nextChar))
                {
                    point = next;
                    wholeNumber = wholeNumber * 10 + (nextChar - '0');
                    _ = numbers.Dequeue();
                }
                else
                {
                    if (isPartNumber)
                    {
                        partNumbers.Add(wholeNumber);
                    }
                    break;
                }
            }
            while (true);
        }

        return partNumbers.Sum().ToResult();
    }

    private HashGrid<char> ParseGrid()
    {
        var grid = new HashGrid<char>();
        var y = 0;
        foreach (var line in _lines)
        {
            var x = 0;
            foreach (var c in line)
            {
                if (c != '.')
                {
                    grid[(x, y)] = c;
                }
                x++;
            }
            y++;
        }

        return grid;
    }

    public override ValueTask<string> Solve_2()
    {
        var grid = ParseGrid();

        var stars = new Queue<Point2>(grid.Where(kv => kv.Item2 == '*').Select(kv => kv.Item1));

        var gears = new List<(long, long)>();
        while (stars.Count > 0)
        {
            var gear = stars.Dequeue();

            var neighbourNumbers = GetNeighbours(gear).Where(p => IsNumber(grid[p])).ToHashSet();
            if (neighbourNumbers.Count < 2)
            {
                continue;
            }

            var neighbours = new List<int>();
            while (neighbourNumbers.Count > 0)
            {
                var n = neighbourNumbers.First();
                var (number, pos) = FindWholeNumber(grid, n);

                neighbours.Add(number);
                foreach(var p in pos)
                {
                    neighbourNumbers.Remove(p);
                }
            }

            if (neighbours.Count != 2)
            {
                continue;
            }

            gears.Add((neighbours[0], neighbours[1]));
        }

        return gears.Sum(p => p.Item1 * p.Item2).ToResult();
    }

    private static (int Number, HashSet<Point2> Positions) FindWholeNumber(HashGrid<char> grid, Point2 start)
    {
        var pointer = start;
        do
        {
            pointer = pointer.Move(Direction.Left);
        }
        while (IsNumber(grid[pointer]));

        pointer = pointer.Move(Direction.Right);

        var n = 0;
        var pos = new HashSet<Point2>();
        while (IsNumber(grid[pointer]))
        {
            n = 10 * n + grid[pointer] - '0';
            pos.Add(pointer);
            pointer = pointer.Move(Direction.Right);
        }

        return (n, pos);
    }
}
