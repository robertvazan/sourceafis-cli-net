// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Outputs;

namespace SourceAFIS.Cli.Benchmarks
{
    abstract record MatchSpeedCommand : SpeedCommand<FingerprintPair>
    {
        public const int Batch = 1000;
        public const int RamFootprint = 200_000_000;
        protected abstract bool Filter(FingerprintPair pair);
        protected override Dataset Dataset(FingerprintPair pair) => pair.Dataset;
        protected override FingerprintPair[] Shuffle()
        {
            return Shuffle(Fingerprint.All).SelectMany(p =>
            {
                var pairs = Shuffle(p.Dataset.Fingerprints)
                    .Select(c => new FingerprintPair(p, c))
                    .Where(pair => Filter(pair))
                    .ToArray();
                return Enumerable.Repeat(pairs, Batch).SelectMany(s => s).Take(Batch);
            }).ToArray();
        }
        class TimedMatchOperation : TimedOperation<FingerprintPair>
        {
            readonly Dictionary<Fingerprint, FingerprintTemplate[]> Templates;
            readonly Dictionary<Dataset, double[][]> Scores;
            FingerprintPair Pair;
            FingerprintMatcher Matcher;
            FingerprintTemplate Candidate;
            double Score;
            Random Random = new Random();
            public TimedMatchOperation(Dictionary<Fingerprint, FingerprintTemplate[]> templates, Dictionary<Dataset, double[][]> scores)
            {
                Templates = templates;
                Scores = scores;
            }
            public override void Prepare(FingerprintPair pair)
            {
                if (Matcher == null || Pair.Probe != pair.Probe)
                    Matcher = new FingerprintMatcher(Templates[pair.Probe][0]);
                var alternatives = Templates[pair.Candidate];
                Candidate = alternatives[Random.Next(alternatives.Length)];
                Pair = pair;
            }
            public override void Execute() => Score = Matcher.Match(Candidate);
            public override bool Verify() => Scores[Pair.Dataset][Pair.ProbeId][Pair.CandidateId] == Score;
        }
        public override TimingData Measure()
        {
            return Measure(() =>
            {
                var footprint = new FootprintCommand().Sum();
                int ballooning = Math.Max(1, (int)(RamFootprint / (footprint.Memory / footprint.Count * Fingerprint.All.Length)));
                var templates = Fingerprint.All.ToDictionary(fp => fp,
                    fp => Enumerable.Range(0, ballooning).Select(n => TemplateCache.Deserialize(fp)).ToArray());
                var scores = Inputs.Dataset.All.ToDictionary(ds => ds, ds => ScoreCache.Load(ds));
                return () => new TimedMatchOperation(templates, scores);
            });
        }
    }
}
