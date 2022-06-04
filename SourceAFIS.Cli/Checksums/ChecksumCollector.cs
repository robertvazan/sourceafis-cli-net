// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;

namespace SourceAFIS.Cli.Checksums
{
    class ChecksumCollector : FingerprintTransparency
    {
        readonly List<ChecksumTable> Records = new List<ChecksumTable>();
        public override void Take(string key, string mime, byte[] data) => Records.Add(ChecksumTable.Solo(key, mime, data));
        public static ChecksumTable Collect(Action action)
        {
            using (var transparency = new ChecksumCollector())
            {
                action();
                return ChecksumTable.Sum(transparency.Records);
            }
        }
    }
}
