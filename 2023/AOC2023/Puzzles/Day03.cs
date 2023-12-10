namespace AOC2023.Puzzles;
public class Day03 : CustomBaseDay
{
    private readonly Schematic _schematic;

    public Day03()
    {
        _schematic = ParseSchematic(ReadLinesFromFile());
    }

    public Day03(IEnumerable<string> lines)
    {
        _schematic = ParseSchematic(lines);
    }

    private static bool IsSymbol(char c) => (c < '0' || c > '9') && c != '\0' && c != '.';

    private static bool IsNumber(char c) => char.IsAsciiDigit(c);

    private static Schematic ParseSchematic(IEnumerable<string> lines)
    {
        var numbers = new List<Number>();
        var symbols = new List<Symbol>();

        var y = 0;
        foreach (var line in lines)
        {
            var x = 0;
            while (x < line.Length)
            {
                var c = line[x];
                if (IsNumber(c))
                {
                    var n = 0;
                    var xp = x;
                    do
                    {
                        n = n * 10 + line[xp] - '0';
                        xp++;
                    }
                    while(xp < line.Length && IsNumber(line[xp]));

                    numbers.Add(new Number(n, new Point2(x, y), new Point2(xp - 1, y)));
                    x = xp - 1;
                }
                else if (IsSymbol(c))
                {
                    symbols.Add(new Symbol(c, new Point2(x, y)));
                }
                x++;
            }
            y++;
        }

        var numberAdjacency = new Dictionary<Number, List<Symbol>>();
        var symbolAdjacency = new Dictionary<Symbol, List<Number>>();

        var symbolPositions = symbols.ToDictionary(s => s.Position);

        foreach (var number in numbers)
        {
            var adjacentPoints = new Box(number.Start + new Point2(-1, -1), number.End + new Point2(1, 1)).GetPoints();
            var adjacentSymbols = adjacentPoints.Where(symbolPositions.ContainsKey).Select(p => symbolPositions[p]).ToList();
            foreach (var symbol in adjacentSymbols)
            {
                numberAdjacency[number] = adjacentSymbols;
                if (symbolAdjacency.TryGetValue(symbol, out List<Number>? value))
                {
                    value.Add(number);
                }
                else
                {
                    symbolAdjacency[symbol] = [number];
                }
            }
        }

        return new Schematic(numbers, symbols, numberAdjacency, symbolAdjacency);
    }

    public override ValueTask<string> Solve_1() =>
        _schematic.NumberAdjacency
            .Where(kv => kv.Value.Count > 0)
            .Select(kv => kv.Key.Value)
            .Sum()
            .ToResult();

    public override ValueTask<string> Solve_2() =>
        _schematic.SymbolAdjacency
            .Where(kv => kv.Key.Type == '*' && kv.Value.Count == 2)
            .Sum(kv => kv.Value.Product(n => n.Value))
            .ToResult();

    private record Schematic(IReadOnlyList<Number> Numbers, IReadOnlyList<Symbol> Symbols,
        Dictionary<Number, List<Symbol>> NumberAdjacency, Dictionary<Symbol, List<Number>> SymbolAdjacency);

    private readonly record struct Number(int Value, Point2 Start, Point2 End);
    private readonly record struct Symbol(char Type, Point2 Position);
}
