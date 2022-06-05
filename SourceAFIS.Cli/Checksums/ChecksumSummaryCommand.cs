// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using SourceAFIS.Cli.Utils;
using SourceAFIS.Cli.Utils.Args;

namespace SourceAFIS.Cli.Checksums
{
    record ChecksumSummaryCommand : SimpleCommand
    {
        public override string[] Subcommand => new[] { "checksum" };
        public override string Description => "Compute consistency checksum of all algorithm outputs.";
        record GlobalHasher(string Name, Func<byte[]> Runner)
        {
            public GlobalHasher(string name, ChecksumCommand command) : this(name, command.Global) { }
        }
        static byte[] Total()
        {
            var sum = new Hasher();
            foreach (var hasher in GlobalHashers)
                if (object.ReferenceEquals(hasher, TotalHasher))
                    sum.Add(hasher.Runner());
            return sum.Compute();
        }
        static readonly GlobalHasher TotalHasher = new GlobalHasher("Total", Total);
        static readonly GlobalHasher[] GlobalHashers = new GlobalHasher[]
        {
            new GlobalHasher("Templates", new TemplateChecksumCommand().Global),
            new GlobalHasher("Scores", new ScoreChecksumCommand().Global),
            new GlobalHasher("Extraction", new ExtractionChecksumCommand()),
            new GlobalHasher("Probe", new ProbeChecksumCommand()),
            new GlobalHasher("Comparison", new ComparisonChecksumCommand()),
            TotalHasher
        };
        public override void Run()
        {
            var table = new PrettyTable();
            foreach (var hasher in GlobalHashers)
            {
                table.Add("Data", hasher.Name);
                table.Add("Hash", Pretty.Hash(hasher.Runner(), hasher.Name));
            }
            table.Print();
        }
    }
}
