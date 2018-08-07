using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ParserHtml
{
    class Attribut
    {
        public List<AttributModel> Gets(string content)
        {
            List<AttributModel> attributes = new List<AttributModel>();

            var pAttribut = "[a-zA-Z-_]+=\"[^\"]*\"";
            var mAttributes = new Regex(pAttribut).Matches(content);

            foreach (Match mAttribut in mAttributes)
            {
                var key = mAttribut.Value.Split('=')[0];
                var equal = mAttribut.Value.IndexOf('=');
                var value = mAttribut.Value.Substring(equal + 2, mAttribut.Value.Length - equal - 3);
                attributes.Add(new AttributModel(key, value));
            }
            return attributes;
        }
    }
}
