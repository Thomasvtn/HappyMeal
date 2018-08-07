using System.Collections.Generic;

namespace ParserHtml
{
    public class BlockModel: InlineModel
    {
        public readonly string Text;

        public BlockModel(): base()
        {
            Text = string.Empty;
        }

        public BlockModel(int index, List<AttributModel> attributes, string value, string text, bool success): base(index, attributes, value, success)
        {
            Text = text;
        }
    }
}
