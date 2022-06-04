// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Inputs
{
    record AllDatasets : Profile
    {
        public override string Name => "All";
        public override Dataset[] Datasets => Dataset.All;
    }
}
