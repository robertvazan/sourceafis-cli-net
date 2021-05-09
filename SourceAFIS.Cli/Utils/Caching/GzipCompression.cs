// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.IO;
using System.IO.Compression;

namespace SourceAFIS.Cli.Utils.Caching
{
    class GzipCompression : ICacheCompression
    {
        public string Rename(string path) { return path + ".gz"; }
        public byte[] Compress(byte[] data)
        {
            using (var buffer = new MemoryStream())
            {
                using (var gzip = new GZipStream(buffer, CompressionMode.Compress))
                    gzip.Write(data);
                return buffer.ToArray();
            }
        }
        public byte[] Decompress(byte[] compressed)
        {
            using (var buffer = new MemoryStream())
            {
                using (var input = new MemoryStream(compressed))
                using (var gzip = new GZipStream(input, CompressionMode.Decompress))
                    gzip.CopyTo(buffer);
                return buffer.ToArray();
            }
        }
    }
}
