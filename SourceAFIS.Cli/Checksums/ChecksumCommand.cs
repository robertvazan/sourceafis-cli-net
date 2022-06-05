// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.IO;
using System.Linq;
using SourceAFIS.Cli.Utils;
using SourceAFIS.Cli.Utils.Args;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Checksums
{
    abstract record ChecksumCommand : SimpleCommand
    {
        public abstract string Name { get; }
        public override string[] Subcommand => new[] { "checksum", Name };
        protected string Category => Path.Combine("checksums", Name);
        public abstract ChecksumTable Checksum();
        public string Mime(string key) => Checksum().Mime(key);
        public byte[] Global()
        {
            var hash = new Hasher();
            foreach (var row in Checksum().Rows)
                if (row.Key != "version")
                    hash.Add(row.Stats.Hash);
            return hash.Compute();
        }
        public override void Run()
        {
            var table = new PrettyTable();
            foreach (var row in Checksum().Rows)
            {
                var stats = row.Stats;
                table.Add("Key", row.Key);
                table.Add("MIME", stats.Mime);
                table.Add("Count", Pretty.Length(stats.Count));
                table.Add("Length", Pretty.Length(stats.Length, row.Key, "length"));
                table.Add("Normalized", Pretty.Length(stats.Normalized, row.Key, "normalized"));
                table.Add("Hash", Pretty.Hash(stats.Hash, row.Key, "hash"));
            }
            table.Print();
        }
    }
    abstract record TransparencyChecksum<K> : ChecksumCommand
    {
        public abstract K[] Ids();
        protected abstract ChecksumTable Checksum(K id);
        public override ChecksumTable Checksum() => Cache.Get(Category, "all", () => ChecksumTable.Sum(Ids().Select(id => Checksum(id))));
        public int Count(K id, string key) => Checksum(id).Count(key);
    }
}
