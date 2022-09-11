// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Checksums;
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Logs
{
    record ExtractionLogCommand : TransparencyLogCommand<Fingerprint>
    {
        public override string Name => "extraction";
        public override string Description => "Log extractor transparency data for given key.";
        protected override TransparencyChecksum<Fingerprint> Checksum() => new ExtractionChecksumCommand();
        protected override byte[] Log(string key, Fingerprint fp, int index, int count, string mime)
        {
            return Cache.Get<byte[]>(Category(key), Identity(key, fp, index, count, mime), batch =>
            {
                Log(key, fp, index, count, mime, () => new FingerprintTemplate(fp.Decode()), batch);
            });
        }
    }
}
