// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Checksums;
using SourceAFIS.Cli.Datasets;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Logs
{
    class ExtractorLog : TransparencyLog<Fingerprint>
    {
        public override string Name => "extractor";
        public override string Description => "Log extractor transparency data for given key.";
        protected override TransparencyChecksum<Fingerprint> Checksum() => new ExtractorChecksum();
        protected override byte[] Log(string key, Fingerprint fp, int index, int count, string mime)
        {
            return Cache.Get<byte[]>(Category(key), Identity(key, fp, index, count, mime), batch =>
            {
                Log(key, fp, index, count, mime, () => new FingerprintTemplate(fp.Decode()), batch);
            });
        }
    }
}
