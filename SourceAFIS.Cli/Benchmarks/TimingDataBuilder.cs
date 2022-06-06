// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Inputs;

namespace SourceAFIS.Cli.Benchmarks
{
    class TimingDataBuilder
    {
        readonly TimingSeriesBuilder Summaries;
        readonly TimingSampleBuilder Sample;
        public TimingDataBuilder(long epoch)
        {
            Summaries = new TimingSeriesBuilder(epoch);
            Sample = new TimingSampleBuilder(epoch);
        }
        public bool Add(Dataset dataset, long start, long end)
        {
            if (Summaries.Add(dataset, start, end))
            {
                Sample.Add(dataset, start, end);
                return true;
            }
            else
                return false;
        }
        public TimingData Build() => new TimingData(1, Summaries.Build(), Sample.Build());
    }
}
