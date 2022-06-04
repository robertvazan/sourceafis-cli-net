// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Checksums
{
    record ExtractionChecksumCommand : TransparencyChecksum<Fingerprint>
    {
        public override string Name => "extraction";
        public override string Description => "Compute consistency checksum of extractor transparency data.";
        public override Fingerprint[] Ids() => Fingerprint.All;
        protected override ChecksumTable Checksum(Fingerprint fp)
        {
            return Cache.Get(Category, fp.Path, () => ChecksumCollector.Collect(() => new FingerprintTemplate(fp.Decode())));
        }
    }
}
