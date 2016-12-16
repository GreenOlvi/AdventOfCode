using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Puzzle11
{
    public class Solver
    {
        public Solver(params string[] input)
        {
            StartConfig = ParseInput(input);
        }

        private Configuration StartConfig { get; }

        public int Solve1()
        {
            var successfull = FindSolutions(StartConfig);
            return successfull.Min(c => c.MoveCount);
        }

        public int Solve2()
        {
            var floors = new[]
            {
                StartConfig.Floors[0].AddItems(new Item(ItemType.Generator, "elerium"), new Item(ItemType.Microchip, "elerium"), new Item(ItemType.Generator, "dilithium"), new Item(ItemType.Microchip, "dilithium")),
                StartConfig.Floors[1],
                StartConfig.Floors[2],
                StartConfig.Floors[3],
            };

            var successfull = FindSolutions(new Configuration(floors));
            return successfull.Min(c => c.MoveCount);
        }

        private static Configuration ParseInput(IEnumerable<string> input)
        {
            var floors = input.Select(line => ParseFloor(line)).ToArray();
            return new Configuration(floors);
        }

        private static readonly Regex ItemRegex = new Regex(@"(?<element>\w+)(?<type>\sgenerator|-compatible microchip)", RegexOptions.Compiled);
        private static Floor ParseFloor(string line)
        {
            var items = new List<Item>();
            foreach (var match in ItemRegex.Matches(line).Cast<Match>())
            {
                var type = match.Groups["type"].Value.StartsWith(" generator")
                    ? ItemType.Generator
                    : ItemType.Microchip;

                items.Add(new Item(type, match.Groups["element"].Value));
            }

            return new Floor(items.ToArray());
        }

        private static IEnumerable<Configuration> FindSolutions(Configuration configuration)
        {
            var seen = new HashSet<string>() {configuration.ToPairsString()};
            var queue = new List<Configuration>() {configuration};
            while (queue.Any())
            {
                var current = queue.First();
                queue.RemoveAt(0);

                seen.Add(current.ToPairsString());

                if (current.IsGoal())
                {
                    yield return current;
                }

                var newConfigs = current.AvaliableMoves()
                    .Select(m => current.Move(m));

                foreach (var config in newConfigs)
                {
                    var shortString = config.ToPairsString();
                    if (!seen.Contains(shortString))
                    {
                        seen.Add(shortString);
                        if (config.IsStable()) 
                            queue.Add(config);
                    }
                }
            }
        } 

        public static IEnumerable<T[]> GetPowerSet<T>(List<T> list)
        {
            return Enumerable.Range(0, 1 << list.Count)
                .Select(m => (Enumerable.Range(0, list.Count)
                    .Where(i => (m & (1 << i)) != 0)
                    .Select(i => list[i]).ToArray()));
        }
    }

    public class Configuration
    {
        public Configuration(int elevator, Floor[] floors)
        {
            Elevator = elevator;
            Floors = floors;
        }

        public Configuration(Floor[] floors) : this(0, floors)
        {
        }

        private Configuration(Configuration fromConfig, Move lastMove, int elevator, Floor[] floors) : this(elevator, floors)
        {
            LastConfiguration = fromConfig;
            LastMove = lastMove;
            MoveCount = LastConfiguration.MoveCount + 1;
        }

        public int Elevator { get; }
        public Floor[] Floors { get; }
        public Move? LastMove { get; }
        public Configuration LastConfiguration { get; }

        public int MoveCount { get; }

        public bool IsStable()
        {
            return Floors.All(f => f.IsStable());
        }

        public bool IsGoal()
        {
            return (Elevator == 3
                && Floors[0].Items.Count == 0
                && Floors[1].Items.Count == 0
                && Floors[2].Items.Count == 0
                && Floors[3].Items.Count > 0);
        }

        public Configuration Move(Move move)
        {
            var df = move.Direction == MoveDirection.Up ? 1 : -1;

            var floors = new Floor[Floors.Length];
            for (var f = 0; f <= 3; f++)
            {
                if (f == Elevator)
                {
                    floors[f] = Floors[f].RemoveItems(move.Items);
                }
                else
                {
                    if (f == Elevator + df)
                    {
                        floors[f] = Floors[f].AddItems(move.Items);
                    }
                    else
                    {
                        floors[f] = Floors[f];
                    }
                }
            }

            return new Configuration(this, move, Elevator + df, floors);
        }

        public IEnumerable<Move> AvaliableMoves()
        {
            if (IsGoal())
                yield break;

            var powerSet = Solver.GetPowerSet(Floors[Elevator].Items)
                .Where(s => s.Length > 0 && s.Length <= 2);

            var reversed = LastMove?.ReverseMove();

            foreach (var items in powerSet)
            {
                if (Elevator < 3)
                {
                    var move = new Move(MoveDirection.Up, items);
                    if (!(reversed.HasValue && move.Equals(reversed.Value)))
                        yield return move;
                }
                if (Elevator > 0)
                {
                    var move = new Move(MoveDirection.Down, items);
                    if (!(reversed.HasValue && move.Equals(reversed.Value)))
                        yield return move;
                }
            }
        }

        public IEnumerable<Item> AllItems()
        {
            return Floors.SelectMany(f => f.Items)
                .OrderBy(i => i.Element).ThenBy(i => i.Type);
        }

        public string ToPairsString()
        {
            var dict = new Dictionary<string, string>();
            for (var i = 0; i < 3; i++)
            {
                foreach (var item in Floors[i].Items)
                {
                    if (!dict.ContainsKey(item.Element))
                    {
                        dict.Add(item.Element, i.ToString());
                    }
                    else
                    {
                        if (item.Type == ItemType.Generator)
                        {
                            dict[item.Element] = i + dict[item.Element];
                        }
                        else
                        {
                            dict[item.Element] += i;
                        }
                    }
                }
            }
            return Elevator + "|" + String.Join("|", dict.Values.OrderBy(v => v));
        }

        public override string ToString()
        {
            var text = new StringBuilder();
            var allItems = AllItems().ToList();

            for (var f = 3; f >= 0; f--)
            {
                var floor = Floors[f];

                var elementString = String.Join(" ",
                    allItems.Select(item => floor.Contains(item) ? item.ToString() : ".  "));

                var str = String.Format(@"F{0} {1} ",
                    f + 1,
                    Elevator == f ? "E" : ".");

                text.AppendLine(str + elementString);
            }

            return text.ToString();
        }
    }

    public class Floor
    {
        public Floor()
        {
            Items = new List<Item>();
        }

        public Floor(params Item[] items) : this()
        {
            Items.AddRange(items);
        }

        public List<Item> Items { get; }

        public bool Contains(Item item)
        {
            return Items.Contains(item);
        }

        public bool IsStable()
        {
            if (Items.Count == 0)
                return true;

            var chips = Items.Where(i => i.Type == ItemType.Microchip).ToArray();
            if (chips.Length == 0)
                return true;

            var gens = Items.Where(i => i.Type == ItemType.Generator).ToArray();
            if (gens.Length == 0)
                return true;

            return chips.All(c => gens.Contains(new Item(ItemType.Generator, c.Element)));
        }

        public Floor AddItems(params Item[] items)
        {
            return new Floor(Items.Concat(items).ToArray());
        }

        public Floor RemoveItems(params Item[] items)
        {
            return new Floor(Items.Where(i => !items.Contains(i)).ToArray());
        }
    }

    public struct Move
    {
        public Move(MoveDirection direction, params Item[] items)
        {
            Direction = direction;
            Items = items.OrderBy(i => i.ToString()).ToArray();
        }

        public MoveDirection Direction { get; }
        public Item[] Items { get; }

        public bool Equals(Move other)
        {
            return Direction == other.Direction && Enumerable.SequenceEqual(Items, other.Items);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Move && Equals((Move) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Direction*397) ^ (Items != null ? Items.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return String.Format(@"[{0}: {1}]", Direction, String.Join(", ", Items));
        }

        public Move ReverseMove()
        {
            return Direction == MoveDirection.Up
                ? new Move(MoveDirection.Down, Items)
                : new Move(MoveDirection.Up, Items);
        }
    }

    public struct Item
    {
        public Item(ItemType type, string element)
        {
            Type = type;
            Element = Char.ToUpper(element[0]) + element.Substring(1);
        }

        public ItemType Type { get; }
        public string Element { get; }

        public bool Equals(Item other)
        {
            return Type == other.Type && string.Equals(Element, other.Element);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Item && Equals((Item) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Type*397) ^ (Element?.GetHashCode() ?? 0);
            }
        }

        public override string ToString()
        {
            return Element.Substring(0, 2) + Type.ToString()[0];
        }
    }

    public enum MoveDirection { Up, Down }
    public enum ItemType
    {
        Nothing = 0,
        Generator,
        Microchip,
    }
}
