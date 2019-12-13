using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AoC2019.Common
{
    public class Memory : IDictionary<long, long>
    {
        public Memory(IEnumerable<long> initial)
        {
            var i = 0;
            foreach (var e in initial)
            {
                _mem[i] = e;
                i++;
            }
        }

        private readonly Dictionary<long, long> _mem = new Dictionary<long, long>();

        public long this[long key]
        {
            get => GetOrAdd(key);
            set => _mem[key] = value;
        }

        public ICollection<long> Keys => _mem.Keys;

        public ICollection<long> Values => _mem.Values;

        public int Count => _mem.Count;

        public bool IsReadOnly => false;

        public long GetOrAdd(long key)
        {
            if (!_mem.ContainsKey(key))
            {
                _mem.Add(key, 0L);
            }
            return _mem[key];
        }

        public void Add(long key, long value) =>
            _mem.Add(key, value);

        public void Add(KeyValuePair<long, long> item) =>
            _mem.Add(item.Key, item.Value);

        public void Clear() =>
            _mem.Clear();

        public bool Contains(KeyValuePair<long, long> item) =>
            _mem.ContainsKey(item.Key) && _mem[item.Key] == item.Value;

        public bool ContainsKey(long key) =>
            _mem.ContainsKey(key);

        public void CopyTo(KeyValuePair<long, long>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<long, long>> GetEnumerator() =>
            _mem.GetEnumerator();

        public bool Remove(long key) =>
            _mem.Remove(key);

        public bool Remove(KeyValuePair<long, long> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(long key, [MaybeNullWhen(false)] out long value) =>
            _mem.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() =>
            ((IEnumerable)_mem).GetEnumerator();
    }
}
