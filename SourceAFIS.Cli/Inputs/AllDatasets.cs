// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Inputs
{
    class AllDatasets : Profile
    {
        public override string Name => "All";
        public override Dataset[] Datasets => Dataset.All;
        public override bool Equals(object other) => other is AllDatasets;
        public override int GetHashCode() => 0;
    }
}
