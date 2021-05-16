// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Datasets;

namespace SourceAFIS.Cli.Benchmarks
{
    class TimingStats
    {
        public int Threads = 1;
        public Dictionary<string, TimingSummary[]> Segments;
        public OperationTiming[] Sample;
        public static TimingStats Sum(int size, TimingStats[] list)
        {
            var sum = new TimingStats();
            sum.Threads = list.Select(s => s.Threads).Sum();
            sum.Segments = TimingSummary.Aggregate(list.Select(s => s.Segments));
            sum.Sample = OperationTiming.Sample(size, Enumerable.Range(0, list.Length).Select(n =>
            {
                var totals = TimingSummary.SumAll(list.SelectMany(s => s.Segments.Values).SelectMany(a => a));
                return (totals, list[n].Sample);
            }).ToArray());
            return sum;
        }
        public TimingStats Skip(int seconds)
        {
            var result = new TimingStats();
            result.Threads = Threads;
            result.Segments = Segments.ToDictionary(e => e.Key, e => e.Value.Skip(seconds).ToArray());
            result.Sample = Sample.Where(s => s.End >= seconds).ToArray();
            return result;
        }
        public TimingStats Narrow(Profile profile)
        {
            var names = profile.Datasets.Select(ds => ds.Name).ToHashSet();
            var result = new TimingStats();
            result.Threads = Threads;
            result.Segments = Segments.Where(e => names.Contains(e.Key)).ToDictionary(e => e.Key, e => e.Value);
            result.Sample = Sample.Where(s => names.Contains(s.Dataset)).ToArray();
            return result;
        }
    }
}
