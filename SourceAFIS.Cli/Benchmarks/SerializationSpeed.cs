// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Datasets;
using SourceAFIS.Cli.Outputs;

namespace SourceAFIS.Cli.Benchmarks
{
    class SerializationSpeed : SoloSpeed
    {
        public override string Name => "serialization";
        public override string Description => "Measure speed of template serialization.";
        class TimedSerialization : TimedOperation<Fingerprint>
        {
            readonly Dictionary<Fingerprint, FingerprintTemplate> Templates;
            readonly Dictionary<Fingerprint, byte[]> Serialized;
            FingerprintTemplate Template;
            byte[] Output;
            byte[] Expected;
            public TimedSerialization(Dictionary<Fingerprint, FingerprintTemplate> templates, Dictionary<Fingerprint, byte[]> serialized)
            {
                Templates = templates;
                Serialized = serialized;
            }
            public override void Prepare(Fingerprint fp)
            {
                Template = Templates[fp];
                Expected = Serialized[fp];
            }
            public override void Execute() => Output = Template.ToByteArray();
            public override bool Verify() => Enumerable.SequenceEqual(Expected, Output);
        }
        public override TimingStats Measure()
        {
            return Measure(() =>
            {
                var templates = Fingerprint.All.ToDictionary(fp => fp, fp => TemplateCache.Deserialize(fp));
                var serialized = Fingerprint.All.ToDictionary(fp => fp, fp => TemplateCache.Load(fp));
                return () => new TimedSerialization(templates, serialized);
            });
        }
    }
}
