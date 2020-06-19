using ParserLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace YandexParserLib
{
    public class YandexParserSettings : IParserSettings
    {
        public YandexParserSettings(string query) {
            Prefix = Prefix.Replace("{query}",query);
        }
        public string BaseUrl { get; set; } = "https://yandex.ru/images/";
        public string Prefix { get; set; } = "search?text={query}&p={page}";
        public int StartPoint { get; set; } = 1;
        public int EndPoint { get; set; } = 1;
    }
}
