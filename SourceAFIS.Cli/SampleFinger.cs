// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli
{
    class SampleFinger
    {
        public readonly Dataset Dataset;
        public readonly int Id;
        public SampleFinger(Dataset dataset, int id)
        {
            Dataset = dataset;
            Id = id;
        }
    }
}
