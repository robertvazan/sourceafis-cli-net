// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cmd
{
    class SampleFinger
    {
        public readonly SampleDataset Dataset;
        public readonly int Id;
        public SampleFinger(SampleDataset dataset, int id)
        {
            Dataset = dataset;
            Id = id;
        }
    }
}
