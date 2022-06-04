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
        readonly long Epoch;
        readonly int Capacity;
        readonly bool[] Datasets;
        readonly long[] Counts;
        readonly long[] Sums;
        readonly long[] Maxima;
        readonly long[] Minima;
        public TimingSeriesBuilder(long epoch, int capacity)
        {
            Epoch = epoch;
            Datasets = new bool[Dataset.All.Length];
            Capacity = capacity;
            int segments = Datasets.Length * capacity;
            Counts = new long[segments];
            Sums = new long[segments];
            Maxima = new long[segments];
            Minima = new long[segments];
            Array.Fill(Minima, long.MaxValue);
        }
        public bool Add(Dataset dataset, long start, long end)
        {
            int interval = (int)((end - Epoch) / Stopwatch.Frequency);
            long duration = end - start;
            if (interval >= 0 && interval < Capacity && duration >= 0)
            {
                int datasetId = (int)dataset.Code;
                Datasets[datasetId] = true;
                int segment = datasetId * Capacity + interval;
                Sums[segment] += duration;
                Maxima[segment] = Math.Max(Maxima[segment], duration);
                Minima[segment] = Math.Min(Minima[segment], duration);
                ++Counts[segment];
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
                if (Datasets[datasetId])
                {
                    map[dataset.Name] = Enumerable.Range(0, Capacity).Select(interval =>
                    {
                        int segment = datasetId * Capacity + interval;
                        return new TimingSummary(
                            Counts[segment],
                            Sums[segment] / (double)Stopwatch.Frequency,
                            Counts[segment] > 0 ? Minima[segment] / (double)Stopwatch.Frequency : 0,
                            Maxima[segment] / (double)Stopwatch.Frequency
                        );
                    }).ToArray();
                }
            }
            return map;
        }
    }
}
