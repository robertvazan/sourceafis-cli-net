// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Utils.Args;

namespace SourceAFIS.Cli.Config
{
    class NormalizationOption : Option
    {
        public override string Name => "normalize";
        public override string Description => "Log normalized transparency data instead of raw data obtained from the library.";
        public override string[] Parameters => new string[0];
        public override void Run(string[] parameters) => Configuration.Normalized = true;
    }
}
