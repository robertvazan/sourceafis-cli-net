// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Benchmarks
{
    record TimingData(
        // Number of cores the benchmark ran on.
        int Threads,
        // Summary statistics by dataset and interval. Intervals are 1s long.
        Dictionary<string, TimingSummary[]> Series,
        // Random sample of all measurements.
        TimingMeasurement[] Sample,
        // Only used for blackholing outputs to prevent the compiler from optimizing the benchmarked operations out.
        byte[] Hash)
    {
        public static TimingData Sum(TimingData[] list)
        {
            return new TimingData(
                list.Sum(s => s.Threads),
                TimingSeries.Sum(list.Select(s => s.Series)),
                TimingSample.Sum(list.Select(s => s.Sample)),
                Hasher.Hash(list, s => s.Hash)
            );
        }
        public TimingData Warmup() => new TimingData(Threads, TimingSeries.Warmup(Series), TimingSample.Warmup(Sample), Hash);
        public TimingData Narrow(Profile profile) => new TimingData(Threads, TimingSeries.Narrow(Series, profile), TimingSample.Narrow(Sample, profile), Hash);
    }
}
