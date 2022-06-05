// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Linq;
using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Benchmarks
{
    class SpeedTable
    {
        readonly PrettyTable table = new PrettyTable();
        readonly string key;
        public SpeedTable(string key) => this.key = key;
        public void Add(string name, TimingData stats)
        {
            var total = TimingSummary.SumAll(stats.Series.Values.SelectMany(a => a));
            double mean = total.Mean;
            double speed = 1 / mean;
            var sample = stats.Sample.Select(o => o.End - o.Start).OrderBy(t => t).ToArray();
            double median = sample.Length % 2 == 0
                ? 0.5 * (sample[sample.Length / 2 - 1] + sample[sample.Length / 2])
                : sample[sample.Length / 2];
            var sd = Math.Sqrt(sample.Select(v => Math.Pow(v - mean, 2)).Sum() / (sample.Length - 1));
            var positive = sample.Where(v => v > 0).ToArray();
            var gm = Math.Exp(positive.Select(v => Math.Log(v)).Sum() / positive.Length);
            var gsd = Math.Exp(Math.Sqrt(positive.Select(v => Math.Pow(Math.Log(v / gm), 2)).Sum() / positive.Length));
            table.Add(key, name);
            table.Add("Iterations", Pretty.Length(total.Count));
            table.Add("Parallel", Pretty.Speed(speed * stats.Threads));
            table.Add("Thread", Pretty.Speed(speed, name, "thread"));
            table.Add("Mean", Pretty.Time(mean));
            table.Add("Min", Pretty.Time(total.Min));
            table.Add("Max", Pretty.Time(total.Max));
            table.Add("Sample", Pretty.Length(sample.Length));
            table.Add("Median", Pretty.Time(median));
            table.Add("SD", Pretty.Time(sd));
            table.Add("Geom.mean", Pretty.Time(gm));
            table.Add("GSD", Pretty.Factor(gsd));
        }
        public void Print() => table.Print();
    }
}
