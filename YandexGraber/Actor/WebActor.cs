using Akka.Actor;
using System;
using System.IO;
using System.Net;
using ParserLib.Message;

namespace ParserLib.Actor
{
    public class WebActor : ReceiveActor
    {
        private CookieContainer container = new CookieContainer();
        public WebActor()
        {
            Receive<WebMessage>(WebMessageHandler);
        }

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
