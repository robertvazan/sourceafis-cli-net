// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Datasets;

namespace SourceAFIS.Cli.Benchmarks
{
    class IdentificationSpeed : MatchSpeed
    {
        public override string Name => "identification";
        public override string Description => "Measure speed of identification, i.e. calling match() with non-matching candidate.";
        protected override bool Filter(FingerprintPair pair) => pair.Probe.Finger != pair.Candidate.Finger;
    }
}
