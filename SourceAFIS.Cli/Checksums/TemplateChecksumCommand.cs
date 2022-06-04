// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.IO;
using System.Linq;
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Outputs;
using SourceAFIS.Cli.Utils;
using SourceAFIS.Cli.Utils.Args;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Checksums
{
    record TemplateChecksumCommand : SimpleCommand
    {
        public override string[] Subcommand => new[] { "checksum", "templates" };
        public override string Description => "Compute consistency checksum of templates.";
        TemplateStats Checksum(Fingerprint fp)
        {
            return Cache.Get(Path.Combine("checksums", "templates"), fp.Path, () =>
            {
                var serialized = TemplateCache.Load(fp);
                var normalized = Serializer.Normalize(serialized);
                return new TemplateStats(1, serialized.Length, normalized.Length, Hasher.Hash(normalized));
            });
        }
        TemplateStats Checksum(Profile profile) => TemplateStats.Sum(profile.Fingerprints.Select(fp => Checksum(fp)));
        public byte[] Global() => Checksum(Profile.Everything).Hash;
        public override void Run()
        {
            var table = new PrettyTable("Dataset", "Count", "Length", "Normalized", "Total", "Hash");
            foreach (var profile in Profile.All)
            {
                var stats = Checksum(profile);
                table.Add(
                    profile.Name,
                    Pretty.Length(stats.Count),
                    Pretty.Length(stats.Length / stats.Count, profile.Name, "length"),
                    Pretty.Length(stats.Normalized / stats.Count, profile.Name, "normalized"),
                    Pretty.Length(stats.Normalized, profile.Name, "total"),
                    Pretty.Hash(stats.Hash, profile.Name, "hash"));
            }
            Pretty.Print(table.Format());
        }
    }
}
