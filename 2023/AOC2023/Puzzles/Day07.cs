namespace AOC2023.Puzzles;
public class Day07 : CustomBaseDay
{
    private readonly HandBid[] _handBids;

    public Day07()
    {
        _handBids = ReadLinesFromFile().Select(ParseHandBid).ToArray();
    }

    public Day07(IEnumerable<string> lines)
    {
        _handBids = lines.Select(ParseHandBid).ToArray();
    }

    private HandBid ParseHandBid(string line)
    {
        var l = line.Split(' ', StringSplitOptions.TrimEntries);
        return new HandBid(l[0], l[1].ToInt());
    }

    private static HandType GetHandType(string hand)
    {
        var g = hand.GroupBy(c => c)
            .Select(g => (Card: g.Key, Count: g.Count()))
            .OrderByDescending(p => p.Count)
            .ToArray();

        var first = g[0].Count;
        var second = g.Length > 1 ? g[1].Count : 0;

        return (first, second) switch
        {
            (5, _) => HandType.FiveOfAKind,
            (4, _) => HandType.FourOfAKind,
            (3, 2) => HandType.FullHouse,
            (3, _) => HandType.ThreeOfAKind,
            (2, 2) => HandType.TwoPair,
            (2, _) => HandType.OnePair,
            (1, _) => HandType.HighCard,
            _ => throw new InvalidDataException(hand),
        };
    }

    private static HandType GetHandType2(string hand)
    {
        var g = hand.Where(c => c != 'J').GroupBy(c => c)
            .Select(g => (Card: g.Key, Count: g.Count()))
            .OrderByDescending(p => p.Count)
            .ToArray();

        var jokers = hand.Count(c => c == 'J');

        var first = g.Length > 0 ? g[0].Count : 0;
        var second = g.Length > 1 ? g[1].Count : 0;

        return (first, second, jokers) switch
        {
            (var f, _, var j) when f + j == 5 => HandType.FiveOfAKind,
            (var f, _, var j) when f + j == 4 => HandType.FourOfAKind,

            (3, 2, _) => HandType.FullHouse,
            (3, _, _) => HandType.ThreeOfAKind,

            (2, 2, 1) => HandType.FullHouse,
            (2, _, 1) => HandType.ThreeOfAKind,
            (2, 2, _) => HandType.TwoPair,
            (2, _, _) => HandType.OnePair,

            (1, _, 2) => HandType.ThreeOfAKind,
            (1, _, 1) => HandType.OnePair,
            (1, _, _) => HandType.HighCard,

            _ => throw new InvalidDataException(hand),
        };
    }

    private static int GetCardRank1(char card) => card switch
    {
        '2' => 2,
        '3' => 3,
        '4' => 4,
        '5' => 5,
        '6' => 6,
        '7' => 7,
        '8' => 8,
        '9' => 9,
        'T' => 10,
        'J' => 11,
        'Q' => 12,
        'K' => 13,
        'A' => 14,
        _ => throw new InvalidDataException(card.ToString()),
    };

    private static long GetHandRank1(string hand)
    {
        var rank = (long)GetHandType(hand);
        for (int i = 0; i < 5; i++)
        {
            rank = rank * 16 + GetCardRank1(hand[i]);
        }
        return rank;
    }

    private static int GetCardRank2(char card) => card switch
    {
        'J' => 1,
        '2' => 2,
        '3' => 3,
        '4' => 4,
        '5' => 5,
        '6' => 6,
        '7' => 7,
        '8' => 8,
        '9' => 9,
        'T' => 10,
        'Q' => 12,
        'K' => 13,
        'A' => 14,
        _ => throw new InvalidDataException(card.ToString()),
    };

    private static long GetHandRank2(string hand)
    {
        var rank = (long)GetHandType2(hand);
        for (int i = 0; i < 5; i++)
        {
            rank = rank * 16 + GetCardRank2(hand[i]);
        }
        return rank;
    }

    private long GetWinnings(Func<string, long> getHandRank) =>
        _handBids.Select(hb => (getHandRank(hb.Hand), hb.Bid))
            .OrderBy(p => p.Item1)
            .Select((p, i) => (long)(p.Bid * (i + 1)))
            .Sum();

    public override ValueTask<string> Solve_1() => GetWinnings(GetHandRank1).ToResult();

    public override ValueTask<string> Solve_2() => GetWinnings(GetHandRank2).ToResult();

    private readonly record struct HandBid(string Hand, int Bid);

    private enum HandType : byte
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind,
    };
}
