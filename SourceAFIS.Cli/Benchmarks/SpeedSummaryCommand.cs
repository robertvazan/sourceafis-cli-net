// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Utils.Args;

namespace SourceAFIS.Cli.Benchmarks
{
    record SpeedSummaryCommand : SimpleCommand
    {
        public override string[] Subcommand => new[] { "benchmark", "speed" };
        public override string Description => "Measure algorithm speed.";
        public override void Run()
        {
            var benchmarks = new SpeedCommand[]
            {
                new ExtractionSpeedCommand(),
                new IdentificationSpeedCommand(),
                new VerificationSpeedCommand(),
                new ProbeSpeedCommand(),
                new DeserializationSpeedCommand()
            };
            var table = new SpeedTable("Operation");
            foreach (var benchmark in benchmarks)
                table.Add(benchmark.Name, benchmark.Measure().Skip(SpeedCommand.Warmup));
            table.Print();
        }
    }
}
