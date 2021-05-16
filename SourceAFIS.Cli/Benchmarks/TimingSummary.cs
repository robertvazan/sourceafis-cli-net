// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;

namespace SourceAFIS.Cli.Benchmarks
{
    class TimingSummary
    {
        public long Count;
        public double Sum;
        public double Max;
        public double Min;
        public static TimingSummary SumAll(IEnumerable<TimingSummary> list)
        {
            var sum = new TimingSummary();
            sum.Count = list.Select(s => s.Count).Sum();
            sum.Sum = list.Select(s => s.Sum).Sum();
            sum.Max = list.Select(s => (double?)s.Max).Max() ?? 0;
            sum.Min = list.Where(s => s.Count > 0).Select(s => (double?)s.Min).Min() ?? 0;
            return sum;
        }
        public static Dictionary<string, TimingSummary[]> Aggregate(IEnumerable<Dictionary<string, TimingSummary[]>> list)
        {
            var datasets = list.SelectMany(s => s.Keys).Distinct().ToHashSet();
            var seconds = list.SelectMany(s => s.Values.Select(ts => ts.Length)).Min();
            return datasets.ToDictionary(ds => ds, dataset => Enumerable.Range(0, seconds)
                    .Select(interval => SumAll(list.Where(s => s.ContainsKey(dataset)).Select(s => s[dataset][interval])))
                    .ToArray());
        }
    }
}
