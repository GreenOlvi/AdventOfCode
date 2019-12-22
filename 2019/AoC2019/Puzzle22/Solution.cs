using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2019.Puzzle22
{
    public class Solution : IPuzzle
    {
        public Solution(IEnumerable<string> input)
        {
            _input = input.ToList();
        }

        private readonly List<string> _input;

        public static IEnumerable<Func<Deck, Deck>> ParseInstructions(IEnumerable<string> instructions)
        {
            return instructions.Select(line => ParseLine(line));
        }

        private static readonly Regex LineRegex =
            new Regex(@"deal into (?<name>new stack)|(?<name>cut) (?<n>-?\d+)|deal with (?<name>increment) (?<n>\d+)", RegexOptions.Compiled);
        private static Func<Deck, Deck> ParseLine(string line)
        {
            var m = LineRegex.Match(line);
            if (!m.Success)
            {
                throw new ArgumentException("Line does not match the format", nameof(line));
            }
            var op = m.Groups["name"].Value;
            switch (op)
            {
                case "new stack":
                    return deck => deck.DealIntoNewStack();
                case "cut":
                    var n = int.Parse(m.Groups["n"].Value);
                    return deck => deck.Cut(n);
                case "increment":
                    var n2 = int.Parse(m.Groups["n"].Value);
                    return deck => deck.DealWithIncrement(n2);
                default:
                    throw new ArgumentOutOfRangeException("name");
            }
        }

        public static IEnumerable<Func<DeckByIndex, long, long>> ParseIndexInstructions(IEnumerable<string> instructions)
        {
            return instructions.Select(line => ParseLineIndex(line));
        }

        public static Deck RunInstructions(Deck deck, IEnumerable<Func<Deck, Deck>> ops)
        {
            return ops.Aggregate(deck, (d, op) => op(d));
        }

        private static Func<DeckByIndex, long, long> ParseLineIndex(string line)
        {
            var m = LineRegex.Match(line);
            if (!m.Success)
            {
                throw new ArgumentException("Line does not match the format", nameof(line));
            }
            var op = m.Groups["name"].Value;
            switch (op)
            {
                case "new stack":
                    return (deck, i) => deck.DealIntoNewStack(i);
                case "cut":
                    var n = int.Parse(m.Groups["n"].Value);
                    return (deck, i) => deck.Cut(i, n);
                case "increment":
                    var n2 = int.Parse(m.Groups["n"].Value);
                    return (deck, i) => deck.DealWithIncrement(i, n2);
                default:
                    throw new ArgumentOutOfRangeException("name");
            }
        }

        public static long RunIndexInstructions(DeckByIndex deck, IEnumerable<Func<DeckByIndex, long, long>> ops, long index)
        {
            return ops.Aggregate(index, (i, op) => op(deck, i));
        }

        public static long Solve1(IEnumerable<string> input, long deckSize)
        {
            var ops = ParseInstructions(input);
            var newDeck = RunInstructions(new Deck(deckSize), ops);
            return newDeck.FindIndex(2019);
        }

        public static long Solve2(IEnumerable<string> input, long deckSize, long repeats)
        {
            var ops = ParseIndexInstructions(input).Reverse();
            long index = 2020;
            for (var i = 0L; i < repeats; i++)
            {
                index = RunIndexInstructions(new DeckByIndex(deckSize), ops, index);
            }
            return index;
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_input, 10007).ToString());

        public Task<string> Solve2Async() =>
            Task.Run(() => Solve2(_input, 119315717514047, 101741582076661).ToString());
    }
}
