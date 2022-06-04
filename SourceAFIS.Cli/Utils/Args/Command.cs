// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Utils.Args
{
    abstract record Command
    {
        public abstract string[] Subcommand { get; }
        public abstract string Description { get; }
        public abstract string[] Parameters { get; }
        public abstract void Run(string[] parameters);
    }
}
