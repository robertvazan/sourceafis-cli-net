// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.IO;
using System.Linq;
using SourceAFIS.Cli.Benchmarks;
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Outputs;
using SourceAFIS.Cli.Utils;
using SourceAFIS.Cli.Utils.Args;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Checksums
{
    record ScoreChecksumCommand : SimpleCommand
    {
        public override string[] Subcommand => new[] { "checksum", "scores" };
        public override string Description => "Compute consistency checksum of similarity scores.";
        ScoreStats Checksum(Dataset dataset)
        {
            return Cache.Get(Path.Combine("checksums", "scores"), dataset.Path, () =>
            {
                var trio = new QuantileTrio(dataset);
                var hash = new Hasher();
                foreach (var row in ScoreCache.Load(dataset))
                    foreach (var score in row)
                        hash.Add(score);
                return new ScoreStats(
                    trio.Matching.Average(),
                    trio.Nonmatching.Average(),
                    trio.Selfmatching.Average(),
                    hash.Compute()
                );
            });
        }
        ScoreStats Checksum(Profile profile) => ScoreStats.Sum(profile.Datasets.Select(ds => Checksum(ds)));
        public byte[] Global() => Checksum(Profile.Everything).Hash;
        public override void Run()
        {
            var table = new PrettyTable();
            foreach (var profile in Profile.All)
            {
                var stats = Checksum(profile);
                table.Add("Dataset", profile.Name);
                table.Add("Matching", Pretty.Decibans(stats.Matching, profile.Name, "matching"));
                table.Add("Non-matching", Pretty.Decibans(stats.Nonmatching, profile.Name, "nonmatching"));
                table.Add("Self-matching", Pretty.Decibans(stats.Selfmatching, profile.Name, "selfmatching"));
                table.Add("Hash", Pretty.Hash(stats.Hash, profile.Name, "hash"));
            }
            table.Print();
        }
    }
}
