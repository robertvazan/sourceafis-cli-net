// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Inputs;

namespace SourceAFIS.Cli.Benchmarks
{
    record VerificationSpeedCommand : MatchSpeedCommand
    {
        public override string Name => "verification";
        public override string Description => "Measure speed of verification, i.e. calling match() with matching candidate.";
        protected override bool Filter(FingerprintPair pair) => pair.Probe.Finger == pair.Candidate.Finger && pair.Probe != pair.Candidate;
    }
}
