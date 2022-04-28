using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

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

            using (ApplicationContext db = new ApplicationContext())
            {
                isMainOperator = db.Operators.Any(k => k.OperatorID == chatID && k.isMain == true);
                isOperator = db.Operators.Any(k => k.OperatorID == chatID);
            }

            //await botClient.SendChatActionAsync( // анимация "Печатать", пока получается информация из бд
            //        chatId: chatID,
            //        chatAction: ChatAction.Typing
            //        );

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

            Console.WriteLine(callbackQuery.Data);

            switch (partsQuery[0]) //выбор между абитуриентом и студентом
            {
                case "абитур":
                    {
                        switch (partsQuery[1]) //выбор категории интересующей темы
                        {
                            case "абитур":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Выбери одну из категорий, чтобы узнать больше",
                                        replyMarkup: Keyboards.User.Enrollee.startKeyboard
                                        );
                                }
                                break;
                            case "поступ":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Чтобы изучить вопрос более детально выбери категорию, к которой ты относишься",
                                        replyMarkup: Keyboards.User.Enrollee.Admission.basicKeyboard
                                        );
                                }
                                break;
                            case "выбратьпроф":
                                {
                                    switch (partsQuery[2])
                                    {
                                        case "выбратьпроф":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Выбери, какой раздел тебе наиболее подходит.",
                                                    replyMarkup: Keyboards.User.Enrollee.ChooseProfession.chooseKeyboard
                                                    );
                                            }
                                            break;
                                        case "бака":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "бака":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Выберите удобное для вас представление",
                                                                replyMarkup: Keyboards.User.Enrollee.ChooseProfession.undergraduateKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "буклет":
                                                        {
                                                            //сделать
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case "мага":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "мага":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Выберите удобное для вас представление",
                                                                replyMarkup: Keyboards.User.Enrollee.ChooseProfession.magistracyKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "буклет":
                                                        {
                                                            //сделать
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
                                                    text: "Выбери, какой раздел тебе наиболее подходит.",
                                                    replyMarkup: Keyboards.User.Enrollee.ChooseProfession.chooseKeyboard
                                                    );
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "знаком":
                                {
                                    switch (partsQuery[2])
                                    {
                                        case "знаком":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Давай знакомиться",
                                                    replyMarkup: Keyboards.User.Enrollee.AboutUniversity.aboutUniversityKeyboard
                                                    );
                                            }
                                            break;
                                        case "ролик":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Полет <a href='https://www.youtube.com/watch?v=6D_iskPXBno'>над кампусом #ПолитехПетра</a>",
                                                    replyMarkup: Keyboards.backKeyboard,
                                                    parseMode: ParseMode.Html
                                                    );
                                            }
                                            break;
                                        case "дод":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Ежемесячно в Политехе проводятся дни открытых дверей разных институтом. С расписанием можно ознакомиться, нажав на кнопку",
                                                    replyMarkup: Keyboards.User.Enrollee.AboutUniversity.scheduleOpenDoorsKeyboard
                                                    );
                                            }
                                            break;
                                        case "экскур":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "экскур":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Что бы вы хотели посетить?",
                                                                replyMarkup: Keyboards.User.Enrollee.AboutUniversity.toursKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "лабор":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Что бы вы хотели посетить?",
                                                                replyMarkup: Keyboards.User.Enrollee.AboutUniversity.scheduleOpenDoorsKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "кпк":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Что бы вы хотели посетить?",
                                                                replyMarkup: Keyboards.User.Enrollee.AboutUniversity.scheduleOpenDoorsKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case "аудиогид":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "На площадке <a href='https://izi.travel/ru'>izi.Travel</a> доступен уникальный аудиогид по кампусу университета, который вы можете прослушать, оказавшись на нашей территории.",
                                                    replyMarkup: Keyboards.User.Enrollee.AboutUniversity.audioGuideKeyboard,
                                                    parseMode: ParseMode.Html
                                                    );
                                            }
                                            break;
                                        case "медиа":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Соцсети и сайты для абитуриентов Политехнического",
                                                    replyMarkup: Keyboards.User.Enrollee.AboutUniversity.mediaKeyboard
                                                    );
                                            }
                                            break;
                                        case "почему?":
                                            {

                                            }
                                            break;
                                    }
                                }
                                break;
                            case "мерикурсы":
                                {
                                    switch (partsQuery[2])
                                    {
                                        case "мерикурсы":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Какой тип мероприятия или курса тебя интересует?",
                                                    replyMarkup: Keyboards.User.Enrollee.EventsCourse.basicKeyboard
                                                    );
                                            }
                                            break;
                                        case "пк":
                                            {
                                                //сделать
                                            }
                                            break;
                                        case "ом":
                                            {
                                                //сделать
                                            }
                                            break;
                                        case "олимп":
                                            {
                                                //сделать
                                            }
                                            break;
                                        case "соб":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "В этом блоке вы сможете узнать подробную информацию о ближайших мероприятиях университета и партнеров. Описание всех мероприятий партнеров у нас на сайте.",
                                                    replyMarkup: Keyboards.User.Enrollee.EventsCourse.closeEventsKeyboard
                                                    );
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "чзв": //выбор темы вопроса
                                {
                                    switch (partsQuery[2])
                                    {
                                        case "чзв":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Выберите интересующую категорию",
                                                    replyMarkup: Keyboards.User.Enrollee.Questions.basicQuestionsKeyboard
                                                    );
                                            }
                                            break;
                                        case "поступ":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "поступ":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "1.Вся информация о поступлении представлена на сайте в <a href='https://www.spbstu.ru/abit/bachelor/'>разделе</a>.\n" +
                                                                "2.Рекомендуем заранее ознакомиться с основными датами календаря абитуриента.\n" +
                                                                "3.К сожалению, мы не можем гарантировать достоверность предоставленной информации сторонними ресурсами(сайтами образовательных организаций и порталов), " +
                                                                "поэтому настоятельно рекомендуем вам проверять информацию о поступлении на официальном сайте университета или по телефонам Центра профориентации и довузовской подготовки:\n" +
                                                                "8(812) 775 - 05 - 30 - для звонков из Санкт - Петербурга\n" +
                                                                "8(800) 707 - 18 - 99 - для звонков из любого региона РФ(звонок бесплатный)\n" +
                                                                "<a href='http://mailto:abitur@spbstu.ru/'>abitur@spbstu.ru</a>\n" +
                                                                "195251, ул.Гидротехников, 5\n" +
                                                                "4.Если Вы являетесь гражданином другого государства, то для понимания порядка зачисления вам необходимо определить свой статус в соответствующем разделе.",
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.admissionKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "кален":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "кален":
                                                                    {
                                                                        await botClient.EditMessageReplyMarkupAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            replyMarkup: Keyboards.User.Enrollee.Questions.calendar1Keyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageReplyMarkupAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            replyMarkup: Keyboards.User.Enrollee.Questions.admissionKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "иностранцы":
                                                        {
                                                            await botClient.EditMessageReplyMarkupAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.admissionKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "назад":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Выберите интересующую категорию",
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.basicQuestionsKeyboard
                                                                );
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case "доки":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "доки":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "1.Документы, необходимые для поступления можно подать в электронной форме через личный кабинет абитуриента. Сформированное в личном кабинете заявление необходимо распечатать, подписать, отсканировать и в формате .pdf загрузить в <a href='https://enroll.spbstu.ru/'>Личный кабинет</a> абитуриента в разделе “Загрузить документы”.\n" +
                                                                "2.Для подтверждения ваших баллов ЕГЭ, вам не требуется предоставлять никаких документов.Если вы не знаете или не помните результатов ЕГЭ, вы можете подавать документы, так как все данные подгружаются автоматически из федеральной базы ЕГЭ.\n" +
                                                                "3.Если ваши баллы за предмет меньше установленного университетом минимума, то вы не можете участвовать в конкурсе на поступление.С минимальными(пороговыми) баллами вы можете ознакомиться ниже.\n" +
                                                                "4.Копии документов для подачи в приёмную комиссию заверять не нужно.\n" +
                                                                "5.Даже если вы подаете заявление на несколько направлений в университет, вы всё равно предоставляете только один комплект документов.\n" +
                                                                "6.В рамках университета вы можете подать только на четыре направления.Однако в рамках каждого направления вы можете выбирать разные формы обучения(очная, очно - заочная, заочная) и способ финансирования обучения(бюджет / контракт).\n" +
                                                                "7.Если вы поступаете на бюджетную форму обучения, то заявление о согласии на зачисление можно переписать только ЧЕТЫРЕ РАЗА.Напоминаем, что без заявления на согласие абитуриент не может быть зачислен.Со сроками подачи документов можно ознакомиться ниже.\n" +
                                                                "8.Для подтверждения льготы вам необходимо предоставить в приёмную комиссию подтверждающие документы.Для уточнения, какие именно документы необходимы, необходимо обращаться в Центр профориентации и довузовской подготовки: 8(812) 775 - 05 - 30, 8(800) 707 - 18 - 99.\n" +
                                                                "9.Если ваш документ об образовании выдан в другом государстве, вам необходимо пройти процедуру экспертизы, которая подтверждает уровень вашего образования.\n" +
                                                                "10.Обратите внимание, что стоимость обучения может варьироваться в зависимости от направления и формы обучения, а также в неё могут вноситься изменения.Единственно верный документ о стоимости образовательных программ располагается на сайте «Поступление 2021» в разделе <a href='https://www.spbstu.ru/abit/bachelor/apply/stoimost-obucheniya/'>«Стоимость обучения»</a>.\n" +
                                                                "11.Во время подачи документов в приёмную комиссию вам не нужно предоставлять медицинские документы, а также дополнительные медицинские заключения на ряд направлений подготовки.Данные документы будут вам необходимы после зачисления для прохождения медицинского осмотра.",
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.documentsKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "кален":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "кален":
                                                                    {
                                                                        await botClient.EditMessageReplyMarkupAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            replyMarkup: Keyboards.User.Enrollee.Questions.calendar2Keyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageReplyMarkupAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            replyMarkup: Keyboards.User.Enrollee.Questions.documentsKeyboard
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
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.basicQuestionsKeyboard
                                                                );
                                                        }
                                                        break;
                                                }
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
                                                        replyMarkup: Keyboards.User.Enrollee.Questions.achievementsKeyboard,
                                                        parseMode: ParseMode.Html
                                                        );
                                            }
                                            break;
                                        case "вступ.исп":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "вступ.исп":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Здесь есть все, что вы хотели знать о вступительных испытаниях:",
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.entranceTestsKeyboard
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
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.basicQuestionsKeyboard
                                                                );
                                                        }
                                                        break;

                                                }
                                            }
                                            break;
                                        case "после поступления":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                        chatId: callbackQuery.Message.Chat.Id,
                                                        messageId: callbackQuery.Message.MessageId,
                                                        text: "1.В военном учебном центре СПбПУ осуществляется военная подготовка студентов. Узнать о ней подробнее можно на <a href='https://fvo.spbstu.ru/'>сайте</a>.\n" +
                                                        "2.Приёмная комиссия занимается только вопросами поступления на первый курс.Вопросами перевода занимаются дирекции институтов.\n" +
                                                        "3.Согласно порядку приёма, не предусмотрена сокращённая форма обучения, однако если вы уже имеете среднее профессиональное образование или высшее образование, то некоторые дисциплины могут быть перезачтены в индивидуальном порядке при совпадении учебных планов.\n" +
                                                        "4.С информацией об общежитиях университета можно ознакомиться на сайте <a href='https://www.spbstu.ru/students/social-security/hostel/'>Студенческого городка СПбПУ</a>",
                                                        replyMarkup: Keyboards.backKeyboard,
                                                        parseMode: ParseMode.Html
                                                        );
                                            }
                                            break;
                                        case "стип/гранты":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                        chatId: callbackQuery.Message.Chat.Id,
                                                        messageId: callbackQuery.Message.MessageId,
                                                        text: "Виды Стипендий:\n\n" +
                                                        "1.Государственная академическая стипендия\n" +
                                                        "-выплачивается всем студентам-бюджетникам в первом семестре обучения\n" +
                                                        "-для студентов, окончивших сессию на “хорошо” и “отлично” стипендия составляет 2200 рублей, для окончивших только на отлично – 4400 рублей\n" +
                                                        "-размер повышенной стипендии(ПГАС) определяется на основе конкурса и может составлять от 5 до 20 тысяч рублей\n" +
                                                        "2.Государственная социальная стипендия\n" +
                                                        "-назначается определенной группе лиц\n" +
                                                        "-размер стипендии – 3300(может быть также повышенная)\n" +
                                                        "-необходимо написать заявление и предоставить справку от местного органа соц.защиты населения\n" +
                                                        "-студент, получающий эту стипендию, также может претендовать на общих условиях на академическую стипендию\n" +
                                                        "3.Специальные и именные стипендии\n" +
                                                        "-назначаются студентам, достигшим выдающихся успехов в учебной и научной деятельности\n" +
                                                        "-требования к именным стипендиям регламентируются отдельными положениями\n" +
                                                        "-размер выплат также регламентируется учредителями стипендий, либо в соответствии с их положениями\n\n" +
                                                        "Грант студентам 1 курса\n" +
                                                        "Назначается:\n" +
                                                        "-студентам, поступившим без вступительных испытаний\n" +
                                                        "-абитуриентам, набравшим по результатам ЕГЭ по трем предметам 290 и более баллов\n" +
                                                        "В 2021 - 2022 учебном году размер стипендии составил 12000 рублей.\n" +
                                                        "Назначается в первом семестре, а при отличной учебе продлевается на второй\n\n" +
                                                        "Специальная стипендия студентам 1 курса\n" +
                                                        "Назначается:\n" +
                                                        "-абитуриентам, набравшим по результатам ЕГЭ по трем предметам сумму баллов от 270 до 289\n" +
                                                        "В 2021 - 2022 учебном году размер стипендии составил 6000 рублей.Также назначается в первом семестре и при отличной учебе продлевается на второй.",
                                                        replyMarkup: Keyboards.User.Enrollee.Questions.grantKeyboard
                                                        );
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
                                                switch (partsQuery[3])
                                                {
                                                    case "общежития":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Расскажем все про общежития:",
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.hostelKeyboard
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
                                                            switch (partsQuery[4])
                                                            {
                                                                case "карта":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Выберите нужное общежитие или откройте <a href='https://www.spbstu.ru/campus-map/'>карту кампуса</a>",
                                                                            replyMarkup: Keyboards.User.Enrollee.Questions.hostelMapKeyboard,
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
                                                                            replyMarkup: Keyboards.User.Enrollee.Questions.hostelKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                default:
                                                                    {
                                                                        await botClient.SendLocationAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            latitude: Lists.hostelList[partsQuery[4]].Latitude,
                                                                            longitude: Lists.hostelList[partsQuery[4]].Longitude
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
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.basicQuestionsKeyboard
                                                                );
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case "оператор":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "оператор":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Для того, чтобы начать диалог с представителем Политеха, нажмите кнопку ниже.\n\n" +
                                                                "А ещё вы можете связаться с Приемной комиссией Политеха по телефону\n" +
                                                                "8(812) 775 - 05 - 30 — для звонков из Санкт - Петербурга\n" +
                                                                "8(800) 707 - 18 - 99 — для звонков из любого региона РФ(звонок бесплатный)\n\n" +
                                                                "Или написать на почту abitur@spbstu.ru.",
                                                                replyMarkup: Keyboards.User.callOperatorKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "вызов":
                                                        {
                                                            bool isWithOperator;
                                                            using (ApplicationContext db = new ApplicationContext())
                                                            {
                                                                isWithOperator = db.UserAssistance.Any(k => k.UserID == callbackQuery.From.Id && k.operatorID == null);
                                                                if (isWithOperator)
                                                                {
                                                                    var users = db.Users.Where(k => k.UserID == callbackQuery.From.Id);
                                                                    foreach (var user in users)
                                                                    {
                                                                        user.operatorID = -1;
                                                                    }
                                                                    db.SaveChanges();
                                                                    await botClient.SendTextMessageAsync(
                                                                        chatId: callbackQuery.From.Id,
                                                                        text: "Вы добавлены в очередь ожиданий, к вам подключится первый освободившийся оператора"
                                                                        );
                                                                }
                                                                else
                                                                {
                                                                    await botClient.SendTextMessageAsync(
                                                                        chatId: callbackQuery.From.Id,
                                                                        text: "Вы уже добавлены в очередь ожиданий, к вам подключится первый освободившийся оператора"
                                                                        );
                                                                }
                                                            }
                                                        }
                                                        break;
                                                }
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
                                        text: "Выбери ону из категорий, чтобы узнать больше",
                                        replyMarkup: Keyboards.User.chooseKeyboard
                                        );
                                }
                                break;
                        }
                    }
                    break;
                case "студент":
                    {
                        //сделать 
                    }
                    break;
                case "глоператор":
                    {
                        switch (partsQuery[1])
                        {
                            case "удалить":
                                {
                                    long operID = long.Parse(callbackQuery.Message.Text.Split("Оператор")[0].Split(" ")[1]);
                                    using (ApplicationContext db = new ApplicationContext())
                                    {
                                        Operator oper = db.Operators.Find(operID);
                                        db.Operators.Remove(oper);
                                        await botClient.SendTextMessageAsync(
                                            chatId: operID,
                                            text: "Вы больше не являетесь оператором"
                                            );
                                        db.SaveChanges();
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case "назад":
                    {
                        string substr = callbackQuery.Message.Text.Substring(0, 17);

                        Console.WriteLine(substr);

                        switch (substr)
                        {
                            case "Выберите интересу":
                            case "Давай знакомиться":
                            case "Выбери, какой раз":
                            case "Чтобы изучить воп":
                            case "Какой тип меропри":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Выбери одну из категорий, чтобы узнать больше.",
                                        replyMarkup: Keyboards.User.Enrollee.startKeyboard
                                        );
                                }
                                break;
                            case "Выбери одну из ка":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Выбери одну из категорий, чтобы узнать больше.",
                                        replyMarkup: Keyboards.User.chooseKeyboard
                                        );
                                }
                                break;
                            case "В этом блоке вы с":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Какой тип мероприятия или курса тебя интересует?",
                                        replyMarkup: Keyboards.User.Enrollee.EventsCourse.basicKeyboard
                                        );
                                }
                                break;
                            case "Полет над кампусо":
                            case "Ежемесячно в Поли":
                            case "Что бы вы хотели ":
                            case "На площадке izi.T":
                            case "Соцсети и сайты д":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Давай знакомиться",
                                        replyMarkup: Keyboards.User.Enrollee.AboutUniversity.aboutUniversityKeyboard
                                        );
                                }
                                break;
                            case "С правилами перев":
                            case "1.Максимальная сумма ":
                            case "Здесь есть все, ч":
                            case "Виды Стипендий:\n\n":
                            case "1.В военном учебн":
                            case "Для того, чтобы н":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Выберите интересующую категорию",
                                        replyMarkup: Keyboards.User.Enrollee.Questions.basicQuestionsKeyboard
                                        );
                                }
                                break;
                            case "Специально для на":
                            case "В первую очередь ":
                                {
                                    await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Расскажем все про общежития:",
                                                    replyMarkup: Keyboards.User.Enrollee.Questions.hostelKeyboard
                                                    );
                                }
                                break;
                            case "Нужно ли и можно ":
                            case "Перечень вступите":
                            case "Расписание вступи":
                            case "С программами вст":
                            case "На Дизайн и Дизай":
                            case "В 2022 году вступ":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: callbackQuery.Message.Chat.Id,
                                        messageId: callbackQuery.Message.MessageId,
                                        text: "Здесь есть все, что вы хотели знать о вступительных испытаниях:",
                                        replyMarkup: Keyboards.User.Enrollee.Questions.entranceTestsKeyboard,
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
                            text: "Привет, главный оператор! \nСписок доступных команд только что открылся, будь острожнее и не добавляй кого попало)))",
                            replyMarkup: Keyboards.MainOperator.basicKeyboard
                            );
                    }
                    break;

                case "добавить нового оператора":
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: chatID,
                            text: "Введите id пользователя в формате: 'id:'"
                            );
                    }
                    break;

                case "список операторов":
                    {
                        using (ApplicationContext db = new ApplicationContext())
                        {
                            var operatorsList = db.Operators.Where(m => m.isMain == false);
                            if (operatorsList.Count() == 0)
                            {
                                await botClient.SendTextMessageAsync(
                                    chatId: chatID,
                                    text: "Нет ни одного неосновного оператора"
                                    );
                            }
                            else
                            {
                                foreach (var oper in operatorsList)
                                {
                                    await botClient.SendTextMessageAsync(
                                        chatId: chatID,
                                        text: "ID " + oper.OperatorID + "\n" +
                                        "Оператор " + oper.Username,
                                        replyMarkup: Keyboards.MainOperator.deleteKeyboard
                                        );
                                }
                            }
                        }
                    }
                    break;

                default:
                    {
                        if (message.Text.Substring(0, 3) == "id:")
                        {
                            Operator oper;
                            long userID;
                            bool flag = long.TryParse(message.Text.Substring(3), out userID);
                            if (!flag)
                            {
                                await botClient.SendTextMessageAsync(
                                    chatId: chatID,
                                    text: "Неправильный формат ID пользователя"
                                    );
                                break;
                            }
                            using (ApplicationContext db = new ApplicationContext())
                            {
                                if (db.Operators.FirstOrDefault(k => k.OperatorID == userID) == null)
                                {
                                    if (db.Users.FirstOrDefault(k => k.UserID == userID) == null)
                                    {
                                        db.Operators.Add(new Operator()
                                        {
                                            OperatorID = userID,
                                            isMain = false
                                        });
                                    }
                                    else
                                    {
                                        User user = db.Users.Find(userID);
                                        db.Users.Remove(user);
                                        db.Operators.Add(new Operator()
                                        {
                                            OperatorID = userID,
                                            isMain = false,
                                            Username = user.Username
                                        });
                                        await botClient.SendTextMessageAsync(
                                            chatId: userID,
                                            text: "Поздравляем, теперь Вы являетесь оператором!\n\n" +
                                            "Пожалуйста, очистите историю сообщений в данном канале для корректной работы бота"
                                            );
                                    }
                                    await botClient.SendTextMessageAsync(
                                        chatId: chatID,
                                        text: "Оператор добавлен"
                                        );
                                    db.SaveChanges();
                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(
                                        chatId: chatID,
                                        text: "Данный пользователь уже является оператором"
                                        );
                                }
                            }
                        }
                        else
                        {
                            await botClient.DeleteMessageAsync(
                                chatId: chatID,
                                messageId: message.MessageId
                                );
                        }
                    }
                    break;
            }
        }

        private static async Task ReceivedFromOperator(ITelegramBotClient botClient, Message message) // сообщения от оператора
        {
            long chatsOperatorID = message.Chat.Id;
            Operator oper;

            using (ApplicationContext db = new ApplicationContext())
            {
                oper = db.Operators.Find(chatsOperatorID);
            }
            if (oper.userID == null)
            {
                switch (message.Text.ToLower())
                {
                    case "/start":
                        {
                            using (ApplicationContext db = new ApplicationContext())
                            {
                                if (db.Operators.Find(chatsOperatorID).Username == null)
                                {
                                    db.Operators.Find(chatsOperatorID).Username = message.From.Username;
                                }
                            }
                            await botClient.DeleteMessageAsync(
                                chatId: chatsOperatorID,
                                messageId: message.MessageId
                                );

                            await botClient.SendTextMessageAsync(
                                chatId: chatsOperatorID,
                                text: "Привет, оператор! \nСписок доступных команд только что открылся, будь вежлив!",
                                replyMarkup: Keyboards.Operator.mainKeyboard
                                );
                        }
                        break;
                    case "помочь":
                        {
                            User userForHelp;
                            using (ApplicationContext db = new ApplicationContext())
                            {
                                userForHelp = db.Users.FirstOrDefault(k => k.operatorID == -1);
                                if (userForHelp != null)
                                {
                                    userForHelp.operatorID = chatsOperatorID; // меняем у пользователя operatorID на того, кто сейчас готов помочь
                                    db.Operators.Find(chatsOperatorID).userID = userForHelp.UserID; // оператору ставим айди пользователя, которому он помогает
                                    db.SaveChanges();

                                    await botClient.SendTextMessageAsync(
                                        chatId: chatsOperatorID,
                                        text: "Вы подключились к пользователю",
                                        replyMarkup: Keyboards.Operator.whileChattingKeyboard
                                        );

                                    await botClient.SendTextMessageAsync(
                                        chatId: userForHelp.UserID,
                                        text: "К вам подключился оператор",
                                        replyMarkup: Keyboards.User.whileChattingKeyboard
                                        );

                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(
                                        chatId: chatsOperatorID,
                                        text: "На данный момент помощь никому не нужна"
                                        );
                                }
                            }

                        }
                        break;
                }
            }
            else
            {
                switch (message.Text.ToLower())
                {
                    case "закончить диалог":
                        {
                            using (ApplicationContext db = new ApplicationContext())
                            {
                                db.Users.Find(oper.userID).operatorID = null;

                                await botClient.SendTextMessageAsync(
                                    chatId: oper.userID,
                                    text: "Оператор окончил диалог",
                                    replyMarkup: new ReplyKeyboardRemove()
                                    {
                                        Selective = true
                                    }
                                    );

                                await botClient.DeleteMessageAsync(
                                    chatId: oper.userID,
                                    messageId: db.Users.Find(oper.userID).messageMenuID
                                    );

                                var messageMenu = await botClient.SendTextMessageAsync(
                                    chatId: oper.userID,
                                    text: "Выбери кем ты являешься",
                                    replyMarkup: Keyboards.User.chooseKeyboard
                                    );

                                db.Users.Find(oper.userID).messageMenuID = messageMenu.MessageId;
                                db.SaveChanges();

                                db.Operators.Find(chatsOperatorID).userID = null;
                                db.SaveChanges();

                                await botClient.SendTextMessageAsync(
                                    chatId: chatsOperatorID,
                                    text: "Диалог закончен",
                                    replyMarkup: Keyboards.Operator.mainKeyboard
                                    );
                            }
                        }
                        break;
                    default:
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: oper.userID,
                                text: message.Text
                                );
                        }
                        break;
                }
            }
        }

        private static async Task ReceivedFromUser(ITelegramBotClient botClient, Message message) // сообщения от пользователя
        {
            long chatsUserID = message.Chat.Id;
            User user;

            await botClient.SendChatActionAsync( // анимация "Печатать", пока получается информация из бд
                    chatId: chatsUserID,
                    chatAction: ChatAction.Typing
                    );

            using (ApplicationContext db = new ApplicationContext())
            {
                if (!db.Users.Any(k => k.UserID == chatsUserID))
                {
                    user = new User() // добавление
                    {
                        UserID = chatsUserID,
                        Username = message.From.Username,
                        operatorID = null
                    };
                    db.Users.Add(user);
                    db.SaveChanges(); // сохранение изменений
                }
                else
                {
                    user = db.Users.Find(chatsUserID);
                }
            }

            if (user.operatorID != null && user.operatorID != -1)
            {
                switch (message.Text.ToLower())
                {
                    case "закончить диалог":
                        {
                            using (ApplicationContext db = new ApplicationContext())
                            {
                                db.Operators.Find(user.operatorID).userID = null;

                                await botClient.SendTextMessageAsync(
                                    chatId: user.operatorID,
                                    text: "Пользователь окончил диалог",
                                    replyMarkup: Keyboards.Operator.mainKeyboard
                                    );

                                db.Users.Find(chatsUserID).operatorID = null;
                                db.SaveChanges();

                                await botClient.SendTextMessageAsync(
                                    chatId: chatsUserID,
                                    text: "Диалог закончен",
                                    replyMarkup: new ReplyKeyboardRemove()
                                    {
                                        Selective = true
                                    }
                                    );

                                await botClient.DeleteMessageAsync(
                                    chatId: chatsUserID,
                                    messageId: db.Users.Find(chatsUserID).messageMenuID
                                    );

                                var messageMenu = await botClient.SendTextMessageAsync(
                                    chatId: chatsUserID,
                                    text: "Выбери кем ты являешься",
                                    replyMarkup: Keyboards.User.chooseKeyboard
                                    );

                                db.Users.Find(chatsUserID).messageMenuID = messageMenu.MessageId;
                                db.SaveChanges();
                            }
                        }
                        break;
                    default:
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: user.operatorID,
                                text: message.Text
                                );
                        }
                        break;
                }
            }
            else
            {
                switch (message.Text.ToLower())
                {
                    case "/start":
                        {
                            await botClient.DeleteMessageAsync(
                                chatId: chatsUserID,
                                messageId: message.MessageId
                                );

                            await botClient.SendTextMessageAsync(
                                chatId: chatsUserID,
                                text: "Привет! 🤖\n\n" +
                                "Я твой чат-бот для связи с Политехническим университетом."
                                );
                            var messageMenu = await botClient.SendTextMessageAsync(
                                chatId: chatsUserID,
                                text: "Выбери кем ты являешься",
                                replyMarkup: Keyboards.User.chooseKeyboard
                                );

                            using (ApplicationContext db = new ApplicationContext())
                            {
                                db.Users.Find(chatsUserID).messageMenuID = messageMenu.MessageId;
                                db.SaveChanges();
                            }
                        }
                        break;
                    default:
                        {
                            await botClient.DeleteMessageAsync(
                                chatId: chatsUserID,
                                messageId: message.MessageId
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
