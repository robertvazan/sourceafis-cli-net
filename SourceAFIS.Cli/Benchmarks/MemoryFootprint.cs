// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Reflection;

namespace SourceAFIS.Cli.Benchmarks
{
    class MemoryFootprint
    {
        static int Align(int alignment, int size) => (size + alignment - 1) / alignment * alignment;
        static int ObjectHeader() => 2 * 8;
        static int ArrayHeader() => ObjectHeader() + 8;
        static int IntPoint() => 2 * sizeof(int);
        static int MinutiaType() => sizeof(byte);
        static int Minutia() => ObjectHeader() + IntPoint() + sizeof(double) + Align(8, MinutiaType());
        static int EdgeShape() => ObjectHeader() + Align(8, sizeof(int)) + 2 * sizeof(double);
        static int NeighborEdge() => EdgeShape() + Align(8, sizeof(int));
        static int Minutiae(Array minutiae) => ArrayHeader() + minutiae.Length * (8 + Minutia());
        static int Edges(Array edges)
        {
            int sum = ArrayHeader() + edges.Length * 8;
            for (int i = 0; i < edges.Length; ++i)
                sum += ArrayHeader() + ((Array)edges.GetValue(i)).Length * (8 + NeighborEdge());
            return sum;
        }
        public static int Measure(FingerprintTemplate template)
        {
            var type = template.GetType();
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var minutiae = type.GetField("Minutiae", flags).GetValue(template);
            var edges = type.GetField("Edges", flags).GetValue(template);
            return ObjectHeader() + IntPoint() + 8 + 8 + Minutiae((Array)minutiae) + Edges((Array)edges);
        }
    }
}
