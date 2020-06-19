namespace ParserLib.Message
{
    public class TaskMessage
    {
        public TaskMessage(IParserSettings settings)
        {
            Settings = settings;
        }
        public IParserSettings Settings { get; set; }
    }
}
