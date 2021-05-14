// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Checksums
{
    class TransparencyStats
    {
        public string Mime;
        public int Count;
        public long Length;
        public long Normalized;
        public byte[] Hash;
        public static TransparencyStats Of(string mime, byte[] data)
        {
            var stats = new TransparencyStats();
            stats.Mime = mime;
            stats.Count = 1;
            stats.Length = data.Length;
            var normalized = Serializer.Normalize(mime, data);
            stats.Normalized = normalized.Length;
            stats.Hash = Hasher.Of(normalized);
            return stats;
        }
        public static TransparencyStats Sum(IEnumerable<TransparencyStats> list)
        {
            var sum = new TransparencyStats();
            sum.Mime = list.First().Mime;
            var hash = new Hasher();
            foreach (var stats in list)
            {
                sum.Count += stats.Count;
                sum.Length += stats.Length;
                sum.Normalized += stats.Normalized;
                hash.Add(stats.Hash);
            }
            sum.Hash = hash.Compute();
            return sum;
        }
    }
}
