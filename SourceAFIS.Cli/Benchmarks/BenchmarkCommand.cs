// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Utils;
using SourceAFIS.Cli.Utils.Args;

namespace SourceAFIS.Cli.Benchmarks
{
    record BenchmarkCommand : SimpleCommand
    {
        public override string[] Subcommand => new[] { "benchmark" };
        public override string Description => "Measure algorithm accuracy, template footprint, and implementation speed.";
        public override void Run()
        {
            new AccuracyCommand().Print(Profile.Aggregate);
            Pretty.Print("");
            new FootprintCommand().Print(Profile.Aggregate);
            Pretty.Print("");
            new SpeedSummaryCommand().Run();
        }
    }
}
