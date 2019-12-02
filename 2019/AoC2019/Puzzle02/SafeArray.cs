using AoC2019.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Puzzle02
{
    public class SafeArray<T>
    {
        private readonly T[] _array;

        public SafeArray(IEnumerable<T> array)
        {
            _array = array.ToArray();
        }

        public IOption<T> Get(int index) =>
            index >= 0 && index < _array.Length
                ? Option<T>.Some(_array[index])
                : (IOption<T>)Option<T>.None();
    }
}
