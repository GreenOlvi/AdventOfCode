
namespace AOC2024.Puzzles;

public class Day06 : CustomBaseProblem<int>
{
    private readonly HashGrid<Tile> _map;
    private readonly Point2 _start;

    public Day06()
    {
        (_map, _start) = ParseInput(ReadLinesFromFile());
    }

    public Day06(IEnumerable<string> lines)
    {
        (_map, _start) = ParseInput(lines);
    }

    private (HashGrid<Tile> _map, Point2 _start) ParseInput(IEnumerable<string> lines)
    {
        var grid = new HashGrid<Tile>();
        var start = new Point2();

        var y = 0;
        foreach (var line in lines)
        {
            var x = 0;
            foreach (var c in line)
            {
                if (c == '#')
                {
                    grid[(x, y)] = Tile.Blocked;
                }
                else if (c == '^')
                {
                    start = new Point2(x, y);
                }
                x++;
            }
            y++;
        }
        return (grid, start);
    }

    private static readonly Direction[] _directions =
        [
            Direction.Up,
            Direction.Right,
            Direction.Down,
            Direction.Left,
        ];

    private static void Draw(HashGrid<Tile> map, HashSet<Point2> visited, Point2 position)
    {
        var drawing = map.Clone();
        foreach (var v in visited)
        {
            drawing[v] = Tile.Visited;
        }
        drawing[position] = Tile.Guard;

        static char Tiles(Tile t) => t switch
        {
            Tile.Empty => '.',
            Tile.Guard => '^',
            Tile.Blocked => '#',
            Tile.Visited => 'X',
            _ => ' ',
        };

        Console.WriteLine(drawing.Draw(Tiles));
    }

    private static HashSet<Point2> FindVisited(HashGrid<Tile> map, Point2 start)
    {
        var borders = map.Box;
        var d = 0;
        var dir = _directions[d];
        var p = start;
        HashSet<Point2> visited = [p];
        while (true)
        {
            var next = p.Move(dir);
            if (map[next] == Tile.Blocked)
            {
                d = (d + 1) % 4;
                dir = _directions[d];
                // Draw(map, visited, p);
                continue;
            }

            if (!borders.IsInside(next))
            {
                // Draw(map, visited, p);
                break;
            }

            p = next;
            _ = visited.Add(p);
        }

        return visited;
    }

    private static bool FindLoop(HashGrid<Tile> map, Point2 start)
    {
        var borders = map.Box;
        var d = 0;
        var dir = _directions[d];
        var p = start;
        HashSet<(Point2, Direction)> visited = [(p, dir)];
        while (true)
        {
            var next = p.Move(dir);

            if (map[next] == Tile.Blocked)
            {
                d = (d + 1) % 4;
                dir = _directions[d];
                // Draw(map, visited, p);
                continue;
            }

            if (!borders.IsInside(next))
            {
                // Draw(map, visited, p);
                return false;
            }

            if (visited.Contains((next, dir)))
            {
                return true;
            }


            p = next;
            _ = visited.Add((p, dir));
        }
    }

    public override int Solve1() => FindVisited(_map, _start).Count;

    public override int Solve2()
    {
        var defaultPath = FindVisited(_map, _start);
        _ = defaultPath.Remove(_start);

        List<Point2> possibleObstacles = [];
        foreach (var obstacle in defaultPath)
        {
            var m = _map.Clone();
            m[obstacle] = Tile.Blocked;
            if (FindLoop(m, _start))
            {
                possibleObstacles.Add(obstacle);
            }
        }


        return possibleObstacles.Count;
    }

    private enum Tile
    {
        Empty = 0,
        Blocked,
        Guard,
        Visited,
    }
}
