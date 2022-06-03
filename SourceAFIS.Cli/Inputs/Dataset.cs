// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Linq;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Inputs
{
    readonly struct Dataset : DataIdentifier, IEquatable<Dataset>
    {
        public readonly Sample Sample;
        public Dataset(Sample sample)
        {
            Sample = sample;
        }
        public bool Equals(Dataset other) => Sample == other.Sample;
        public override bool Equals(object other) => other is Dataset && Equals((Dataset)other);
        public static bool operator ==(Dataset left, Dataset right) => left.Equals(right);
        public static bool operator !=(Dataset left, Dataset right) => !left.Equals(right);
        public override int GetHashCode() => Sample.GetHashCode();
        public DatasetLayout Layout => DatasetLayout.Get(this);
        public static Dataset[] All => Samples.All().Select(s => new Dataset(s)).ToArray();
        public Fingerprint[] Fingerprints
        {
            get
            {
                var self = this;
                return Enumerable.Range(0, Layout.Fingerprints).Select(n => new Fingerprint(self, n)).ToArray();
            }
        }
        public string Name => Sample.Name();
        public string Path => Name;
    }
}
