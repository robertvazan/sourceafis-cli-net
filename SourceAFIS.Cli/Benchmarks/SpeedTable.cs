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
        public void Add(string name, TimingData data, TimingData unfiltered)
        {
            var summary = TimingSeries.Summary(data.Series);
            double gross = TimingSeries.Summary(unfiltered.Series).Count / (double)TimingSeries.DurationOf(unfiltered.Series);
            double mean = summary.Mean;
            double speed = 1 / mean;
            var sample = data.Sample.Select(o => o.Duration).OrderBy(t => t).ToArray();
            double median = sample.Length % 2 == 0
                ? 0.5 * (sample[sample.Length / 2 - 1] + sample[sample.Length / 2])
                : sample[sample.Length / 2];
            var sd = Math.Sqrt(sample.Sum(v => Math.Pow(v - mean, 2)) / (sample.Length - 1));
            var positive = sample.Where(v => v > 0).ToArray();
            var gm = Math.Exp(positive.Sum(v => Math.Log(v)) / positive.Length);
            var gsd = Math.Exp(Math.Sqrt(positive.Sum(v => Math.Pow(Math.Log(v / gm), 2)) / positive.Length));
            table.Add(key, name);
            table.Add("Iterations", Pretty.Length(summary.Count));
            table.Add("Gross", Pretty.Speed(gross));
            table.Add("Net", Pretty.Speed(speed * data.Threads));
            table.Add("Thread", Pretty.Speed(speed, name, "thread"));
            table.Add("Mean", Pretty.Time(mean));
            table.Add("Min", Pretty.Time(summary.Min));
            table.Add("Max", Pretty.Time(summary.Max));
            table.Add("Sample", Pretty.Length(sample.Length));
            table.Add("Median", Pretty.Time(median));
            table.Add("SD", Pretty.Time(sd));
            table.Add("Geom.mean", Pretty.Time(gm));
            table.Add("GSD", Pretty.Factor(gsd));
        }
        public void Print() => table.Print();
    }
}
