// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Diagnostics;
using System.Linq;
using SourceAFIS.Cli.Inputs;

namespace SourceAFIS.Cli.Benchmarks
{
    class SampleRecorder
    {
        readonly long Epoch;
        readonly int Capacity;
        readonly long[] Starts;
        readonly long[] Ends;
        readonly int[] Datasets;
        int Size;
        int Generation;
        readonly Random Random = new Random();
        public SampleRecorder(long epoch, int capacity)
        {
            Epoch = epoch;
            Capacity = capacity;
            Starts = new long[2 * capacity];
            Ends = new long[2 * capacity];
            Datasets = new int[2 * capacity];
        }
        void Compact()
        {
            for (int i = 0; i < Capacity; ++i)
            {
                int next = i + Random.Next(Size - i);
                long start = Starts[next];
                long end = Ends[next];
                var dataset = Datasets[next];
                Starts[next] = Starts[i];
                Ends[next] = Ends[i];
                Datasets[next] = Datasets[i];
                Starts[i] = start;
                Ends[i] = end;
                Datasets[i] = dataset;
            }
            Size = Capacity;
            ++Generation;
        }
        public void Record(Dataset dataset, long start, long end)
        {
            if (Generation == 0 || Random.Next(1 << Generation) == 0)
            {
                Starts[Size] = start;
                Ends[Size] = end;
                Datasets[Size] = (int)dataset.Code;
                ++Size;
                if (Size >= 2 * Capacity)
                    Compact();
            }
        }
        public OperationTiming[] Complete()
        {
            return Enumerable.Range(0, Size).Select(n =>
            {
                var operation = new OperationTiming();
                operation.Start = (Starts[n] - Epoch) / (double)Stopwatch.Frequency;
                operation.End = (Ends[n] - Epoch) / (double)Stopwatch.Frequency;
                operation.Dataset = new Dataset((DatasetCode)Datasets[n]).Name;
                return operation;
            }).ToArray();
        }
    }
}
