// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Datasets
{
    class SingleSample : SampleProfile
    {
        readonly Sample Sample;
        public SingleSample(Sample sample) => Sample = sample;
        public override string Name => Sample.Name();
        public override Sample[] Samples => new[] { Sample };
        public override bool Equals(object other) => other is SingleSample && ((SingleSample)other).Sample == Sample;
        public override int GetHashCode() => Sample.GetHashCode();
    }
}
