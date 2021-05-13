// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;

namespace SourceAFIS.Cli.Benchmarks
{
    class QuantileFunction
    {
        public double[] Function;
        public QuantileFunction(List<double> function)
        {
            function.Sort();
            Function = function.ToArray();
        }
        public double Read(double probability)
        {
            double index = probability * Function.Length;
            // Quantile function can be visualized as a histogram with equally wide bars.
            // Provided probability lies between centers of two bars.
            int upperBar = (int)(index + 0.5);
            int lowerBar = upperBar - 1;
            // Extrapolation to infinity for first and last half-bar is safe and realistic.
            if (upperBar >= Function.Length)
                return double.PositiveInfinity;
            if (lowerBar < 0)
                return double.NegativeInfinity;
            // Interpolate between bar centers.
            double upperWeight = index - lowerBar - 0.5;
            double lowerWeight = 1 - upperWeight;
            return Function[lowerBar] * lowerWeight + Function[upperBar] * upperWeight;
        }
        public double Cdf(double threshold)
        {
            // Return 0%/100% if we sample data does not cover the threshold.
            // This also covers cases when threshold is infinite.
            if (threshold <= Function[0])
                return 0;
            if (threshold > Function[Function.Length - 1])
                return 1;
            double min = 0, max = 1;
            for (int i = 0; i < 30; ++i)
            {
                double probability = (min + max) / 2;
                double score = Read(probability);
                // Quantile function is monotonically rising.
                // If we overshoot probability, we will also overshoot score.
                // So if score is too high, we need to guess lower probability.
                if (score >= threshold)
                    max = probability;
                else
                    min = probability;
            }
            return (min + max) / 2;
        }
    }
}
