using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2019.Puzzle14
{
    public partial class Solution : IPuzzle
    {
        public Solution(IEnumerable<string> input)
        {
            _input = input.Select(ParseRecipe).ToList();
        }

        public static Recipe ParseRecipe(string line)
        {
            var l = line.Split(" => ", StringSplitOptions.RemoveEmptyEntries);

            var result = ParsePart(l[1]);
            var ingredients = l[0].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(ParsePart);
            return new Recipe(result, ingredients);
        }

        private static readonly Regex PartRegex = new Regex(@"(?<amount>\d+)\s(?<name>\w+)");
        private static (string Name, long Amount) ParsePart(string part)
        {
            var match = PartRegex.Match(part);
            if (!match.Success)
            {
                throw new ArgumentException($"Invalid part format '{part}'", nameof(part));
            }
            return (match.Groups["name"].Value, int.Parse(match.Groups["amount"].Value));
        }

        private readonly List<Recipe> _input;

        public static long Solve1(IEnumerable<Recipe> input)
        {
            var chemBook = new ChemBook(input);
            return chemBook.ProduceFuelFromOre();
        }

        public static long Solve2(IEnumerable<Recipe> input, long ore)
        {
            var chembook = new ChemBook(input);
            var n = ore / chembook.ProduceFuelFromOre();
            var d = n / 2;

            while (true)
            {
                var req = chembook.ProduceFuelFromOre(n);

                if (req == ore)
                {
                    break;
                }

                d /= 2;
                n = req > ore ? n - d : n + d;

                if (d == 1)
                {
                    break;
                }
            }

            var oreReq = chembook.ProduceFuelFromOre(n);
            if (oreReq < ore)
            {
                do
                {
                    n++;
                    oreReq = chembook.ProduceFuelFromOre(n);
                }
                while (oreReq < ore);
                n--;
            }
            else
            {
                do
                {
                    n--;
                    oreReq = chembook.ProduceFuelFromOre(n);
                }
                while (oreReq > ore);
            }

            return n;
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_input).ToString());

        public Task<string> Solve2Async() =>
            Task.Run(() => Solve2(_input, 1_000_000_000_000).ToString());
    }
}
