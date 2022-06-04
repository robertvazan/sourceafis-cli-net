// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Utils.Args
{
    abstract class Option
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string[] Parameters { get; }
        public virtual string Fallback => null;
        public abstract void Run(string[] parameters);
    }
}
