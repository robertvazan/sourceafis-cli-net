// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Benchmarks;
using SourceAFIS.Cli.Checksums;
using SourceAFIS.Cli.Config;
using SourceAFIS.Cli.Logs;
using SourceAFIS.Cli.Utils.Args;

namespace SourceAFIS.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new CommandParser()
                .Add(new HomeOption())
                .Add(new NormalizationOption())
                .Add(new BaselineOption())
                .Add(new VersionReport())
                .Add(new BenchmarkOverview())
                .Add(new AccuracyBenchmark())
                .Add(new SpeedOverview())
                .Add(new ExtractionSpeed())
                .Add(new IdentificationSpeed())
                .Add(new VerificationSpeed())
                .Add(new ProbeSpeed())
                .Add(new SerializationSpeed())
                .Add(new DeserializationSpeed())
                .Add(new FootprintBenchmark())
                .Add(new Checksum())
                .Add(new TemplateChecksum())
                .Add(new ScoreChecksum())
                .Add(new ExtractorChecksum())
                .Add(new ProbeChecksum())
                .Add(new MatchChecksum())
                .Add(new ExtractorLog())
                .Add(new ProbeLog())
                .Add(new MatchLog());
            var command = parser.Parse(args);
            if (Configuration.Baseline != null)
            {
                Configuration.BaselineMode = true;
                command();
                Configuration.BaselineMode = false;
            }
            command();
        }
    }
}
