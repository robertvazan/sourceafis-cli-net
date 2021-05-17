// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Utils.Args;

namespace SourceAFIS.Cli.Benchmarks
{
    class SpeedOverview : Command
    {
        public override string[] Subcommand => new[] { "benchmark", "speed" };
        public override string Description => "Measure algorithm speed.";
        public override void Run()
        {
            var benchmarks = new SpeedBenchmark[]
            {
                new ExtractionSpeed(),
                new IdentificationSpeed(),
                new VerificationSpeed(),
                new ProbeSpeed(),
                new SerializationSpeed(),
                new DeserializationSpeed()
            };
            var table = new SpeedTable("Operation");
            foreach (var benchmark in benchmarks)
                table.Add(benchmark.Name, benchmark.Measure().Skip(SpeedBenchmark.Warmup));
            table.Print();
        }
    }
}
