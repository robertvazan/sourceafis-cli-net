// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Inputs;

namespace SourceAFIS.Cli.Benchmarks
{
    class TimingRecorder
    {
        readonly SummaryRecorder Summaries;
        readonly SampleRecorder Sample;
        public TimingRecorder(long epoch, int seconds, int measurements)
        {
            Summaries = new SummaryRecorder(epoch, seconds);
            Sample = new SampleRecorder(epoch, measurements);
        }
        public bool Record(Dataset dataset, long start, long end)
        {
            if (Summaries.Record(dataset, start, end))
            {
                Sample.Record(dataset, start, end);
                return true;
            }
            else
                return false;
        }
        public TimingStats Complete()
        {
            var stats = new TimingStats();
            stats.Segments = Summaries.Complete();
            stats.Sample = Sample.Complete();
            return stats;
        }
    }
}
