// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.IO;
using SourceAFIS.Cli.Utils.Args;

namespace SourceAFIS.Cli.Config
{
    class HomeOption : Option
    {
        public override string Name => "home";
        public override string Description => "Location of cache directory.";
        public override string[] Parameters => new[] { "path" };
        public override string Fallback => Configuration.Home;
        public override void Run(string[] parameters) => Configuration.Home = Path.GetFullPath(parameters[0]);
    }
}
