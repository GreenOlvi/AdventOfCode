using AOC2021.Day23;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace AocTests
{
    public class Day23Tests
    {
        private static readonly string[] _testInput = new string[]
        {
            "#############",
            "#...........#",
            "###B#C#B#D###",
            "  #A#D#C#A#",
            "  #########",
        };

        private readonly Puzzle _puzzle = new(_testInput);

        [Test]
        public void Solution1Test()
        {
            _puzzle.Solution1().Should().Be(12521);
        }

        [Test]
        public void Solution2Test()
        {
            _puzzle.Solution2().Should().Be(44169);
        }

        [TestCase(Room.H1, Room.H2, 1)]
        [TestCase(Room.H2, Room.H1, 1)]
        [TestCase(Room.H1, Room.H7, 10)]
        [TestCase(Room.D2, Room.H1, 10)]
        public void MapGetDistanceTests(Room a, Room b, int expected)
        {
            _puzzle.Map.GetDistance(a, b).Should().Be(expected);
        }

        [TestCase(Room.H1, Room.H2, new[] { Room.H2 })]
        [TestCase(Room.H2, Room.H1, new[] { Room.H1 })]
        [TestCase(Room.H1, Room.H7, new[] { Room.H2, Room.H3, Room.H4, Room.H5, Room.H6, Room.H7 })]
        [TestCase(Room.D2, Room.H1, new[] { Room.D1, Room.H5, Room.H4, Room.H3, Room.H2, Room.H1 })]
        public void MapGetRoutesTests(Room a, Room b, Room[] route)
        {
            _puzzle.Map.GetRoute(a, b).Should().BeEquivalentTo(route);
        }

        /*
         * #############
         * #...........#
         * ###B#C#B#D###
         *   #A#D#C#A#
         *   #########
        */
        private static readonly Dictionary<Amphipod, Room[]> _initialTestPositions = new()
        {
            { Amphipod.A, new[] { Room.A2, Room.D2 } },
            { Amphipod.B, new[] { Room.A1, Room.C1 } },
            { Amphipod.C, new[] { Room.B1, Room.C2 } },
            { Amphipod.D, new[] { Room.B2, Room.D1 } },
        };

        /*
         * #############
         * #C..B.....AD#
         * ###B#.#.#.###
         *   #A#D#C#.#
         *   #########
        */
        private static readonly Dictionary<Amphipod, Room[]> _firstStepTestPositions = new()
        {
            { Amphipod.A, new[] { Room.A2, Room.H6 } },
            { Amphipod.B, new[] { Room.A1, Room.H3 } },
            { Amphipod.C, new[] { Room.H1, Room.C2 } },
            { Amphipod.D, new[] { Room.B2, Room.H7 } },
        };

        /*
         * #############
         * #...B.......#
         * ###B#.#C#D###
         *   #A#D#C#A#
         *   #########
        */
        private static readonly Dictionary<Amphipod, Room[]> _secondStepTestPositions = new()
        {
            { Amphipod.A, new[] { Room.A2, Room.D2 } },
            { Amphipod.B, new[] { Room.A1, Room.H3 } },
            { Amphipod.C, new[] { Room.C1, Room.C2 } },
            { Amphipod.D, new[] { Room.B2, Room.D1 } },
        };

        private static readonly TestCaseData[] _canGoTestCases = new[]
        {
            new TestCaseData(_initialTestPositions, Amphipod.B, Room.C1, Room.H3, true) { TestName = "CanGoTests(go to empty hallway)" },
            new TestCaseData(_initialTestPositions, Amphipod.B, Room.C1, Room.B1, false) { TestName = "CanGoTests(occupied)" },
            new TestCaseData(_initialTestPositions, Amphipod.A, Room.D2, Room.H2, false) { TestName = "CanGoTests(someone in the way)" },
            new TestCaseData(_firstStepTestPositions, Amphipod.B, Room.H3, Room.D2, false) { TestName = "CanGoTests(not my room)" },
            new TestCaseData(_firstStepTestPositions, Amphipod.D, Room.B2, Room.D1, false) { TestName = "CanGoTests(leaving a gap)" },
            new TestCaseData(_firstStepTestPositions, Amphipod.D, Room.B2, Room.D2, true) { TestName = "CanGoTests(move straight to my home)" },
            new TestCaseData(_secondStepTestPositions, Amphipod.B, Room.H3, Room.B1, false) { TestName = "CanGoTests(room with someone else)" },
        };

        [TestCaseSource(nameof(_canGoTestCases))]
        public void CanGoTests(Dictionary<Amphipod, Room[]> state, Amphipod a, Room from, Room to, bool expected)
        {
            MapState.FromPositions(_puzzle.Map, state).CanGo(a, from, to).Should().Be(expected);
        }

        private static readonly TestCaseData[] _possibleMovesTestCases = new[]
        {
            new TestCaseData(_initialTestPositions, new[] {
                (Amphipod.B, Room.A1, Room.H1),
                (Amphipod.B, Room.A1, Room.H2),
                (Amphipod.B, Room.A1, Room.H3),
                (Amphipod.B, Room.A1, Room.H4),
                (Amphipod.B, Room.A1, Room.H5),
                (Amphipod.B, Room.A1, Room.H6),
                (Amphipod.B, Room.A1, Room.H7),
                (Amphipod.B, Room.C1, Room.H1),
                (Amphipod.B, Room.C1, Room.H2),
                (Amphipod.B, Room.C1, Room.H3),
                (Amphipod.B, Room.C1, Room.H4),
                (Amphipod.B, Room.C1, Room.H5),
                (Amphipod.B, Room.C1, Room.H6),
                (Amphipod.B, Room.C1, Room.H7),
                (Amphipod.C, Room.B1, Room.H1),
                (Amphipod.C, Room.B1, Room.H2),
                (Amphipod.C, Room.B1, Room.H3),
                (Amphipod.C, Room.B1, Room.H4),
                (Amphipod.C, Room.B1, Room.H5),
                (Amphipod.C, Room.B1, Room.H6),
                (Amphipod.C, Room.B1, Room.H7),
                (Amphipod.D, Room.D1, Room.H1),
                (Amphipod.D, Room.D1, Room.H2),
                (Amphipod.D, Room.D1, Room.H3),
                (Amphipod.D, Room.D1, Room.H4),
                (Amphipod.D, Room.D1, Room.H5),
                (Amphipod.D, Room.D1, Room.H6),
                (Amphipod.D, Room.D1, Room.H7),
            }) { TestName = "GetPossibleMovesTests(initial test positions)" },
            new TestCaseData(_firstStepTestPositions, new[] {
                (Amphipod.B, Room.A1, Room.H2),
                (Amphipod.D, Room.B2, Room.H4),
                (Amphipod.D, Room.B2, Room.H5),
                (Amphipod.D, Room.B2, Room.D2),
            }) { TestName = "GetPossibleMovesTests(after first step positions" },
        };

        [TestCaseSource(nameof(_possibleMovesTestCases))]
        public void GetPossibleMovesTests(Dictionary<Amphipod, Room[]> state, IEnumerable<(Amphipod A, Room from, Room to)> possibleMoves)
        {
            MapState.FromPositions(_puzzle.Map, state).GetPossibleMoves().Should().BeEquivalentTo(possibleMoves);
        }
    }
}