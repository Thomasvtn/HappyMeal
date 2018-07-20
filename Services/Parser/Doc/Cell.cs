namespace Parsing
{
    public class Cell
    {
        public readonly string Value;
        public readonly int Index;
        public readonly string Text;

        public Cell(string text, int index)
        {
            Value = text;
            Index = index;
            Text = new Text(text).Value;
        }
    }
}
