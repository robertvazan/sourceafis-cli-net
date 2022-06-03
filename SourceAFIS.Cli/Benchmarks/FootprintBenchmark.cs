// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.IO;
using System.Linq;
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Outputs;
using SourceAFIS.Cli.Utils;
using SourceAFIS.Cli.Utils.Args;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Benchmarks
{
    class FootprintBenchmark : Command
    {
        public override string[] Subcommand => new[] { "benchmark", "footprint" };
        public override string Description => "Measure template footprint.";
        static FootprintStats Measure(Fingerprint fp)
        {
            return Cache.Get(Path.Combine("benchmarks", "footprint"), fp.Path, () =>
            {
                var footprint = new FootprintStats();
                footprint.Count = 1;
                footprint.Serialized = TemplateCache.Load(fp).Length;
                footprint.Memory = MemoryFootprint.Measure(TemplateCache.Deserialize(fp));
                footprint.Minutiae = ParsedTemplate.Parse(fp).Types.Length;
                return footprint;
            });
        }
        static FootprintStats Sum(Profile profile) => FootprintStats.Sum(profile.Fingerprints.Select(fp => Measure(fp)));
        public FootprintStats Sum() => Sum(Profile.Everything);
        public void Print(Profile[] profiles)
        {
            var table = new PrettyTable("Dataset", "Serialized", "Memory (est.)", "Minutiae");
            foreach (var profile in profiles)
            {
                var stats = Sum(profile);
                table.Add(
                    profile.Name,
                    Pretty.Bytes(stats.Serialized / stats.Count, profile.Name, "serialized"),
                    Pretty.Bytes(stats.Memory / stats.Count, profile.Name, "memory"),
                    Pretty.Minutiae(stats.Minutiae / stats.Count, profile.Name, "minutiae"));
            }
            Pretty.Print(table.Format());
        }
        public override void Run() => Print(Profile.All);
    }
}
