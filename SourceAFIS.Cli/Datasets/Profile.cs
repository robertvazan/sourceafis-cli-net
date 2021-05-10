// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Linq;

namespace SourceAFIS.Cli.Datasets
{
    readonly struct Profile
    {
        public readonly SampleProfile Kind;
        public readonly ImageFormat Format;
        public Profile(SampleProfile kind, ImageFormat format)
        {
            Kind = kind;
            Format = format;
        }
        public static Profile Everything => new Profile(new AllSamples(), ImageFormat.Default);
        public static Profile[] Aggregate => SampleProfile.Aggregate.Select(sp => new Profile(sp, ImageFormat.Default)).ToArray();
        public static Profile[] All => SampleProfile.All.Select(sp => new Profile(sp, ImageFormat.Default)).ToArray();
        public Dataset[] Datasets
        {
            get
            {
                var self = this;
                return Kind.Samples.Select(s => new Dataset(s, self.Format)).ToArray();
            }
        }
        public Fingerprint[] Fingerprints => Datasets.SelectMany(ds => ds.Fingerprints).ToArray();
    }
}
