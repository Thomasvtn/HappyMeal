using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Parsing
{
    public class Row
    {
        public readonly string Value;
        private int index;
        public int Index
        {
            get { return index; }
        }
        private List<Cell> cells;
        public List<Cell> Cells
        {
            get { return cells; }
        }
        private int count;
        public int Count
        {
            get { return count; }
        }

        public Row(string text, int index)
        {
            Value = text;
            this.index = index;
            cells = GetCells();
        }

        private List<Cell> GetCells()
        {
            var regex = new Regex("<w:tc(?:(?!</w:tc>).)+</w:tc>");
            var matches = regex.Matches(Value);
            var cells = new List<Cell>();
            foreach (Match match in matches)
            {
                var text = Value.Substring(match.Index, match.Length);
                cells.Add(new Cell(text, index + match.Index));
                count += 1;
            }
            return cells;
        }

    }
}
