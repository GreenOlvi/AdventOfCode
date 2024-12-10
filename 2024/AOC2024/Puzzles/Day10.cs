


namespace AOC2024.Puzzles;

public class Day10 : CustomBaseProblem<long>
{
    private readonly IHashGrid2<byte> _map;

    public Day10()
    {
        _map = ParseInput(ReadLinesFromFile());
    }

    public Day10(IEnumerable<string> lines)
    {
        _map = ParseInput(lines);
    }

    private static IHashGrid2<byte> ParseInput(IEnumerable<string> lines)
    {
        var map = new HashGrid<byte>(100);
        var y = 0;
        foreach (var line in lines)
        {
            var x = 0;
            foreach (var c in line)
            {
                if (c is >= '0' and <= '9')
                {
                    map[(x, y)] = (byte)(c - '0');
                }
                x++;
            }
            y++;
        }

        return map.ToFrozen();
    }

    private static readonly Point2[] Directions = [Point2.Up, Point2.Right, Point2.Down, Point2.Left];

    private static IEnumerable<(Point2 Position, byte Tile)> GetNeighbours(IHashGrid2<byte> map, (Point2 Position, byte Tile) start) =>
        Directions.Select(d => (start.Position + d, map[start.Position + d]))
            .Where(p => p.Item2 == start.Tile + 1);

    private static int FindTrailScore(IHashGrid2<byte> map, (Point2 Position, byte Tile) start)
    {
        var score = 0;
        var visited = new HashSet<Point2>();
        var q = new Queue<(Point2 Position, byte Tile)>();
        q.Enqueue(start);

        while (q.TryDequeue(out var p))
        {
            if (visited.Contains(p.Position))
            {
                continue;
            }

            _ = visited.Add(p.Position);

            if (p.Tile == 9)
            {
                score++;
            }
            else
            {
                foreach (var n in GetNeighbours(map, p).Where(n => !visited.Contains(n.Position)))
                {
                    q.Enqueue(n);
                }
            }
        }

        return score;
    }

    private static int FindTrailRate(IHashGrid2<byte> map, (Point2 Position, byte Tile) start)
    {
        var rate = 0;
        var q = new Queue<(Point2 Position, byte Tile)>();
        q.Enqueue(start);

        while (q.TryDequeue(out var p))
        {
            if (p.Tile == 9)
            {
                rate++;
            }
            else
            {
                var neighbours = GetNeighbours(map, p);
                foreach (var n in neighbours)
                {
                    q.Enqueue(n);
                }
            }
        }

        return rate;
    }

    public override long Solve1() =>
        _map.Where(static p => p.Tile == 0)
            .Sum(s => FindTrailScore(_map, s));

    public override long Solve2() =>
        _map.Where(static p => p.Tile == 0)
            .Sum(s => FindTrailRate(_map, s));
}
