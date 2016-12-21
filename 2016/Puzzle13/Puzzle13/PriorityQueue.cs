using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzle13
{
    class PriorityQueue<T>
    {
        private readonly List<Tuple<T, IComparable>> _list;
        private readonly Func<T, IComparable> _attribute;
        public int Count => _list.Count;

        public PriorityQueue(Func<T, IComparable> attribute)
        {
            _list = new List<Tuple<T, IComparable>>();
            _attribute = attribute;
        }

        public void Enqueue(T item)
        {
            _list.Add(new Tuple<T, IComparable>(item, _attribute(item)));
        }

        public void EnqueueRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Enqueue(item);
            }
        }

        public T Dequeue()
        {
            if (!_list.Any())
                return default(T);

            var item = _list.OrderBy(t => t.Item2).First();
            _list.Remove(item);
            return item.Item1;
        }

        public bool Any()
        {
            return Count > 0;
        }
    }
}
