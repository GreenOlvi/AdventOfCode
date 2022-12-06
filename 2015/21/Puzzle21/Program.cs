using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

internal partial class Program
{
    private const string InputFile = "input.txt";

    [GeneratedRegex("Hit Points: (?<hp>\\d+).*Damage: (?<dmg>\\d+).*Armor: (?<arm>\\d+)", RegexOptions.Singleline)]
    private static partial Regex MakeInputPattern();

    private static void Main(string[] args)
    {
        var input = File.ReadAllText(InputFile);

        var boss = ParseInput(input);
        Console.WriteLine($"Boss: {boss}");

        Test();

        var result1 = Solve1(boss);
        Console.WriteLine($"Result 1 = {result1}");

        var result2 = Solve2(boss);
        Console.WriteLine($"Result 2 = {result2}");
    }

    private readonly record struct Item(int Cost, int Damage, int Armor);
    private readonly record struct Character(int Hp, int Damage, int Armor);

    private static Character ParseInput(string input)
    {
        var inputPattern = MakeInputPattern();
        var match = inputPattern.Match(input);
        if (!match.Success)
        {
            throw new InvalidDataException();
        }

        var hp = int.Parse(match.Groups["hp"].Value);
        var dmg = int.Parse(match.Groups["dmg"].Value);
        var arm = int.Parse(match.Groups["arm"].Value);

        return new Character(hp, dmg, arm);
    }

    private static void Test()
    {
        var hero = new Character(8, 5, 5);
        var boss = new Character(12, 7, 2);
        Assert.AreEqual(4, HitsToKill(hero, boss));
        Assert.AreEqual(4, HitsToKill(boss, hero));
        Assert.AreEqual(true, CanWin(hero, boss));
        Console.WriteLine($"{nameof(Test)} passed");
    }

    private static int Solve1(Character boss)
    {
        const int heroHp = 100;
        var minWonCost = int.MaxValue;

        foreach (var equipped in GenerateEquipment())
        {
            var cost = equipped.Sum(i => i.Cost);
            if (cost >= minWonCost)
            {
                continue;
            }

            var damage = equipped.Sum(i => i.Damage);
            var armor = equipped.Sum(i => i.Armor);

            var hero = new Character(heroHp, damage, armor);
            if (CanWin(hero, boss))
            {
                minWonCost = cost;
            }
        }

        return minWonCost;
    }

    private static int Solve2(Character boss)
    {
        const int heroHp = 100;
        var maxLoseCost = 0;

        foreach (var equipped in GenerateEquipment())
        {
            var cost = equipped.Sum(i => i.Cost);
            if (cost <= maxLoseCost)
            {
                continue;
            }

            var damage = equipped.Sum(i => i.Damage);
            var armor = equipped.Sum(i => i.Armor);

            var hero = new Character(heroHp, damage, armor);
            if (!CanWin(hero, boss))
            {
                maxLoseCost = cost;
            }
        }

        return maxLoseCost;
    }

    private static readonly Item[] Weapons = new[] { Items.Dagger, Items.Shortsword, Items.Warhammer, Items.Longsword, Items.Greataxe };
    private static readonly Item[] Armors = new[] { Items.Nothing, Items.Leather, Items.Chainmail, Items.Splintmail, Items.Bandedmail, Items.Platemail };
    private static readonly Item[] Rings = new[] { Items.Damage_1, Items.Damage_2, Items.Damage_3, Items.Defense_1, Items.Defense_2, Items.Defense_3 };

    private static int CountBits(byte i)
    {
        var sum = 0;
        for (var j = 0; j < 6; j++)
        {
            if ((i & (1 << j)) > 0)
            {
                sum++;
            }
        }
        return sum;
    }

    private static IEnumerable<Item[]> GenerateRings()
    {
        for (byte i = 0; i < (1 << 6); i++)
        {
            if (CountBits(i) > 2)
            {
                continue;
            }

            var rings = Enumerable.Empty<Item>();

            for (var j = 0; j < 6; j++)
            {
                if ((i & (1 << j)) > 0)
                {
                    rings = rings.Append(Rings[j]);
                }
            }

            yield return rings.ToArray();
        }
    }

    private static IEnumerable<Item[]> GenerateEquipment()
    {
        foreach (var weapon in Weapons)
        {
            foreach (var armor in Armors)
            {
                foreach (var rings in GenerateRings())
                {
                    yield return rings.Append(weapon).Append(armor).ToArray();
                }
            }
        }
    }

    private static bool CanWin(Character hero, Character boss)
    {
        double hitsToKillBoss = HitsToKill(hero, boss);
        var bossDmg = Math.Max(boss.Damage - hero.Armor, 1.0);
        var hitsToKillHero = Math.Ceiling(hero.Hp / bossDmg);
        return hitsToKillBoss <= hitsToKillHero;
    }

    private static double HitsToKill(Character killer, Character victim)
    {
        var killerDmg = Math.Max(killer.Damage - victim.Armor, 1.0);
        return (double)Math.Ceiling(victim.Hp / killerDmg);
    }

    private static class Items
    {
        public static Item Nothing = new(0, 0, 0);

        //Weapons
        public static Item Dagger = new(8, 4, 0);
        public static Item Shortsword = new(10, 5, 0);
        public static Item Warhammer = new(25, 6, 0);
        public static Item Longsword = new(40, 7, 0);
        public static Item Greataxe = new(74, 8, 0);

        //Armor
        public static Item Leather = new(13, 0, 1);
        public static Item Chainmail = new(31, 0, 2);
        public static Item Splintmail = new(53, 0, 3);
        public static Item Bandedmail = new(75, 0, 4);
        public static Item Platemail = new(102, 0, 5);

        //Rings
        public static Item Damage_1 = new(25, 1, 0);
        public static Item Damage_2 = new(50, 2, 0);
        public static Item Damage_3 = new(100, 3, 0);
        public static Item Defense_1 = new(20, 0, 1);
        public static Item Defense_2 = new(40, 0, 2);
        public static Item Defense_3 = new(80, 0, 3);
    }
}