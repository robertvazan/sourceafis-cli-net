// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Outputs;
using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Benchmarks
{
    record DeserializationSpeedCommand : SpeedCommand<Fingerprint>
    {
        public override string Name => "deserialization";
        public override string Description => "Measure speed of template deserialization.";
        protected override Sampler<Fingerprint> Sampler() => new FingerprintSampler();
        class TimedDeserialization : TimedOperation<Fingerprint>
        {
            readonly Dictionary<Fingerprint, byte[]> serialized;
            byte[] input;
            FingerprintTemplate deserialized;
            public TimedDeserialization(Dictionary<Fingerprint, byte[]> serialized) => this.serialized = serialized;
            public override void Prepare(Fingerprint fp) => input = serialized[fp];
            public override void Execute() => deserialized = new FingerprintTemplate(input);
            public override void Blackhole(Hasher hasher) => hasher.Add(deserialized.ToByteArray());
        }
        public override TimingData Measure()
        {
            return Measure(() =>
            {
                var serialized = Fingerprint.All.ToDictionary(fp => fp, fp => TemplateCache.Load(fp));
                return () => new TimedDeserialization(serialized);
            });
        }
    }
}
