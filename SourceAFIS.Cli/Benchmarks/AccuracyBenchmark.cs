// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.IO;
using System.Linq;
using SourceAFIS.Cli.Datasets;
using SourceAFIS.Cli.Utils;
using SourceAFIS.Cli.Utils.Args;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Benchmarks
{
    class AccuracyBenchmark : Command
    {
        public override string[] Subcommand => new[] { "benchmark", "accuracy" };
        public override string Description => "Measure algorithm accuracy.";
        public static AccuracyStats Measure(Dataset dataset)
        {
            return Cache.Get(Path.Combine("benchmarks", "accuracy"), dataset.Path, () =>
            {
                var trio = new QuantileTrio(dataset);
                var stats = new AccuracyStats();
                stats.Fmr100 = trio.FnmrAtFmr(1.0 / 100);
                stats.Fmr1K = trio.FnmrAtFmr(1.0 / 1_000);
                stats.Fmr10K = trio.FnmrAtFmr(1.0 / 10_000);
                stats.Eer = trio.Eer();
                return stats;
            });
        }
        AccuracyStats Sum(Profile profile) => AccuracyStats.Sum(profile.Datasets.Select(ds => Measure(ds)));
        public void Print(Profile[] profiles)
        {
            var table = new PrettyTable("Dataset", "EER", "FMR100", "FMR1K", "FMR10K");
            foreach (var profile in profiles)
            {
                var stats = Sum(profile);
                table.Add(profile.Name,
                    Pretty.Accuracy(stats.Eer, profile.Name, "EER"),
                    Pretty.Accuracy(stats.Fmr100, profile.Name, "FMR100"),
                    Pretty.Accuracy(stats.Fmr1K, profile.Name, "FMR1K"),
                    Pretty.Accuracy(stats.Fmr10K, profile.Name, "FMR10K"));
            }
            Pretty.Print(table.Format());
        }
        public override void Run() => Print(Profile.All);
    }
}
