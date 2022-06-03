// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;
using SourceAFIS.Cli.Inputs;
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
        // Equivalent of Java's Math.nextUp().
        static double NextUp(double value)
        {
            // We are neglecting to handle non-fininte numbers and negative zero, which do not occur in scores.
            long bits = BitConverter.DoubleToInt64Bits(value);
            return BitConverter.Int64BitsToDouble(bits + (bits >= 0 ? 1 : -1));
        }

        public double FnmrAtFmr(double fmr)
        {
            // Next higher threshold, so that score at FMR is not included.
            // This ensures that the FNMR is reached at or below given FMR rather than at or above it.
            double threshold = NextUp(Nonmatching.Read(1 - fmr));
            return Matching.Cdf(threshold);
        }
        public double Eer()
        {
            int min = 0, max = Nonmatching.Resolution;
            while (true)
            {
                // If min+1 < max, then pivot will be between min and max. If min+1 == max, then pivot == min.
                int pivot = (min + max) / 2;
                // Allow past-the-end pivots with threshold higher than all non-matching scores.
                double threshold = pivot < Nonmatching.Resolution ? Nonmatching.Bar(pivot) : NextUp(Nonmatching.Bar(pivot - 1));
                double fmr = 1 - Nonmatching.Cdf(threshold);
                double fnmr = Matching.Cdf(threshold);
                // If FMR and FNMR are not equal, return the higher one (FNMR) as a conservative estimate.
                if (min >= max)
                    return fnmr;
                // We want lowest threshold with FMR no higher than FNMR, so keep looking lower while the condition is satisfied.
                // If FMR is higher than FNMR, the threshold is definitely unacceptable, so go above the pivot.
                if (fmr <= fnmr)
                {
                    // If min+1 == max, then max will be set to min here.
                    max = pivot;
                }
                else
                {
                    // If min+1 == max, then min will be set to max here.
                    min = pivot + 1;
                }
            }
        }
    }
}
