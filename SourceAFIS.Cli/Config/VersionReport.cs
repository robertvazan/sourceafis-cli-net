// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Utils;
using SourceAFIS.Cli.Utils.Args;

namespace SourceAFIS.Cli.Config
{
    class VersionReport : Command
    {
        public override string[] Subcommand => new[] { "version" };
        public override string Description => "Report version of SourceAFIS library being used.";
        public override void Run() => Pretty.Print(FingerprintCompatibility.Version);
    }
}
