// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.IO;
using System.Linq;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Inputs
{
    readonly record struct Fingerprint(Dataset Dataset, int Id) : DataIdentifier
    {
        public string Name => Dataset.Layout.Name(Id);
        public string Path => System.IO.Path.Combine(Dataset.Path, Name);
        public Finger Finger => new Finger(Dataset, Dataset.Layout.Finger(Id));
        public static Fingerprint[] All => Dataset.All.SelectMany(ds => ds.Fingerprints).ToArray();
        public byte[] Load() => File.ReadAllBytes(System.IO.Path.Combine(Dataset.Layout.directory, Dataset.Layout.Filename(Id)));
        public FingerprintImage Decode()
        {
            var gray = Load();
            int width = (gray[0] << 8) | gray[1];
            int height = (gray[2] << 8) | gray[3];
            var pixels = new byte[gray.Length - 4];
            Array.Copy(gray, 4, pixels, 0, pixels.Length);
            return new FingerprintImage(width, height, pixels, new FingerprintImageOptions() { Dpi = Dataset.Dpi });
        }
    }
}
