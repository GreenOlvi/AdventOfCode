using NUnit.Framework;
using AOC2021.Day18;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace AocTests
{
    public class Day18Tests
    {
        private static readonly string[] _testHomework = new[]
        {
            "[[[0,[5, 8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]",
            "[[[5,[2,8]],4],[5,[[9,9],0]]]",
            "[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]",
            "[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]",
            "[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]",
            "[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]",
            "[[[[5,4],[7,7]],8],[[8,3],8]]",
            "[[9,3],[[9,9],[6,[4,9]]]]",
            "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]",
            "[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]",
        };

        private readonly Puzzle _puzzle = new(_testHomework);

        private static readonly string[] _testInput1 = new string[]
        {
            "[1, 1]", "[2, 2]", "[3, 3]", "[4, 4]",
        };

        private static readonly string[] _testInput2 = new string[]
        {
            "[1, 1]", "[2, 2]", "[3, 3]", "[4, 4]", "[5, 5]",
        };

        private static readonly string[] _testInput3 = new string[]
        {
            "[1, 1]", "[2, 2]", "[3, 3]", "[4, 4]", "[5, 5]", "[6, 6]",
        };

        private static readonly string[] _testInput4 = new string[]
        {
            "[[[0,[4, 5]],[0, 0]],[[[4,5],[2,6]],[9,5]]]",
            "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]",
            "[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]",
            "[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]",
            "[7,[5,[[3,8],[1,4]]]]",
            "[[2,[2,2]],[8,[8,1]]]",
            "[2,9]",
            "[1,[[[9,3],9],[[9,0],[0,7]]]]",
            "[[[5,[7,4]],7],1]",
            "[[[[4,2],2],6],[8,7]]",
        };

        private static readonly TestCaseData[] _addTestCases = new[]
        {
            new TestCaseData(_testInput1, "[[[[1,1],[2,2]],[3,3]],[4,4]]"),
            new TestCaseData(_testInput2, "[[[[3,0],[5,3]],[4,4]],[5,5]]"),
            new TestCaseData(_testInput3, "[[[[5,0],[7,4]],[5,5]],[6,6]]"),
            new TestCaseData(_testInput4, "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]"),
            new TestCaseData(new[]
            {
                "[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]", "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]"
            },
                "[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]")
        };

        [Test]
        public void ParseNumberTests()
        {
            Puzzle.ParseNode("1").Should().Be(new Number(1));
            Puzzle.ParseNode("42").Should().Be(new Number(42));
        }

        [Test]
        public void ParsePairTests()
        {
            Puzzle.ParseNode("[4,9]").ToString()
                .Should().Be("[4,9]");
        }

        [Test]
        public void GetDepthTests()
        {
            Puzzle.ParseNode("42").GetDepth().Should().Be(0);
            Puzzle.ParseNode("[2, 3]").GetDepth().Should().Be(1);
            Puzzle.ParseNode("[[[[[9,8],1],2],3],4]").GetDepth().Should().Be(5);
        }

        [Test]
        [TestCase("[[[[1,1],[2,2]],[3,3]],[4,4]]", "[[[[1,1],[2,2]],[3,3]],[4,4]]")]
        [TestCase("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]")]
        [TestCase("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]")]
        [TestCase("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]")]
        [TestCase("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")]
        [TestCase("[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]", "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]")]
        [TestCase("[11,0]", "[[5,6],0]")]
        public void ReduceTests(string input, string expected)
        {
            var node = Puzzle.ParseNode(input);
            var reduced = Puzzle.Reduce(node);
            reduced.ToString().Should().Be(expected);
        }

        [TestCaseSource(nameof(_addTestCases))]
        public void AddAndReduceTests(IEnumerable<string> input, string expected)
        {
            var result = input.Select(Puzzle.ParseNode)
                .Aggregate((a, b) => Puzzle.AddAndReduce(a, b));

            result.ToString().Should().Be(expected);
        }

        [Test]
        public void Solution1Test()
        {
            _puzzle.Solution1().Should().Be(4140);
        }

        [Test]
        public void Solution2Test()
        {
            _puzzle.Solution2().Should().Be(3993);
        }
    }
}