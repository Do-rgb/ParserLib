using Akka.Actor;
using System;
using System.IO;
using System.Net;
using ParserLib.Message;

namespace ParserLib.Actor
{
    public class WebActor : ReceiveActor
    {
        /// <summary>
        /// Контейнер содержит cookie, чтобы не вводить капчу при каждом запросе.
        /// </summary>
        private CookieContainer container = new CookieContainer();
        public WebActor()
        {
            //Регистрация обработчиков входящих сообщений
            Receive<WebMessage>(WebMessageHandler);
        }

        /// <summary>
        /// Загружает страницу переданную в сообщении
        /// </summary>
        /// <param name="message">Неизменяемое сообщение, содержащее URL страницы для загрузки</param>
        private void WebMessageHandler(WebMessage message)
        {
            string result = null;

            HttpWebRequest request = WebRequest.CreateHttp(message.Url);
            request.CookieContainer = container;

            try
            {
                using (var response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }
            catch
            {
                Sender.Tell(new BadResponseMessage(message.Url));
            }

            Sender.Tell(new GoodResponseMessage(message.Url, result));
        }
    }
}
