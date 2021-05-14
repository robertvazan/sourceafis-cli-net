// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SourceAFIS.Cli.Utils;
using SourceAFIS.Cli.Utils.Args;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Checksums
{
    abstract class TransparencyChecksum<K> : Command
    {
        public abstract string Name { get; }
        public abstract K[] Ids();
        protected abstract TransparencyTable Checksum(K id);
        public override string[] Subcommand => new[] { "checksum", "transparency", Name };
        protected string Category => Path.Combine("checksums", "transparency", Name);
        public TransparencyTable Checksum() => Cache.Get(Category, "all", () => TransparencyTable.Sum(Ids().Select(id => Checksum(id))));
        public string Mime(string key) => Checksum().Mime(key);
        public int Count(K id, string key) => Checksum(id).Count(key);
        public byte[] Global()
        {
            var hash = new Hasher();
            foreach (var row in Checksum().Rows)
                hash.Add(row.Stats.Hash);
            return hash.Compute();
        }
        public override void Run()
        {
            var table = new PrettyTable("Key", "MIME", "Count", "Length", "Normalized", "Total", "Hash");
            foreach (var row in Checksum().Rows)
            {
                var stats = row.Stats;
                table.Add(
                    row.Key,
                    stats.Mime,
                    Pretty.Length(stats.Count),
                    Pretty.Length(stats.Length / stats.Count, row.Key, "length"),
                    Pretty.Length(stats.Normalized / stats.Count, row.Key, "normalized"),
                    Pretty.Length(stats.Normalized, row.Key, "total"),
                    Pretty.Hash(stats.Hash, row.Key, "hash"));
            }
            Pretty.Print(table.Format());
        }
    }
}
