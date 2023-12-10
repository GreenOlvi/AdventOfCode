namespace AOC2023.Puzzles;
public class Day10 : CustomBaseDay
{
    private readonly HashGrid<Pipe> _pipes;

    public Day10()
    {
        _pipes = ParsePipes(ReadLinesFromFile());
    }

    public Day10(IEnumerable<string> lines)
    {
        _pipes = ParsePipes(lines);
    }

    private static HashGrid<Pipe> ParsePipes(IEnumerable<string> lines)
    {
        var grid = new HashGrid<Pipe>();
        var y = 0;
        foreach (var line in lines)
        {
            var x = 0;
            foreach (var c in line)
            {
                var pipe = c switch
                {
                    '.' => Pipe.None,
                    '|' => Pipe.Vertical,
                    '-' => Pipe.Horizontal,
                    'L' => Pipe.NE,
                    'J' => Pipe.NW,
                    '7' => Pipe.SW,
                    'F' => Pipe.SE,
                    'S' => Pipe.Start,
                    _ => Pipe.None,
                };

                if (pipe != Pipe.None)
                {
                    grid[(x, y)] = pipe;
                }

                x++;
            }
            y++;
        }
        return grid;
    }

    private static IEnumerable<Direction> FindConnectedNeighbours(HashGrid<Pipe> pipes, Point2 point)
    {
        var l = pipes[point.Move(Direction.Left)];
        if (l == Pipe.Horizontal || l == Pipe.NE || l == Pipe.SE)
        {
            yield return Direction.Left;
        }

        var r = pipes[point.Move(Direction.Right)];
        if (r == Pipe.Horizontal || r == Pipe.NW || r == Pipe.SW)
        {
            yield return Direction.Right;
        }

        var u = pipes[point.Move(Direction.Up)];
        if (u == Pipe.Vertical || u == Pipe.SW || u == Pipe.SE)
        {
            yield return Direction.Up;
        }

        var d = pipes[point.Move(Direction.Down)];
        if (d == Pipe.Vertical || d == Pipe.NW || d == Pipe.NE)
        {
            yield return Direction.Down;
        }
    }

    private static Pipe FindPipeForStart(HashGrid<Pipe> pipes, Point2 start)
    {
        var neighbours = FindConnectedNeighbours(pipes, start);
        var r = 0;
        foreach (var n in neighbours)
        {
            r |= n switch
            {
                Direction.Left => 1,
                Direction.Right => 2,
                Direction.Up => 4,
                Direction.Down => 8,
                _ => throw new InvalidOperationException(),
            };
        }

        return r switch
        {
            3 => Pipe.Horizontal,
            5 => Pipe.NW,
            6 => Pipe.NE,
            9 => Pipe.SW,
            10 => Pipe.SE,
            12 => Pipe.Vertical,
            _ => throw new InvalidOperationException("Invalid pipe configuration"),
        };
    }

    private static IEnumerable<Point2> FindLoop(HashGrid<Pipe> pipes, Point2 start)
    {
        var dirs = FindConnectedNeighbours(pipes, start).ToArray();
        if (dirs.Length != 2)
        {
            throw new InvalidOperationException("More than 2 directions from start");
        }

        var direction = dirs[0];
        Point2 curr = start;

        while (direction != Direction.None)
        {
            curr = curr.Move(direction);
            direction = (pipes[curr], direction) switch
            {
                (Pipe.Horizontal, Direction.Left) => Direction.Left,
                (Pipe.Horizontal, Direction.Right) => Direction.Right,

                (Pipe.Vertical, Direction.Up) => Direction.Up,
                (Pipe.Vertical, Direction.Down) => Direction.Down,

                (Pipe.NE, Direction.Down) => Direction.Right,
                (Pipe.NE, Direction.Left) => Direction.Up,

                (Pipe.NW, Direction.Down) => Direction.Left,
                (Pipe.NW, Direction.Right) => Direction.Up,

                (Pipe.SE, Direction.Up) => Direction.Right,
                (Pipe.SE, Direction.Left) => Direction.Down,

                (Pipe.SW, Direction.Up) => Direction.Left,
                (Pipe.SW, Direction.Right) => Direction.Down,

                (Pipe.Start, _) => Direction.None,

                _ => throw new InvalidOperationException("Invalid pipe"),
            };

            yield return curr;
        }
    }

    private static int CountInsides(HashGrid<Pipe> pipes)
    {
        var start = pipes.Where(p => p.Item2 == Pipe.Start).First().Item1;
        var loop = FindLoop(pipes, start).ToHashSet();

        var s = FindPipeForStart(pipes, start);

        var box = pipes.GetSurroundingBox();
        var leftOut = pipes.MinX - 1;

        var inside = 0;
        foreach (var p in box.GetPoints().Where(p => !loop.Contains(p)))
        {
            var crossed = 0;
            var c = p;
            var lastBend = Pipe.None;
            for (var i = p.X; i > leftOut; i--)
            {
                c = c.Move(Direction.Left);
                if (loop.Contains(c))
                {
                    var pipe = pipes[c];
                    if (pipe == Pipe.Start)
                    {
                        pipe = s;
                    }

                    if (pipe == Pipe.Vertical)
                    {
                        crossed++;
                    }

                    if (lastBend == Pipe.NW)
                    {
                        if (pipe == Pipe.SE)
                        {
                            lastBend = Pipe.None;
                            crossed++;
                        }
                        else if (pipe == Pipe.NE)
                        {
                            lastBend = Pipe.None;
                        }
                        else if (pipe != Pipe.Horizontal)
                        {
                        }
                    }

                    if (lastBend == Pipe.SW)
                    {
                        if (pipe == Pipe.NE)
                        {
                            lastBend = Pipe.None;
                            crossed++;
                        }
                        else if (pipe == Pipe.SE)
                        {
                            lastBend = Pipe.None;
                        }
                        else if (pipe != Pipe.Horizontal)
                        {
                        }
                    }

                    if (pipe == Pipe.NW || pipe == Pipe.SW)
                    {
                        if (lastBend == Pipe.None)
                        {
                            lastBend = pipe;
                        }
                        else
                        {
                        }
                    }
                }
            }

            if (crossed % 2 == 1)
            {
                inside++;
            }
        }

        return inside;
    }

    public override ValueTask<string> Solve_1()
    {
        var start = _pipes.Where(p => p.Item2 == Pipe.Start).First().Item1;
        var loopLength = FindLoop(_pipes, start).Count();
        return (loopLength / 2).ToResult();
    }

    public override ValueTask<string> Solve_2() => CountInsides(_pipes).ToResult();

    private enum Pipe
    {
        None,
        Vertical,
        Horizontal,
        NE,
        NW,
        SW,
        SE,
        Start,
    };

#pragma warning disable IDE0052 // Remove unread private members
    private static readonly Dictionary<Pipe, char> _pipeChars = new()
    {
        [Pipe.None] = ' ',
        [Pipe.Vertical] = '║',
        [Pipe.Horizontal] = '═',
        [Pipe.NE] = '╚',
        [Pipe.NW] = '╝',
        [Pipe.SW] = '╗',
        [Pipe.SE] = '╔',
        [Pipe.Start] = 'X',
    };
#pragma warning restore IDE0052 // Remove unread private members
}
