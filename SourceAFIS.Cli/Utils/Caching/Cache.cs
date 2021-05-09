// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;
using System.IO;
using Serilog;
using SourceAFIS.Cli.Config;

namespace SourceAFIS.Cli.Utils.Caching
{
    class Cache
    {
        static readonly HashSet<string> Reported = new HashSet<string>();
        public static T Get<T>(string category, string identity, Action<CacheBatch> generator)
        {
            var path = Path.Combine(Configuration.Output, category, identity);
            var serialization = ICacheSerialization.Select<T>();
            path = serialization.Rename(path);
            var compression = ICacheCompression.Select(path);
            path = compression.Rename(path);
            if (!File.Exists(path))
            {
                lock (Reported)
                {
                    if (!Reported.Contains(category))
                    {
                        Reported.Add(category);
                        Log.Information("Computing {Category}...", category);
                    }
                }
                generator(new CacheBatch(category));
            }
            return serialization.Deserialize<T>(compression.Decompress(File.ReadAllBytes(path)));
        }
        public static T Get<T>(string category, string identity, Func<T> supplier)
        {
            return Get<T>(category, identity, batch => batch.Add(identity, supplier()));
        }
    }
}
