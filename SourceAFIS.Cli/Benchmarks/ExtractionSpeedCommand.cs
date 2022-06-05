// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Outputs;

namespace SourceAFIS.Cli.Benchmarks
{
    record ExtractionSpeedCommand : SoloSpeedCommand
    {
        public override string Name => "extraction";
        public override string Description => "Measure speed of feature extraction, i.e. FingerprintTemplate constructor.";
        class TimedExtraction : TimedOperation<Fingerprint>
        {
            readonly Dictionary<Fingerprint, byte[]> Templates;
            FingerprintImage Image;
            byte[] Template;
            byte[] Expected;
            public TimedExtraction(Dictionary<Fingerprint, byte[]> templates) => Templates = templates;
            public override void Prepare(Fingerprint fp)
            {
                Image = fp.Decode();
                Expected = Templates[fp];
            }
            // Include serialization in extractor benchmark, because the two are often performed together
            // and serialization is not important enough to warrant its own benchmark.
            public override void Execute() => Template = new FingerprintTemplate(Image).ToByteArray();
            public override bool Verify() => Enumerable.SequenceEqual(Expected, Template);
        }
        public override TimingData Measure()
        {
            return Measure(() =>
            {
                var templates = Fingerprint.All.ToDictionary(fp => fp, fp => TemplateCache.Load(fp));
                return () => new TimedExtraction(templates);
            });
        }
    }
}
