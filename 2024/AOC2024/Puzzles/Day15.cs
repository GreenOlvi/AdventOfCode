
namespace AOC2024.Puzzles;

public class Day15 : CustomBaseProblem<long>
{
    private readonly IHashGrid2<bool> _walls;
    private readonly HashGrid<bool> _boxes;
    private readonly Point2 _start;
    private readonly Direction[] _moves;

    public Day15()
    {
        (_walls, _boxes, _start, _moves) = ParseInput(ReadLinesFromFile());
    }

    public Day15(IEnumerable<string> lines)
    {
        (_walls, _boxes, _start, _moves) = ParseInput(lines);
    }

    private (IHashGrid2<bool> _walls, HashGrid<bool> _boxes, Point2 _start, Direction[] _moves) ParseInput(IEnumerable<string> lines)
    {
        var walls = new HashGrid<bool>(true);
        var boxes = new HashGrid<bool>();
        var start = new Point2();
        var y = 0;
        foreach (var line in lines.TakeWhile(static l => !string.IsNullOrWhiteSpace(l)))
        {
            var x = 0;
            foreach (var c in line)
            {
                if (c == '.')
                {
                    walls[(x, y)] = false;
                }
                else if (c == 'O')
                {
                    boxes[(x, y)] = true;
                    walls[(x, y)] = false;
                }
                else if (c == '@')
                {
                    start = new Point2(x, y);
                    walls[(x, y)] = false;
                }

                x++;
            }
            y++;
        }

        var moves = new List<Direction>();
        foreach (var line in lines.Skip(y + 1))
        {
            foreach (var c in line)
            {
                var d = c switch
                {
                    '^' => Direction.Up,
                    'v' => Direction.Down,
                    '>' => Direction.Right,
                    '<' => Direction.Left,
                    _ => Direction.None,
                };
                if (d != Direction.None)
                {
                    moves.Add(d);
                }
            }
        }

        var wText = walls.Draw();
        var bText = boxes.Draw();

        return (walls.ToFrozen(), boxes, start, moves.ToArray());
    }

    private static (Point2 p, HashGrid<bool> boxes) Move(Point2 p, Direction d, HashGrid<bool> boxes, IHashGrid2<bool> walls)
    {
        if (!CanMove(p, d, boxes, walls))
        {
            return (p, boxes);
        }

        var np = p.Move(d);
        if (boxes[np])
        {
            var bp = np;
            while (boxes[bp])
            {
                bp = bp.Move(d);
            }
            boxes[np] = false;
            boxes[bp] = true;
        }

        return (np, boxes);
    }

    private static bool CanMove(Point2 p, Direction d, HashGrid<bool> boxes, IHashGrid2<bool> walls)
    {
        var np = p;
        while (true)
        {
            np = np.Move(d);
            if (walls[np]) // wall
            {
                return false;
            }

            if (!boxes[np]) // no wall & no box
            {
                return true;
            }

            // else become a box
        }
    }

    private static (IHashGrid2<bool> Walls, HashGrid<Tiles> Boxes, Point2 Start) Widen(IHashGrid2<bool> walls, HashGrid<bool> boxes, Point2 start)
    {
        var wideWalls = new HashGrid<bool>(true);
        foreach (var (pos, wall) in walls)
        {
            var newX = pos.X * 2;
            wideWalls[(newX, pos.Y)] = wall;
            wideWalls[(newX + 1, pos.Y)] = wall;
        }

        var wideBoxes = new HashGrid<Tiles>();
        foreach (var (pos, _) in boxes)
        {
            var newX = pos.X * 2;
            wideBoxes[(newX, pos.Y)] = Tiles.BoxLeftSide;
            wideBoxes[(newX + 1, pos.Y)] = Tiles.BoxRightSide;
        }

        return (wideWalls.ToFrozen(), wideBoxes, new Point2(start.X * 2, start.Y));
    }


    private static (Point2 p, HashGrid<Tiles> boxes) MoveWide(Point2 p, Direction d, HashGrid<Tiles> boxes, IHashGrid2<bool> walls)
    {
        if (!CanMoveWide(p, d, boxes, walls))
        {
            return (p, boxes);
        }

        var np = p.Move(d);

        if (boxes[np] != Tiles.None)
        {
            if (d is Direction.Left)
            {
                MoveWideBoxLeft(np, boxes);
            }
            else if (d is Direction.Right)
            {
                MoveWideBoxRight(np, boxes);
            }
            else
            {
                MoveWideBoxVertically(np, d, boxes);
            }
        }

        return (np, boxes);
    }

    private static void MoveWideBoxLeft(Point2 p, HashGrid<Tiles> boxes)
    {
        var bp = boxes[p] == Tiles.BoxRightSide ? p.Add(Point2.Left) : p;   // align to left side of the box
        var newPos = bp.Add(Point2.Left);

        if (boxes[newPos] != Tiles.None)
        {
            MoveWideBoxLeft(newPos, boxes);
        }

        boxes[newPos] = Tiles.BoxLeftSide;
        boxes[bp] = Tiles.BoxRightSide;
        boxes[bp.Add(Point2.Right)] = Tiles.None;
    }

    private static void MoveWideBoxRight(Point2 p, HashGrid<Tiles> boxes)
    {
        var bp = boxes[p] == Tiles.BoxRightSide ? p.Add(Point2.Left) : p;   // align to left side of the box
        var newPos = bp.Add(Point2.Right * 2);

        if (boxes[newPos] != Tiles.None)
        {
            MoveWideBoxRight(newPos, boxes);
        }

        boxes[newPos] = Tiles.BoxRightSide;
        boxes[bp] = Tiles.None;
        boxes[bp.Add(Point2.Right)] = Tiles.BoxLeftSide;
    }

