// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Datasets;
using SourceAFIS.Cli.Utils;
using SourceAFIS.Cli.Utils.Args;

namespace SourceAFIS.Cli.Benchmarks
{
    class BenchmarkOverview : Command
    {
        public override string[] Subcommand => new[] { "benchmark" };
        public override string Description => "Measure algorithm accuracy, template footprint, and implementation speed.";
        public override void Run()
        {
            new AccuracyBenchmark().Print(Profile.Aggregate);
            Pretty.Print("");
            new FootprintBenchmark().Print(Profile.Aggregate);
            Pretty.Print("");
            new SpeedOverview().Run();
        }
    }
}
