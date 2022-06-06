// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceAFIS.Cli.Benchmarks
{
    record TimingSummary(long Count, double Mean, double Min, double Max)
    {
        public static TimingSummary Sum(IEnumerable<TimingSummary> list)
        {
            return new TimingSummary(
                list.Sum(s => s.Count),
                list.Average(s => Double.IsFinite(s.Mean) ? (double?)s.Mean : null) ?? Double.NaN,
                list.Min(s => Double.IsFinite(s.Min) ? (double?)s.Min : null) ?? Double.NaN,
                list.Max(s => Double.IsFinite(s.Max) ? (double?)s.Max : null) ?? Double.NaN
            );
        }
    }
}
