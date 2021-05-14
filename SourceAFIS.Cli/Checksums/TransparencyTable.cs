// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Linq;
using System.Collections.Generic;

namespace SourceAFIS.Cli.Checksums
{
    class TransparencyTable
    {
        public readonly List<TransparencyRow> Rows = new List<TransparencyRow>();
        public static TransparencyTable Solo(string key, string mime, byte[] data)
        {
            var row = new TransparencyRow();
            row.Key = key;
            row.Stats = TransparencyStats.Of(mime, data);
            var table = new TransparencyTable();
            table.Rows.Add(row);
            return table;
        }
        public static TransparencyTable Sum(IEnumerable<TransparencyTable> list)
        {
            var groups = new Dictionary<string, List<TransparencyStats>>();
            var sum = new TransparencyTable();
            foreach (var table in list)
            {
                foreach (var row in table.Rows)
                {
                    List<TransparencyStats> group;
                    if (!groups.TryGetValue(row.Key, out group))
                    {
                        var srow = new TransparencyRow();
                        srow.Key = row.Key;
                        sum.Rows.Add(srow);
                        groups[row.Key] = group = new List<TransparencyStats>();
                    }
                    group.Add(row.Stats);
                }
            }
            foreach (var row in sum.Rows)
                row.Stats = TransparencyStats.Sum(groups[row.Key]);
            return sum;
        }
        public TransparencyStats Stats(string key) => Rows.Where(r => r.Key == key).Select(r => r.Stats).FirstOrDefault();
        public string Mime(string key) => (Stats(key) ?? throw new ArgumentException("Transparency key not found: " + key)).Mime;
        public int Count(string key) => Stats(key)?.Count ?? 0;
    }
}
