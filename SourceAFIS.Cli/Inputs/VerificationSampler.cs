// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Linq;

namespace SourceAFIS.Cli.Inputs
{
    class VerificationSampler : Sampler<FingerprintPair>
    {
        readonly Random random = new Random();
        readonly Fingerprint[] fingerprints;
        public VerificationSampler(Profile profile) => fingerprints = profile.Fingerprints;
        public VerificationSampler() : this(Profile.Everything) { }
        public FingerprintPair Next()
        {
            var probe = fingerprints[random.Next(fingerprints.Length)];
            var candidates = probe.Finger.Fingerprints.Where(c => probe != c).ToArray();
            var candidate = candidates[random.Next(candidates.Length)];
            return new FingerprintPair(probe, candidate);
        }
        public Dataset Dataset(FingerprintPair pair) => pair.Dataset;
    }
}
