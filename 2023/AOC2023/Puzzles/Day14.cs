
namespace AOC2023.Puzzles;
public class Day14 : CustomBaseDay
{
    private readonly HashGrid<Tile> _platform;
    private readonly int _width;
    private readonly int _height;

    public Day14()
    {
        _platform = ParsePlatform(ReadLinesFromFile());
        _width = (int)_platform.MaxX - 1;
        _height = (int)_platform.MaxY - 1;
    }

    public Day14(IEnumerable<string> lines)
    {
        _platform = ParsePlatform(lines);
        _width = (int)_platform.MaxX - 1;
        _height = (int)_platform.MaxY - 1;
    }

    private static HashGrid<Tile> ParsePlatform(IEnumerable<string> lines)
    {
        var grid = new HashGrid<Tile>();
        var y = 0;
        foreach (var line in lines)
        {
            var x = 0;
            foreach (var c in line)
            {
                var tile = c switch
                {
                    '.' => Tile.Empty,
                    '#' => Tile.Cube,
                    'O' => Tile.Rounded,
                    _ => throw new InvalidDataException(c.ToString()),
                };

                if (tile != Tile.Empty)
                {
                    grid[(x, y)] = tile;
                }
                x++;
            }
            y++;
        }

        // add border
        var maxX = grid.MaxX;
        var maxY = grid.MaxY;
        for (var i = -1; i <= maxX + 1; i++)
        {
            grid[(i, -1)] = Tile.Cube;
            grid[(i, maxY + 1)] = Tile.Cube;
        }

        for (var i = 0; i <= maxY; i++)
        {
            grid[(-1, i)] = Tile.Cube;
            grid[(maxY + 1, i)] = Tile.Cube;
        }

        return grid;
    }

    private static void TiltNorth(HashGrid<Tile> grid)
    {
        var rounded = grid.Where(pair => pair.Item2 == Tile.Rounded && pair.Item1.Y > 0)
            .Select(p => p.Item1)
            .OrderBy(p => p.Y)
            .ToArray();

        foreach (var r in rounded)
        {
            var p = r;
            var moved = false;
            while(true)
            {
                var next = p.Move(Direction.Up);
                if (grid[next] != Tile.Empty)
                {
                    break;
                }
                p = next;
                moved = true;
            }

            if (moved)
            {
                grid[p] = Tile.Rounded;
                grid[r] = Tile.Empty;
            }
        }
    }

    private void TiltEast(HashGrid<Tile> grid)
    {
        var rounded = grid.Where(pair => pair.Item2 == Tile.Rounded && pair.Item1.X < _width)
            .Select(p => p.Item1)
            .OrderByDescending(p => p.X)
            .ToArray();

        foreach (var r in rounded)
        {
            var p = r;
            var moved = false;
            while(true)
            {
                var next = p.Move(Direction.Right);
                if (grid[next] != Tile.Empty)
                {
                    break;
                }
                p = next;
                moved = true;
            }

            if (moved)
            {
                grid[p] = Tile.Rounded;
                grid[r] = Tile.Empty;
            }
        }
    }

    private void TiltSouth(HashGrid<Tile> grid)
    {
        var rounded = grid.Where(pair => pair.Item2 == Tile.Rounded && pair.Item1.Y < _height)
            .Select(p => p.Item1)
            .OrderByDescending(p => p.Y)
            .ToArray();

        foreach (var r in rounded)
        {
            var p = r;
            var moved = false;
            while(true)
            {
                var next = p.Move(Direction.Down);
                if (grid[next] != Tile.Empty)
                {
                    break;
                }
                p = next;
                moved = true;
            }

            if (moved)
            {
                grid[p] = Tile.Rounded;
                grid[r] = Tile.Empty;
            }
        }
    }

    private static void TiltWest(HashGrid<Tile> grid)
    {
        var rounded = grid.Where(pair => pair.Item2 == Tile.Rounded && pair.Item1.X > 0)
            .Select(p => p.Item1)
            .OrderBy(p => p.X)
            .ToArray();

        foreach (var r in rounded)
        {
            var p = r;
            var moved = false;
            while(true)
            {
                var next = p.Move(Direction.Left);
                if (grid[next] != Tile.Empty)
                {
                    break;
                }
                p = next;
                moved = true;
            }

            if (moved)
            {
                grid[p] = Tile.Rounded;
                grid[r] = Tile.Empty;
            }
        }
    }

    private void SpinCycle(HashGrid<Tile> grid)
    {
        TiltNorth(grid);
        TiltWest(grid);
        TiltSouth(grid);
        TiltEast(grid);
    }

    private long NorthBeamLoad(HashGrid<Tile> grid) =>
        grid.Where(pair => pair.Item2 == Tile.Rounded).Sum(p => _height - p.Item1.Y + 1);

    public override ValueTask<string> Solve_1()
    {
        var p1 = _platform.Clone();
        TiltNorth(p1);
        return NorthBeamLoad(p1).ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        const long cycles = 1_000_000_000;

        var p2 = _platform.Clone();

        var states = new Dictionary<string, long>();
        var repeats = new List<long>();

        long i;
        for (i = 0L; i < cycles; i++)
        {
            SpinCycle(p2);

            var d = p2.Draw(_tileChars);
            if (!states.TryAdd(d, i))
            {
                var c = states[d];
                repeats.Add(i - c);
                if (repeats.Count >= 3)
                {
                    break;
                }
            }
        }

        var left = (cycles - i) % repeats.Last();
        for (var j = 0; j < left - 1; j++)
        {
            SpinCycle(p2);
        }

        return NorthBeamLoad(p2).ToResult();
    }

    private static readonly Dictionary<Tile, char> _tileChars = new()
    {
        [Tile.Empty] = '.',
        [Tile.Cube] = '#',
        [Tile.Rounded] = 'O',
    };

    private enum Tile
    {
        Empty = 0,
        Cube,
        Rounded,
    };
}
