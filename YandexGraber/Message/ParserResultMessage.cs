using System;
using System.Collections.Generic;
using System.Text;

namespace ParserLib.Message
{
    public class ParserResultMessage<T>
    {
        public ParserResultMessage(string url,T result) {
            Url = url;
            Result = result;
        }
        public string Url { get; set; }
        public T Result { get; set; }
    }
}
