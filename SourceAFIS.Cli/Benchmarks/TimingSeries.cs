// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Inputs;

namespace SourceAFIS.Cli.Benchmarks
{
    static class TimingSeries
    {
        public const int Duration = 60;
        public const int WarmupTime = 20;
        public const int NetDuration = Duration - WarmupTime;
        public static int DurationOf(Dictionary<string, TimingSummary[]> series) => series.Values.Min(s => s.Length);
        public static Dictionary<string, TimingSummary[]> Sum(IEnumerable<Dictionary<string, TimingSummary[]>> list)
        {
            var datasets = list.SelectMany(s => s.Keys).Distinct().ToHashSet();
            var duration = list.Min(DurationOf);
            return datasets.ToDictionary(ds => ds, dataset => Enumerable.Range(0, duration)
                    .Select(interval => TimingSummary.Sum(list.Where(s => s.ContainsKey(dataset)).Select(s => s[dataset][interval])))
                    .ToArray());
        }
        public static Dictionary<string, TimingSummary[]> Warmup(Dictionary<string, TimingSummary[]> series)
        {
            return series.ToDictionary(e => e.Key, e => e.Value.Skip(TimingSeries.WarmupTime).ToArray());
        }
        public static Dictionary<string, TimingSummary[]> Narrow(Dictionary<string, TimingSummary[]> series, Profile profile)
        {
            var names = profile.Datasets.Select(ds => ds.Name).ToHashSet();
            return series.Where(e => names.Contains(e.Key)).ToDictionary(e => e.Key, e => e.Value);
        }
        public static TimingSummary Summary(Dictionary<string, TimingSummary[]> series) => TimingSummary.Sum(series.Values.SelectMany(a => a));
    }
}
