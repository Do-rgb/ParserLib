using Akka.Actor;
using ParserLib.Actor;
using ParserLib.Message;
using System;
using YandexParserLib.Message;

namespace YandexParserLib
{

    public class YandexParserActor : ParserActor<string[]>
    {
        public YandexParserActor() : base(new YandexParser()) {
            Receive<ReloadMessage>(ReloadMessageHandler);
        }

        private void ReloadMessageHandler(ReloadMessage message)
        {
            _webActor.Tell(message.Url);
        }
    }
}
