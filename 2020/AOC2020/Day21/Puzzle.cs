using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2020.Day21
{
    public class Puzzle : PuzzleBase<int, string>
    {
        public Puzzle(IEnumerable<string> input)
        {
            _input = input.Select(ParseFood).ToArray();
        }

        private readonly Food[] _input;

        private record Food(string[] Ingredients, string[] Allergens);

        private static readonly Regex FoodRegex = new Regex(@"^(?<ingredients>.+)\s\(contains (?<allergens>.+)\)$");
        private static Food ParseFood(string line)
        {
            if (!FoodRegex.TryMatch(line, out var match))
            {
                throw new PuzzleException("Invalid line format");
            }

            var ingredients = match.Groups["ingredients"].Value.Split(" ", System.StringSplitOptions.TrimEntries);
            var allergens = match.Groups["allergens"].Value.Split(", ", System.StringSplitOptions.TrimEntries);

            return new Food(ingredients, allergens);
        }

        private static Dictionary<string, Food[]> GetFoodByAllergen(IEnumerable<Food> food) =>
            food.SelectMany(f => f.Allergens.Select(a => (a, f)))
                .GroupBy(p => p.a)
                .ToDictionary(g => g.Key, g => g.Select(p => p.f).ToArray());

        private static IEnumerable<string> GetIntersecting(IEnumerable<IEnumerable<string>> ingredients) =>
            ingredients.Aggregate((a, b) => a.Intersect(b));

        public override int Solution1()
        {
            var knownIngredients = MatchIngredientsToAllergens();
            return _input.SelectMany(f => f.Ingredients)
                .Count(i => !knownIngredients.ContainsKey(i));
        }

        private Dictionary<string, string> MatchIngredientsToAllergens()
        {
            var knownIngredients = new Dictionary<string, string>();

            var dict = GetFoodByAllergen(_input);
            var unmatchedAllergens = new Queue<string>(dict.Keys);

            while (unmatchedAllergens.Any())
            {
                var allergen = unmatchedAllergens.Dequeue();
                var commonIngredients = GetIntersecting(
                    dict[allergen].Select(f => f.Ingredients.Where(i => !knownIngredients.ContainsKey(i))))
                    .ToArray();

                if (commonIngredients.Length == 1)
                {
                    knownIngredients.Add(commonIngredients.First(), allergen);
                }
                else
                {
                    unmatchedAllergens.Enqueue(allergen);
                }
            }

            return knownIngredients;
        }

        public override string Solution2()
        {
            var ingredients = MatchIngredientsToAllergens()
                .Select(kv => (kv.Key, kv.Value))
                .OrderBy(p => p.Value)
                .Select(p => p.Key);

            return string.Join(",", ingredients);
        }
    }
}
