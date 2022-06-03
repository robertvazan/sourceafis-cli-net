// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Linq;

namespace SourceAFIS.Cli.Inputs
{
    readonly struct Profile
    {
        public readonly SampleProfile Kind;
        public Profile(SampleProfile kind)
        {
            Kind = kind;
        }
        public static Profile Everything => new Profile(new AllSamples());
        public static Profile[] Aggregate => SampleProfile.Aggregate.Select(sp => new Profile(sp)).ToArray();
        public static Profile[] All => SampleProfile.All.Select(sp => new Profile(sp)).ToArray();
        public string Name => Kind.Name;
        public Dataset[] Datasets
        {
            get
            {
                var self = this;
                return Kind.Samples.Select(s => new Dataset(s)).ToArray();
            }
        }
        public Fingerprint[] Fingerprints => Datasets.SelectMany(ds => ds.Fingerprints).ToArray();
    }
}
