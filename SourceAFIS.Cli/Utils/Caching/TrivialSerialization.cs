// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Utils.Caching
{
    class TrivialSerialization : ICacheSerialization
    {
        public string Rename(string path) => path;
        public byte[] Serialize(object data) => (byte[])data;
        public T Deserialize<T>(byte[] serialized) => (T)(object)serialized;
    }
}
