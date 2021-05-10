// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using PathApi = System.IO.Path;

namespace SourceAFIS.Cli.Datasets
{
    readonly struct Fingerprint : IEquatable<Fingerprint>
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
        public string Path => PathApi.Combine(Dataset.Path, Name);
        public Finger Finger => new Finger(Dataset, Dataset.Layout.Finger(Id));
        public static List<Fingerprint> All => Dataset.All.SelectMany(ds => ds.Fingerprints).ToList();
        public byte[] Load() => File.ReadAllBytes(PathApi.Combine(Dataset.Layout.Directory, Dataset.Layout.Filename(Id)));
        public FingerprintImage Decode()
        {
            if (Dataset.Format == ImageFormat.Gray)
            {
                var gray = Load();
                int width = (gray[0] << 8) | gray[1];
                int height = (gray[2] << 8) | gray[3];
                var pixels = new byte[gray.Length - 4];
                Array.Copy(gray, 4, pixels, 0, pixels.Length);
                return new FingerprintImage(width, height, pixels, new FingerprintImageOptions() { Dpi = Dataset.Sample.Dpi() });
            }
            else
            {
                using (var stream = new MemoryStream(Load()))
                {
                    using (var image = Image.FromStream(stream))
                    {
                        using (var bitmap = new Bitmap(image))
                        {
                            var grayscale = new byte[bitmap.Width * bitmap.Height];
                            var locked = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                            try
                            {
                                var pixels = new byte[locked.Stride * locked.Height];
                                Marshal.Copy(locked.Scan0, pixels, 0, pixels.Length);
                                for (int y = 0; y < bitmap.Height; ++y)
                                {
                                    for (int x = 0; x < bitmap.Width; ++x)
                                    {
                                        int sum = 0;
                                        for (int c = 0; c < 3; ++c)
                                            sum += pixels[y * locked.Stride + x * 3 + c];
                                        grayscale[y * bitmap.Width + x] = (byte)(sum / 3);
                                    }
                                }
                            }
                            finally
                            {
                                bitmap.UnlockBits(locked);
                            }
                            return new FingerprintImage(bitmap.Width, bitmap.Height, grayscale, new FingerprintImageOptions() { Dpi = Dataset.Sample.Dpi() });
                        }
                    }
                }
            }
        }
    }
}
