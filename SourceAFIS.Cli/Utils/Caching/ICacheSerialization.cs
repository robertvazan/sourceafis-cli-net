// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli

namespace SourceAFIS.Cli.Utils.Caching
{
    interface ICacheSerialization
    {
        string Rename(string path);
        byte[] Serialize(object data);
        T Deserialize<T>(byte[] serialized);
        static ICacheSerialization Select<T>()
        {
            if (typeof(T) == typeof(byte[]))
                return new TrivialSerialization();
            return new CborSerialization();
        }
    }
}
