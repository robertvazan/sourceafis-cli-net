// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Utils;
using SourceAFIS.Cli.Utils.Args;

namespace SourceAFIS.Cli.Checksums
{
    class Checksum : Command
    {
        public override string[] Subcommand => new[] { "checksum" };
        public override string Description => "Compute consistency checksum of all algorithm outputs.";
        public override void Run()
        {
            var table = new PrettyTable("Data", "Hash");
            table.Add("Templates", Pretty.Hash(new TemplateChecksum().Global(), "templates"));
            table.Add("Scores", Pretty.Hash(new ScoreChecksum().Global(), "scores"));
            foreach (var transparency in new TransparencyChecksum[] { new ExtractorChecksum(), new ProbeChecksum(), new MatchChecksum() })
                table.Add("Transparency/" + transparency.Name, Pretty.Hash(transparency.Global(), "transparency", transparency.Name));
            Pretty.Print(table.Format());
        }
    }
}
