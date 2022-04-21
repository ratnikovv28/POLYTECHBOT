using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Linq;
using System.Text;

namespace SPbPUBOT
{
    public class Handlers
    {
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(botClient, update.Message),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(botClient, update.CallbackQuery!),
                _ => UnknownUpdateHandlerAsync(botClient, update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        } //основная функция чтения

        private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message) // сообщения
        {
            bool isOperator, isMainOperator;
            long chatID = message.Chat.Id;

            await botClient.SendChatActionAsync( // анимация "Печатать", пока получается информация из бд
                    chatId: chatID,
                    chatAction: ChatAction.Typing
                    );

            using (ApplicationContext db = new ApplicationContext())
            {
                isMainOperator = db.Operators.Any(k => k.OperatorID == chatID && k.isMain == true);
                isOperator = db.Operators.Any(k => k.OperatorID == chatID);
            }
            if (isMainOperator)
            {
                ReceivedFromMainOperator(botClient, message);
            }
            else if (isOperator)
            {
                ReceivedFromOperator(botClient, message);
            }
            else
            {
                ReceivedFromUser(botClient, message);
            }
        }

        private static async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery) // процесс ответа Callback
        {
            string[] partsQuery = callbackQuery.Data.Split("|");

            switch (partsQuery[0])
            {
                case "абитуриент":
                    {
                        switch (partsQuery[1])
                        {
                            case "абитуриент":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Выберите интересующую категорию",
                                        replyMarkup: Keyboards.User.Enrollee.basicKeyboard
                                        );
                                }
                                break;
                            case "поступление":
                                {

                                }
                                break;
                            case "документы":
                                {

                                }
                                break;
                            case "индивид.достижения":
                                {
                                    await botClient.EditMessageTextAsync(
                                            chatId: callbackQuery.Message.Chat.Id,
                                            messageId: callbackQuery.Message.MessageId,
                                            text: "1.Максимальная сумма баллов, которые вы можете получить за индивидуальные достижения - 10 баллов.\n\n" +
                                            "2.Если вы являетесь победителем/призёром заключительного этапа Всероссийской олимпиады школьников, то вы можете быть зачисленными в университет без вступительных испытаний.\n\n" +
                                            "3.Если вы являетесь победителем/призёром олимпиады РСОШ с ЕГЭ не менее 75 баллов, то вы можете быть зачисленными в университет без вступительных испытаний.\n\n" +
                                            "4.Для того чтобы получить дополнительные баллы за золотой значок ГТО, необходимо предоставить удостоверение установленного образца, подписанное министром спорта РФ. Сам значок и какие-либо другие документы в приёмную комиссию предоставлять не нужно.\n\n" +
                                            "5.Более подробнее со списком индивидуальных достижений можно ознакомиться ниже",
                                            replyMarkup: Keyboards.User.Enrollee.achievementsKeyboard,
                                            parseMode: ParseMode.Html
                                            );
                                }
                                break;
                            case "вступ.исп":
                                {
                                    switch (partsQuery[2])
                                    {
                                        case "вступ.исп":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Здесь есть все, что вы хотели знать о вступительных испытаниях:",
                                                    replyMarkup: Keyboards.User.Enrollee.entranceTestsKeyboard
                                                    );
                                            }
                                            break;
                                        case "лица":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Нужно ли и можно ли тебе сдавать вступительные испытания Политеха?\nПроверь на сайте, относишься ли ты <a href='https://www.spbstu.ru/abit/bachelor/entrance-test/allowed-pass-entrance-test/'>к этим категориям</a>.",
                                                    replyMarkup: Keyboards.backKeyboard,
                                                    parseMode: ParseMode.Html
                                                    );
                                            }
                                            break;
                                        case "перечень":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Перечень вступительных испытаний есть <a href='https://www.spbstu.ru/abit/bachelor/entrance-test/the-list-of-entrance-examinations/'>здесь</a>. Обрати, пожалуйста, внимание, что лица, имеющие профессиональное образование (среднее профессиональное или высшее), сдают отдельный перечень вступительных испытаний (см. вступительные испытания для лиц, имеющих профессиональное образование).",
                                                    replyMarkup: Keyboards.backKeyboard,
                                                    parseMode: ParseMode.Html
                                                    );
                                            }
                                            break;
                                        case "расписание":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Расписание вступительных испытаний будет по <a href='https://www.spbstu.ru/abit/bachelor/entrance-test/the-schedule-of-entrance-examinations/'>ссылке</a>.",
                                                    replyMarkup: Keyboards.backKeyboard,
                                                    parseMode: ParseMode.Html
                                                    );
                                            }
                                            break;
                                        case "программы":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "С программами вступительных испытаний можно ознакомиться <a href='https://www.spbstu.ru/abit/bachelor/entrance-test/program-of-entrance-examinations/'>здесь</a>. А образцы заданий доступны <a href='https://www.spbstu.ru/abit/bachelor/entrance-test/obraztsy-zadaniy-vstupitelnykh-ispytaniy/'>тут</a>.",
                                                    replyMarkup: Keyboards.backKeyboard,
                                                    parseMode: ParseMode.Html
                                                    );
                                            }
                                            break;
                                        case "дизайн":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "На Дизайн и Дизайн архитектурной среды сдавать творческие вступительные испытания необходимо всем поступающим. А также необходимо учитывать, что окончание приема документов наступает раньше — 7 июля. Подробная информация есть <a href='https://www.spbstu.ru/abit/bachelor/entrance-test/vstupitelnye-ispytaniya-po-napravleniyu-podgotovki-dizayn/'>на странице</a>",
                                                    replyMarkup: Keyboards.backKeyboard,
                                                    parseMode: ParseMode.Html
                                                    );
                                            }
                                            break;
                                        case "2022":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "В 2022 году вступительные испытания будут проходить дистанционно на портале электронного обучения СПбПУ (<a href='https://lms.spbstu.ru'>LMS Moodle</a>)",
                                                    replyMarkup: Keyboards.backKeyboard,
                                                    parseMode: ParseMode.Html
                                                    );
                                            }
                                            break;
                                        case "назад":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Выберите интересующую категорию",
                                                    replyMarkup: Keyboards.User.Enrollee.basicKeyboard
                                                    );
                                            }
                                            break;

                                    }
                                }
                                break;
                            case "после поступления":
                                {

                                }
                                break;
                            case "стип/гранты":
                                {

                                }
                                break;
                            case "перевод в политех":
                                {
                                    await botClient.EditMessageTextAsync(
                                            chatId: callbackQuery.Message.Chat.Id,
                                            messageId: callbackQuery.Message.MessageId,
                                            text: "С правилами перевода в Политехнический университет из других вузов можно ознакомиться <a href='https://www.spbstu.ru/students/transferring-students-from-other-universities-to-spbpu/'>здесь</a>",
                                            replyMarkup: Keyboards.backKeyboard,
                                            parseMode: ParseMode.Html
                                            );
                                }
                                break;
                            case "общежития":
                                {
                                    switch (partsQuery[2])
                                    {
                                        case "общежития":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Расскажем все про общежития:",
                                                    replyMarkup: Keyboards.User.Enrollee.hostelKeyboard
                                                    );
                                            }
                                            break;
                                        case "репортаж":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Специально для наших абитуриентов мы съездили в один из корпусов общежитий СПбПУ и показали, как живут студенты, а также побеседовали с директором Студенческого городка. Включайте — и вы сразу получите ответы на 99% ваших вопросов об общежитии!\n vk.cc/c9r7IY",
                                                    replyMarkup: Keyboards.backKeyboard
                                                    );
                                            }
                                            break;
                                        case "получу ли я?":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "В первую очередь заселяют студентов очного отделения, обучающихся на бюджете. \nС порядком распределения мест можно ознакомиться <a href='https://www.spbstu.ru/abit/events/poryadok-ocherednosti-predostavleniya-mest-v-obshchezhitiyakh-studgorodka-spbpu/'>здесь</a>",
                                                    replyMarkup: Keyboards.backKeyboard,
                                                    parseMode: ParseMode.Html
                                                    );
                                            }
                                            break;
                                        case "карта":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "карта":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Выберите нужное общежитие или откройте <a href='https://www.spbstu.ru/campus-map/'>карту кампуса</a>",
                                                                replyMarkup: Keyboards.User.Enrollee.hostelMapKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "назад":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                               chatId: callbackQuery.Message.Chat.Id,
                                                               messageId: callbackQuery.Message.MessageId,
                                                               text: "Расскажем все про общежития:",
                                                               replyMarkup: Keyboards.User.Enrollee.hostelKeyboard
                                                               );
                                                        }
                                                        break;
                                                    default:
                                                        {
                                                            await botClient.SendLocationAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                latitude: Lists.hostelList[partsQuery[3]].Latitude,
                                                                longitude: Lists.hostelList[partsQuery[3]].Longitude
                                                                );
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case "назад":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Выберите интересующую категорию",
                                                    replyMarkup: Keyboards.User.Enrollee.basicKeyboard
                                                    );
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "чат с представителем поддержки":
                                {

                                }
                                break;
                        }
                    }
                    break;
                case "студент":
                    {

                    }
                    break;
                case "назад":
                    {
                        string substr = callbackQuery.Message.Text.Substring(0, 21);

                        switch (substr)
                        {
                            case "Выберите интересующую":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Привет! Это твой чат-бот для связи с Политехническим университетом. \nВыбери ону из категорий, чтобы узнать больше.",
                                        replyMarkup: Keyboards.User.chooseKeyboard
                                        );
                                }
                                break;
                            case "Специально для наших ":
                            case "В первую очередь засе":
                                {
                                    await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Расскажем все про общежития:",
                                                    replyMarkup: Keyboards.User.Enrollee.hostelKeyboard
                                                    );
                                }
                                break;
                            case "С правилами перевода ":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Выберите интересующую категорию",
                                        replyMarkup: Keyboards.User.Enrollee.basicKeyboard
                                        );
                                }
                                break;
                            case "1.Максимальная сумма ":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Выберите интересующую категорию",
                                        replyMarkup: Keyboards.User.Enrollee.basicKeyboard
                                        );
                                }
                                break;
                            case "Здесь есть все, что в":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Выберите интересующую категорию",
                                        replyMarkup: Keyboards.User.Enrollee.basicKeyboard
                                        );
                                }
                                break;
                            case "Нужно ли и можно ли т":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Здесь есть все, что вы хотели знать о вступительных испытаниях:",
                                        replyMarkup: Keyboards.User.Enrollee.entranceTestsKeyboard,
                                        parseMode: ParseMode.Html
                                        );
                                }
                                break;
                            case "Перечень вступительны":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Здесь есть все, что вы хотели знать о вступительных испытаниях:",
                                        replyMarkup: Keyboards.User.Enrollee.entranceTestsKeyboard,
                                        parseMode: ParseMode.Html
                                        );
                                }
                                break;
                            case "Расписание вступитель":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Здесь есть все, что вы хотели знать о вступительных испытаниях:",
                                        replyMarkup: Keyboards.User.Enrollee.entranceTestsKeyboard,
                                        parseMode: ParseMode.Html
                                        );
                                }
                                break;
                            case "С программами вступит":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Здесь есть все, что вы хотели знать о вступительных испытаниях:",
                                        replyMarkup: Keyboards.User.Enrollee.entranceTestsKeyboard,
                                        parseMode: ParseMode.Html
                                        );
                                }
                                break;
                            case "На Дизайн и Дизайн ар":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Здесь есть все, что вы хотели знать о вступительных испытаниях:",
                                        replyMarkup: Keyboards.User.Enrollee.entranceTestsKeyboard,
                                        parseMode: ParseMode.Html
                                        );
                                }
                                break;
                            case "В 2022 году вступител":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Здесь есть все, что вы хотели знать о вступительных испытаниях:",
                                        replyMarkup: Keyboards.User.Enrollee.entranceTestsKeyboard,
                                        parseMode: ParseMode.Html
                                        );
                                }
                                break;
                            default:
                                {
                                    Console.WriteLine("Я тут");
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        private static async Task ReceivedFromMainOperator(ITelegramBotClient botClient, Message message) // сообщения от главного оператора
        {
            long chatID = message.Chat.Id;

            switch (message.Text.ToLower())
            {
                case "/start":
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: chatID,
                            text: "Привет, главный оператор! \nСписок доступных команд только что открылся, будь острожнее и не добавляй кого попало",
                            replyMarkup: Keyboards.MainOperator.basicKeyboard
                            );
                    }
                    break;

                case "добавить нового оператора":
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: chatID,
                            text: "Введите имя пользователя в формате: @имя"
                            );
                    }
                    break;

                case "список операторов":
                    {

                    }
                    break;

                default:
                    {
                        if (message.Text[0] == '@')
                        {
                        }
                    }
                    break;
            }
        }

        private static async Task ReceivedFromOperator(ITelegramBotClient botClient, Message message) // сообщения от оператора
        {
            long chatID = message.Chat.Id;
        }

        private static async Task ReceivedFromUser(ITelegramBotClient botClient, Message message) // сообщения от пользователя
        {
            long chatID = message.Chat.Id;
            User user;

            await botClient.SendChatActionAsync( // анимация "Печатать", пока получается информация из бд
                    chatId: chatID,
                    chatAction: ChatAction.Typing
                    );

            using (ApplicationContext db = new ApplicationContext())
            {
                if (!db.Users.Any(k => k.UserID == chatID))
                {
                    user = new User() // добавление
                    {
                        UserID = chatID,
                        FirstName = message.From.FirstName,
                        SecondName = message.From.LastName,
                        UserName = message.From.Username,
                        operatorID = null
                    };
                    db.Users.Add(user);
                    db.SaveChanges(); // сохранение изменений
                }
                else
                {
                    user = db.Users.Find(chatID);
                }
            }

            if (user.operatorID != null)
            {
                await botClient.SendTextMessageAsync(
                    chatId: user.operatorID,
                    text: message.Text
                    );
            }
            else
            {
                switch (message.Text.ToLower())
                {
                    case "/start":
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: chatID,
                                text: "Привет! Это твой чат-бот для связи с Политехническим университетом. \nВыбери ону из категорий, чтобы узнать больше.",
                                replyMarkup: Keyboards.User.chooseKeyboard
                                );
                        }
                        break;
                }
            }
        }

        private static Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            Console.WriteLine($"{update.Type}");
            return Task.CompletedTask;
        }
    }
}
