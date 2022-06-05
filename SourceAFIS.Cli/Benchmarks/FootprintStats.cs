// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;

namespace SourceAFIS.Cli.Benchmarks
{
    record FootprintStats(double Serialized, double Memory, double Minutiae)
    {
        public static FootprintStats Sum(IEnumerable<FootprintStats> list)
        {
            return new FootprintStats(
                list.Average(s => s.Serialized),
                list.Average(s => s.Memory),
                list.Average(s => s.Minutiae)
            );
        }
    }
}
