// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Datasets;

namespace SourceAFIS.Cli.Benchmarks
{
    abstract class SoloSpeed : SpeedBenchmark<Fingerprint>
    {
        protected override Dataset Dataset(Fingerprint fp) => fp.Dataset;
        protected override Fingerprint[] Shuffle() => Shuffle(Fingerprint.All);
    }
}
