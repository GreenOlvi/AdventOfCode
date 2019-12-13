using System;
using System.Collections.Generic;

namespace AoC2019.Puzzle12
{
    public class CycleFinder<TElement, THash>
        where TElement : notnull
        where THash : notnull
    {
        public CycleFinder(Func<TElement, THash> hashFunc)
        {
            _hashFunc = hashFunc;
        }

        public bool Found { get; private set; } = false;
        public long CycleLength { get; private set; }

        private readonly Func<TElement, THash> _hashFunc;
        private readonly Dictionary<THash, long> _visited = new Dictionary<THash, long>(); 
        private long _i = 0;

        public void Add(TElement element)
        {
            var hash = _hashFunc(element);
            if (_visited.TryGetValue(hash, out var first))
            {
                if (!Found)
                {
                    Found = true;
                    CycleLength = _i - first;
                }
            }
            else
            {
                _visited.Add(hash, _i);
            }
            _i++;
        }
    }
}
