// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;

namespace SourceAFIS.Cli.Benchmarks
{
    class AccuracyStats
    {
        public double Eer;
        public double Fmr100;
        public double Fmr1K;
        public double Fmr10K;
        public static AccuracyStats Sum(IEnumerable<AccuracyStats> list)
        {
            var sum = new AccuracyStats();
            int count = list.Count();
            foreach (var stats in list)
            {
                sum.Eer += stats.Eer / count;
                sum.Fmr100 += stats.Fmr100 / count;
                sum.Fmr1K += stats.Fmr1K / count;
                sum.Fmr10K += stats.Fmr10K / count;
            }
            return sum;
        }
    }
}
