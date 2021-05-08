// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using Serilog;
using SourceAFIS.Cli.Benchmarks;
using SourceAFIS.Cli.Config;
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
                .Add(new FootprintBenchmark());
            if (args.Length < 1)
                return;
            switch (args[0])
            {
                case "accuracy":
                    ScalarAccuracy.Report();
                    break;
                case "extractor-transparency-stats":
                    TransparencyStats.Report(TransparencyStats.ExtractorTable());
                    break;
                case "extractor-transparency-files":
                    if (args.Length < 2)
                        return;
                    TransparencyFile.Extractor(args[1]);
                    break;
                case "normalized-extractor-transparency-files":
                    if (args.Length < 2)
                        return;
                    TransparencyFile.ExtractorNormalized(args[1]);
                    break;
            }
        }
    }
}
