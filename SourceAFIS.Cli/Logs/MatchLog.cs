// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Linq;
using SourceAFIS.Cli.Checksums;
using SourceAFIS.Cli.Datasets;
using SourceAFIS.Cli.Outputs;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Logs
{
    class MatchLog : TransparencyLog<FingerprintPair>
    {
        public override string Name => "match";
        public override string Description => "Log transparency data for given key during comparison of probe to candidate.";
        protected override TransparencyChecksum<FingerprintPair> Checksum() => new MatchChecksum();
        protected override byte[] Log(string key, FingerprintPair pair, int index, int count, string mime)
        {
            return Cache.Get<byte[]>(Category(key), Identity(key, pair, index, count, mime), batch =>
            {
                var dataset = pair.Dataset;
                var fingerprints = dataset.Fingerprints;
                var templates = fingerprints.Select(fp => TemplateCache.Deserialize(fp)).ToArray();
                foreach (var probe in fingerprints)
                {
                    var matcher = new FingerprintMatcher(templates[probe.Id]);
                    foreach (var candidate in fingerprints)
                    {
                        var wpair = new FingerprintPair(probe, candidate);
                        int wcount = new MatchChecksum().Count(wpair, key);
                        var template = templates[candidate.Id];
                        Log(key, wpair, index, wcount, mime, () => matcher.Match(template), batch);
                    }
                }
            });
        }
    }
}
