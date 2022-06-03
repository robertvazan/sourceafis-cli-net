// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Inputs
{
    class AllSamples : SampleProfile
    {
        public override string Name => "All";
        public override Sample[] Samples => Inputs.Samples.All();
        public override bool Equals(object other) => other is AllSamples;
        public override int GetHashCode() => 0;
    }
}
