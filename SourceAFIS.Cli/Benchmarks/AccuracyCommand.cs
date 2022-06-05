// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.IO;
using System.Linq;
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Utils;
using SourceAFIS.Cli.Utils.Args;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Benchmarks
{
    record AccuracyCommand : SimpleCommand
    {
        public override string[] Subcommand => new[] { "benchmark", "accuracy" };
        public override string Description => "Measure algorithm accuracy.";
        public static AccuracyStats Measure(Dataset dataset)
        {
            return Cache.Get(Path.Combine("benchmarks", "accuracy"), dataset.Path, () =>
            {
                var trio = new QuantileTrio(dataset);
                return new AccuracyStats(
                    trio.Eer(),
                    trio.FnmrAtFmr(1.0 / 100),
                    trio.FnmrAtFmr(1.0 / 1_000),
                    trio.FnmrAtFmr(1.0 / 10_000));
            });
        }
        AccuracyStats Sum(Profile profile) => AccuracyStats.Sum(profile.Datasets.Select(ds => Measure(ds)));
        public void Print(Profile[] profiles)
        {
            var table = new PrettyTable();
            foreach (var profile in profiles)
            {
                var stats = Sum(profile);
                table.Add("Dataset", profile.Name);
                table.Add("EER", Pretty.Accuracy(stats.Eer, profile.Name, "EER"));
                table.Add("FMR100", Pretty.Accuracy(stats.Fmr100, profile.Name, "FMR100"));
                table.Add("FMR1K", Pretty.Accuracy(stats.Fmr1K, profile.Name, "FMR1K"));
                table.Add("FMR10K", Pretty.Accuracy(stats.Fmr10K, profile.Name, "FMR10K"));
            }
            table.Print();
        }
        public override void Run() => Print(Profile.All);
    }
}
