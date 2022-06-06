// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Inputs;

namespace SourceAFIS.Cli.Benchmarks
{
    static class TimingSample
    {
        public const int SampleSize = 10_000;
        // We can assume that strata will be about the same size,
        // because we are compacting sample buffer at the end of sample collection.
        public static TimingMeasurement[] Sum(IEnumerable<TimingMeasurement[]> list)
        {
            var remaining = list.SelectMany(s => s).ToList();
            if (remaining.Count <= SampleSize)
                return remaining.ToArray();
            var selected = new TimingMeasurement[SampleSize];
            var random = new Random();
            for (int i = 0; i < SampleSize; ++i)
            {
                var next = random.Next(remaining.Count);
                selected[i] = remaining[next];
                remaining[next] = remaining[^1];
                remaining.RemoveAt(remaining.Count - 1);
            }
            return selected;
        }
        public static TimingMeasurement[] Warmup(TimingMeasurement[] sample) => sample.Where(s => s.End >= TimingSeries.WarmupTime).ToArray();
        public static TimingMeasurement[] Narrow(TimingMeasurement[] sample, Profile profile)
        {
            var names = profile.Datasets.Select(ds => ds.Name).ToHashSet();
            return sample.Where(s => names.Contains(s.Dataset)).ToArray();
        }
    }
}
