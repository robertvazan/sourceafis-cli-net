// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Inputs;

namespace SourceAFIS.Cli.Benchmarks
{
    abstract record SoloSpeedCommand : SpeedCommand<Fingerprint>
    {
        protected override Dataset Dataset(Fingerprint fp) => fp.Dataset;
        protected override Fingerprint[] Shuffle() => Shuffle(Fingerprint.All);
    }
}
