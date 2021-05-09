// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Utils.Caching
{
    class TrivialCompression : ICacheCompression
    {
        public string Rename(string path) => path;
        public byte[] Compress(byte[] data) => data;
        public byte[] Decompress(byte[] compressed) => compressed;
    }
}
