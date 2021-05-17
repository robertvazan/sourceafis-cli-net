// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;
using System.Linq;
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
            try
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
            catch (Exception ex)
            {
                var chain = new List<Exception>();
                for (var cause = ex; cause != null; cause = cause.InnerException)
                    chain.Add(cause);
                Console.WriteLine(string.Join(" -> ", chain.Select(e => e.GetType().Name + ": " + e.Message)));
            }
        }
    }
}
