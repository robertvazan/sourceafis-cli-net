// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Utils.Args
{
    abstract class Option
    {
        public abstract string Name { get; }
        public virtual string[] Parameters => new string[0];
        public abstract string Description { get; }
        public virtual string Fallback => null;
        public virtual void Run() { }
        public virtual void Run(string[] parameters) => Run();
    }
}
