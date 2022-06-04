// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Checksums
{
    record TemplateStats(int Count, long Length, long Normalized, byte[] Hash)
    {
        public static TemplateStats Sum(IEnumerable<TemplateStats> list)
        {
            return new TemplateStats(
                list.Sum(s => s.Count),
                list.Sum(s => s.Length),
                list.Sum(s => s.Normalized),
                Hasher.Hash(list, s => s.Hash)
            );
        }
    }
}
