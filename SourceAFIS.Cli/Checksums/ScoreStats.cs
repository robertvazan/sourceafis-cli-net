// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Checksums
{
    class ScoreStats
    {
        public double Matching;
        public double Nonmatching;
        public double Selfmatching;
        public byte[] Hash;
        public static ScoreStats Sum(IEnumerable<ScoreStats> list)
        {
            var sum = new ScoreStats();
            sum.Matching = list.Select(s => s.Matching).Average();
            sum.Nonmatching = list.Select(s => s.Nonmatching).Average();
            sum.Selfmatching = list.Select(s => s.Selfmatching).Average();
            var hash = new Hasher();
            foreach (var stats in list)
                hash.Add(stats.Hash);
            sum.Hash = hash.Compute();
            return sum;
        }
    }
}
