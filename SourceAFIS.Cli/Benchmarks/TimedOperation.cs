// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Benchmarks
{
    abstract class TimedOperation<K>
    {
        public abstract void Prepare(K id);
        public abstract void Execute();
        public abstract bool Verify();
    }
}
