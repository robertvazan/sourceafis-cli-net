// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Utils;
using SourceAFIS.Cli.Utils.Args;

namespace SourceAFIS.Cli.Checksums
{
    record ChecksumSummaryCommand : SimpleCommand
    {
        public override string[] Subcommand => new[] { "checksum" };
        public override string Description => "Compute consistency checksum of all algorithm outputs.";
        public override void Run()
        {
            var table = new PrettyTable("Data", "Hash");
            table.Add("Templates", Pretty.Hash(new TemplateChecksumCommand().Global(), "templates"));
            table.Add("Scores", Pretty.Hash(new ScoreChecksumCommand().Global(), "scores"));
            foreach (var transparency in new TransparencyChecksumCommand[] { new ExtractionChecksumCommand(), new ProbeChecksumCommand(), new ComparisonChecksumCommand() })
                table.Add("Transparency/" + transparency.Name, Pretty.Hash(transparency.Global(), "transparency", transparency.Name));
            Pretty.Print(table.Format());
        }
    }
}
