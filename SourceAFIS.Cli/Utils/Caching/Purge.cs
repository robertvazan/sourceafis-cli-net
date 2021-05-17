// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.IO;
using SourceAFIS.Cli.Config;
using SourceAFIS.Cli.Utils.Args;

namespace SourceAFIS.Cli.Utils.Caching
{
    class Purge : Command
    {
        public override string[] Subcommand => new[] { "purge" };
        public override string Description => "Remove cached data except downloads.";
        public override void Run() => Directory.Delete(Configuration.Output, true);
    }
}
