// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Outputs
{
    record ParsedTemplate(
         string Version,
         int Width,
         int Height,
         int[] PositionsX,
         int[] PositionsY,
         double[] Directions,
         string Types)
    {
        public static ParsedTemplate Parse(Fingerprint fp) => Serializer.Deserialize<ParsedTemplate>(TemplateCache.Load(fp));
    }
}
