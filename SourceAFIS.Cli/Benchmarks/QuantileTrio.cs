// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using SourceAFIS.Cli.Datasets;
using SourceAFIS.Cli.Outputs;

namespace SourceAFIS.Cli.Benchmarks
{
    class QuantileTrio
    {
        public readonly QuantileFunction Matching;
        public readonly QuantileFunction Nonmatching;
        public readonly QuantileFunction Selfmatching;
        public QuantileTrio(Dataset dataset)
        {
            var fingerprints = dataset.Fingerprints;
            var scores = ScoreCache.Load(dataset);
            var matching = new List<double>();
            var nonmatching = new List<double>();
            var selfmatching = new List<double>();
            foreach (var probe in fingerprints)
            {
                foreach (var candidate in fingerprints)
                {
                    var score = scores[probe.Id][candidate.Id];
                    if (probe.Id == candidate.Id)
                        selfmatching.Add(score);
                    else if (probe.Finger.Id == candidate.Finger.Id)
                        matching.Add(score);
                    else
                        nonmatching.Add(score);
                }
            }
            Matching = new QuantileFunction(matching);
            Nonmatching = new QuantileFunction(nonmatching);
            Selfmatching = new QuantileFunction(selfmatching);
        }
        public double FnmrAtFmr(double fmr)
        {
            double threshold = Nonmatching.Read(1 - fmr);
            return Matching.Cdf(threshold);
        }
        public double Eer()
        {
            double min = 0, max = 1;
            int iteration = 0;
            while (true)
            {
                double fmr = (min + max) / 2;
                double fnmr = FnmrAtFmr(fmr);
                if (iteration >= 30)
                    return fnmr;
                // FMR and FNMR change at the same time, but the basic rule still works:
                // If FMR > FNMR, we need to try lower FMR, otherwise higher FMR.
                if (fmr > fnmr)
                    max = fmr;
                else
                    min = fmr;
                ++iteration;
            }
        }
    }
}
