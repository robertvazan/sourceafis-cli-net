// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Benchmarks
{
    abstract class TimedOperation<K>
    {
        public abstract void Prepare(K id);
        public abstract void Execute();
        public abstract void Blackhole(Hasher hasher);
    }
}
