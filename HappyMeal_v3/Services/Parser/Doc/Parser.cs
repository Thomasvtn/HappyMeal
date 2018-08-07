using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Parsing
{
    public class Parser
    {
        private readonly string Text;
        private List<Table> tables;
        public List<Table> Tables
        {
            get
            {
                if (tables == null)
                    tables = GetTables();
                return tables;
            }
        }
        private List<Paragraph> paragraphs;
        public List<Paragraph> Paragraphs
        {
            get
            {
                if (paragraphs == null)
                    paragraphs = GetParagraphs();
                return paragraphs;
            }
        }

        public Parser(string text)
        {
            this.Text = text;
        }

        private List<Table> GetTables()
        {
            var regex = new Regex("<w:tbl(?:(?!</w:tbl>).)+</w:tbl>");
            var matches = regex.Matches(Text);
            var tables = new List<Table>();
            foreach (Match match in matches)
            {
                var text = this.Text.Substring(match.Index, match.Length);
                tables.Add(new Table(text, match.Index));
            }
            return tables;
        }

        private List<Paragraph> GetParagraphs()
        {
            var regex = new Regex("<w:p(?:(?!</w:p>).)+</w:p>");
            var matches = regex.Matches(Text);
            var paragraphs = new List<Paragraph>();
            foreach (Match match in matches)
            {
                var text = this.Text.Substring(match.Index, match.Length);
                paragraphs.Add(new Paragraph(text, match.Index));
            }
            return paragraphs;
        }
    }
}
