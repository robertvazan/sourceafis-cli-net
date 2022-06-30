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
    record FootprintCommand : SimpleCommand
    {
        public override string[] Subcommand => new[] { "benchmark", "footprint" };
        public override string Description => "Measure template footprint.";
        static FootprintStats Measure(Fingerprint fp)
        {
            return Cache.Get(Path.Combine("benchmarks", "footprint"), fp.Path, () =>
            {
                return new FootprintStats(
                    TemplateCache.Load(fp).Length,
                    TemplateCache.Deserialize(fp).Memory(),
                    ParsedTemplate.Parse(fp).Types.Length
                );
            });
        }
        static FootprintStats Sum(Profile profile) => FootprintStats.Sum(profile.Fingerprints.Select(fp => Measure(fp)));
        public FootprintStats Sum() => Sum(Profile.Everything);
        public void Print(Profile[] profiles)
        {
            var table = new PrettyTable();
            foreach (var profile in profiles)
            {
                var stats = Sum(profile);
                table.Add("Dataset", profile.Name);
                table.Add("Serialized", Pretty.Bytes(stats.Serialized, profile.Name, "serialized"));
                table.Add("Memory", Pretty.Bytes(stats.Memory, profile.Name, "memory"));
                table.Add("Minutiae", Pretty.Minutiae(stats.Minutiae, profile.Name, "minutiae"));
            }
            table.Print();
        }
        public override void Run() => Print(Profile.All);
    }
}
