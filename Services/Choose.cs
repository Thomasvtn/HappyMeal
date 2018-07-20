using Parsing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HappyMeal_v3.Models;
using System;

namespace HappyMeal_v3.Services
{
    public enum Extra
    {
        Bread, Cultery
    }

    public class Choose
    {
        public Parser Parser;

        public Choose(string text)
        {
            Parser = new Parser(text);
        }

        public string InsertChooses(string text, List<Choice> cells)
        {
            var insert = new Regex("</w:pPr>");
            foreach (var c in cells)
            {
                var cell = FindCellById(c.Id);
                if (cell != null)
                {
                    var pattern = Regex.Escape(cell.Value);
                    var regex = new Regex(pattern);
                    var match = regex.Match(text);

                    var pos = insert.Match(text.Substring(match.Index + match.Length));
                    var fontSize = 30;
                    var quantity = new StringBuilder();
                    quantity.Append(c.Quantity);
                    if (c.HasMulti)
                    {
                        fontSize = 14;
                        var multi = GetMultichoice(c.Id, c.Multichoice);
                        if (!string.IsNullOrEmpty(multi))
                            quantity.Append($" - {multi}");
                    }

                    var add = $@"<w:r><w:rPr><w:b/><w:color w:val=""FF0000""/><w:sz w:val=""{fontSize}""/><w:szCs w:val=""30""/></w:rPr><w:t>{quantity}</w:t></w:r><w:bookmarkStart w:id=""0"" w:name=""_GoBack"" /><w:bookmarkEnd w:id=""0""/>";
                    text = text.Insert(match.Index + match.Length + pos.Index + pos.Length, add);
                }
            }
            return text;
        }

        public string InsertMessage(string text, string message, Dictionary<Extra, int> extras)
        {
            var add = new StringBuilder();
            add.AppendFormat(@"<w:p w:rsidR=""000941C4"" w:rsidRPr=""00B865A9"" w:rsidRDefault=""00B865A9"" w:rsidP=""00B865A9"">" +
                @"<w:pPr><w:tabs><w:tab w:val=""left"" w:pos=""3580""/></w:tabs><w:spacing w:after=""0""/><w:jc w:val=""center""/>" +
                @"<w:rPr><w:b/><w:sz w:val=""32""/><w:szCs w:val=""44""/></w:rPr></w:pPr><w:r w:rsidRPr=""00B865A9""><w:rPr><w:b/><w:sz w:val=""32""/>" +
                @"<w:szCs w:val=""44""/></w:rPr><w:t>{0}</w:t></w:r><w:bookmarkStart w:id=""0"" w:name=""_GoBack""/><w:bookmarkEnd w:id=""0""/></w:p>", message);

            var search = new Regex("[c|C]ommande(r)? [a|A]vant");
            var replace = new Regex("<w:p (?:(?!</w:p>).)+</w:p>");

            var paragraphs = new Parser(text).Paragraphs;
            var index = 0;
            foreach (var p in paragraphs.Reverse<Paragraph>())
            {
                var match = search.Match(p.Value);
                if (match.Success)
                {
                    index = p.Index + match.Index;
                    break;
                }
            }

            var replaceMatch = replace.Match(text.Substring(index));
            if (replaceMatch.Success)
            {
                text = text.Remove(index + replaceMatch.Index, replaceMatch.Length);
                text = text.Insert(index + replaceMatch.Index, add.ToString());
            }
            else
            {
                var wp = new Regex("</w:p>");
                var findWp = wp.Match(text.Substring(index));
                if (findWp.Success)
                    text = text.Insert(index + findWp.Index + findWp.Length, add.ToString());
                else return string.Empty;
            }
            return text;
        }

        public Cell FindCellById(int id)
        {
            foreach (var t in Parser.Tables)
            {
                foreach (var r in t.Rows)
                {
                    foreach (var c in r.Cells)
                        if (c.Index == id)
                            return c;
                }
            }
            return null;
        }

        public string GetMultichoice(int id, List<int> choices)
        {
            var cell = FindCellById(id);
            if (cell == null) return string.Empty;

            var text = cell.Text;
            if (Regex.Matches(text, "hamburger", RegexOptions.IgnoreCase).Count > 1)
            {
                text = Regex.Replace(text, "hamburger", string.Empty, RegexOptions.IgnoreCase);
                text = $"Hamburger {text}";
            }

            var matches = Regex.Matches(text, @"\w+(\s+ou\s+\w+)+", RegexOptions.IgnoreCase);
            if (matches.Count != choices.Count) return string.Empty;

            var sb = new StringBuilder();
            for (var i = 0; i < choices.Count; i += 1)
            {
                var match = matches[i];
                var words = Regex.Matches(match.Value, @"\w+").Cast<Match>()
                    .Where(m => !m.Value.Equals("ou", StringComparison.CurrentCultureIgnoreCase)).ToList();
                sb.Append(words[choices[i]]);
                if (i + 1 < choices.Count) sb.Append(" - ");
            }
            return sb.ToString();
        }
    }
}