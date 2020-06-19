using AngleSharp.Html.Dom;
using ParserLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace YandexParserLib
{
    public class YandexParser : IParser<string[]>
    {
        private static string pattern = @"\""dups\"":\[[\w\W][^{]+{[\w\W][^l]+l"":""([\w\W][^""]+)";
        private static Regex regex = new Regex(pattern);
        public string[] Parse(IHtmlDocument document)
        {
            List<string> result = new List<string>();
            //Проверка на капчу
            var forms = document.QuerySelectorAll("form").Where(item => item.ClassName != null && item.ClassName.Contains("form_error_no"));

            foreach (var form in forms)
            {
                string key = form.GetElementsByClassName("form__key").First().GetAttribute("value");
                string retpath = form.GetElementsByClassName("form__retpath").First().GetAttribute("value");
                string image = form.GetElementsByClassName("captcha__image")[0].Children[0].GetAttribute("src");

                result.Add("captcha");
                result.Add(image);
                result.Add(key);
                result.Add(retpath);
                return result.ToArray();
            }

            var allScripts = document.QuerySelectorAll("div").Where(item => item.ClassName != null && item.ClassName.Contains("serp-list"));

            foreach (var script in allScripts)
            {
                string input = HttpUtility.HtmlDecode(script.InnerHtml);
                foreach (Match m in regex.Matches(input))
                {
                    result.Add(m.Groups[1].ToString());
                }
            }

            return result.ToArray();
        }
    }
}
