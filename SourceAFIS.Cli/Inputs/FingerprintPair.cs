// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Inputs
{
    readonly struct FingerprintPair : DataIdentifier, IEquatable<FingerprintPair>
    {
        public readonly Dataset Dataset;
        public readonly int ProbeId;
        public readonly int CandidateId;
        public FingerprintPair(Fingerprint probe, Fingerprint candidate)
        {
            if (probe.Dataset != candidate.Dataset)
                throw new ArgumentException();
            Dataset = probe.Dataset;
            ProbeId = probe.Id;
            CandidateId = candidate.Id;
        }
        public bool Equals(FingerprintPair other) => Dataset == other.Dataset && ProbeId == other.ProbeId && CandidateId == other.CandidateId;
        public override bool Equals(object other) => other is FingerprintPair && Equals((FingerprintPair)other);
        public static bool operator ==(FingerprintPair left, FingerprintPair right) => left.Equals(right);
        public static bool operator !=(FingerprintPair left, FingerprintPair right) => !left.Equals(right);
        public override int GetHashCode() => 31 * (31 * Dataset.GetHashCode() + ProbeId) + CandidateId;
        public Fingerprint Probe => new Fingerprint(Dataset, ProbeId);
        public Fingerprint Candidate => new Fingerprint(Dataset, CandidateId);
        public string Path => System.IO.Path.Combine(Probe.Path, Candidate.Name);
    }
}
