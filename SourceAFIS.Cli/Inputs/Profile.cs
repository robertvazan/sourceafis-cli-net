// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;

namespace SourceAFIS.Cli.Inputs
{
    abstract class Profile
    {
        public abstract string Name { get; }
        public abstract Dataset[] Datasets { get; }
        public static Profile[] Aggregate => new Profile[] { new BestDatasets(), new AllDatasets() };
        public static Profile[] All
        {
            get
            {
                var all = new List<Profile>();
                foreach (var sample in Dataset.All)
                    all.Add(new SingleDataset(sample));
                all.AddRange(Aggregate);
                return all.ToArray();
            }
        }
        public static Profile Everything => new AllDatasets();
        public Fingerprint[] Fingerprints => Datasets.SelectMany(ds => ds.Fingerprints).ToArray();
    }
}
