// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

using System.Linq;

namespace SourceAFIS.Cli.Inputs
{
    readonly record struct Finger(Dataset Dataset, ushort Id)
    {
        public Fingerprint[] Fingerprints
        {
            get
            {
                var dataset = Dataset;
                var id = Id;
                var layout = dataset.Layout;
                return Enumerable.Range(0, layout.Impressions(id))
                    .Select(n => new Fingerprint(dataset, layout.Fingerprint(id, n)))
                    .ToArray();
            }
        }
    }
}
