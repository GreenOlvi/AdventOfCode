using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzle15
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var filename in args)
            {
                var ingredients = Ingredient.FromFile(filename).ToArray();

                var max = GetAmounts().Select(a => Ingredient.Score(a, ingredients)).Max();
                Console.WriteLine("Best score: {0}", max);

                var max500 = GetAmounts().Where(a => Ingredient.SumCalories(a, ingredients) == 500).Select(a => Ingredient.Score(a, ingredients)).Max();
                Console.WriteLine("Best score (500 calories): {0}", max500);
            }

            Console.ReadLine();
        }

        private static IEnumerable<int[]> GetAmounts()
        {
            return from i in Enumerable.Range(0, 100)
                   from j in Enumerable.Range(0, 100 - i)
                   from k in Enumerable.Range(0, 100 - i - j)
                   select new[] { i, j, k, 100 - i - j - k };
        }

        private class Ingredient
        {
            public static IEnumerable<Ingredient> FromFile(string filename)
            {
                var ingredients = new List<Ingredient>();

                using (var reader = new StreamReader(filename))
                {
                    while (!reader.EndOfStream)
                    {
                        ingredients.Add(new Ingredient(reader.ReadLine()));
                    }
                }

                return ingredients;
            }

            public string Name { get; }
            public int Capacity { get; }
            public int Durability { get; }
            public int Flavor { get; }
            public int Texture { get; }
            public int Calories { get; }

            private static readonly Regex LineRegex =
                new Regex(@"^(?<name>\w+): capacity (?<capacity>-?\d+), durability (?<durability>-?\d+), flavor (?<flavor>-?\d+), texture (?<texture>-?\d+), calories (?<calories>-?\d+)$");

            public Ingredient(string input)
            {
                var match = LineRegex.Match(input);

                if (!match.Success)
                    throw new ArgumentException("Invalid input format!", "input");

                Name = match.Groups["name"].Value;
                Capacity = int.Parse(match.Groups["capacity"].Value);
                Durability = int.Parse(match.Groups["durability"].Value);
                Flavor = int.Parse(match.Groups["flavor"].Value);
                Texture = int.Parse(match.Groups["texture"].Value);
                Calories = int.Parse(match.Groups["calories"].Value);
            }

            public override string ToString()
            {
                return string.Format(@"{0}: [{1}, {2}, {3}, {4}, {5}]", Name, Capacity, Durability, Flavor, Texture, Calories);
            }

            public static long Score(int[] amounts, Ingredient[] ingredients)
            {
                long capacity = Math.Max(ingredients.Select((x, i) => x.Capacity * amounts[i]).Sum(), 0);
                long durability = Math.Max(ingredients.Select((x, i) => x.Durability * amounts[i]).Sum(), 0);
                long flavour = Math.Max(ingredients.Select((x, i) => x.Flavor * amounts[i]).Sum(), 0);
                long texture = Math.Max(ingredients.Select((x, i) => x.Texture * amounts[i]).Sum(), 0);

                return capacity*durability*flavour*texture;
            }

            public static long SumCalories(int[] amounts, Ingredient[] ingredients)
            {
                return ingredients.Select((x, i) => x.Calories * amounts[i]).Sum();
            }
        }
    }
}
