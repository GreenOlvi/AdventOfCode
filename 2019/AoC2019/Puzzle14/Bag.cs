using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Puzzle14
{
    public class Bag
    {
        public Bag(IDictionary<string, int> sortOrder)
        {
            _sortOrder = sortOrder;
            _bag = new Dictionary<string, long>();
            _queue = new List<string>();
            _ore = 0;
        }

        private readonly List<string> _queue;
        private readonly Dictionary<string, long> _bag;
        private readonly IDictionary<string, int> _sortOrder;
        private long _ore;

        public void Add((string Name, long Amount) chem)
        {
            var (name, amount) = chem;
            if (name == "ORE")
            {
                _ore += amount;
            }
            else
            {
                if (_bag.TryGetValue(name, out var has))
                {
                    _bag[name] = has + amount;
                }
                else
                {
                    _bag.Add(name, amount);
                    _queue.Add(name);
                }
            }
        }

        public void AddRange(IEnumerable<(string Name, long Amount)> chems)
        {
            foreach (var ch in chems)
            {
                Add(ch);
            }
        }

        public bool QueueEmpty()
        {
            return !_bag.Where(p => p.Value > 0).Any();
        }

        public (string Name, long Amount) Dequeue()
        {
            var item = _queue.OrderBy(i => _sortOrder[i]).First(i => _bag[i] > 0);
            _queue.Remove(item);
            var pair = (item, _bag[item]);
            _bag.Remove(item);
            return pair;
            
        }

        public IEnumerable<(string Name, long Amount)> Chems()
        {
            return new[] { ("ORE", _ore) }
                .Concat(_bag.Select(p => (p.Key, p.Value)).OrderBy(p => p.Key));
        }
    }
}
