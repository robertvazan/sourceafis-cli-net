// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Utils.Caching
{
    interface ICacheCompression
    {
        string Rename(string path);
        byte[] Compress(byte[] data);
        byte[] Decompress(byte[] compressed);
        static ICacheCompression Select(string path)
        {
            if (path.EndsWith(".cbor"))
                return new GzipCompression();
            return new TrivialCompression();
        }
    }
}
