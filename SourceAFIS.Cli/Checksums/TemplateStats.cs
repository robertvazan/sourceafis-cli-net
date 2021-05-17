// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Checksums
{
    class TemplateStats
    {
        public int Count;
        public long Length;
        public long Normalized;
        public byte[] Hash;
        public static TemplateStats Sum(IEnumerable<TemplateStats> list)
        {
            var sum = new TemplateStats();
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
