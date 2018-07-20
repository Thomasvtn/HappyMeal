namespace Parsing
{
    public class Paragraph
    {
        private readonly string text;
        private int index;
        public int Index
        {
            get { return index; }
        }
        private string value;
        public string Value
        {
            get { return value; }
        }

        public Paragraph(string text, int index)
        {
            this.text = text;
            this.index = index;
            value = new Text(text).Value;
        }
    }
}