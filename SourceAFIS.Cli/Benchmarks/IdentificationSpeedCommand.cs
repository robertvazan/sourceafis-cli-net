// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Outputs;
using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Benchmarks
{
    record IdentificationSpeedCommand : SpeedCommand<CrossDatasetPair>
    {
        public const int RamFootprint = 200_000_000;
        public override string Name => "identification";
        public override string Description => "Measure speed of identification, i.e. calling Match() with non-matching candidate.";
        protected override Sampler<CrossDatasetPair> Sampler() => new IdentificationSampler();
        class IdentificationOperation : TimedOperation<CrossDatasetPair>
        {
            readonly Dictionary<Fingerprint, FingerprintTemplate[]> templates;
            CrossDatasetPair pair;
            FingerprintMatcher matcher;
            FingerprintTemplate candidate;
            double score;
            Random random = new Random();
            public IdentificationOperation(Dictionary<Fingerprint, FingerprintTemplate[]> templates) => this.templates = templates;
            public override void Prepare(CrossDatasetPair pair)
            {
                if (matcher == null || this.pair.Probe != pair.Probe)
                    matcher = new FingerprintMatcher(templates[pair.Probe][0]);
                var alternatives = templates[pair.Candidate];
                candidate = alternatives[random.Next(alternatives.Length)];
                this.pair = pair;
            }
            public override void Execute() => score = matcher.Match(candidate);
            public override void Blackhole(Hasher hasher) => hasher.Add(score);
        }
        public override TimingData Measure()
        {
            return Measure(() =>
            {
                var footprint = new FootprintCommand().Sum();
                int ballooning = Math.Max(1, (int)(RamFootprint / (footprint.Memory * Fingerprint.All.Length)));
                var templates = Fingerprint.All.ToDictionary(fp => fp,
                    fp => Enumerable.Range(0, ballooning).Select(n => TemplateCache.Deserialize(fp)).ToArray());
                return () => new IdentificationOperation(templates);
            });
        }
    }
}
