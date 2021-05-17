// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Datasets;
using SourceAFIS.Cli.Outputs;

namespace SourceAFIS.Cli.Benchmarks
{
    class DeserializationSpeed : SoloSpeed
    {
        public override string Name => "deserialization";
        public override string Description => "Measure speed of template deserialization.";
        class TimedDeserialization : TimedOperation<Fingerprint>
        {
            readonly Dictionary<Fingerprint, byte[]> Serialized;
            byte[] Input;
            FingerprintTemplate Deserialized;
            public TimedDeserialization(Dictionary<Fingerprint, byte[]> serialized) => Serialized = serialized;
            public override void Prepare(Fingerprint fp) => Input = Serialized[fp];
            public override void Execute() => Deserialized = new FingerprintTemplate(Input);
            public override bool Verify() => Enumerable.SequenceEqual(Input, Deserialized.ToByteArray());
        }
        public override TimingStats Measure()
        {
            return Measure(() =>
            {
                var serialized = Fingerprint.All.ToDictionary(fp => fp, fp => TemplateCache.Load(fp));
                return () => new TimedDeserialization(serialized);
            });
        }
    }
}
