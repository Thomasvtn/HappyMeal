using System.Collections.Generic;

namespace ParserHtml
{
    public enum Tag
    {
        div, ul, li, span, p, img, a, th, tr, td, input, h1
    }

    public class ParserFacade
    {
        private readonly Block block;
        private readonly Inline inline;

		public ParserFacade()
        {
            block = new Block();
            inline = new Inline();
        }

        public BlockModel GetBlock(string str, Tag e)
        {
            return block.Get(str, e);
        }

        public List<BlockModel> GetBlocks(string str, Tag e)
        {
            return block.Gets(str, e);
        }

        public BlockModel GetBlockById(string str, Tag e, string id)
        {
            return block.GetById(str, e, id);
        }

        public List<BlockModel> GetBlocksById(string str, Tag e, string id)
        {
            return block.GetsById(str, e, id);
        }

        public List<BlockModel> GetBlockByIdStartWith(string str, Tag e, string start)
        {
            return block.GetsByIdStartWith(str, e, start);
        }

        public List<BlockModel> GetBlocksByClass(string str, Tag e, string classe)
        {
            return block.GetsByClass(str, e, classe);
        }

        public InlineModel GetInline(string str, Tag e)
        {
            return inline.Get(str, e);
        }

        public List<InlineModel> GetInlines(string str, Tag e)
        {
            return inline.Gets(str, e);
        }

        public InlineModel GetInlineById(string str, Tag e, string id)
        {
            return inline.GetById(str, e, id);
        }

        public List<InlineModel> GetsInlineByClass(string str, Tag e, string id)
        {
            return inline.GetsByClass(str, e, id);
        }

        public InlineModel GetMarker(string str, Tag e)
        {
            return inline.GetMarker(str, e);
        }
    }
}
