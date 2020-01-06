using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AoC2019.Common
{
    public class CacheStats
    {
        private long _hit;
        private long _miss;

        public long Hit => _hit;
        public long Miss => _miss;
        public double HitRatio => Hit / (double)(Hit + Miss);
        public double MissRatio => Miss / (double)(Hit + Miss);

        public void IncHit()
        {
            Interlocked.Increment(ref _hit);
        }

        public void IncMiss()
        {
            Interlocked.Increment(ref _miss);
        }

        public override string ToString() =>
            $"Hit: {Hit} ({(HitRatio * 100):0.##}%), Miss: {Miss} ({(MissRatio * 100):0.##}%)";
    }
}
