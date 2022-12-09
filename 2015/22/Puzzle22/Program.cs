using Microsoft.VisualStudio.TestTools.UnitTesting;
using Puzzle22;
using System.Text.RegularExpressions;

internal partial class Program
{
    private const string InputFile = "input.txt";

    [GeneratedRegex("Hit Points: (?<hp>\\d+).*Damage: (?<dmg>\\d+)", RegexOptions.Singleline)]
    private static partial Regex MakeInputPattern();

    private static void Main(string[] args)
    {
        Test1();
        Test2();

        var input = File.ReadAllText(InputFile);
        var boss = ParseInput(input);
        var player = new Player(50, 500);

        var result1 = Solve1(player, boss);
        Console.WriteLine($"Result 1 = {result1}");
        Assert.AreEqual(953, result1);

        var result2 = Solve2(player, boss);
        Console.WriteLine($"Result 2 = {result2}");
    }

    private static Boss ParseInput(string input)
    {
        var pattern = MakeInputPattern();
        var match = pattern.Match(input);
        if (!match.Success)
        {
            throw new InvalidDataException();
        }

        var hp = int.Parse(match.Groups["hp"].Value);
        var dmg = int.Parse(match.Groups["dmg"].Value);

        return new Boss(hp, dmg);
    }


    private static void Test1()
    {
        var state = new State(new Player(10, 250), new Boss(13, 8));
        Console.WriteLine(state);

        var step1 = state.MakeAction(PlayerAction.Poison);
        Console.WriteLine(step1);

        var step2 = step1.MakeAction(PlayerAction.MagicMissile);
        Console.WriteLine(step2);

        Assert.IsTrue(step2.IsWin);
    }

    private static void Test2()
    {
        var state = new State(new Player(10, 250), new Boss(14, 8));

        var end = state.MakeAction(PlayerAction.Recharge)
            .MakeAction(PlayerAction.Shield)
            .MakeAction(PlayerAction.Drain)
            .MakeAction(PlayerAction.Poison)
            .MakeAction(PlayerAction.MagicMissile);

        Console.WriteLine(end);

        Assert.IsTrue(end.IsWin);
    }

    private static IEnumerable<PlayerAction> CanDo(State state)
    {
        var mana = state.Player.Mana;
        if (mana < 53)
        {
            yield break;
        }
        yield return PlayerAction.MagicMissile;

        if (mana < 73)
        {
            yield break;
        }
        yield return PlayerAction.Drain;

        if (mana < 113)
        {
            yield break;
        }
        if (state.ShieldTimer == 0)
        {
            yield return PlayerAction.Shield;
        }

        if (mana < 173)
        {
            yield break;
        }
        if (state.PoisonTimer == 0)
        {
            yield return PlayerAction.Poison;
        }

        if (mana < 229)
        {
            yield break;
        }
        if (state.RechargeTimer == 0)
        {
            yield return PlayerAction.Recharge;
        }
    }

    private static int Solve1(Player player, Boss boss)
    {
        var start = new State(player, boss);
        var minWinMana = FindMinMana(start, (s, a) => s.MakeAction(a), 0, int.MaxValue);

        return minWinMana;
    }

    private static int Solve2(Player player, Boss boss)
    {
        var start = new State(player, boss);
        var minWinMana = FindMinMana(start, (s, a) => s.MakeActionHard(a), 0, int.MaxValue);

        return minWinMana;
    }

    private static int FindMinMana(State state, Func<State, PlayerAction, State> makeAction, int usedMana, int currentMinMana, int depth = 0)
    {
        var minMana = currentMinMana;
        var possibleMoves = CanDo(state).ToArray();
        foreach (var move in possibleMoves)
        {
            var cost = StateActions.ManaCost[move];
            if (usedMana + cost >= minMana)
            {
                continue;
            }

            var newState = makeAction(state, move);
            if (newState.IsFinished)
            {
                if (newState.IsWin)
                {
                    PrintDepth(depth, $"Player won, {usedMana + cost} mana");
                    minMana = usedMana + cost;
                }
                else
                {
                    //PrintDepth(depth, "Player lost");
                    continue;
                }
            }
            else
            {
                var min = FindMinMana(newState, makeAction, usedMana + cost, minMana, depth + 1);
                if (min < minMana)
                {
                    minMana = min;
                }
            }
        }

        return minMana;
    }

    private static void PrintDepth(int depth, string text)
    {
        for (var i = 0; i < depth; i++)
        {
            Console.Write(">\t");
        }
        Console.WriteLine(text);
    }
}
