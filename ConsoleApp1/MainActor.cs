using Akka.Actor;
using ParserLib.Message;
using System;
using YandexParserLib;

namespace ConsoleApp1
{
    partial class Program
    {
        class MainActor : ReceiveActor
        {
            public static event Action<string[]> OnCaptcha;
            public static event Action<string> OnNewData;

            IActorRef _yandexActor = Context.ActorOf<YandexParserActor>("YandexActor");
            public MainActor()
            {
                Receive<ParserResultMessage<string[]>>(ParserResultMessageHandler);
                Receive<TaskMessage>(message => _yandexActor.Tell(message));
            }

            private void ParserResultMessageHandler(ParserResultMessage<string[]> message)
            {
                if (message.Result != null)
                {
                    if (message.Result.Length > 1 && message.Result[0].Equals("captcha"))
                    {
                        OnCaptcha?.Invoke(message.Result);
                        return;
                    }
                    foreach (var item in message.Result)
                    {
                        OnNewData?.Invoke(item);
                    }
                }
            }
        }
    }
}
