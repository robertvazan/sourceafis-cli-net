// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceAFIS.Cli.Benchmarks
{
    record TimingMeasurement(string Dataset, double Start, double End)
    {
        public static TimingMeasurement[] Sample(int size, IEnumerable<(TimingSummary Summary, TimingMeasurement[] Sample)> strata)
        {
            if (strata.Select(s => s.Sample.Length).Sum() <= size)
                return strata.SelectMany(s => s.Sample).ToArray();
            if (strata.Any(s => s.Sample.Length == 0))
                throw new ArgumentException("Empty sample.");
            var weights = strata.Select(s => s.Summary.Count / (double)s.Sample.Length).ToArray();
            double total = weights.Sum();
            for (int i = 0; i < weights.Length; ++i)
                weights[i] /= total;
            var available = strata.Select(s => s.Sample.ToList()).ToArray();
            var sample = new List<TimingMeasurement>();
            var random = new Random();
            while (sample.Count < size)
            {
                var weight = random.NextDouble();
                double cumulative = 0;
                for (int i = 0; i < weights.Length; ++i)
                {
                    cumulative += weights[i];
                    if (weight < cumulative)
                    {
                        var remaining = available[i];
                        if (remaining.Count > 0)
                        {
                            int choice = random.Next(remaining.Count);
                            sample.Add(remaining[choice]);
                            int last = remaining.Count - 1;
                            remaining[choice] = remaining[last];
                            remaining.RemoveAt(last);
                        }
                        break;
                    }
                }
            }
            return sample.ToArray();
        }
    }
}
