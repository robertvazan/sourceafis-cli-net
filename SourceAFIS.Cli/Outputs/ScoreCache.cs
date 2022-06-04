// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Linq;
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Outputs
{
    static class ScoreCache
    {
        public static double[][] Load(Dataset dataset)
        {
            return Cache.Get("scores", dataset.Path, () =>
            {
                var fingerprints = dataset.Fingerprints;
                var templates = fingerprints.Select(fp => TemplateCache.Deserialize(fp)).ToList();
                var scores = new double[fingerprints.Length][];
                foreach (var probe in fingerprints)
                {
                    var matcher = new FingerprintMatcher(templates[probe.Id]);
                    scores[probe.Id] = new double[fingerprints.Length];
                    foreach (var candidate in fingerprints)
                        scores[probe.Id][candidate.Id] = matcher.Match(templates[candidate.Id]);
                }
                return scores;
            });
        }
    }
}
