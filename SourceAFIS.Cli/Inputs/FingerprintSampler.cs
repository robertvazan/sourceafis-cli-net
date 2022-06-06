// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;

namespace SourceAFIS.Cli.Inputs
{
    class FingerprintSampler : Sampler<Fingerprint>
    {
        readonly Random random = new Random();
        readonly Fingerprint[] fingerprints;
        public FingerprintSampler(Profile profile) => fingerprints = profile.Fingerprints;
        public FingerprintSampler() : this(Profile.Everything) { }
        public Fingerprint Next() => fingerprints[random.Next(fingerprints.Length)];
        public Dataset Dataset(Fingerprint fp) => fp.Dataset;

    }
}
