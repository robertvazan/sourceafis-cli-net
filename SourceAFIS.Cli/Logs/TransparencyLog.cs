// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.IO;
using SourceAFIS.Cli.Checksums;
using SourceAFIS.Cli.Config;
using SourceAFIS.Cli.Utils;
using SourceAFIS.Cli.Utils.Args;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Logs
{
    abstract class TransparencyLog<K> : Command
        where K : DataIdentifier
    {
        public abstract string Name { get; }
        protected abstract TransparencyChecksum<K> Checksum();
        protected abstract byte[] Log(string key, K id, int index, int count, string mime);
        public override String[] Subcommand => new[] { "log", Name };
        public override String[] Parameters => new[] { "key" };
        protected string Category(string key)
        {
            if (Configuration.Normalized)
                return Path.Combine("logs", Name, "normalized", key);
            else
                return Path.Combine("logs", Name, key);
        }
        protected string Identity(string key, K id, int index, int count, string mime)
        {
            var path = id.Path;
            if (count > 1)
                path = Path.Combine(path, index.ToString());
            return path + Pretty.Extension(mime);
        }
        protected void Log(string key, K id, int index, int count, string mime, Action action, CacheBatch batch)
        {
            var collected = KeyDataCollector.Collect(key, action);
            for (int i = 0; i < count; ++i)
            {
                var raw = collected[index];
                var normalized = Configuration.Normalized ? Serializer.Normalize(mime, raw) : raw;
                batch.Add(Identity(key, id, i, count, mime), normalized);
            }
        }
        public void Log(string key)
        {
            var mime = Checksum().Mime(key);
            foreach (var id in Checksum().Ids())
            {
                int count = Checksum().Count(id, key);
                if (count > 0)
                    Log(key, id, 0, count, mime);
            }
            Pretty.Print("Saved: " + Pretty.Dump(Category(key)));
        }
        public override void Run(string[] parameters) => Log(parameters[0]);
    }
}
