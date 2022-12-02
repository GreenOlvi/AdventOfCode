namespace AOC2022.Puzzles;

public class Day02 : CustomBaseDay
{
    private readonly string[] _lines;

    public Day02()
    {
        _lines = ReadLinesFromFile().ToArray();
    }

    public Day02(IEnumerable<string> lines)
    {
        _lines = lines.ToArray();
    }

    private enum Shape
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3,
    }

    private int[][] Results = new[]
    {
        new[] { 3, 6, 0 },
        new[] { 0, 3, 6 },
        new[] { 6, 0, 3 },
    };

    private int Score((Shape, Shape) match) =>
        Results[(int)(match.Item1 - 1)][(int)(match.Item2 - 1)] + (int)match.Item2;

    private static Shape ParseOpponent(string input) =>
        input switch
        {
            "A" => Shape.Rock,
            "B" => Shape.Paper,
            "C" => Shape.Scissors,
            _ => throw new InvalidDataException(),
        };

    private static Shape Strategy1(string input, Shape opponent) =>
        input switch
        {
            "X" => Shape.Rock,
            "Y" => Shape.Paper,
            "Z" => Shape.Scissors,
            _ => throw new InvalidDataException(),
        };

    private static Shape Strategy2(string input, Shape opponent) =>
        input switch
        {
            "X" => opponent switch
            {
                Shape.Rock => Shape.Scissors,
                Shape.Paper => Shape.Rock,
                Shape.Scissors => Shape.Paper,
                _ => throw new InvalidOperationException(),
            },
            "Y" => opponent,
            "Z" => opponent switch
            {
                Shape.Rock => Shape.Paper,
                Shape.Paper => Shape.Scissors,
                Shape.Scissors => Shape.Rock,
                _ => throw new InvalidOperationException(),
            },
            _ => throw new InvalidDataException(),
        };

    private static (Shape, Shape) ParseMatch(string line, Func<string, Shape, Shape> strategy)
    {
        var a = line.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var o = ParseOpponent(a[0]);
        return (o, strategy(a[1], o));
    }

    private static IEnumerable<(Shape, Shape)> ParseInput(IEnumerable<string> lines, Func<string, Shape, Shape> strategy) =>
        lines.Select(line => ParseMatch(line, strategy));

    private int SumPoints(IEnumerable<(Shape, Shape)> matches) => matches.Select(Score).Sum();

    public override ValueTask<string> Solve_1() =>
        SumPoints(ParseInput(_lines, Strategy1)).ToResult();

    public override ValueTask<string> Solve_2() =>
        SumPoints(ParseInput(_lines, Strategy2)).ToResult();
}
