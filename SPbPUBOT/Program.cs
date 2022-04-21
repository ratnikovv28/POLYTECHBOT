using System;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SPbPUBOT
{
    class Program
    {
        private static TelegramBotClient? Bot;
        public static async Task Main()
        {
            Bot = new TelegramBotClient(Configuration.BotToken); //инициализируем бота с помощью токена

            using var cts = new CancellationTokenSource(); //CancellationTokenSource — создает маркеры отмены, которые можно получить из свойства Token и обрабатывает запросы на отмену.

            ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };

            Bot.StartReceiving(Handlers.HandleUpdateAsync,
                Handlers.HandleErrorAsync,
                receiverOptions,
                cts.Token);

            Console.ReadLine();

            cts.Cancel(); // Send cancellation request to stop bot
        }
    }
}
