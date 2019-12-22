using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using AoC2019.Puzzle22;
using System.Linq;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle22Tests
    {
        [Test]
        public void DealIntoNewDeckTest()
        {
            DeckExtensions.NewDeck(10).DealIntoNewStack()
                .Should().BeEquivalentTo(new[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }, o => o.WithStrictOrdering());

            new Deck(10).DealIntoNewStack().Cards
                .Should().BeEquivalentTo(new[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }, o => o.WithStrictOrdering());
        }

        [TestCase(3, new[] { 3, 4, 5, 6, 7, 8, 9, 0, 1, 2 })]
        [TestCase(-4, new[] { 6, 7, 8, 9, 0, 1, 2, 3, 4, 5 })]
        public void CutTests(int n, IEnumerable<int> expected)
        {
            DeckExtensions.NewDeck(10).Cut(n)
                .Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());

            new Deck(10).Cut(n).Cards
                .Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [TestCase(3, new long[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }, new[] { 6, 5, 4, 3, 2, 1, 0, 9, 8, 7 })]
        [TestCase(7, new long[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }, new[] { 2, 1, 0, 9, 8, 7, 6, 5, 4, 3 })]
        [TestCase(9, new long[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }, new[] { 0, 9, 8, 7, 6, 5, 4, 3, 2, 1 })]
        public void CutWithInitialTests(int n, long[] initial, IEnumerable<int> expected)
        {
            initial.Cut(n)
                .Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());

            new Deck(initial).Cut(n).Cards
                .Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [TestCase(3, new[] { 0, 7, 4, 1, 8, 5, 2, 9, 6, 3 })]
        [TestCase(7, new[] { 0, 3, 6, 9, 2, 5, 8, 1, 4, 7 })]
        [TestCase(9, new[] { 0, 9, 8, 7, 6, 5, 4, 3, 2, 1 })]
        public void DealWithIncrementTests(int n, IEnumerable<int> expected)
        {
            DeckExtensions.NewDeck(10).DealWithIncrement(n)
                .Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
            new Deck(10).DealWithIncrement(n).Cards
                .Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [TestCase(3, new long[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }, new[] { 9, 2, 5, 8, 1, 4, 7, 0, 3, 6 })]
        [TestCase(7, new long[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }, new[] { 9, 6, 3, 0, 7, 4, 1, 8, 5, 2 })]
        [TestCase(9, new long[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }, new[] { 9, 0, 1, 2, 3, 4, 5, 6, 7, 8 })]
        public void DealWithIncrementWithInitialTests(int n, long[] initial, IEnumerable<int> expected)
        {
            initial.DealWithIncrement(n)
                .Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
            new Deck(initial).DealWithIncrement(n).Cards
                .Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        public static readonly IEnumerable<TestCaseData> RunInstructionsTestCases = new[]
        {
            new TestCaseData(new[]
            {
                "deal with increment 7",
                "deal into new stack",
                "deal into new stack",
            }, new[] { 0, 3, 6, 9, 2, 5, 8, 1, 4, 7 }),
            new TestCaseData(new[]
            {
                "cut 6",
                "deal with increment 7",
                "deal into new stack",
            }, new[] { 3, 0, 7, 4, 1, 8, 5, 2, 9, 6 }),
            new TestCaseData(new[]
            {
                "deal with increment 7",
                "deal with increment 9",
                "cut -2",
            }, new[] { 6, 3, 0, 7, 4, 1, 8, 5, 2, 9 }),
            new TestCaseData(new[]
            {
                "deal into new stack",
                "cut -2",
                "deal with increment 7",
                "cut 8",
                "cut -4",
                "deal with increment 7",
                "cut 3",
                "deal with increment 9",
                "deal with increment 3",
                "cut -1",
            }, new[] { 9, 2, 5, 8, 1, 4, 7, 0, 3, 6 }),
        };

        [TestCaseSource(nameof(RunInstructionsTestCases))]
        public void RunInstructionsTests(IEnumerable<string> instructions, IEnumerable<int> expected)
        {
            var ops = Solution.ParseInstructions(instructions);
            Solution.RunInstructions(new Deck(10), ops).Cards
                .Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [TestCaseSource(nameof(RunInstructionsTestCases))]
        public void RunIndexInstructionsTests(IEnumerable<string> instructions, IEnumerable<int> expected)
        {
            var ops = Solution.ParseIndexInstructions(instructions);
            Enumerable.Range(0, 10).Select(i => Solution.RunIndexInstructions(new DeckByIndex(10), ops, i))
                .Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Test]
        public void ReverseByIndexTests()
        {
            var fullDeck = new Deck(10).DealIntoNewStack().Cards;
            var deck = new DeckByIndex(10);

            Enumerable.Range(0, 10).Select(i => deck.DealIntoNewStack(i))
                .Should().BeEquivalentTo(fullDeck, o => o.WithStrictOrdering());
        }

        [Test]
        public void CutByIndexTests()
        {
            for (var n = -9; n < 10; n++)
            {
                var fullDeck = new Deck(10).Cut(n).Cards;
                var deck = new DeckByIndex(10);

                Enumerable.Range(0, 10).Select(i => deck.Cut(i, n))
                    .Should().BeEquivalentTo(fullDeck, o => o.WithStrictOrdering());
            }
        }

        [TestCase(3)]
        [TestCase(7)]
        [TestCase(9)]
        public void DealWithIncrementByIndexTests(int n)
        {
            var fullDeck = new Deck(10).DealWithIncrement(n).Cards;
            var deck = new DeckByIndex(10);

            var a = Enumerable.Range(0, 10).Select(i => deck.DealWithIncrement(i, n)).ToArray();
            a.Should().BeEquivalentTo(fullDeck, o => o.WithStrictOrdering());
        }
    }
}
