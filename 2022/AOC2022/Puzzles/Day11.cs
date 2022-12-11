using System.Numerics;

namespace AOC2022.Puzzles;

public partial class Day11 : CustomBaseDay
{
    private readonly string[] _lines;

    public Day11()
    {
        _lines = ReadLinesFromFile().ToArray();
    }

    public Day11(IEnumerable<string> lines)
    {
        _lines = lines.ToArray();
    }

    private static IEnumerable<Monkey> ParseInput(IEnumerable<string> lines) =>
        lines.SplitGroups().Select(ParseMonkey);

    [GeneratedRegex("^Monkey (?<id>\\d+):$", RegexOptions.Compiled)]
    private static partial Regex MakeMonkeyPattern1();
    [GeneratedRegex("^\\s+Starting items: (?<items>.+)$", RegexOptions.Compiled)]
    private static partial Regex MakeMonkeyPattern2();
    [GeneratedRegex("^\\s+Operation: new = old (?<operation>.+)$", RegexOptions.Compiled)]
    private static partial Regex MakeMonkeyPattern3();
    [GeneratedRegex("^\\s+Test: divisible by (?<test>\\d+)$", RegexOptions.Compiled)]
    private static partial Regex MakeMonkeyPattern4();
    [GeneratedRegex("^\\s+If true: throw to monkey (?<true_monkey>\\d+)$", RegexOptions.Compiled)]
    private static partial Regex MakeMonkeyPattern5();
    [GeneratedRegex("^\\s+If false: throw to monkey (?<false_monkey>\\d+)$", RegexOptions.Compiled)]
    private static partial Regex MakeMonkeyPattern6();

    private static readonly Regex[] MonkeyPatterns = new[]
    {
        MakeMonkeyPattern1(),
        MakeMonkeyPattern2(),
        MakeMonkeyPattern3(),
        MakeMonkeyPattern4(),
        MakeMonkeyPattern5(),
        MakeMonkeyPattern6(),
    };

    private static Monkey ParseMonkey(IEnumerable<string> lines)
    {
        var monkeyLines = lines.ToArray();
        var matches = new Match[MonkeyPatterns.Length];
        for (var i = 0; i < MonkeyPatterns.Length; i++)
        {
            if (!MonkeyPatterns[i].TryMatch(monkeyLines[i], out var m))
            {
                throw new InvalidDataException(monkeyLines[i]);
            }
            matches[i] = m;
        }

        var id = int.Parse(matches[0].Groups["id"].Value);
        var items = matches[1].Groups["items"].Value
            .Split(", ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(long.Parse);

        var op = ParseOperation<long>(matches[2].Groups["operation"].Value);

        var tests = int.Parse(matches[3].Groups["test"].Value);
        var trueMonkey = int.Parse(matches[4].Groups["true_monkey"].Value);
        var falseMonkey = int.Parse(matches[5].Groups["false_monkey"].Value);

        return new Monkey()
        {
            Id = id,
            Items = items.ToList(),
            Operation = op,
            TestValue = tests,
            TrueMonkey = trueMonkey,
            FalseMonkey = falseMonkey,
        };
    }

    private static Func<TNumber, TNumber> ParseOperation<TNumber>(string value)
        where TNumber : INumber<TNumber>, IParsable<TNumber>
    {

        var parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts[0] == "*")
        {
            if (parts[1] == "old")
            {
                return x => x * x;
            }
            var b = TNumber.Parse(parts[1], null);
            return x => x * b;
        }
        else if (parts[0] == "+")
        {
            var b = TNumber.Parse(parts[1], null);
            return x => x + b;
        }

        throw new InvalidDataException();
    }

    public override ValueTask<string> Solve_1()
    {
        var monkeys = ParseInput(_lines).ToDictionary(m => m.Id);
        for (var i = 0; i < 20; i++)
        {
            DoRound(monkeys);
        }

        var mostActive = monkeys.Values
            .OrderByDescending(m => m.ItemsInspected)
            .ToArray();
        return (mostActive[0].ItemsInspected * mostActive[1].ItemsInspected).ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        var monkeys = ParseInput(_lines).ToDictionary(m => m.Id);
        var combinedTest = monkeys.Values.Select(m => m.TestValue).Product();
        for (var i = 0; i < 10_000; i++)
        {
            DoRound2(monkeys, combinedTest);
        }

        var mostActive = monkeys.Values
            .OrderByDescending(m => m.ItemsInspected)
            .ToArray();
        return (mostActive[0].ItemsInspected * mostActive[1].ItemsInspected).ToResult();
    }

    private static void DoRound(Dictionary<int, Monkey> monkeys)
    {
        foreach (var monkey in monkeys.Values.OrderBy(m => m.Id))
        {
            foreach (var item in monkey.Items.OrderBy(i => i))
            {
                var newValue = monkey.Operation(item) / 3;
                var throwTo = newValue % monkey.TestValue == 0
                    ? monkey.TrueMonkey
                    : monkey.FalseMonkey;
                monkeys[throwTo].Items.Add(newValue);
                monkey.ItemsInspected++;
            }

            monkey.Items.Clear();
        }
    }

    private static void DoRound2(Dictionary<int, Monkey> monkeys, long combinedTest)
    {
        foreach (var monkey in monkeys.Values.OrderBy(m => m.Id))
        {
            foreach (var item in monkey.Items.OrderBy(i => i))
            {
                var newValue = monkey.Operation(item) % combinedTest;
                var throwTo = newValue % monkey.TestValue == 0
                    ? monkey.TrueMonkey
                    : monkey.FalseMonkey;
                monkeys[throwTo].Items.Add(newValue);
                monkey.ItemsInspected++;
            }

            monkey.Items.Clear();
        }
    }

    public class Monkey
    {
        public int Id { get; init; }
        public List<long> Items { get; init; } = new();
        public Func<long, long> Operation { get; init; } = x => x;
        public int TestValue { get; init; } = 1;
        public int TrueMonkey { get; init; }
        public int FalseMonkey { get; init; }
        public long ItemsInspected { get; set; }
    }
}
