// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SourceAFIS.Cli.Inputs;

namespace SourceAFIS.Cli.Benchmarks
{
    class TimingSeriesBuilder
    {
        readonly long epoch;
        readonly bool[] datasets;
        readonly long[] counts;
        readonly long[] sums;
        readonly long[] maxima;
        readonly long[] minima;
        public TimingSeriesBuilder(long epoch)
        {
            this.epoch = epoch;
            datasets = new bool[Dataset.All.Length];
            int segments = datasets.Length * TimingSeries.Duration;
            counts = new long[segments];
            sums = new long[segments];
            maxima = new long[segments];
            minima = new long[segments];
            Array.Fill(minima, long.MaxValue);
        }
        public bool Add(Dataset dataset, long start, long end)
        {
            int interval = (int)((end - epoch) / Stopwatch.Frequency);
            long duration = end - start;
            if (interval >= 0 && interval < TimingSeries.Duration && duration >= 0)
            {
                int datasetId = (int)dataset.Code;
                datasets[datasetId] = true;
                int segment = datasetId * TimingSeries.Duration + interval;
                sums[segment] += duration;
                maxima[segment] = Math.Max(maxima[segment], duration);
                minima[segment] = Math.Min(minima[segment], duration);
                ++counts[segment];
                return true;
            }
            else
                return false;
        }
        public Dictionary<string, TimingSummary[]> Build()
        {
            var map = new Dictionary<string, TimingSummary[]>();
            foreach (var dataset in Dataset.All)
            {
                int datasetId = (int)dataset.Code;
                if (datasets[datasetId])
                {
                    map[dataset.Name] = Enumerable.Range(0, TimingSeries.Duration).Select(interval =>
                    {
                        int segment = datasetId * TimingSeries.Duration + interval;
                        if (counts[segment] == 0)
                            return new TimingSummary(0, Double.NaN, Double.NaN, Double.NaN);
                        return new TimingSummary(
                            counts[segment],
                            sums[segment] / (double)Stopwatch.Frequency / counts[segment],
                            minima[segment] / (double)Stopwatch.Frequency,
                            maxima[segment] / (double)Stopwatch.Frequency
                        );
                    }).ToArray();
                }
            }
            return map;
        }
    }
}
