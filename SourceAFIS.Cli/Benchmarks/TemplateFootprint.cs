// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.IO;
using System.Linq;
using Serilog;
using SourceAFIS.Cli.Datasets;
using SourceAFIS.Cli.Outputs;
using SourceAFIS.Cli.Utils.Args;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Benchmarks
{
    class FootprintBenchmark : Command
    {
        public override string[] Subcommand => new[] { "benchmark", "footprint" };
        public override string Description => "Measure template footprint.";
        public static FootprintStats Measure(Fingerprint fp)
        {
            return Cache.Get(Path.Combine("benchmarks", "footprint"), fp.Path, () =>
            {
                var footprint = new FootprintStats();
                var serialized = TemplateCache.Load(fp);
                footprint.Count = 1;
                footprint.Serialized = serialized.Length;
                return footprint;
            });
        }
        public static FootprintStats Sum() { return FootprintStats.Sum(Fingerprint.All.Select(fp => Measure(fp)).ToList()); }
        public override void Run()
        {
            var sum = Sum();
            Log.Information("Template footprint: {Serialized} B serialized", sum.Serialized / sum.Count);
        }
    }
}
