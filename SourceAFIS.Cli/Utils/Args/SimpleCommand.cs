// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Utils.Args
{
    abstract record SimpleCommand : Command
    {
        public override string[] Parameters => new string[0];
        public abstract void Run();
        public override void Run(string[] parameters) => Run();
    }
}
