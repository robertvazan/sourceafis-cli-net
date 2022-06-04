// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;

namespace SourceAFIS.Cli.Benchmarks
{
    record FootprintStats(int Count, double Serialized, double Memory, double Minutiae)
    {
        public static FootprintStats Sum(IEnumerable<FootprintStats> list)
        {
            return new FootprintStats(
                list.Sum(s => s.Count),
                list.Sum(s => s.Serialized),
                list.Sum(s => s.Memory),
                list.Sum(s => s.Minutiae)
            );
        }
    }
}
