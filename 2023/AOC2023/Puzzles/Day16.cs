
using Spectre.Console;
using System.Runtime.InteropServices;

namespace AOC2023.Puzzles;
public class Day16 : CustomBaseDay
{
    private readonly HashGrid<Tile> _grid;

    public Day16()
    {
        _grid = ParseMirrors(ReadLinesFromFile());
    }

    public Day16(IEnumerable<string> lines)
    {
        _grid = ParseMirrors(lines);
    }

    private static HashGrid<Tile> ParseMirrors(IEnumerable<string> lines)
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
                    '#' => Tile.Wall,
                    '/' => Tile.MirrorSlash,
                    '\\' => Tile.MirrorBackslash,
                    '|' => Tile.SplitterVertical,
                    '-' => Tile.SplitterHorizontal,
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

        var walls = new Box(grid.TopLeft - Point2.One, grid.BottomRight + Point2.One)
            .GetBorderPoints();

        foreach (var wall in walls)
        {
            grid[wall] = Tile.Wall;
        }

        return grid;
    }

    private static int TraceLight(HashGrid<Tile> grid, PosDir start)
    {
        var lightQueue = new Queue<PosDir>();
        lightQueue.Enqueue(start);

        var seen = new HashSet<PosDir>();

        while (lightQueue.Count != 0)
        {
            var next = lightQueue.Dequeue();
            if (seen.Contains(next))
            {
                continue;
            }

            do
            {
                seen.Add(next);
                next = next.Next();
            }
            while (grid[next.Position] == Tile.Empty);

            var nextElement = grid[next.Position];

            if (nextElement == Tile.Wall)
            {
                continue;
            }

            if (nextElement == Tile.MirrorSlash)
            {
                lightQueue.Enqueue(next with { Direction = MirrorSlashDirectionChange[next.Direction] });
                continue;
            }

            if (nextElement == Tile.MirrorBackslash)
            {
                lightQueue.Enqueue(next with { Direction = MirrorBackslashDirectionChange[next.Direction] });
                continue;
            }

            if (nextElement == Tile.SplitterVertical)
            {
                if (next.Direction == Direction.Right || next.Direction == Direction.Left)
                {
                    lightQueue.Enqueue(next with { Direction = Direction.Up });
                    lightQueue.Enqueue(next with { Direction = Direction.Down });
                }
                else
                {
                    lightQueue.Enqueue(next);
                }
                continue;
            }

            if (nextElement == Tile.SplitterHorizontal)
            {
                if (next.Direction == Direction.Up || next.Direction == Direction.Down)
                {
                    lightQueue.Enqueue(next with { Direction = Direction.Left });
                    lightQueue.Enqueue(next with { Direction = Direction.Right });
                }
                else
                {
                    lightQueue.Enqueue(next);
                }
            }
        }

        return seen.DistinctBy(pd => pd.Position).Count() - 1;
    }

    public override ValueTask<string> Solve_1() =>
        TraceLight(_grid, new PosDir(new Point2(-1, 0), Direction.Right)).ToResult();

    public override ValueTask<string> Solve_2()
    {
        var results = new List<int>();

        for (var y = _grid.TopLeft.Y + 1; y < _grid.BottomRight.Y; y++)
        {
            results.Add(TraceLight(_grid, new PosDir(new Point2(-1, y), Direction.Right)));
            results.Add(TraceLight(_grid, new PosDir(new Point2(_grid.MaxX, y), Direction.Left)));
        }

        for (var x = _grid.TopLeft.X + 1; x < _grid.BottomRight.X; x++)
        {
            results.Add(TraceLight(_grid, new PosDir(new Point2(x, -1), Direction.Down)));
            results.Add(TraceLight(_grid, new PosDir(new Point2(x, _grid.MaxY), Direction.Up)));
        }

        return results.Max().ToResult();
    }

    private static readonly Dictionary<Direction, Direction> MirrorSlashDirectionChange = new()
    {
        [Direction.Right] = Direction.Up,
        [Direction.Left] = Direction.Down,
        [Direction.Up] = Direction.Right,
        [Direction.Down] = Direction.Left,
    };

    private static readonly Dictionary<Direction, Direction> MirrorBackslashDirectionChange = new()
    {
        [Direction.Right] = Direction.Down,
        [Direction.Left] = Direction.Up,
        [Direction.Up] = Direction.Left,
        [Direction.Down] = Direction.Right,
    };

    private static readonly Dictionary<Tile, char> _tilesToChar = new()
    {
        [Tile.Empty] = '.',
        [Tile.Wall] = '#',
        [Tile.MirrorSlash] = '/',
        [Tile.MirrorBackslash] = '\\',
        [Tile.SplitterVertical] = '|',
        [Tile.SplitterHorizontal] = '-',
    };

    private enum Tile
    {
        Empty = 0,
        Wall,
        MirrorSlash,
        MirrorBackslash,
        SplitterVertical,
        SplitterHorizontal,
    }

    private readonly record struct PosDir(Point2 Position, Direction Direction)
    {
        public PosDir Next() => new(Position.Move(Direction), Direction);
    }
}
