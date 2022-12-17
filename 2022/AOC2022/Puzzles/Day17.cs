using System.Runtime.CompilerServices;

namespace AOC2022.Puzzles;

public class Day17 : CustomBaseDay
{
    private readonly Direction[] _inputs;

    public Day17()
    {
        _inputs = ParseInput(ReadLinesFromFile()).ToArray();
    }

    public Day17(IEnumerable<string> lines)
    {
        _inputs = ParseInput(lines).ToArray();
    }

    private static IEnumerable<Direction> ParseInput(IEnumerable<string> lines)
    {
        var line = lines.Single();
        foreach (var ch in line)
        {
            yield return ch switch
            {
                '<' => Direction.Left,
                '>' => Direction.Right,
                _ => throw new InvalidDataException(),
            };
        }
    }

    private static readonly Point2[] RockFlat = new[] { new Point2(0, 0), new Point2(1, 0), new Point2(2, 0), new Point2(3, 0) };
    private static readonly Point2[] RockPlus = new[] { new Point2(0, 1), new Point2(1, 0), new Point2(1, 1), new Point2(1, 2), new Point2(2, 1) };
    private static readonly Point2[] RockL = new[] { new Point2(0, 0), new Point2(1, 0), new Point2(2, 0), new Point2(2, 1), new Point2(2, 2) };
    private static readonly Point2[] RockTallBoi = new[] { new Point2(0, 0), new Point2(0, 1), new Point2(0, 2), new Point2(0, 3) };
    private static readonly Point2[] RockChonk = new[] { new Point2(0, 0), new Point2(0, 1), new Point2(1, 0), new Point2(1, 1) };

    private static readonly RockType[] Rocks = Enum.GetValues<RockType>();

    private enum RockType
    {
        Flat,
        Plus,
        L,
        TallBoi,
        Chonk,
    }

    private static readonly Dictionary<RockType, Point2[]> RockPoints = new()
    {
        [RockType.Flat] = RockFlat,
        [RockType.Plus] = RockPlus,
        [RockType.L] = RockL,
        [RockType.TallBoi] = RockTallBoi,
        [RockType.Chonk] = RockChonk,
    };

    private static readonly Dictionary<RockType, int> RockWidth = new()
    {
        [RockType.Flat] = 4,
        [RockType.Plus] = 3,
        [RockType.L] = 3,
        [RockType.TallBoi] = 1,
        [RockType.Chonk] = 2,
    };

    private static readonly Dictionary<RockType, Point2[]> LeftFeelers = new()
    {
        [RockType.Flat] = new[] { new Point2(-1, 0) },
        [RockType.Plus] = new[] { new Point2(-1, 1), new Point2(0, 0), new Point2(0, 2) },
        [RockType.L] = new[] { new Point2(-1, 0), new Point2(1, 1), new Point2(1, 2) },
        [RockType.TallBoi] = new[] { new Point2(-1, 0), new Point2(-1, 1), new Point2(-1, 2), new Point2(-1, 3) },
        [RockType.Chonk] = new[] { new Point2(-1, 0), new Point2(-1, 1) },
    };

    private static readonly Dictionary<RockType, Point2[]> RightFeelers = new()
    {
        [RockType.Flat] = new[] { new Point2(4, 0) },
        [RockType.Plus] = new[] { new Point2(3, 1), new Point2(2, 0), new Point2(2, 2) },
        [RockType.L] = new[] { new Point2(3, 0), new Point2(3, 1), new Point2(3, 2) },
        [RockType.TallBoi] = new[] { new Point2(1, 0), new Point2(1, 1), new Point2(1, 2), new Point2(1, 3) },
        [RockType.Chonk] = new[] { new Point2(2, 0), new Point2(2, 1) },
    };

    private static readonly Dictionary<RockType, Point2[]> BottomFeelers = new()
    {
        [RockType.Flat] = new[] { new Point2(0, -1), new Point2(1, -1), new Point2(2, -1), new Point2(3, -1) },
        [RockType.Plus] = new[] { new Point2(1, -1), new Point2(0, 0), new Point2(2, 0) },
        [RockType.L] = new[] { new Point2(0, -1), new Point2(1, -1), new Point2(2, -1) },
        [RockType.TallBoi] = new[] { new Point2(0, -1) },
        [RockType.Chonk] = new[] { new Point2(0, -1), new Point2(1, -1) },
    };

    private static LimitedHashGrid<Tile> NewGrid()
    {
        var grid = new LimitedHashGrid<Tile>(TileChars, 1000, true);

        for (var x = 1; x < 8; x++)
        {
            grid[(x, 0)] = Tile.Floor;
        }
        grid[(0, 0)] = Tile.BorderCorner;
        grid[(8, 0)] = Tile.BorderCorner;

        return grid;
    }

    private static void Solidify(HashGrid<Tile> grid, Point2[] rock, Point2 position)
    {
        foreach (var r in rock)
        {
            grid[position + r] = Tile.Rock;
        }
    }

