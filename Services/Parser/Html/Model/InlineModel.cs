using System.Collections.Generic;

namespace ParserHtml
{
    public class InlineModel
    {
        public readonly int Index;
        public readonly List<AttributModel> Attributes;
        public readonly string Value;
        public int Length
        {
            get { return Value.Length; }
        }
        public readonly bool Success;

        public InlineModel()
        {
            Index = 0;
            Attributes = null;
            Value = string.Empty;
            Success = false;
        }

        public InlineModel(int index, List<AttributModel> attributes, string value, bool success)
        {
            Index = index;
            Attributes = attributes;
            Value = value;
            Success = success;
        }
    }
}
