// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;

namespace SourceAFIS.Cli.Utils
{
    class PrettyTable
    {
        readonly List<string> columns;
        readonly List<List<string>> rows = new List<List<string>>();
        public PrettyTable(params string[] columns)
        {
            this.columns = columns.ToList();
            rows.Add(this.columns);
        }
        public void Add(params string[] cells) => rows.Add(cells.ToList());
        public string Format()
        {
            var widths = Enumerable.Range(0, columns.Count).Select(cn => rows.Select(r => r[cn].Length).Max()).ToArray();
            var lines = new List<string>();
            foreach (var row in rows)
            {
                var line = "";
                for (int i = 0; i < columns.Count; ++i)
                {
                    if (i + 1 < columns.Count)
                        line += string.Format(string.Format("{{0,-{0}}}", widths[i] + 2), row[i]);
                    else
                        line += row[i];
                }
                lines.Add(line);
            }
            return string.Join("\n", lines);
        }
    }
}
