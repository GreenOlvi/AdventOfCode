using NUnit.Framework;

namespace AOC2022.Puzzles;

public partial class Day19 : CustomBaseDay
{
    private readonly string[] _lines;

    public Day19()
    {
        _lines = ReadLinesFromFile().ToArray();
    }

    public Day19(IEnumerable<string> lines)
    {
        _lines = lines.ToArray();
    }

    private static IEnumerable<Recipe> ParseInput(IEnumerable<string> lines) =>
        lines.Select(ParseRecipe);


    [GeneratedRegex("^Blueprint (?<id>\\d+): Each ore robot costs (?<ore_bot_ore>\\d+) ore\\. Each clay robot costs (?<clay_bot_ore>\\d+) ore\\. Each obsidian robot costs (?<obsidian_bot_ore>\\d+) ore and (?<obsidian_bot_clay>\\d+) clay\\. Each geode robot costs (?<geode_bot_ore>\\d+) ore and (?<geode_bot_obsidian>\\d+) obsidian\\.$")]
    private static partial Regex MakeRecipePattern();
    private readonly static Regex RecipePattern = MakeRecipePattern();

    private static Recipe ParseRecipe(string line)
    {
        if (!RecipePattern.TryMatchAll<int>(line, out var results))
        {
            throw new InvalidDataException(line);
        }

        return new Recipe
        {
            Id = results["id"],
            OreBot = new Cost(results["ore_bot_ore"], 0, 0),
            ClayBot = new Cost(results["clay_bot_ore"], 0, 0),
            ObsidianBot = new Cost(results["obsidian_bot_ore"], results["obsidian_bot_clay"], 0),
            GeodeBot = new Cost(results["geode_bot_ore"], 0, results["geode_bot_obsidian"]),
        };
    }

    private static int FindMaxGeodesBFS(Recipe recipe, int maxTime)
    {
        var states = new Dictionary<State, int>
        {
            [State.Initial] = 0,
        };

        var queue = new Queue<State>();
        queue.Enqueue(State.Initial);

        State maxState = State.Initial;
        var maxGeodesOnLevel = new int[maxTime];

        while (queue.TryDequeue(out var state))
        {
            var time = states[state];
            if (time >= maxTime)
            {
                if (state.Geode > maxState.Geode)
                {
                    maxState = state;
                }
                continue;
            }

            if (state.Geode > maxGeodesOnLevel[time])
            {
                maxGeodesOnLevel[time] = state.Geode;
            }
            else if (state.Geode + 2 < maxGeodesOnLevel[time])
            {
                continue;
            }

            var possibleActions = AllActions.Where(a => CanDo(state, recipe, a)).Reverse().ToArray();

            state = CollectResources(state);
            time++;

            foreach (var action in possibleActions)
            {
                var newState = DoAction(state, recipe, action);

                if (states.TryGetValue(newState, out var existing))
                {
                    if (existing > time)
                    {
                        states[newState] = time;
                        if (!queue.Contains(newState))
                        {
                            queue.Enqueue(newState);
                        }
                    }
                }
                else
                {
                    states[newState] = time;
                    queue.Enqueue(newState);
                }
            }
        }

        return maxState.Geode;
    }

    private static State CollectResources(State state) =>
        state with
        {
            Ore = state.Ore + state.OreBots,
            Clay = state.Clay + state.ClayBots,
            Obsidian = state.Obsidian + state.ObsidianBots,
            Geode = state.Geode + state.GeodeBots,
        };

    private static State DoAction(State state, Recipe recipe, Action action) =>
        action switch
        {
            Action.Wait => state,
            Action.NewOreBot => Spend(state, recipe.OreBot) with { OreBots = state.OreBots + 1 },
            Action.NewClayBot => Spend(state, recipe.ClayBot) with { ClayBots = state.ClayBots + 1 },
            Action.NewObsidianBot => Spend(state, recipe.ObsidianBot) with { ObsidianBots = state.ObsidianBots + 1 },
            Action.NewGeodeBot => Spend(state, recipe.GeodeBot) with { GeodeBots = state.GeodeBots + 1 },
            _ => throw new NotImplementedException(),
        };

    private static State Spend(State state, Cost cost)
    {
        Assert.IsTrue(state.Ore >= cost.Ore && state.Clay >= cost.Clay && state.Obsidian >= cost.Obsidian);
        return state with
        {
            Ore = state.Ore - cost.Ore,
            Clay = state.Clay - cost.Clay,
            Obsidian = state.Obsidian - cost.Obsidian,
        };
    }

    private static bool CanDo(State state, Recipe recipe, Action action) =>
        action switch
        {
            Action.Wait => true,
            Action.NewOreBot => CanAfford(state, recipe.OreBot),
            Action.NewClayBot => CanAfford(state, recipe.ClayBot),
            Action.NewObsidianBot => CanAfford(state, recipe.ObsidianBot),
            Action.NewGeodeBot => CanAfford(state, recipe.GeodeBot),
            _ => throw new NotImplementedException(),
        };

    private static bool CanAfford(State state, Cost cost) =>
        state.Ore >= cost.Ore && state.Clay >= cost.Clay && state.Obsidian >= cost.Obsidian;

    public override ValueTask<string> Solve_1()
    {
        var input = ParseInput(_lines);
        var sum = 0;
        foreach (var r in input)
        {
            sum += FindMaxGeodesBFS(r, 24) * r.Id;
            GC.Collect();
        }

        return sum.ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        var input = ParseInput(_lines).Take(3);
        var prod = 1L;
        foreach (var r in input)
        {
            prod *= FindMaxGeodesBFS(r, 32);
            GC.Collect();
        }

        return prod.ToResult();
    }

    private enum Action
    {
        Wait,
        NewOreBot,
        NewClayBot,
        NewObsidianBot,
        NewGeodeBot,
    }

    private static readonly Action[] AllActions = Enum.GetValues<Action>();

    private readonly record struct State
    {
        public readonly int Ore { get; init; }
        public readonly int Clay { get; init; }
        public readonly int Obsidian { get; init; }
        public readonly int Geode { get; init; }

        public readonly int OreBots { get; init; }
        public readonly int ClayBots { get; init; }
        public readonly int ObsidianBots { get; init; }
        public readonly int GeodeBots { get; init; }

        public static readonly State Initial = new State() { OreBots = 1 };
    }

    private readonly record struct Cost(int Ore, int Clay, int Obsidian);

    private readonly record struct Recipe
    {
        public readonly int Id { get; init; }
        public readonly Cost OreBot { get; init; }
        public readonly Cost ClayBot { get; init; }
        public readonly Cost ObsidianBot { get; init; }
        public readonly Cost GeodeBot { get; init; }
    }
}
