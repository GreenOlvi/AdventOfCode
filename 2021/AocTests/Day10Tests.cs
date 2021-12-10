using NUnit.Framework;
using AOC2021.Day10;
using FluentAssertions;

namespace AocTests
{
    public class Day10Tests
    {
        private static readonly string[] _testInput = new string[]
        {
            "[({(<(())[]>[[{[]{<()<>>",
            "[(()[<>])]({[<{<<[]>>(",
            "{([(<{}[<>[]}>{[]{[(<()>",
            "(((({<>}<{<{<>}{[]{[]{}",
            "[[<[([]))<([[{}[[()]]]",
            "[{[{({}]{}}([{[{{{}}([]",
            "{<[[]]>}<{[{[{[]{()[[[]",
            "[<(<(<(<{}))><([]([]()",
            "<{([([[(<>()){}]>(<<{{",
            "<{([{{}}[<[[[<>{}]]]>[]]",
        };

        private readonly Puzzle _puzzle = new(_testInput);

        [Test]
        public void Solution1Test()
        {
            _puzzle.Solution1().Should().Be(26397);
        }

        [Test]
        [TestCase("{([(<{}[<>[]}>{[]{[(<()>", '}')]
        [TestCase("[[<[([]))<([[{}[[()]]]", ')')]
        [TestCase("[{[{({}]{}}([{[{{{}}([]", ']')]
        [TestCase("[<(<(<(<{}))><([]([]()", ')')]
        [TestCase("<{([([[(<>()){}]>(<<{{", '>')]
        public void IsValidTest(string line, char c)
        {
            Puzzle.IsValid(line, out var invalid, out var r).Should().BeFalse();
            invalid.Should().Be(c);
        }

        [Test]
        public void Solution2Test()
        {
            _puzzle.Solution2().Should().Be(288957);
        }
    }
}