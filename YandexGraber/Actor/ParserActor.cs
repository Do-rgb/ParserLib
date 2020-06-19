using Akka.Actor;
using Akka.Util;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Security.Policy;
using ParserLib.Message;

namespace ParserLib.Actor
{
    public class ParserActor<T> : ReceiveActor where T : class
    {
        protected IActorRef _webActor = Context.ActorOf<WebActor>("WebActor");
        HtmlParser domParser = new HtmlParser();
        IParser<T> _parser;

        public ParserActor(IParser<T> parser)
        {
            _parser = parser;
            Receive<TaskMessage>(TaskMessageHandler);
            Receive<GoodResponseMessage>(GoodResponseMessageHandler);
            Receive<BadResponseMessage>(BadResponseMessageHandler);
        }

        public static Props Props(IParser<T> parser)
        {
            return Akka.Actor.Props.Create(() => new ParserActor<T>(parser));
        }

        private void GoodResponseMessageHandler(GoodResponseMessage message)
        {
            var document = domParser.ParseDocument(message.Response);
            T result = _parser.Parse(document);
            Context.Parent.Tell(new ParserResultMessage<T>(message.Url, result));
        }

        private void BadResponseMessageHandler(BadResponseMessage message)
        {
            Context.Parent.Tell(new ParserResultMessage<T>(message.Url,null));
        }

        private void TaskMessageHandler(TaskMessage message)
        {
            var startPoint = message.Settings.StartPoint;
            var endPoint = message.Settings.EndPoint;

            var urlFormed = message.Settings.BaseUrl + message.Settings.Prefix;

            for (int i = startPoint; i <= endPoint; i++)
            {
                _webActor.Tell(new WebMessage(urlFormed.Replace("{page}", i.ToString())));
            }
        }
    }
}
