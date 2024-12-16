


namespace AOC2024.Puzzles;

public class Day16 : CustomBaseProblem<long>
{
    private readonly HashGrid<bool> _walls;
    private readonly Point2 _start;
    private readonly Point2 _end;

    public Day16()
    {
        (_walls, _start, _end) = ParseInput(ReadLinesFromFile());
    }

    public Day16(IEnumerable<string> lines)
    {
        (_walls, _start, _end) = ParseInput(lines);
    }

    private static (HashGrid<bool> _walls, Point2 _start, Point2 _end) ParseInput(IEnumerable<string> lines)
    {
        var walls = new HashGrid<bool>(true);
        Point2 start = new();
        Point2 end = new();

        var y = 0;
        foreach (var line in lines)
        {
            var x = 0;
            foreach (var c in line)
            {
                if (c == '.')
                {
                    walls[(x, y)] = false;
                }
                else if (c == 'S')
                {
                    start = new Point2(x, y);
                    walls[(x, y)] = false;
                }
                else if (c == 'E')
                {
                    end = new Point2(x, y);
                    walls[(x, y)] = false;
                }
                x++;
            }
            y++;
        }

        return (walls, start, end);
    }

    private static void PrintPathCost(HashGrid<PathElement> paths)
    {
        paths.Print(static (p, t) =>
        {
            return t switch
            {
                (var c, _) => (ConsoleColor.Red, c < long.MaxValue ? $"[{c:00000}]" : "       "),
            };
        }, paths.TopLeft - Point2.One, paths.BottomRight + Point2.One);
    }

    private static readonly Direction[] Neighbours = [Direction.Up, Direction.Right, Direction.Down, Direction.Left];

    private readonly record struct PathElement(long Cost, Direction TravellingDirection);

    private static HashGrid<long> FindLowestCosts(HashGrid<bool> walls, Point2 start)
    {
        var paths = new HashGrid<PathElement>(new PathElement(long.MaxValue, Direction.None));
        paths[start] = new PathElement(0, Direction.None);

        var queue = new PriorityQueue<(Point2 Position, Direction Direction, long Cost), long>();
        queue.Enqueue((start, Direction.Right, 0), 0);

        while (queue.TryDequeue(out var node, out var _))
        {
            var existingNode = paths[node.Position];
            if (node.Cost > existingNode.Cost)
            {
                continue;
            }

            var opposite = node.Direction.Opposite();
            var neighbours = Neighbours.Where(d => d != opposite)
                .Select(d => (Direction: d, Position: node.Position.Move(d)))
                .Where(p => !walls[p.Position]);

            foreach (var n in neighbours)
            {
                var travelCost = 1;
                if (n.Direction != node.Direction)
                {
                    travelCost += 1000;
                }

                var currentNeighbour = paths[n.Position];
                var neighbourCost = node.Cost + travelCost;
                if (neighbourCost < currentNeighbour.Cost)
                {
                    // paths[n.Position] = new PathElement(neighbourCost, n.Direction);
                    paths[node.Position] = new PathElement(neighbourCost, n.Direction);
                    queue.Enqueue((n.Position, n.Direction, neighbourCost), neighbourCost);
                }
            }

        }

        PrintPathCost(paths);

        return new HashGrid<long>(paths.Select(p => (p.Position, p.Tile.Cost)), long.MaxValue);
    }

    public override long Solve1()
    {
        var costs = FindLowestCosts(_walls, _start);
        return costs[_end] - 1001;
    }

    private static long FindShortestPathsSeats(HashGrid<long> costs, Point2 end)
    {
        var seats = new HashSet<Point2>
        {
            end,
        };

        var queue = new Queue<Point2>();
        queue.Enqueue(end);

        while (queue.TryDequeue(out var node))
        {
            var cost = costs[node];

            var neighbours = Neighbours.Select(d => node.Move(d))
                .Where(n => costs[n] < cost);

            foreach (var n in neighbours)
            {
                _ = seats.Add(n);
                queue.Enqueue(n);
            }
        }

        foreach (var s in seats.OrderBy(p => p.X + p.Y * costs.MaxX))
        {
            Console.WriteLine(s);
        }

        return seats.Count;
    }

    public override long Solve2()
    {
        var costs = FindLowestCosts(_walls, _start);
        var seats = FindShortestPathsSeats(costs, _end);
        return seats;
    }
}
