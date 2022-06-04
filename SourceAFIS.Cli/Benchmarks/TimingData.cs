// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Inputs;

namespace SourceAFIS.Cli.Benchmarks
{
    record TimingData(int Threads, Dictionary<string, TimingSummary[]> Series, TimingMeasurement[] Sample)
    {
        public static TimingData Sum(int size, TimingData[] list)
        {
            return new TimingData(
                list.Sum(s => s.Threads),
                TimingSummary.Aggregate(list.Select(s => s.Series)),
                TimingMeasurement.Sample(size, Enumerable.Range(0, list.Length).Select(n =>
                {
                    var totals = TimingSummary.SumAll(list.SelectMany(s => s.Series.Values).SelectMany(a => a));
                    return (totals, list[n].Sample);
                }).ToArray())
            );
        }
        public TimingData Skip(int seconds)
        {
            return new TimingData(
                Threads,
                Series.ToDictionary(e => e.Key, e => e.Value.Skip(seconds).ToArray()),
                Sample.Where(s => s.End >= seconds).ToArray()
            );
        }
        public TimingData Narrow(Profile profile)
        {
            var names = profile.Datasets.Select(ds => ds.Name).ToHashSet();
            return new TimingData(
                Threads,
                Series.Where(e => names.Contains(e.Key)).ToDictionary(e => e.Key, e => e.Value),
                Sample.Where(s => names.Contains(s.Dataset)).ToArray()
            );
        }
    }
}
