namespace ParserLib.Message
{
    public class GoodResponseMessage
    {
        public GoodResponseMessage(string url, string response)
        {
            Url = url;
            Response = response;
        }
        public string Url { get; set; }
        public string Response { get; set; }
    }
}
