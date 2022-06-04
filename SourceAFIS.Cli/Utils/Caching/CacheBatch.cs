// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.IO;
using SourceAFIS.Cli.Config;

namespace SourceAFIS.Cli.Utils.Caching
{
    class CacheBatch
    {
        readonly string category;
        public CacheBatch(string category) => this.category = category;
        public void Add<T>(string identity, T data)
        {
            var path = Path.Combine(Configuration.Output, category, identity);
            var serialization = ICacheSerialization.Select<T>();
            path = serialization.Rename(path);
            var compression = ICacheCompression.Select(path);
            path = compression.Rename(path);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllBytes(path, compression.Compress(serialization.Serialize(data)));
        }
    }
}
