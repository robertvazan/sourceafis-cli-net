// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Datasets;
using SourceAFIS.Cli.Outputs;

namespace SourceAFIS.Cli.Benchmarks
{
    class ExtractionSpeed : SoloSpeed
    {
        public override string Name => "extraction";
        public override string Description => "Measure speed of feature extraction, i.e. FingerprintTemplate constructor.";
        class TimedExtraction : TimedOperation<Fingerprint>
        {
            readonly Dictionary<Fingerprint, byte[]> Templates;
            FingerprintImage Image;
            FingerprintTemplate Template;
            byte[] Expected;
            public TimedExtraction(Dictionary<Fingerprint, byte[]> templates) => Templates = templates;
            public override void Prepare(Fingerprint fp)
            {
                Image = fp.Decode();
                Expected = Templates[fp];
            }
            public override void Execute() => Template = new FingerprintTemplate(Image);
            public override bool Verify() => Enumerable.SequenceEqual(Expected, Template.ToByteArray());
        }
        public override TimingStats Measure()
        {
            return Measure(() =>
            {
                var templates = Fingerprint.All.ToDictionary(fp => fp, fp => TemplateCache.Load(fp));
                return () => new TimedExtraction(templates);
            });
        }
    }
}
