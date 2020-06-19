namespace ParserLib.Message
{
    public class WebMessage
    {
        public WebMessage(string url)
        {
            Url = url;
        }
        public string Url { get; set; }
    }
}
