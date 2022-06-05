
using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace TG_BOT
{
    class Program
    {
        private static string Token { get; set; } = "5375941676:AAEcjQlcFnrjxdZ7nlDQ0dYLxAM3-ID45PY";
        private static TelegramBotClient _client;

        static string Namecity;
        static float tempOfCity;
        static string nameOfCity;
       
        static string answerOnWether;

        [Obsolete("Obsolete")]
        public static void Main(string[] args)
        {
            _client = new TelegramBotClient(Token) { Timeout = TimeSpan.FromSeconds(8) };

            var me = _client.GetMeAsync().Result;
            Console.WriteLine($"Bot_Id: {me.Id} \nBot_Name: {me.FirstName} ");

            _client.OnMessage += Bot_OnMessage;
            _client.StartReceiving();
            Console.ReadLine();
            _client.StopReceiving();
        }


        [Obsolete("Obsolete")]
        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            if (message.Type == MessageType.Text)
            {
                Namecity = message.Text;
                Weather(Namecity);
                Celsius(tempOfCity);
                File.Exists(Namecity);
                await _client.SendTextMessageAsync(message.Chat.Id,
                    $"{answerOnWether} \n\nТемпература в {nameOfCity}: {Math.Round(tempOfCity)} °C");
                Console.WriteLine(message.Text);
            }
        }

        public static void Weather(string cityName)
        {
            try
            {
                string url = "https://api.openweathermap.org/data/2.5/weather?q=" + cityName +
                             "&unit=metric&appid=f895b5f761f98aaa3ee01605a5b2061c";
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string response;
                using (StreamReader streamReader =
                       new StreamReader(httpWebResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    response = streamReader.ReadToEnd();
                }

                WeathResp weatherResponse = JsonConvert.DeserializeObject<WeathResp>(response);

                nameOfCity = weatherResponse.Name;
                tempOfCity = weatherResponse.Main.Temp - 273;
            }
            catch (System.Net.WebException)
            {
                Console.WriteLine("Виник виняток");
                return;
            }
        }

        public static void Celsius(float celsius)
        {
            if (celsius <= 10)
                answerOnWether = "Сьогодні холодно, одягайся тепліше";
            else
                answerOnWether = "Сьогодні дуже жарко";
        }
    }
}
