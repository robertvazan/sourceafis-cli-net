// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Linq;
using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Benchmarks
{
    class SpeedTable
    {
        readonly PrettyTable Table;
        public SpeedTable(string key)
        {
            Table = new PrettyTable(key, "Iterations", "Parallel", "Thread", "Mean", "Min", "Max", "Sample", "Median", "SD", "Geom.mean", "GSD");
        }
        public void Add(string name, TimingStats stats)
        {
            var total = TimingSummary.SumAll(stats.Segments.Values.SelectMany(a => a));
            double mean = total.Sum / total.Count;
            double speed = 1 / mean;
            var sample = stats.Sample.Select(o => o.End - o.Start).OrderBy(t => t).ToArray();
            double median = sample.Length % 2 == 0
                ? 0.5 * (sample[sample.Length / 2 - 1] + sample[sample.Length / 2])
                : sample[sample.Length / 2];
            var sd = Math.Sqrt(sample.Select(v => Math.Pow(v - mean, 2)).Sum() / (sample.Length - 1));
            var positive = sample.Where(v => v > 0).ToArray();
            var gm = Math.Exp(positive.Select(v => Math.Log(v)).Sum() / positive.Length);
            var gsd = Math.Exp(Math.Sqrt(positive.Select(v => Math.Pow(Math.Log(v / gm), 2)).Sum() / positive.Length));
            Table.Add(
                name,
                Pretty.Length(total.Count),
                Pretty.Speed(speed * stats.Threads),
                Pretty.Speed(speed, name, "thread"),
                Pretty.Time(mean),
                Pretty.Time(total.Min),
                Pretty.Time(total.Max),
                Pretty.Length(sample.Length),
                Pretty.Time(median),
                Pretty.Time(sd),
                Pretty.Time(gm),
                Pretty.Factor(gsd));
        }
        public void Print() => Pretty.Print(Table.Format());
    }
}
