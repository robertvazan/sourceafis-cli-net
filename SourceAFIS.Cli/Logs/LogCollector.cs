// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;

namespace SourceAFIS.Cli.Logs
{
    class LogCollector : FingerprintTransparency
    {
        public readonly string Key;
        public readonly List<byte[]> Files = new List<byte[]>();
        public LogCollector(string key) => Key = key;
        public override bool Accepts(string key) => Key == key;
        public override void Take(string key, string mime, byte[] data) => Files.Add(data);
        public static byte[][] Collect(string key, Action action)
        {
            using (var logger = new LogCollector(key))
            {
                action();
                return logger.Files.ToArray();
            }
        }
    }
}
