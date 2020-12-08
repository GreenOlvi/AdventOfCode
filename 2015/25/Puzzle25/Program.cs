using System;
using NUnit.Framework;
using FluentAssertions;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Puzzle25
{
    [TestFixture]
    public class Program
    {
        internal static void Main(string[] args)
        {
            var (row, column) = ParseInput(File.ReadAllLines("input.txt").First());
            Console.WriteLine($"({row}, {column})");

            var id = GetGridId(row, column);
            Console.WriteLine($"Id = {id}");

            var codeVal = 20151125L;
            for (var i = 0L; i < id - 1; i++)
            {
                codeVal = (codeVal * 252533L) % 33554393;
            }

            Console.WriteLine($"Result = {codeVal}");
        }

        private static HashSet<long> Seen = new HashSet<long>();

        private static readonly Regex LineRegex = new Regex(@"To continue, please consult the code grid in the manual.  Enter the code at row (?<row>\d+), column (?<column>\d+).");
        private static (int Row, int Column) ParseInput(string line)
        {
            var m = LineRegex.Match(line);
            if (!m.Success)
            {
                throw new InvalidDataException("Invalid input");
            }
            if (!int.TryParse(m.Groups["row"].Value, out var row) || !int.TryParse(m.Groups["column"].Value, out var column))
            {
                throw new InvalidDataException("Invalid input");
            }
            return (row, column);
        }

        private static long GetGridId(int row, int column) =>
            (column * column + row * row + 2 * column * row - column - 3 * row) / 2 + 1;

        [Test]
        public void GetGridIdTests()
        {
            var expected = new int[][]
            {
                new int[] {  1,  3,  6, 10, 15, 21 },
                new int[] {  2,  5,  9, 14, 20 },
                new int[] {  4,  8, 13, 19 },
                new int[] {  7, 12, 18 },
                new int[] { 11, 17 },
                new int[] { 16 },
            };

            for (var r = 0; r < expected.Length; r++)
            {
                for (var c = 0; c < expected[r].Length; c++)
                {
                    GetGridId(r + 1, c + 1).Should().Be(expected[r][c], $"GetGridId({r + 1}, {c + 1})={expected[r][c]}");
                }
            }
        }
    }
}
