// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Linq;
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Outputs;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Checksums
{
    record ComparisonChecksumCommand : TransparencyChecksum<FingerprintPair>
    {
        public override string Name => "comparison";
        public override string Description => "Compute consistency checksum of transparency data generated during comparison of probe to candidate.";
        public override FingerprintPair[] Ids() => Fingerprint.All.SelectMany(p => p.Dataset.Fingerprints.Select(c => new FingerprintPair(p, c))).ToArray();
        protected override ChecksumTable Checksum(FingerprintPair pair)
        {
            return Cache.Get<ChecksumTable>(Category, pair.Path, batch =>
            {
                var dataset = pair.Dataset;
                var fingerprints = dataset.Fingerprints;
                var templates = fingerprints.Select(fp => TemplateCache.Deserialize(fp)).ToArray();
                foreach (var probe in fingerprints)
                {
                    var matcher = new FingerprintMatcher(templates[probe.Id]);
                    foreach (var candidate in fingerprints)
                    {
                        var template = templates[candidate.Id];
                        var table = ChecksumCollector.Collect(() => matcher.Match(template));
                        batch.Add(new FingerprintPair(probe, candidate).Path, table);
                    }
                }
            });
        }
    }
}
