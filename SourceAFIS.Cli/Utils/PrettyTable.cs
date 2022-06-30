// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SourceAFIS.Cli.Utils
{
    class PrettyTable
    {
        readonly List<string> columns = new();
        readonly List<string> cells = new();
        public void Add(string column, string cell)
        {
            if (!columns.Contains(column))
                columns.Add(column);
            cells.Add(cell);
        }
        public string Format()
        {
            var rows = new List<List<string>>();
            rows.Add(columns);
            int rank = columns.Count;
            if (rank == 0)
                return "";
            for (int i = 0; i < cells.Count / rank; ++i)
                rows.Add(cells.GetRange(i * rank, rank));
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
        public void Print() => Pretty.Print(Format());
    }
}
