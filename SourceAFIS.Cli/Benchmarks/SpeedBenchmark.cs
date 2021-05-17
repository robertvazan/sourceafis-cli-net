// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SourceAFIS.Cli.Datasets;
using SourceAFIS.Cli.Utils;
using SourceAFIS.Cli.Utils.Args;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Benchmarks
{
    abstract class SpeedBenchmark : Command
    {
        public const int Duration = 60;
        public const int Warmup = 20;
        public const int NetDuration = Duration - Warmup;
        public const int SampleSize = 10_000;
        public abstract string Name { get; }
        public override string[] Subcommand => new[] { "benchmark", "speed", Name };
        public abstract TimingStats Measure();
    }
    abstract class SpeedBenchmark<K> : SpeedBenchmark
    {
        protected abstract Dataset Dataset(K id);
        protected abstract K[] Shuffle();
        protected static T[] Shuffle<T>(IEnumerable<T> list)
        {
            var random = new Random();
            var shuffled = list.ToArray();
            for (int i = shuffled.Length; i > 0; --i)
            {
                int k = random.Next(i);
                T tmp = shuffled[k];
                shuffled[k] = shuffled[i - 1];
                shuffled[i - 1] = tmp;
            }
            return shuffled;
        }
        static TimingStats[] Parallelize(Func<Func<TimingStats>> setup)
        {
            var threads = new List<Thread>();
            var futures = new List<TaskCompletionSource<TimingStats>>();
            for (int i = 0; i < Environment.ProcessorCount; ++i)
            {
                var future = new TaskCompletionSource<TimingStats>();
                futures.Add(future);
                var benchmark = setup();
                var thread = new Thread(() =>
                {
                    try
                    {
                        future.SetResult(benchmark());
                    }
                    catch (Exception ex)
                    {
                        future.SetException(ex);
                    }
                });
                threads.Add(thread);
            }
            foreach (var thread in threads)
                thread.Start();
            foreach (var thread in threads)
                thread.Join();
            return futures.Select(f => f.Task.Result).ToArray();
        }
        protected TimingStats Measure(Func<Func<TimedOperation<K>>> setup)
        {
            return Cache.Get(Path.Combine("benchmarks", "speed", Name), "measurement", () =>
            {
                var nondeterministic = false;
                var epoch = Stopwatch.GetTimestamp();
                var allocator = setup();
                var strata = Parallelize(() =>
                {
                    var ids = Shuffle();
                    var recorder = new TimingRecorder(epoch, Duration, SampleSize);
                    var operation = allocator();
                    return () =>
                    {
                        while (true)
                        {
                            foreach (var id in ids)
                            {
                                operation.Prepare(id);
                                long start = Stopwatch.GetTimestamp();
                                operation.Execute();
                                long end = Stopwatch.GetTimestamp();
                                if (!operation.Verify())
                                    Volatile.Write(ref nondeterministic, true);
                                if (!recorder.Record(Dataset(id), start, end))
                                    return recorder.Complete();
                            }
                        }
                    };
                });
                if (Volatile.Read(ref nondeterministic))
                    Pretty.Print("Non-deterministic algorithm.");
                return TimingStats.Sum(SampleSize, strata);
            });
        }
        public override void Run()
        {
            var all = Measure().Skip(Warmup);
            var global = TimingSummary.SumAll(all.Segments.Values.SelectMany(a => a));
            Pretty.Print("Gross speed: " + Pretty.Speed(global.Count / (double)NetDuration, "gross"));
            var table = new SpeedTable("Dataset");
            foreach (var profile in Profile.All)
                table.Add(profile.Name, all.Narrow(profile));
            table.Print();
        }
    }
}
