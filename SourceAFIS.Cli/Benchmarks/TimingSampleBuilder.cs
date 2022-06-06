// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Diagnostics;
using System.Linq;
using SourceAFIS.Cli.Inputs;

namespace SourceAFIS.Cli.Benchmarks
{
    class TimingSampleBuilder
    {
        readonly long epoch;
        readonly long[] starts;
        readonly long[] ends;
        readonly int[] datasets;
        int size;
        int generation;
        readonly Random random = new Random();
        public TimingSampleBuilder(long epoch)
        {
            this.epoch = epoch;
            starts = new long[2 * TimingSample.SampleSize];
            ends = new long[2 * TimingSample.SampleSize];
            datasets = new int[2 * TimingSample.SampleSize];
        }
        void Compact()
        {
            for (int i = 0; i < TimingSample.SampleSize; ++i)
            {
                int next = i + random.Next(size - i);
                long start = starts[next];
                long end = ends[next];
                var dataset = datasets[next];
                starts[next] = starts[i];
                ends[next] = ends[i];
                datasets[next] = datasets[i];
                starts[i] = start;
                ends[i] = end;
                datasets[i] = dataset;
            }
            size = TimingSample.SampleSize;
            ++generation;
        }
        public void Add(Dataset dataset, long start, long end)
        {
            if (generation == 0 || random.Next(1 << generation) == 0)
            {
                starts[size] = start;
                ends[size] = end;
                datasets[size] = (int)dataset.Code;
                ++size;
                if (size >= 2 * TimingSample.SampleSize)
                    Compact();
            }
        }
        public TimingMeasurement[] Build()
        {
            // Limit size to capacity, so that size does not randomly vary between threads.
            // If we are in generation 0 and size is still below capacity, threads should still have comparable number of samples.
            if (size > TimingSample.SampleSize)
                Compact();
            return Enumerable.Range(0, size).Select(n =>
            {
                return new TimingMeasurement(
                    new Dataset((DatasetCode)datasets[n]).Name,
                    (starts[n] - epoch) / (double)Stopwatch.Frequency,
                    (ends[n] - epoch) / (double)Stopwatch.Frequency
                );
            }).ToArray();
        }
    }
}
