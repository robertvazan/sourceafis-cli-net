// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Inputs
{
    readonly record struct FingerprintPair(Dataset Dataset, ushort ProbeId, ushort CandidateId) : DataIdentifier
    {
        public FingerprintPair(Fingerprint probe, Fingerprint candidate) : this(probe.Dataset, probe.Id, candidate.Id)
        {
            if (probe.Dataset != candidate.Dataset)
                throw new ArgumentException();
        }
        public Fingerprint Probe => new Fingerprint(Dataset, ProbeId);
        public Fingerprint Candidate => new Fingerprint(Dataset, CandidateId);
        public string Path => System.IO.Path.Combine(Probe.Path, Candidate.Name);
    }
}
