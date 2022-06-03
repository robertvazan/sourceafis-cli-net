// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;

namespace SourceAFIS.Cli.Inputs
{
    abstract class SampleProfile
    {
        public abstract string Name { get; }
        public abstract Sample[] Samples { get; }
        public static SampleProfile[] Aggregate => new SampleProfile[] { new BestSamples(), new AllSamples() };
        public static SampleProfile[] All
        {
            get
            {
                var all = new List<SampleProfile>();
                foreach (var sample in Inputs.Samples.All())
                    all.Add(new SingleSample(sample));
                all.AddRange(Aggregate);
                return all.ToArray();
            }
        }
    }
}
