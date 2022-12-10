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
        Test3();

        var input = File.ReadAllText(InputFile);
        var boss = ParseInput(input);
        var player = new Player(50, 500);

        StateActions.Debug = false;

        var result1 = Solve1(player, boss);
        Console.WriteLine($"Result 1 = {result1}");
        Assert.AreEqual(953, result1);

        var result2 = Solve2(player, boss);
        Console.WriteLine($"Result 2 = {result2}");
        Assert.AreEqual(1289, result2);
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
        StateActions.Debug = false;

        var state = new State(new Player(10, 250), new Boss(13, 8))
            .MakeAction(Spell.Poison)
            .MakeAction(Spell.MagicMissile);

        Assert.IsTrue(state.IsWin);
    }

    private static void Test2()
    {
        StateActions.Debug = false;

        var state = new State(new Player(10, 250), new Boss(14, 8));

        var spells = new Spell[] {
            Spell.Recharge,
            Spell.Shield,
            Spell.Drain,
            Spell.Poison,
            Spell.MagicMissile,
        };

        var s = state;
        foreach (var spell in spells)
        {
            s = s.MakeAction(spell);
        }

        Assert.IsTrue(s.IsWin);
    }

    private static void Test3()
    {
        StateActions.Debug = false;

        var state = new State(new Player(50, 500), new Boss(55, 8));

        var spells = new Spell[] {
            Spell.Poison,
            Spell.Drain,
            Spell.Recharge,
            Spell.Poison,
            Spell.Shield,
            Spell.Recharge,
            Spell.Poison,
            Spell.Drain,
            Spell.MagicMissile,
        };

        var s = state;
        foreach (var spell in spells)
        {
            s = s.MakeAction(spell);
        }

        Assert.IsTrue(s.IsWin);
    }

    private static IEnumerable<Spell> CanDo(State state)
    {
        var mana = state.Player.Mana;
        if (state.RechargeTimer > 0)
        {
            mana += 101;
        }

        if (mana < 53)
        {
            yield break;
        }
        yield return Spell.MagicMissile;

        if (mana < 73)
        {
            yield break;
        }
        yield return Spell.Drain;

        if (mana < 113)
        {
            yield break;
        }
        if (state.ShieldTimer <= 1)
        {
            yield return Spell.Shield;
        }

        if (mana < 173)
        {
            yield break;
        }
        if (state.PoisonTimer <= 1)
        {
            yield return Spell.Poison;
        }

        if (mana < 229)
        {
            yield break;
        }
        if (state.RechargeTimer <= 1)
        {
            yield return Spell.Recharge;
        }
    }

    private static int Solve1(Player player, Boss boss)
    {
        var start = new State(player, boss);
        var minWinMana = FindMinMana(start, (s, a) => s.MakeAction(a), 0, int.MaxValue, Enumerable.Empty<Spell>());

        return minWinMana;
    }

    private static int Solve2(Player player, Boss boss)
    {
        var start = new State(player, boss);
        var minWinMana = FindMinMana(start, (s, a) => s.MakeActionHard(a), 0, int.MaxValue, Enumerable.Empty<Spell>());

        return minWinMana;
    }

    private static int FindMinMana(State state, Func<State, Spell, State> makeAction, int usedMana, int currentMinMana, IEnumerable<Spell> spells, int depth = 0)
    {
        Assert.IsFalse(state.Player.IsDead);
        Assert.IsFalse(state.Boss.IsDead);
        Assert.AreEqual(usedMana, spells.Sum(s => StateActions.ManaCost[s]));

        var minMana = currentMinMana;
        var possibleMoves = CanDo(state).Where(m => StateActions.ManaCost[m] + usedMana < minMana).ToArray();

        foreach (var move in possibleMoves)
        {
            var cost = StateActions.ManaCost[move];
            var newState = makeAction(state, move);
            if (newState.IsFinished)
            {
                if (newState.IsWin)
                {
                    //PrintDepth(depth, $"Player won, {usedMana + cost} mana");
                    //PrintSpells(spells.Append(move));
                    //Console.WriteLine(newState);
                    if (usedMana + cost < minMana)
                    {
                        minMana = usedMana + cost;
                    }
                    else
                    {
                    }
                }
                else
                {
                    //PrintDepth(depth, "Player lost");
                    continue;
                }
            }
            else
            {
                var min = FindMinMana(newState, makeAction, usedMana + cost, minMana, spells.Append(move), depth + 1);
                if (min < minMana)
                {
                    minMana = min;
                }
                else
                {
                }
            }
        }

        return minMana;
    }

    private static void PrintSpells(IEnumerable<Spell> spells)
    {
        foreach (var spell in spells)
        {
            Console.Write($"{spell} ({StateActions.ManaCost[spell]}); ");
        }
        Console.WriteLine($"Total {spells.Sum(s => StateActions.ManaCost[s])}");
    }

    private static void PrintDepth(int depth, string text)
    {
        for (var i = 0; i < depth; i++)
        {
            Console.Write("> ");
        }
        Console.WriteLine(text);
    }
}
