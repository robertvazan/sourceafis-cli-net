// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;

namespace SourceAFIS.Cli.Inputs
{
    class IdentificationSampler : Sampler<CrossDatasetPair>
    {
        // 100ms is enough to drown out probe overhead.
        static readonly long BatchNanos = 100_000_000;
        // Current algorithm costs about 140us per candidate on dev hardware. This does not need to be precise.
        static readonly int Batch = (int)(BatchNanos / 140_000);
        readonly Random random = new Random();
        readonly Fingerprint[] fingerprints;
        Fingerprint probe;
        int remaining;
        public IdentificationSampler(Profile profile) => fingerprints = profile.Fingerprints;
        public IdentificationSampler() : this(Profile.Everything) { }
        public CrossDatasetPair Next()
        {
            if (remaining <= 0)
            {
                probe = fingerprints[random.Next(fingerprints.Length)];
                remaining = Batch;
            }
            --remaining;
            while (true)
            {
                var candidate = fingerprints[random.Next(fingerprints.Length)];
                if (probe.Finger != candidate.Finger)
                    return new CrossDatasetPair(probe, candidate);
            }
        }
        public Dataset Dataset(CrossDatasetPair pair) => random.Next(2) == 0 ? pair.Probe.Dataset : pair.Candidate.Dataset;
    }
}
