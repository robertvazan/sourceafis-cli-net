// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;

namespace SourceAFIS.Cli.Benchmarks
{
    record TimingSummary(long Count, double Sum, double Min, double Max)
    {
        public static TimingSummary SumAll(IEnumerable<TimingSummary> list)
        {
            return new TimingSummary(
                list.Sum(s => s.Count),
                list.Sum(s => s.Sum),
                list.Where(s => s.Count > 0).Min(s => (double?)s.Min) ?? 0,
                list.Max(s => (double?)s.Max) ?? 0
            );
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
