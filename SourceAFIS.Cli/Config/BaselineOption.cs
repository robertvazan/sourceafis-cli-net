// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Utils.Args;

namespace SourceAFIS.Cli.Config
{
    class BaselineOption : Option
    {
        public override string Name => "baseline";
        public override string Description => "Compare with output of another SourceAFIS CLI. Path may be relative to cache directory, e.g. 'net/1.2.3'.";
        public override string[] Parameters => new[] { "path" };
        public override void Run(string[] parameters) => Configuration.Baseline = parameters[0];
    }
}
