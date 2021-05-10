// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Datasets
{
    class BestSamples : SampleProfile
    {
        public override string Name => "High quality";
        public override Sample[] Samples => new[] { Sample.Fvc2002_1B, Sample.Fvc2002_2B };
        public override bool Equals(object other) => other is BestSamples;
        public override int GetHashCode() => 0;
    }
}
