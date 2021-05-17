// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.IO;
using System.Linq;
using SourceAFIS.Cli.Benchmarks;
using SourceAFIS.Cli.Datasets;
using SourceAFIS.Cli.Outputs;
using SourceAFIS.Cli.Utils;
using SourceAFIS.Cli.Utils.Args;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Checksums
{
    class ScoreChecksum : Command
    {
        public override string[] Subcommand => new[] { "checksum", "scores" };
        public override string Description => "Compute consistency checksum of similarity scores.";
        ScoreStats Checksum(Dataset dataset)
        {
            return Cache.Get(Path.Combine("checksums", "scores"), dataset.Path, () =>
            {
                var trio = new QuantileTrio(dataset);
                var stats = new ScoreStats();
                stats.Matching = trio.Matching.Average();
                stats.Nonmatching = trio.Nonmatching.Average();
                stats.Selfmatching = trio.Selfmatching.Average();
                var hash = new Hasher();
                foreach (var row in ScoreCache.Load(dataset))
                {
                    foreach (var score in row)
                    {
                        var buffer = BitConverter.GetBytes(score);
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(buffer);
                        hash.Add(buffer);
                    }
                }
                stats.Hash = hash.Compute();
                return stats;
            });
        }
        ScoreStats Checksum(Profile profile) => ScoreStats.Sum(profile.Datasets.Select(ds => Checksum(ds)));
        public byte[] Global() => Checksum(Profile.Everything).Hash;
        public override void Run()
        {
            var table = new PrettyTable("Dataset", "Matching", "Non-matching", "Self-matching", "Hash");
            foreach (var profile in Profile.All)
            {
                var stats = Checksum(profile);
                table.Add(
                    profile.Name,
                    Pretty.Decibans(stats.Matching, profile.Name, "matching"),
                    Pretty.Decibans(stats.Nonmatching, profile.Name, "nonmatching"),
                    Pretty.Decibans(stats.Selfmatching, profile.Name, "selfmatching"),
                    Pretty.Hash(stats.Hash, profile.Name, "hash"));
            }
            Pretty.Print(table.Format());
        }
    }
}
