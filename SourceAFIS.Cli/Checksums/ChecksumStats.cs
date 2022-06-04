// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Checksums
{
    record ChecksumStats(string Mime, int Count, long Length, long Normalized, byte[] Hash)
    {
        public static ChecksumStats Of(string mime, byte[] data)
        {
            var normalized = Serializer.Normalize(mime, data);
            return new ChecksumStats(
                mime,
                1,
                data.Length,
                normalized.Length,
                Hasher.Hash(normalized)
            );
        }
        public static ChecksumStats Sum(IEnumerable<ChecksumStats> list)
        {
            return new ChecksumStats(
                list.First().Mime,
                list.Sum(s => s.Count),
                list.Sum(s => s.Length),
                list.Sum(s => s.Normalized),
                Hasher.Hash(list, s => s.Hash)
            );
        }
    }
}
