// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;

namespace SourceAFIS.Cli.Benchmarks
{
    class FootprintStats
    {
        public int Count;
        public double Serialized;
        public double Memory;
        public double Minutiae;
        public static FootprintStats Sum(IEnumerable<FootprintStats> list)
        {
            var sum = new FootprintStats();
            foreach (var stats in list)
            {
                sum.Count += stats.Count;
                sum.Serialized += stats.Serialized;
                sum.Memory += stats.Memory;
                sum.Minutiae += stats.Minutiae;
            }
            return sum;
        }
    }
}
