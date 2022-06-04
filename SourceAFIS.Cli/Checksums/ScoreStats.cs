// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Checksums
{
    record ScoreStats(double Matching, double Nonmatching, double Selfmatching, byte[] Hash)
    {
        public static ScoreStats Sum(IEnumerable<ScoreStats> list)
        {
            return new ScoreStats(
                list.Average(s => s.Matching),
                list.Average(s => s.Nonmatching),
                list.Average(s => s.Selfmatching),
                Hasher.Hash(list, s => s.Hash)
            );
        }
    }
}
