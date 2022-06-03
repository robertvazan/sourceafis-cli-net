// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Inputs
{
    class SingleDataset : Profile
    {
        readonly Dataset Dataset;
        public SingleDataset(Dataset sample) => Dataset = sample;
        public override string Name => Dataset.Name;
        public override Dataset[] Datasets => new[] { Dataset };
        public override bool Equals(object other) => other is SingleDataset && ((SingleDataset)other).Dataset == Dataset;
        public override int GetHashCode() => Dataset.GetHashCode();
    }
}
