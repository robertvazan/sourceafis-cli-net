// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SourceAFIS.Cli.Utils
{
    class Hasher
    {
        readonly IncrementalHash engine = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        public void Add(byte[] data) => engine.AppendData(data);
        public void Add(double value)
        {
            var buffer = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
            Add(buffer);
        }
        public byte[] Compute() => engine.GetHashAndReset();
        public static byte[] Hash(byte[] data)
        {
            var hash = new Hasher();
            hash.Add(data);
            return hash.Compute();
        }
        public static byte[] Hash(string mime, byte[] data) => Hash(Serializer.Normalize(mime, data));
        public static byte[] Hash<T>(IEnumerable<T> list, Func<T, byte[]> getter)
        {
            var hash = new Hasher();
            foreach (var item in list)
                hash.Add(getter(item));
            return hash.Compute();
        }
    }
}
