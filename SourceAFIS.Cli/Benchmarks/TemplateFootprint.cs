// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog;
using SourceAFIS.Cli.Utils.Args;

namespace SourceAFIS.Cli.Benchmarks
{
    class FootprintBenchmark : Command
    {
        public override string[] Subcommand => new[] { "benchmark", "footprint" };
        public override string Description => "Measure template footprint.";
        public static FootprintStats Measure(SampleFingerprint fp)
        {
            return PersistentCache.Get(Path.Combine("benchmarks", "footprint"), fp.Path, () =>
            {
                var footprint = new FootprintStats();
                var serialized = NativeTemplate.Serialized(fp);
                footprint.Count = 1;
                footprint.Serialized = serialized.Length;
                return footprint;
            });
        }
        public static FootprintStats Sum() { return FootprintStats.Sum(SampleFingerprint.All.Select(fp => Measure(fp)).ToList()); }
        public override void Run()
        {
            var sum = Sum();
            Log.Information("Template footprint: {Serialized} B serialized", sum.Serialized / sum.Count);
        }
    }
}
