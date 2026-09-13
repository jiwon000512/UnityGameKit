using System.Collections.Generic;

namespace GameKit.Tables
{
    public sealed class TableFile<TRow>
    {
        public string Table { get; set; }
        public int Version { get; set; }
        public List<string> Notes { get; set; }
        public List<TRow> Rows { get; set; }
    }
}
