// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SourceAFIS.Cli.Datasets;

namespace SourceAFIS.Cli.Benchmarks
{
    class SummaryRecorder
    {
        readonly long Epoch;
        readonly int Capacity;
        readonly bool[] Datasets;
        readonly long[] Counts;
        readonly long[] Sums;
        readonly long[] Maxima;
        readonly long[] Minima;
        public SummaryRecorder(long epoch, int capacity)
        {
            Epoch = epoch;
            Datasets = new bool[Samples.All().Length];
            Capacity = capacity;
            int segments = Datasets.Length * capacity;
            Counts = new long[segments];
            Sums = new long[segments];
            Maxima = new long[segments];
            Minima = new long[segments];
            Array.Fill(Minima, long.MaxValue);
        }
        public bool Record(Dataset dataset, long start, long end)
        {
            int interval = (int)((end - Epoch) / Stopwatch.Frequency);
            long duration = end - start;
            if (interval >= 0 && interval < Capacity && duration >= 0)
            {
                int datasetId = (int)dataset.Sample;
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
        public Dictionary<string, TimingSummary[]> Complete()
        {
            var map = new Dictionary<string, TimingSummary[]>();
            foreach (var sample in Samples.All())
            {
                int datasetId = (int)sample;
                if (Datasets[datasetId])
                {
                    map[Samples.Name(sample)] = Enumerable.Range(0, Capacity).Select(interval =>
                    {
                        var summary = new TimingSummary();
                        int segment = datasetId * Capacity + interval;
                        summary.Count = Counts[segment];
                        summary.Sum = Sums[segment] / (double)Stopwatch.Frequency;
                        summary.Max = Maxima[segment] / (double)Stopwatch.Frequency;
                        summary.Min = summary.Count > 0 ? Minima[segment] / (double)Stopwatch.Frequency : 0;
                        return summary;
                    }).ToArray();
                }
            }
            return map;
        }
    }
}
