using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOC2020.Day25
{
    public class Puzzle : PuzzleBase<long, int>
    {
        public Puzzle(IEnumerable<long> input)
        {
            _pubKeys = input.ToArray();
        }

        private readonly long[] _pubKeys;

        private const long _initialSubjectNumber = 7;
        private const long _base = 20201227;

        private static async IAsyncEnumerable<(long Value, long LoopSize)> FindLoopSizes(IEnumerable<long> publicKeys)
        {
            var pubKeys = new HashSet<long>(publicKeys);
            var val = 1L;
            var loop = 0L;

            while (pubKeys.Any())
            {
                (val, loop) = await Task.Run(() => FindNextLoopSize(val, loop, _initialSubjectNumber, pubKeys));
                pubKeys.Remove(val);
                yield return (val, loop);
            }
        }

        private static (long Value, long LoopSize) FindNextLoopSize(long startVal, long startLoop, long subject, HashSet<long> pubKeys)
        {
            if (!pubKeys.Any())
            {
                throw new InvalidOperationException("Collection is empty");
            }

            var val = startVal;
            var loop = startLoop;
            while (!pubKeys.Contains(val))
            {
                val = (val * subject).Modulo(_base);
                loop++;
            }

            return (val, loop);
        }

        private static long Transform(long subject, long loopSize)
        {
            var val = 1L;
            for (var i = 0L; i < loopSize; i++)
            {
                val = (val * subject).Modulo(_base);
            }
            return val;
        }

        public override long Solution1()
        {
            var loopSizes = FindLoopSizes(_pubKeys).ToArrayAsync().GetAwaiter().GetResult();
            return Transform(loopSizes[0].Value, loopSizes[1].LoopSize);
        }

        public override int Solution2() => 0;
    }
}
