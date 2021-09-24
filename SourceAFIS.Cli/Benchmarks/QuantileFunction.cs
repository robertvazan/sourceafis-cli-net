// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;

namespace SourceAFIS.Cli.Benchmarks
{
    class QuantileFunction
    {
        double[] Function;
        public QuantileFunction(List<double> function)
        {
            function.Sort();
            Function = function.ToArray();
        }
        public int Resolution => Function.Length;
        public double Bar(int index) => Function[index];
        public double Read(double probability)
        {
            int index = (int)(probability * Function.Length);
            if (index < 0)
                return Function[0];
            if (index >= Function.Length)
                return Function[Function.Length - 1];
            return Function[index];
        }
        public double Cdf(double threshold)
        {
            // Return 0%/100% if sample data does not cover the threshold.
            // This also covers cases when threshold is infinite.
            if (threshold <= Function[0])
                return 0;
            if (threshold > Function[Function.Length - 1])
                return 1;
            int min = 0, max = Function.Length - 1;
            while (min < max)
            {
                // If min+1 < max, then pivot will be between min and max. If min+1 == max, then pivot == min.
                int pivot = (min + max) / 2;
                double score = Function[pivot];
                // Quantile function is monotonically rising (but not strictly rising).
                // If we overshoot pivot, we will either overshoot score or get score equal to threshold.
                // If we undershoot pivot, we will also undershoot score.
                if (score >= threshold)
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
            return min / (double)Function.Length;
        }
        public double Average() => Function.Average();
    }
}
