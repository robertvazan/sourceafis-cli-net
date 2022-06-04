// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;

namespace SourceAFIS.Cli.Benchmarks
{
    record AccuracyStats(double Eer, double Fmr100, double Fmr1K, double Fmr10K)
    {
        public static AccuracyStats Sum(IEnumerable<AccuracyStats> list)
        {
            return new AccuracyStats(
                list.Average(s => s.Eer),
                list.Average(s => s.Fmr100),
                list.Average(s => s.Fmr1K),
                list.Average(s => s.Fmr10K)
            );
        }
    }
}