    private static void MoveWideBoxVertically(Point2 p, Direction d, HashGrid<Tiles> boxes)
    {
        var bp = boxes[p] == Tiles.BoxRightSide ? p.Add(Point2.Left) : p;   // align to left side of the box
        var newPos1 = bp.Move(d);
        var newPos2 = bp.Move(d).Move(Direction.Right);

        if (boxes[newPos1] != Tiles.None)
        {
            MoveWideBoxVertically(newPos1, d, boxes);
        }

        if (boxes[newPos2] != Tiles.None)
        {
            MoveWideBoxVertically(newPos2, d, boxes);
        }

        boxes[bp] = Tiles.None;
        boxes[bp + Point2.Right] = Tiles.None;
        boxes[newPos1] = Tiles.BoxLeftSide;
        boxes[newPos2] = Tiles.BoxRightSide;
    }

    private static bool CanMoveWide(Point2 p, Direction d, HashGrid<Tiles> boxes, IHashGrid2<bool> walls)
    {
        var np = p.Move(d);
        if (walls[np]) // wall
        {
            return false;
        }

        if (boxes[np] == Tiles.None) // no wall & no box
        {
            return true;
        }

        return d switch
        {
            Direction.Left => CanMoveWideBoxHorizontally(np, d, boxes, walls),
            Direction.Right => CanMoveWideBoxHorizontally(np, d, boxes, walls),
            Direction.Up => CanMoveWideBoxVertically(np, d, boxes, walls),
            Direction.Down => CanMoveWideBoxVertically(np, d, boxes, walls),
            _ => throw new InvalidOperationException("Invalid direction"),
        };
    }

    private static bool CanMoveWideBoxHorizontally(Point2 p, Direction d, HashGrid<Tiles> boxes, IHashGrid2<bool> walls)
    {
        var bp = boxes[p] == Tiles.BoxRightSide ? p.Add(Point2.Left) : p;   // align to left side of the box
        var newPos = d switch
        {
            Direction.Left => bp.Add(Point2.Left),
            Direction.Right => bp.Add(Point2.Right * 2),
            _ => throw new InvalidOperationException("Invalid direction"),
        };

        if (walls[newPos]) // wall
        {
            return false;
        }

        if (boxes[newPos] == Tiles.None) // no wall & no box
        {
            return true;
        }

        return CanMoveWideBoxHorizontally(newPos, d, boxes, walls); // next box
    }

    private static bool CanMoveWideBoxVertically(Point2 p, Direction d, HashGrid<Tiles> boxes, IHashGrid2<bool> walls)
    {
        var bp = boxes[p] == Tiles.BoxRightSide ? p.Add(Point2.Left) : p;   // align to left side of the box

        var newPos1 = bp.Move(d);
        var newPos2 = bp.Move(d).Move(Direction.Right);

        if (walls[newPos1] || walls[newPos2]) // wall
        {
            return false;
        }

        if (boxes[newPos1] == Tiles.None && boxes[newPos2] == Tiles.None) // no wall & no box
        {
            return true;
        }

        if (!(boxes[newPos1] == Tiles.None || CanMoveWideBoxVertically(newPos1, d, boxes, walls))) // left side can go
        {
            return false;
        }

        return boxes[newPos2] == Tiles.None || CanMoveWideBoxVertically(newPos2, d, boxes, walls); // right side can go
    }

    private static string DrawAll(IHashGrid2<bool> walls, IHashGrid2<bool> boxes, Point2 robot)
    {
        var grid = new HashGrid<Tiles>(Tiles.Wall);

        foreach (var (p, _) in walls)
        {
            grid[p] = Tiles.None;
        }

        foreach (var (p, _) in boxes)
        {
            grid[p] = Tiles.Box;
        }

        grid[robot] = Tiles.Robot;

        return grid.Draw(TilesDraw);
    }

    private static string DrawAllWide(IHashGrid2<bool> walls, IHashGrid2<Tiles> boxes, Point2 robot)
    {
        var grid = new HashGrid<Tiles>(Tiles.Wall);

        foreach (var (p, _) in walls)
        {
            grid[p] = Tiles.None;
        }

        foreach (var (p, t) in boxes)
        {
            grid[p] = t;
        }

        grid[robot] = Tiles.Robot;

        return grid.Draw(TilesDraw);
    }

    public override long Solve1()
    {
        var p = _start;
        var boxes = _boxes.Clone();
        // Console.WriteLine(DrawAll(_walls, boxes, p));

        foreach (var m in _moves)
        {
            (p, boxes) = Move(p, m, boxes, _walls);
            // Console.WriteLine(DrawAll(_walls, boxes, p));
        }

        return boxes.Sum(static b => (b.Position.Y * 100) + b.Position.X);
    }

    public override long Solve2()
    {
        var (walls, boxes, p) = Widen(_walls, _boxes, _start);
        // Console.WriteLine(DrawAllWide(walls, boxes, p));

        foreach (var m in _moves)
        {
            // Console.WriteLine(m);
            (p, boxes) = MoveWide(p, m, boxes, walls);
            // Console.WriteLine(DrawAllWide(walls, boxes, p));
        }

        return boxes.Where(static p => p.Tile == Tiles.BoxLeftSide)
            .Sum(static b => (b.Position.Y * 100) + b.Position.X);
    }

    private static char TilesDraw(Tiles t) => t switch
    {
        Tiles.Wall => '#',
        Tiles.Box => 'O',
        Tiles.BoxLeftSide => '[',
        Tiles.BoxRightSide => ']',
        Tiles.Robot => '@',
        _ => '.',
    };

    private enum Tiles
    {
        None = 0,
        Wall,
        Box,
        BoxLeftSide,
        BoxRightSide,
        Robot,
    }
}
