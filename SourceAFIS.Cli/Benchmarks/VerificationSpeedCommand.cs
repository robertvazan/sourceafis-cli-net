// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Outputs;
using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Benchmarks
{
    record VerificationSpeedCommand : SpeedCommand<FingerprintPair>
    {
        public override string Name => "verification";
        public override string Description => "Measure speed of verification, i.e. creating FingerprintMatcher and calling Match() with matching candidate.";
        protected override Sampler<FingerprintPair> Sampler() => new VerificationSampler();
        class VerificationOperation : TimedOperation<FingerprintPair>
        {
            readonly Dictionary<Fingerprint, FingerprintTemplate> templates;
            FingerprintTemplate probe;
            FingerprintTemplate candidate;
            double score;
            public VerificationOperation(Dictionary<Fingerprint, FingerprintTemplate> templates) => this.templates = templates;
            public override void Prepare(FingerprintPair pair)
            {
                probe = templates[pair.Probe];
                candidate = templates[pair.Candidate];
            }
            public override void Execute() => score = new FingerprintMatcher(probe).Match(candidate);
            public override void Blackhole(Hasher hasher) => hasher.Add(score);
        }
        public override TimingData Measure()
        {
            return Measure(() =>
            {
                var templates = Fingerprint.All.ToDictionary(fp => fp, fp => TemplateCache.Deserialize(fp));
                return () => new VerificationOperation(templates);
            });
        }
    }
}
