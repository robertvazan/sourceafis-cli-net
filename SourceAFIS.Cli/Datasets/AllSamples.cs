// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Datasets
{
    class AllSamples : SampleProfile
    {
        public override string Name => "All";
        public override Sample[] Samples => Datasets.Samples.All();
        public override bool Equals(object other) => other is AllSamples;
        public override int GetHashCode() => 0;
    }
}
