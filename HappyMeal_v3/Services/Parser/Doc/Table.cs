using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Parsing
{
    public class Table
    {
        public string Text;
        private int index;
        public int Index
        {
            get { return index; }
        }
        private List<Row> rows;
        public List<Row> Rows
        {
            get { return rows; }
        }
        
        public Table(string text, int index)
        {
            this.Text = text;
            this.index = index;
            rows = GetRows();
        }

        private List<Row> GetRows()
        {
            var regex = new Regex("<w:tr(?:(?!</w:tr>).)+</w:tr>");
            var matches = regex.Matches(Text);
            var rows = new List<Row>();
            foreach (Match match in matches)
            {
                var text = this.Text.Substring(match.Index, match.Length);
                rows.Add(new Row(text, index + match.Index));
            }
            return rows;
        }
    }
}