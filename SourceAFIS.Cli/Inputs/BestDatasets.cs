// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Inputs
{
    record BestDatasets : Profile
    {
        public override string Name => "High quality";
        public override Dataset[] Datasets => new[] { new Dataset(DatasetCode.Fvc2002_1B), new Dataset(DatasetCode.Fvc2002_2B) };
    }
}
