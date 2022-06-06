// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Benchmarks
{
    record TimingMeasurement(string Dataset, double Start, double End)
    {
        public double Duration => End - Start;
    }
}
