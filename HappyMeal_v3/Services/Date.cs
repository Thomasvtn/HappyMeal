using Parsing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HappyMeal_v3.Services
{
    public enum DateType
    {
        BeforeToday, Today, AfterToday, Null
    }

    public class Date
    {
        private readonly string text;

        public Date(string text)
        {
            this.text = text;
        }

        public DateType IsToday()
        {
            var now = DateTime.Now;
            var date = ParseDate();
            if (date.Count == 0)
                return DateType.Null;

            var temp = new DateTime(date[2], date[1], date[0]);
            if (now.Date == temp.Date)
                return DateType.Today;
            if (now.Date > temp.Date)
                return DateType.BeforeToday;
            return DateType.AfterToday;
        }

        private List<int> ParseDate()
        {
            var date = new List<int>();
            var parser = new Parser(this.text);
            var regex = new Regex("Menu\\s*du\\s*jour", RegexOptions.IgnoreCase);
            foreach (var paragraph in parser.Paragraphs)
            {
                var match = regex.Match(paragraph.Value);
                if (match.Success)
                {
                    var text = paragraph.Value.Remove(match.Index, match.Length);

                    var nbs = new Regex("\\d+").Matches(text);
                    var words = new Regex("[^\\s\\d]+").Matches(text);
                    if (nbs.Count < 2 || words.Count < 2)
                        break;

                    int year, day;
                    int.TryParse(nbs[nbs.Count - 1].Value, out year);
                    int.TryParse(nbs[nbs.Count - 2].Value, out day);
                    var month = words[words.Count - 1].Value;

                    date.Add(day);
                    date.Add(MonthToInt(month));
                    date.Add(year);
                    break;
                }   
            }
            return date;
        }

        private int MonthToInt(string month)
        {
            var text = month.ToLower().Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            text = new string(chars).Normalize(NormalizationForm.FormC);

            if (text.Equals("janvier"))
                return 1;
            if (text.Equals("fevrier"))
                return 2;
            if (text.Equals("mars"))
                return 3;
            if (text.Equals("avril"))
                return 4;
            if (text.Equals("mai"))
                return 5;
            if (text.Equals("juin"))
                return 6;
            if (text.Equals("juillet"))
                return 7;
            if (text.Equals("aout"))
                return 8;
            if (text.Equals("septembre"))
                return 9;
            if (text.Equals("octobre"))
                return 10;
            if (text.Equals("novembre"))
                return 11;
            if (text.Equals("decembre"))
                return 12;
            return 0;
        }
    }
}