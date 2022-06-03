// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Inputs
{
    readonly struct Fingerprint : DataIdentifier, IEquatable<Fingerprint>
    {
        public readonly Dataset Dataset;
        public readonly int Id;
        public Fingerprint(Dataset dataset, int id)
        {
            Dataset = dataset;
            Id = id;
        }
        public bool Equals(Fingerprint other) => Dataset == other.Dataset && Id == other.Id;
        public override bool Equals(object other) => other is Fingerprint && Equals((Fingerprint)other);
        public static bool operator ==(Fingerprint left, Fingerprint right) => left.Equals(right);
        public static bool operator !=(Fingerprint left, Fingerprint right) => !left.Equals(right);
        public override int GetHashCode() => 31 * Dataset.GetHashCode() + Id;
        public string Name => Dataset.Layout.Name(Id);
        public string Path => System.IO.Path.Combine(Dataset.Path, Name);
        public Finger Finger => new Finger(Dataset, Dataset.Layout.Finger(Id));
        public static Fingerprint[] All => Dataset.All.SelectMany(ds => ds.Fingerprints).ToArray();
        public byte[] Load() => File.ReadAllBytes(System.IO.Path.Combine(Dataset.Layout.Directory, Dataset.Layout.Filename(Id)));
        public FingerprintImage Decode()
        {
            var gray = Load();
            int width = (gray[0] << 8) | gray[1];
            int height = (gray[2] << 8) | gray[3];
            var pixels = new byte[gray.Length - 4];
            Array.Copy(gray, 4, pixels, 0, pixels.Length);
            return new FingerprintImage(width, height, pixels, new FingerprintImageOptions() { Dpi = Dataset.Sample.Dpi() });
        }
    }
}
