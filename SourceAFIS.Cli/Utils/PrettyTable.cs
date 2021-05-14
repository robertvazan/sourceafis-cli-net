// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.Linq;

namespace SourceAFIS.Cli.Utils
{
    class PrettyTable
    {
        readonly List<string> Columns;
        readonly List<List<string>> Rows = new List<List<string>>();
        public PrettyTable(params string[] columns)
        {
            Columns = columns.ToList();
            Rows.Add(Columns);
        }
        public void Add(params string[] cells) => Rows.Add(cells.ToList());
        public string Format()
        {
            var widths = Enumerable.Range(0, Columns.Count).Select(cn => Rows.Select(r => r[cn].Length).Max()).ToArray();
            var lines = new List<string>();
            foreach (var row in Rows)
            {
                var line = "";
                for (int i = 0; i < Columns.Count; ++i)
                {
                    if (i + 1 < Columns.Count)
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
