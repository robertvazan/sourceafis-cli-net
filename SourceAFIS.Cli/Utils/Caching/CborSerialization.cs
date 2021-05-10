// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Utils.Caching
{
    class CborSerialization : ICacheSerialization
    {
        public string Rename(string path) => path + ".cbor";
        public byte[] Serialize(object data) => Serializer.Serialize(data);
        public T Deserialize<T>(byte[] serialized) => Serializer.Deserialize<T>(serialized);
    }
}
