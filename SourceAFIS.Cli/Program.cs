// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Benchmarks;
using SourceAFIS.Cli.Checksums;
using SourceAFIS.Cli.Config;
using SourceAFIS.Cli.Logs;
using SourceAFIS.Cli.Utils.Args;
using SourceAFIS.Cli.Utils.Caching;

// TODO: Directory cache from Java: no compression, cache-defining classes, commands as records, SimpleCommand, drop DataIdentifier.
// TODO: MissingBaselineException from Java. Locate all references.
// TODO: Transparency logging commands should take dataset parameter like in Java.
// TODO: Parallelization like in Java.
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
                .Add(new VersionCommand())
                .Add(new BenchmarkCommand())
                .Add(new AccuracyCommand())
                .Add(new SpeedSummaryCommand())
                .Add(new ExtractionSpeedCommand())
                .Add(new IdentificationSpeedCommand())
                .Add(new VerificationSpeedCommand())
                .Add(new DeserializationSpeedCommand())
                .Add(new FootprintCommand())
                .Add(new ChecksumSummaryCommand())
                .Add(new TemplateChecksumCommand())
                .Add(new ScoreChecksumCommand())
                .Add(new ExtractionChecksumCommand())
                .Add(new ProbeChecksumCommand())
                .Add(new ComparisonChecksumCommand())
                .Add(new ExtractionLogCommand())
                .Add(new ProbeLogCommand())
                .Add(new ComparisonLogCommand())
                .Add(new PurgeCommand());
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
