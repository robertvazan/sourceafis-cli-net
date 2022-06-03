// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Security.Cryptography;
using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Utils
{
    class Hasher
    {
        readonly IncrementalHash Engine = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        public void Add(byte[] data) => Engine.AppendData(data);
        public byte[] Compute() => Engine.GetHashAndReset();
        public static byte[] Hash(byte[] data)
        {
            var hash = new Hasher();
            hash.Add(data);
            return hash.Compute();
        }
        public static byte[] Hash(string mime, byte[] data) => Hash(Serializer.Normalize(mime, data));
    }
}
