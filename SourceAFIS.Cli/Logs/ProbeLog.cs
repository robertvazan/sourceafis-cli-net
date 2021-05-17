// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Checksums;
using SourceAFIS.Cli.Datasets;
using SourceAFIS.Cli.Outputs;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Logs
{
    class ProbeLog : TransparencyLog<Fingerprint>
    {
        public override string Name => "probe";
        public override string Description => "Log transparency data for given key while preparing probe for matching.";
        protected override TransparencyChecksum<Fingerprint> Checksum() => new ProbeChecksum();
        protected override byte[] Log(string key, Fingerprint fp, int index, int count, string mime)
        {
            return Cache.Get<byte[]>(Category(key), Identity(key, fp, index, count, mime), batch =>
            {
                var template = TemplateCache.Deserialize(fp);
                Log(key, fp, index, count, mime, () => new FingerprintMatcher(template), batch);
            });
        }
    }
}
