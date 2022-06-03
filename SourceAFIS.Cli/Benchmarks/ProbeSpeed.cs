// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Outputs;

namespace SourceAFIS.Cli.Benchmarks
{
    class ProbeSpeed : SoloSpeed
    {
        public override string Name => "probe";
        public override string Description => "Measure speed of preparing probe template for matching, i.e. FingerprintMatcher constructor.";
        class TimedProbeConstruction : TimedOperation<Fingerprint>
        {
            readonly Dictionary<Fingerprint, FingerprintTemplate> Templates;
            readonly Dictionary<Dataset, double[][]> Scores;
            FingerprintTemplate Template;
            FingerprintMatcher Matcher;
            double Expected;
            public TimedProbeConstruction(Dictionary<Fingerprint, FingerprintTemplate> templates, Dictionary<Dataset, double[][]> scores)
            {
                Templates = templates;
                Scores = scores;
            }
            public override void Prepare(Fingerprint fp)
            {
                Template = Templates[fp];
                Expected = Scores[fp.Dataset][fp.Id][fp.Id];
            }
            public override void Execute() => Matcher = new FingerprintMatcher(Template);
            public override bool Verify() => Matcher.Match(Template) == Expected;
        }
        public override TimingStats Measure()
        {
            return Measure(() =>
            {
                var templates = Fingerprint.All.ToDictionary(fp => fp, fp => TemplateCache.Deserialize(fp));
                var scores = Inputs.Dataset.All.ToDictionary(ds => ds, ds => ScoreCache.Load(ds));
                return () => new TimedProbeConstruction(templates, scores);
            });
        }
    }
}
