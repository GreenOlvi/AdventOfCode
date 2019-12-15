using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Puzzle14
{
    public class ChemBook
    {
        public ChemBook(IEnumerable<Recipe> recipes)
        {
            _recipes = recipes.ToDictionary(r => r.Result.Name, r => r);
            var edges = _recipes.Values.SelectMany(r => r.Ingredients.Select(i => (r.Result.Name, i.Name)));
            _sortOrder = TopoSort.TopologicalSort(edges)
                .Select((n, i) => (n, i))
                .ToDictionary(t => t.n, t => t.i);
        }

        private readonly Dictionary<string, Recipe> _recipes;
        private readonly Dictionary<string, int> _sortOrder;

        public IEnumerable<(string Name, long Amount)> Required(string name, long amount)
        {
            if (!_recipes.TryGetValue(name, out var r))
            {
                return Enumerable.Empty<(string, long)>();
            }

            var factor = (long)Math.Ceiling(amount / (double)r.Result.Amount);

            var required = r.Ingredients.Select(i => (i.Name, i.Amount * factor));

            var leftover = (r.Result.Amount * factor) - amount;
            return leftover > 0
                ? required.Concat(new[] { (name, -leftover) })
                : required;
        }

        public IEnumerable<(string Name, long Amount)> Produce(string name, long amount)
        {
            var bag = new Bag(_sortOrder);
            bag.Add((name, amount));

            while (!bag.QueueEmpty())
            {
                var (n, a) = bag.Dequeue();
                var result = Required(n, a);
                bag.AddRange(result);
            }

            return bag.Chems();
        }

        public long ProduceFuelFromOre(long amount = 1) =>
            Produce("FUEL", amount).First(c => c.Name == "ORE").Amount;
    }
}
