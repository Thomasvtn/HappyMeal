using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ParserHtml
{
    class Block
    {
        public BlockModel Get(string str, Tag e)
        {
            BlockModel block;

            var pStart = "<" + e + "[^>]*>";
            var pEnd = "</" + e + ">";

            var mElement = new Regex(pStart).Match(str);

            if (mElement.Success)
            {
                var count = 1;
                var start = mElement.Index + mElement.Length;
                var content = mElement.Length;

                while (count > 0)
                {
                    var sub = str.Substring(start);
                    var rStart = new Regex(pStart).Match(sub);
                    var rEnd = new Regex(pEnd).Match(sub);

                    if (rStart.Index < rEnd.Index && rStart.Success && rEnd.Success)
                    {
                        start += rStart.Index + rStart.Length;
                        count += 1;
                        content += rStart.Index + rStart.Length;
                    }
                    else
                    {
                        start += rEnd.Index + rEnd.Length;
                        count -= 1;
                        content += rEnd.Index + rEnd.Length;
                    }
                }

                var attributes = new Attribut().Gets(mElement.Value);
                var value = str.Substring(mElement.Index, content);
                var index = mElement.Index;
                var length = value.Length;
                var text = str.Substring(index + mElement.Length, length - mElement.Length - 3 - e.ToString().Length);
                var success = true;
                block = new BlockModel(index, attributes, value, text, success);
            }
            else block = new BlockModel();
            return block;
        }

        public List<BlockModel> Gets(string str, Tag e)
        {
            var blocks = new List<BlockModel>();

            var pStart = "<" + e + "[^>]*>";
            var pEnd = "</" + e + ">";

            var mElements = new Regex(pStart).Matches(str);

            if (mElements.Count > 0)
            {
                foreach (Match mElement in mElements)
                {
                    var count = 1;
                    var start = mElement.Index + mElement.Length;
                    var content = mElement.Length;

                    while (count > 0)
                    {
                        var sub = str.Substring(start);
                        var rStart = new Regex(pStart).Match(sub);
                        var rEnd = new Regex(pEnd).Match(sub);

                        if (rStart.Index < rEnd.Index && rStart.Success && rEnd.Success)
                        {
                            start += rStart.Index + rStart.Length;
                            count += 1;
                            content += rStart.Index + rStart.Length;
                        }
                        else
                        {
                            start += rEnd.Index + rEnd.Length;
                            count -= 1;
                            content += rEnd.Index + rEnd.Length;
                        }
                    }
                    var attributes = new Attribut().Gets(mElement.Value);
                    var value = str.Substring(mElement.Index, content);
                    var index = mElement.Index;
                    var length = value.Length;
                    var text = str.Substring(index + mElement.Length, length - mElement.Length - 3 - e.ToString().Length);
                    var success = true;
                    blocks.Add(new BlockModel(index, attributes, value, text, success));
                }
            }
            return blocks;
        }

        public BlockModel GetById(string str, Tag e, string id)
        {
            BlockModel block;

            var pStart = "<" + e + "[^>]*>";
            var pEnd = "</" + e + ">";

            var pId = "<" + e + "(?:(?!id=)[^>])*id=\"(?:(?!" + id + ")[^\"])*" + id + "[^\"]*\"[^>]*>";
            var mId = new Regex(pId).Match(str);

            if (mId.Success)
            {
                var count = 1;
                var start = mId.Index + mId.Length;
                var content = mId.Length;

                while (count > 0)
                {
                    var sub = str.Substring(start);
                    var rStart = new Regex(pStart).Match(sub);
                    var rEnd = new Regex(pEnd).Match(sub);

                    if (rStart.Index < rEnd.Index && rStart.Success && rEnd.Success)
                    {
                        start += rStart.Index + rStart.Length;
                        count += 1;
                        content += rStart.Index + rStart.Length;
                    }
                    else
                    {
                        start += rEnd.Index + rEnd.Length;
                        count -= 1;
                        content += rEnd.Index + rEnd.Length;
                    }
                }
                var attributes = new Attribut().Gets(mId.Value);
                var value = str.Substring(mId.Index, content);
                var index = mId.Index;
                var length = value.Length;
                var text = str.Substring(index + mId.Length, length - mId.Length - 3 - e.ToString().Length);
                var success = true;
                block = new BlockModel(index, attributes, value, text, success);
            }
            else block = new BlockModel();
            return block;
        }

        public List<BlockModel> GetsById(string str, Tag e, string id)
        {
            var blocks = new List<BlockModel>();

            var pStart = "<" + e + "[^>]*>";
            var pEnd = "</" + e + ">";

            var pId = "<" + e + "(?:(?!id=).)*id=\"(?:(?!" + id + ").)*" + id + "[^\"]*\"[^>]*>";
            var mIds = new Regex(pId).Matches(str);

            if (mIds.Count > 0)
            {
                foreach (Match mId in mIds)
                {
                    var count = 1;
                    var start = mId.Index + mId.Length;
                    var content = mId.Length;

                    while (count > 0)
                    {
                        var sub = str.Substring(start);
                        var rStart = new Regex(pStart).Match(sub);
                        var rEnd = new Regex(pEnd).Match(sub);

                        if (rStart.Index < rEnd.Index && rStart.Success && rEnd.Success)
                        {
                            start += rStart.Index + rStart.Length;
                            count += 1;
                            content += rStart.Index + rStart.Length;
                        }
                        else
                        {
                            start += rEnd.Index + rEnd.Length;
                            count -= 1;
                            content += rEnd.Index + rEnd.Length;
                        }
                    }
                    var attributes = new Attribut().Gets(mId.Value);
                    var value = str.Substring(mId.Index, content);
                    var index = mId.Index;
                    var length = value.Length;
                    var text = str.Substring(index + mIds[0].Length, length - mIds[0].Length - 3 - e.ToString().Length);
                    var success = true;
                    blocks.Add(new BlockModel(index, attributes, value, text, success));
                }
            }
            return blocks;
        }

        public List<BlockModel> GetsByIdStartWith(string str, Tag e, string id)
        {
            var blocks = new List<BlockModel>();

            var pStart = "<" + e + "[^>]*>";
            var pEnd = "</" + e + ">";

            var pId = "<" + e + "(?:(?!id=).)*id=\"(?:(?!" + id + ").)*" + id + "[^\\s\"]+[^\"]*\"[^>]*>";
            var mIds = new Regex(pId).Matches(str);

            if (mIds.Count > 0)
            {
                foreach (Match mId in mIds)
                {
                    var count = 1;
                    var start = mId.Index + mId.Length;
                    var content = mId.Length;

                    while (count > 0)
                    {
                        var sub = str.Substring(start);
                        var rStart = new Regex(pStart).Match(sub);
                        var rEnd = new Regex(pEnd).Match(sub);

                        if (rStart.Index < rEnd.Index && rStart.Success && rEnd.Success)
                        {
                            start += rStart.Index + rStart.Length;
                            count += 1;
                            content += rStart.Index + rStart.Length;
                        }
                        else
                        {
                            start += rEnd.Index + rEnd.Length;
                            count -= 1;
                            content += rEnd.Index + rEnd.Length;
                        }
                    }
                    var attributes = new Attribut().Gets(mId.Value);
                    var value = str.Substring(mId.Index, content);
                    var index = mId.Index;
                    var length = value.Length;
                    var text = str.Substring(index + mIds[0].Length, length - mIds[0].Length - 3 - e.ToString().Length);
                    var success = true;
                    blocks.Add(new BlockModel(index, attributes, value, text, success));
                }
            }
            return blocks;
        }

        public List<BlockModel> GetsByClass(string str, Tag e, string classe)
        {
            var blocks = new List<BlockModel>();

            var pStart = "<" + e + "[^>]*>";
            var pEnd = "</" + e + ">";

            var pClass = "<" + e + "(?:(?!class=).)*class=\"(?:(?!" + classe + ").)*" + classe + "[^\"]*\"[^>]*>";
            var mClasses = new Regex(pClass).Matches(str);

            if (mClasses.Count > 0)
            {
                foreach (Match mClass in mClasses)
                {
                    var count = 1;
                    var start = mClass.Index + mClass.Length;
                    var content = mClass.Length;

                    while (count > 0)
                    {
                        var sub = str.Substring(start);
                        var rStart = new Regex(pStart).Match(sub);
                        var rEnd = new Regex(pEnd).Match(sub);

                        if (rStart.Index < rEnd.Index && rStart.Success && rEnd.Success)
                        {
                            start += rStart.Index + rStart.Length;
                            count += 1;
                            content += rStart.Index + rStart.Length;
                        }
                        else
                        {
                            start += rEnd.Index + rEnd.Length;
                            count -= 1;
                            content += rEnd.Index + rEnd.Length;
                        }
                    }
                    var attributes = new Attribut().Gets(mClass.Value);
                    var value = str.Substring(mClass.Index, content);
                    var index = mClass.Index;
                    var length = value.Length;
                    var text = str.Substring(index + mClasses[0].Length, length - mClasses[0].Length - 3 - e.ToString().Length);
                    var success = true;
                    blocks.Add(new BlockModel(index, attributes, value, text, success));
                }
            }
            return blocks;
        }
    }
}