    private static void SimulateRock(LimitedHashGrid<Tile> grid, CyclicQueue<RockType> rocks, CyclicQueue<Direction> inputs)
    {
        var starting = new Point2(3, grid.MaxY + 4);
        var rock = rocks.Dequeue();

        var rockPoints = RockPoints[rock];
        var width = RockWidth[rock];
        var pos = starting;

        var rightFeelers = RightFeelers[rock];
        var leftFeelers = LeftFeelers[rock];
        var downFeelers = BottomFeelers[rock];

        while (true)
        {
            var dir = inputs.Dequeue();
            if (dir == Direction.Right)
            {
                if (pos.X + width < 8 && rightFeelers.All(f => grid[pos + f] == Tile.Air))
                {
                    pos = pos.Move(dir);
                }
            }
            else
            {
                if (pos.X > 1 && leftFeelers.All(f => grid[pos + f] == Tile.Air))
                {
                    pos = pos.Move(dir);
                }
            }

            if (pos.Y > 1 && downFeelers.All(f => grid[pos + f] == Tile.Air))
            {
                pos = new Point2(pos.X, pos.Y - 1);
            }
            else
            {
                break;
            }
        }
        Solidify(grid, rockPoints, pos);
        grid.Cleanup();
    }

    private static int[] GetProfile(HashGrid<Tile> grid)
    {
        var maxY = grid.MaxY;
        var profile = new int[7];
        for (var i = 0; i < 7; i++)
        {
            var y = 0;
            while (grid[(i + 1, maxY - y)] == Tile.Air && y < 1000)
            {
                y++;
            }
            profile[i] = y;
        }

        return profile;
    }

    public override ValueTask<string> Solve_1()
    {
        var inputs = new CyclicQueue<Direction>(_inputs);
        var rocks = new CyclicQueue<RockType>(Rocks);
        var grid = NewGrid();

        for (var i = 0; i < 2022; i++)
        {
            SimulateRock(grid, rocks, inputs);
        }

        return grid.MaxY.ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        var inputs = new CyclicQueue<Direction>(_inputs);
        var rocks = new CyclicQueue<RockType>(Rocks);
        var grid = NewGrid();

        var states = new Dictionary<StateKey, List<(long Height, long i)>>();

        const long iterations = 1_000_000_000_000;
        StateKey? repeat = null;

        const int repeatsUntilCertain = 2;

        for (var i = 0L; i < iterations; i++)
        {
            SimulateRock(grid, rocks, inputs);

            var state = new StateKey(GetProfile(grid), inputs.Index, rocks.Peek());
            if (!states.TryGetValue(state, out var val))
            {
                states[state] = new List<(long Height, long i)> { (grid.MaxY, i) };
            }
            else
            {
                val.Add((grid.MaxY, i));
                if (val.Count == repeatsUntilCertain)
                {
                    repeat = state;
                    break;
                }
            }
        }

        var prev = states[repeat!.Value][repeatsUntilCertain - 2];
        var last = states[repeat.Value][repeatsUntilCertain - 1];
        var heightDiff = last.Height - prev.Height;
        var iterDiff = last.i - prev.i;

        var repeats = (iterations - last.i) / iterDiff;
        var rest = iterations - (repeats * iterDiff) - last.i - 1;

        for (var i = 0; i < rest; i++)
        {
            SimulateRock(grid, rocks, inputs);
        }

        long height = repeats * heightDiff + grid.MaxY;
        return height.ToResult();
    }

    private readonly record struct StateKey
    {
        public readonly (int, int, int, int, int, int, int) Profile;
        public readonly int Direction;
        public readonly RockType Rock;

        public StateKey(int[] profile, int direction, RockType rock)
        {
            Profile = (profile[0], profile[1], profile[2], profile[3], profile[4], profile[5], profile[6]);
            Direction = direction;
            Rock = rock;
        }
    }

    private enum Tile
    {
        Air,
        Rock,
        Border,
        Floor,
        BorderCorner,
    }

    private static readonly Dictionary<Tile, char> TileChars = new()
    {
        [Tile.Air] = ' ',
        [Tile.Rock] = '#',
        [Tile.Border] = '|',
        [Tile.Floor] = '-',
        [Tile.BorderCorner] = '+',
    };

    private record CyclicQueue<T>
    {
        private readonly T[] _elements;
        private int _i;

        public CyclicQueue(IEnumerable<T> elements)
        {
            _elements = elements as T[] ?? elements.ToArray();
        }

        public T Dequeue()
        {
            var el = _elements[_i];
            _i = (_i + 1) % _elements.Length;
            return el;
        }

        public T Peek() => _elements[_i];

        public int Index => _i;
    }

    private class LimitedHashGrid<T> : HashGrid<T> where T : struct
    {
        private readonly int _maxTiles;
        public LimitedHashGrid(IDictionary<T, char> chars, int maxPoints, bool yInverted = false) : base(chars, yInverted)
        {
            _maxTiles = maxPoints;
        }

        public void Cleanup()
        {
            if (_tiles.Count > _maxTiles)
            {
                var toRemove = _tiles.Keys.OrderBy(p => p.Y).Take(_tiles.Count - _maxTiles);
                foreach (var tile in toRemove)
                {
                    _tiles.Remove(tile);
                }
            }
        }
    }
}
