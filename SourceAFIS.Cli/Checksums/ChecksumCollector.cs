// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;

namespace SourceAFIS.Cli.Checksums
{
    class ChecksumCollector : FingerprintTransparency
    {
        readonly List<TransparencyTable> Records = new List<TransparencyTable>();
        public override void Take(string key, string mime, byte[] data) => Records.Add(TransparencyTable.Solo(key, mime, data));
        public static TransparencyTable Collect(Action action)
        {
            using (var transparency = new ChecksumCollector())
            {
                action();
                return TransparencyTable.Sum(transparency.Records);
            }
        }
    }
}
