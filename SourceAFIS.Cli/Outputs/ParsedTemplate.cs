// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Datasets;
using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Outputs
{
    class ParsedTemplate
    {
        #pragma warning disable 0649
        public string Version;
        public int Width;
        public int Height;
        public int[] PositionsX;
        public int[] PositionsY;
        public double[] Directions;
        public string Types;
        #pragma warning restore 0649
        public static ParsedTemplate Parse(Fingerprint fp) => Serializer.Deserialize<ParsedTemplate>(TemplateCache.Load(fp));
    }
}
