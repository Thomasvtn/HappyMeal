using System.Text.RegularExpressions;

namespace Parsing
{
    public class Text
    {
        private readonly string _text;
        public string Value { get; }

        public Text(string text)
        {
            _text = text;
            Value = GetValue();
        }

        private string GetValue()
        {
            var startRegex = new Regex("<w:t( [^>]*>|>)");
            var startMatches = startRegex.Matches(_text);
            var endRegex = new Regex("</w:t>");
            var endMatches = endRegex.Matches(_text);

            var value = string.Empty;
            for (var i = 0; i < startMatches.Count; i += 1)
            {
                var startSub = startMatches[i].Index + startMatches[i].Length;
                var endSub = endMatches[i].Index;
                value += _text.Substring(startSub, endSub - startSub);
            }
            return value;
        }
    }
}