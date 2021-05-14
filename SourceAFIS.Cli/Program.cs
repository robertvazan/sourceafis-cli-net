// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using Serilog;
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
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "{Message:lj}{NewLine}{Exception}")
                .CreateLogger();
            var parser = new CommandParser()
                .Add(new NormalizationOption())
                .Add(new BaselineOption())
                .Add(new AccuracyBenchmark())
                .Add(new FootprintBenchmark())
                .Add(new ExtractorChecksum())
                .Add(new ExtractorLog());
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
