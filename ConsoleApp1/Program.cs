using Akka.Actor;
using ParserLib.Message;
using System;
using System.Collections.Generic;
using YandexParserLib;
using YandexParserLib.Message;

namespace ConsoleApp1
{
    partial class Program
    {
        static ActorSystem system = ActorSystem.Create("MySystem");
        static void Main(string[] args)
        {
            MainActor.OnCaptcha += MainActor_OnCaptcha;
            MainActor.OnNewData += MainActor_OnNewData;
            string tempStr = "";

            string query = "";
            int page = 1;

            do
            {
                Console.Write("Введите запрос (пример - зебра, minecraft, дом):");
                tempStr = Console.ReadLine();
            } while (String.IsNullOrWhiteSpace(tempStr));

            do
            {
                Console.Write("Укажите количество страниц для парсинга (пример - 1,2,3):");
                tempStr = Console.ReadLine();
            } while (!int.TryParse(tempStr, out page) || page < 1);

            var greeter = system.ActorOf<MainActor>("MainActor");
            greeter.Tell(new TaskMessage(new YandexParserSettings(query) { EndPoint = page }));

            Console.ReadLine();
        }

        private static void MainActor_OnNewData(string obj)
        {
            Console.WriteLine("URL: {0}", obj);
        }

        private static void MainActor_OnCaptcha(string[] captchaInfo)
        {
            Console.WriteLine("Обнаружена капча. Url: {0}", captchaInfo[1]);
        }
    }
}
