// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Inputs;

namespace SourceAFIS.Cli.Benchmarks
{
    record TimingData(int Threads, Dictionary<string, TimingSummary[]> Series, TimingMeasurement[] Sample)
    {
        public static TimingData Sum(TimingData[] list)
        {
            return new TimingData(
                list.Sum(s => s.Threads),
                TimingSeries.Sum(list.Select(s => s.Series)),
                TimingSample.Sum(list.Select(s => s.Sample))
            );
        }
        public TimingData Warmup() => new TimingData(Threads, TimingSeries.Warmup(Series), TimingSample.Warmup(Sample));
        public TimingData Narrow(Profile profile) => new TimingData(Threads, TimingSeries.Narrow(Series, profile), TimingSample.Narrow(Sample, profile));
    }
}
