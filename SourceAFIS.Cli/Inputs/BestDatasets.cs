// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Inputs
{
    class BestDatasets : Profile
    {
        public override string Name => "High quality";
        public override Dataset[] Datasets => new[] { new Dataset(DatasetCode.Fvc2002_1B), new Dataset(DatasetCode.Fvc2002_2B) };
        public override bool Equals(object other) => other is BestDatasets;
        public override int GetHashCode() => 0;
    }
}
