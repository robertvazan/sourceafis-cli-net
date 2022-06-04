// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceAFIS.Cli.Checksums
{
    record ChecksumTable(List<ChecksumRow> Rows)
    {
        public static ChecksumTable Solo(string key, string mime, byte[] data)
        {
            var row = new ChecksumRow(key, ChecksumStats.Of(mime, data));
            return new ChecksumTable(new List<ChecksumRow> { row });
        }
        public static ChecksumTable Sum(IEnumerable<ChecksumTable> list)
        {
            var groups = new Dictionary<string, List<ChecksumStats>>();
            var keys = new List<string>();
            foreach (var table in list)
            {
                foreach (var row in table.Rows)
                {
                    List<ChecksumStats> group;
                    if (!groups.TryGetValue(row.Key, out group))
                    {
                        keys.Add(row.Key);
                        groups[row.Key] = group = new List<ChecksumStats>();
                    }
                    group.Add(row.Stats);
                }
            }
            return new ChecksumTable((from key in keys select new ChecksumRow(key, ChecksumStats.Sum(groups[key]))).ToList());
        }
        public ChecksumStats Stats(string key) => Rows.Where(r => r.Key == key).Select(r => r.Stats).FirstOrDefault();
        public string Mime(string key) => (Stats(key) ?? throw new ArgumentException("Transparency key not found: " + key)).Mime;
        public int Count(string key) => Stats(key)?.Count ?? 0;
    }
}
