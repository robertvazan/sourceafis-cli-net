// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Inputs
{
    interface Sampler<K>
    {
        K Next();
        Dataset Dataset(K id);
    }
}
