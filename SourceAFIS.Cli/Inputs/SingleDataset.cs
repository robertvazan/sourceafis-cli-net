// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Inputs
{
    record SingleDataset(Dataset Dataset) : Profile
    {
        public override string Name => Dataset.Name;
        public override Dataset[] Datasets => new[] { Dataset };
    }
}
