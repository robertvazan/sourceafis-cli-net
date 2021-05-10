// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;

namespace SourceAFIS.Cli.Datasets
{
    readonly struct Finger : IEquatable<Finger>
    {
        public readonly Dataset Dataset;
        public readonly int Id;
        public Finger(Dataset dataset, int id)
        {
            Dataset = dataset;
            Id = id;
        }
        public bool Equals(Finger other) => Dataset == other.Dataset && Id == other.Id;
        public override bool Equals(object other) => other is Finger && Equals((Finger)other);
        public static bool operator ==(Finger left, Finger right) => left.Equals(right);
        public static bool operator !=(Finger left, Finger right) => !left.Equals(right);
        public override int GetHashCode() => 31 * Dataset.GetHashCode() + Id;
    }
}
