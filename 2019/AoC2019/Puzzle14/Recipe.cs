using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Puzzle14
{
    public struct Recipe
    {
        public Recipe((string, long) result, params (string, long)[] ingredients)
        {
            Result = result;
            Ingredients = ingredients.ToList();
        }

        public Recipe((string, long) result, IEnumerable<(string, long)> ingredients)
        {
            Result = result;
            Ingredients = ingredients.ToList();
        }

        public (string Name, long Amount) Result { get; }
        public List<(string Name, long Amount)> Ingredients { get; }

        public override bool Equals(object? obj)
        {
            return obj is Recipe recipe &&
                Result.Equals(recipe.Result) &&
                Ingredients.SequenceEqual(recipe.Ingredients);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Result, Ingredients);
        }

        public static bool operator ==(Recipe left, Recipe right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Recipe left, Recipe right)
        {
            return !(left == right);
        }

        public override string ToString() =>
            string.Join(", ", Ingredients.Select(ChemToString)) + " => " + ChemToString(Result);

        private static string ChemToString((string Name, long Amount) ch) => $"{ch.Amount} {ch.Name}";
    }
}
