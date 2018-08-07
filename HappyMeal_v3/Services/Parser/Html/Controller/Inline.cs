using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ParserHtml
{
    class Inline
    {
        public InlineModel Get(string str, Tag e)
        {
            InlineModel inline;

            var pStart = "<" + e + "(?:(?!/>)[^>])*/>";
            var mElement = new Regex(pStart).Match(str);

            if (mElement.Success)
            {
                var index = mElement.Index;
                var attributes = new Attribut().Gets(mElement.Value);
                var value = mElement.Value;
                var success = true;
                inline = new InlineModel(index, attributes, value, success);
            }
            else inline = new InlineModel();
            return inline;
        }

        public List<InlineModel> Gets(string str, Tag e)
        {
            var inlines = new List<InlineModel>();

            var pStart = "<" + e + "(?:(?!/>)[^>])*/>";
            var mElements = new Regex(pStart).Matches(str);

            foreach (Match mElement in mElements)
            {
                var index = mElement.Index;
                var attributes = new Attribut().Gets(mElement.Value);
                var value = mElement.Value;
                var success = true;
                inlines.Add(new InlineModel(index, attributes, value, success));
            }
            return inlines;
        }

        public InlineModel GetById(string str, Tag e, string id)
        {
            InlineModel inline;

            var pStart = "<" + e + "(?:(?!id=)[^>])*id=\"(?:(?!" + id + ")[^\"])*" + id + "[^\"]*\"[^>]*/>";
            var mElement = new Regex(pStart).Match(str);

            if (mElement.Success)
            {
                var index = mElement.Index;
                var attributes = new Attribut().Gets(mElement.Value);
                var value = mElement.Value;
                var success = true;
                inline = new InlineModel(index, attributes, value, success);
            }
            else inline = new InlineModel();
            return inline;
        }

        public List<InlineModel> GetsByClass(string str, Tag e, string classe)
        {
            var inlines = new List<InlineModel>();

            var pStart = "<" + e + "(?:(?!class=)[^>])*class=\"(?:(?!" + classe + ")[^\"])*" + classe + "[^\"]*\"[^>]*/>";
            var mElements = new Regex(pStart).Matches(str);

            foreach (Match mElement in mElements)
            {
                var index = mElement.Index;
                var attributes = new Attribut().Gets(mElement.Value);
                var value = mElement.Value;
                var success = true;
                inlines.Add(new InlineModel(index, attributes, value, success));
            }
            return inlines;
        }

        public InlineModel GetMarker(string str, Tag e)
        {
            InlineModel inline;

            var pStart = "<" + e + "(?:(?!/>)[^>])*>";
            var mElement = new Regex(pStart).Match(str);

            if (mElement.Success)
            {
                var index = mElement.Index;
                var attributes = new Attribut().Gets(mElement.Value);
                var value = mElement.Value;
                var success = true;
                inline = new InlineModel(index, attributes, value, success);
            }
            else inline = new InlineModel();
            return inline;
        }
    }
}
