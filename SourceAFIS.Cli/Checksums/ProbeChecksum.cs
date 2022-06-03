// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Outputs;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Checksums
{
    class ProbeChecksum : TransparencyChecksum<Fingerprint>
    {
        public override string Name => "probe";
        public override string Description => "Compute consistency checksum of transparency data generated when preparing probe for matching.";
        public override Fingerprint[] Ids() => Fingerprint.All;
        protected override TransparencyTable Checksum(Fingerprint fp)
        {
            return Cache.Get(Category, fp.Path, () =>
            {
                var template = TemplateCache.Deserialize(fp);
                return ChecksumCollector.Collect(() => new FingerprintMatcher(template));
            });
        }
    }
}
