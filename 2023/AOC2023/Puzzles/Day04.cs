using AOC2022.Common;

namespace AOC2023.Puzzles;
public partial class Day04 : CustomBaseDay
{
    private readonly Card[] _cards;

    public Day04()
    {
        _cards = ReadLinesFromFile().Select(ParseLine).OrderBy(c => c.Id).ToArray();
    }

    public Day04(IEnumerable<string> lines)
    {
        _cards = lines.Select(ParseLine).OrderBy(c => c.Id).ToArray();
    }

    private static readonly Regex CardMatch = BuildCardMatch();
    private static Card ParseLine(string line)
    {
        if (!CardMatch.TryMatch(line, out var match))
        {
            throw new InvalidDataException(line);
        }

        var id = int.Parse(match.Groups["id"].Value);
        var winning = match.Groups["winning"].Value
            .Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .ToHashSet();

        var my = match.Groups["my"].Value
            .Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse);

        var won = my.Count(winning.Contains);

        return new Card(id, won);
    }

    public override ValueTask<string> Solve_1() =>
        _cards.Sum(c => c.WinCount > 0 ? 1L << (c.WinCount - 1) : 0).ToResult();

    public override ValueTask<string> Solve_2()
    {
        var cardCount = _cards.Select(c => (c.Id, 1L)).ToDictionary();
        var maxId = _cards.Last().Id;
        var sum = 0L;

        foreach (var card in _cards)
        {
            var count = cardCount[card.Id];
            sum += count;
            for (var i = card.Id + 1; i <= maxId && i <= card.Id + card.WinCount; i++)
            {
                cardCount[i] += count;
            }
        }

        return sum.ToResult();
    }

    private readonly record struct Card(int Id, int WinCount);

    [GeneratedRegex(@"^Card\s+(?<id>\d+):\s+(?<winning>.+)\s+\|\s+(?<my>.+)$")]
    private static partial Regex BuildCardMatch();
}
