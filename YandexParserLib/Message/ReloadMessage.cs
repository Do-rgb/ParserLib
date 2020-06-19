using System;
using System.Collections.Generic;
using System.Text;

namespace YandexParserLib.Message
{
    public class ReloadMessage
    {
        public ReloadMessage(string url) {
            Url = url;
        }
        public string Url { get; set; }
    }
}
