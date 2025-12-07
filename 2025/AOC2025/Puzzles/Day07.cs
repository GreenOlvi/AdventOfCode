namespace AOC2025.Puzzles;

public class Day07 : CustomBaseProblem<long>
{
    private readonly HashGrid<Tile> _grid;

    public Day07()
    {
        _grid = ParseInput(ReadLinesFromFile());
    }

    public Day07(IEnumerable<string> lines)
    {
        _grid = ParseInput(lines);
    }

    private static HashGrid<Tile> ParseInput(IEnumerable<string> lines)
    {
        var grid = new HashGrid<Tile>();
        var y = 0;
        foreach (var line in lines)
        {
            var x = 0;
            foreach (var c in line)
            {
                if (c != '.')
                {
                    var p = new Point2(x, y);
                    var t = c switch
                    {
                        'S' => Tile.Start,
                        '^' => Tile.Splitter,
                        _ => throw new InvalidDataException(),
                    };
                    grid[p] = t;
                }
                x++;
            }
            y++;
        }
        return grid;
    }

    public override long Solve1()
    {
        var grid = _grid.Clone();

        var start = grid.First(t => t.Tile == Tile.Start).Position;
        var bottom = grid.BottomRight.Y + 2;

        var beamQueue = new Queue<Point2>();
        beamQueue.Enqueue(start);

        var totalSplits = 0;

        while (beamQueue.TryDequeue(out var beam))
        {
            var next = beam + Point2.Down;
            if (next.Y == bottom)
            {
                continue;
            }

            var nextTile = grid[next];
            if (nextTile == Tile.Beam)
            {
                continue;
            }

            if (nextTile == Tile.Empty)
            {
                beamQueue.Enqueue(next);
                grid[next] = Tile.Beam;
                continue;
            }

            if (nextTile == Tile.Splitter)
            {
                totalSplits++;
                var left = next + Point2.Left;
                if (grid[left] == Tile.Empty)
                {
                    beamQueue.Enqueue(left);
                    grid[left] = Tile.Beam;
                }

                var right = next + Point2.Right;
                if (grid[right] == Tile.Empty)
                {
                    beamQueue.Enqueue(right);
                    grid[right] = Tile.Beam;
                }
            }
        }

        // grid.Print(TileToPrint);

        return totalSplits;
    }

    public override long Solve2()
    {
        var grid = _grid.Clone();

        var start = grid.First(t => t.Tile == Tile.Start).Position;
        var bottom = grid.BottomRight.Y + 2;

        var beamQueue = new Queue<(Point2 Position, long Count)>([(start, 1)]);

        for (var i = start.Y + 1; i < bottom; i++)
        {
            beamQueue = ProcessLine(grid, beamQueue);
        }

        return beamQueue.Sum(b => b.Count);
    }

    private static Queue<(Point2 Position, long Count)> ProcessLine(HashGrid<Tile> grid, Queue<(Point2 Position, long Count)> beamQueue)
    {
        var nextLine = new Dictionary<Point2, long>();
        void nextLineAdd(Point2 position, long count)
        {
            if (nextLine.TryGetValue(position, out var existing))
            {
                nextLine[position] = existing + count;
            }
            else
            {
                nextLine[position] = count;
            }
        }

        while (beamQueue.TryDequeue(out var beam))
        {
            var next = beam.Position + Point2.Down;
            var nextTile = grid[next];

            if (nextTile == Tile.Empty || nextTile == Tile.Beam)
            {
                nextLineAdd(next, beam.Count);
                grid[next] = Tile.Beam;
                continue;
            }

            if (nextTile == Tile.Splitter)
            {
                var left = next + Point2.Left;
                nextLineAdd(left, beam.Count);
                grid[left] = Tile.Beam;

                var right = next + Point2.Right;
                nextLineAdd(right, beam.Count);
                grid[right] = Tile.Beam;
            }
        }

        return new Queue<(Point2, long)>(nextLine.Keys
                .OrderBy(p => p.X)
                .Select(p => (p, nextLine[p])));
    }

    private static (ConsoleColor?, string) TileToPrint(Point2 _, Tile t) => t switch
    {
        Tile.Empty => (ConsoleColor.DarkGray, "."),
        Tile.Start => (ConsoleColor.White, "S"),
        Tile.Splitter => (ConsoleColor.White, "^"),
        Tile.Beam => (ConsoleColor.DarkGreen, "|"),
        _ => (null, "?"),
    };

    private enum Tile
    {
        Empty = 0,
        Start,
        Splitter,
        Beam,
    }
}
