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
        /// <summary>
        /// Ссылка на WebActor, используется для отправки сообщений актору
        /// </summary>
        protected IActorRef _webActor = Context.ActorOf<WebActor>("WebActor");
        /// <summary>
        /// Преобразует текст в DOM объекты
        /// </summary>
        HtmlParser domParser = new HtmlParser();

        /// <summary>
        /// Парсер обрабатывающий загруженную страницу
        /// </summary>
        IParser<T> _parser;

        public ParserActor(IParser<T> parser)
        {
            _parser = parser;
            Receive<TaskMessage>(TaskMessageHandler);
            Receive<GoodResponseMessage>(GoodResponseMessageHandler);
            Receive<BadResponseMessage>(BadResponseMessageHandler);
        }

        /// <summary>
        /// Обрабатывает событие, при удачном запросе
        /// </summary>
        /// <param name="message">Содержит URL и контент загруженной страницы</param>
        private void GoodResponseMessageHandler(GoodResponseMessage message)
        {
            var document = domParser.ParseDocument(message.Response);
            T result = _parser.Parse(document);
            Context.Parent.Tell(new ParserResultMessage<T>(message.Url, result));
        }

        /// <summary>
        /// Обрабатывает событие, при НЕ удачном запросе
        /// </summary>
        /// <param name="message">Содержит URL неудачного запроса</param>
        private void BadResponseMessageHandler(BadResponseMessage message)
        {
            Context.Parent.Tell(new ParserResultMessage<T>(message.Url,null));
        }

        /// <summary>
        /// Обработчик входящего сообщения. Запускает работу парсера.
        /// </summary>
        /// <param name="message">Содержит информацию о страницах для парсинга</param>
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
