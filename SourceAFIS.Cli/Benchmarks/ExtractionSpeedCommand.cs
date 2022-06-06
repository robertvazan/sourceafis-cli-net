// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Benchmarks
{
    record ExtractionSpeedCommand : SpeedCommand<Fingerprint>
    {
        public override string Name => "extraction";
        public override string Description => "Measure speed of feature extraction, i.e. FingerprintTemplate constructor.";
        protected override Sampler<Fingerprint> Sampler() => new FingerprintSampler();
        class TimedExtraction : TimedOperation<Fingerprint>
        {
            FingerprintImage image;
            byte[] template;
            public override void Prepare(Fingerprint fp) => image = fp.Decode();
            // Include serialization in extractor benchmark, because the two are often performed together
            // and serialization is not important enough to warrant its own benchmark.
            public override void Execute() => template = new FingerprintTemplate(image).ToByteArray();
            public override void Blackhole(Hasher hasher) => hasher.Add(template);
        }
        public override TimingData Measure()
        {
            return Measure(() =>
            {
                return () => new TimedExtraction();
            });
        }
    }
}
