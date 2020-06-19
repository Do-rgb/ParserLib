namespace ParserLib.Message
{
    public class BadResponseMessage {
        public BadResponseMessage(string url) {
            Url = url;
        }
        public string Url { get; set; }
    }
}
