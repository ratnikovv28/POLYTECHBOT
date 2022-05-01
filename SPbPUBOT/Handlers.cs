using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;
using System.IO;
using Telegram.Bot.Types.InputFiles;

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
            long chatID = callbackQuery.Message.Chat.Id;
            int messageID = callbackQuery.Message.MessageId;

            string[] partsQuery = callbackQuery.Data.Split("|");

            switch (partsQuery[0]) //выбор между абитуриентом и студентом
            {
                case "абитур":
                    {
                        switch (partsQuery[1]) //выбор категории интересующей темы
                        {
                            case "абитур":
                            case "назад":
                                {
                                    await botClient.EditMessageTextAsync(
                                        chatId: chatID,
                                        messageId: messageID,
                                        text: "Выбери одну из категорий, чтобы узнать больше",
                                        replyMarkup: Keyboards.User.Enrollee.startKeyboard
                                        );
                                }
                                break;
                            case "поступ": //доделать
                                {
                                    switch (partsQuery[2])
                                    {
                                        case "поступ":
                                        case "назад":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: chatID,
                                                    messageId: messageID,
                                                    text: "Чтобы изучить вопрос более детально выбери категорию, к которой ты относишься",
                                                    replyMarkup: Keyboards.User.Enrollee.Admission.basicKeyboard
                                                    );
                                            }
                                            break;
                                        case "шк":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "шк":
                                                    case "назад":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Вот вопросы, наиболее часто интересующие наших абитуриентов:",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.School.afterSchoolKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "доки":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "доки":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Ответим на все вопросы про подачу документов:",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.Documents.documentsKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "перечень":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "С полным перечнем документов, необходимых для зачисления, можно ознакомиться <a href='https://www.spbstu.ru/abit/bachelor/apply/the-list-of-documents/'>здесь</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.Documents.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "как":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "На данный момент есть несколько способов подачи документов в университет: \n" +
                                                                            "1.Лично по адресу: г.Санкт - Петербург, Политехническая ул., д. 29, Главный учебный корпус\n" +
                                                                            "2.Дистанционно через Личный кабинет абитуриента.Он будет работать с 20 июня по ссылке: https://enroll.spbstu.ru/login\n" +
                                                                            "3.Через Суперсервис «Поступление в вуз онлайн» с помощью «Госуслуг». Также будет доступен после 20 июня по ссылке: https://www.gosuslugi.ru/vuzonline\n" +
                                                                            "4.По почте на адрес: 195251, г.Санкт - Петербург, ул.Политехническая, д.29(Приемная комиссия)",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.Documents.backKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "сроки":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Старт приёмной комиссии 20 июня. Подробно со всеми датами можно ознакомиться в <a href='https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/plan-the-calendar-of-admission-to-the-1st-year/'>календаре абитуриента</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.Documents.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "колво":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "В Политехе можно всего подать документы на три направления подготовки. Причем по каждому направлению можно одновременно подавать документы по различным условиям поступления — то есть целевое, бюджет, контракт, очное, заочное и т.д. \n" +
                                                                            "Подробнее об этом написано в <a href='https://www.spbstu.ru/upload/sveden/Pravila_priema.pdf'>правилах приема 2022</a> Политеха, в пункте 3 на 12 странице.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.Documents.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "баллы":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "баллы":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Выберите год поступления",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.Points.pointsKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                default:
                                                                    {
                                                                        using (ApplicationContext db = new ApplicationContext())
                                                                        {
                                                                            long chatsUserID = callbackQuery.Message.Chat.Id;
                                                                            await botClient.DeleteMessageAsync(
                                                                                chatId: chatsUserID,
                                                                                messageId: db.Users.Find(chatsUserID).messageMenuID
                                                                                );

                                                                            using (FileStream stream = System.IO.File.OpenRead(Lists.pointsList[partsQuery[4]]))
                                                                            {
                                                                                InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, $"Баллы {partsQuery[4]}.pdf");
                                                                                await botClient.SendDocumentAsync(callbackQuery.Message.Chat.Id, inputOnlineFile);
                                                                            }

                                                                            var mesID = await botClient.SendTextMessageAsync(
                                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                                text: "Выберите год поступления",
                                                                                replyMarkup: Keyboards.User.Enrollee.Admission.School.Points.pointsKeyboard
                                                                                );
                                                                            db.Users.Find(chatsUserID).messageMenuID = mesID.MessageId;
                                                                            db.SaveChanges();
                                                                        }
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "колво":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "С количеством мест по разным условиям поступления можно ознакомиться <a href='https://www.spbstu.ru/abit/bachelor/apply/perechen-napravleniy-podgotovki/'>здесь</a>.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.School.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "направ":
                                                        {
                                                            using (ApplicationContext db = new ApplicationContext())
                                                            {
                                                                long chatsUserID = callbackQuery.Message.Chat.Id;
                                                                await botClient.DeleteMessageAsync(
                                                                    chatId: chatsUserID,
                                                                    messageId: db.Users.Find(chatsUserID).messageMenuID
                                                                    );

                                                                using (FileStream stream = System.IO.File.OpenRead("D:/Телеграм бот/SPbPUBOT/SPbPUBOT/Файлы/Obrazovatelnye_programmy_bakalavriata_i_spetsialiteta.pdf"))
                                                                {
                                                                    InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, "Образовательная программа бакалавриата и специалитета.pdf");
                                                                    await botClient.SendDocumentAsync(callbackQuery.Message.Chat.Id, inputOnlineFile);
                                                                }

                                                                var mesID = await botClient.SendTextMessageAsync(
                                                                    chatId: callbackQuery.Message.Chat.Id,
                                                                    text: "Вот вопросы, наиболее часто интересующие наших абитуриентов:",
                                                                    replyMarkup: Keyboards.User.Enrollee.Admission.School.afterSchoolKeyboard
                                                                    );
                                                                db.Users.Find(chatsUserID).messageMenuID = mesID.MessageId;
                                                                db.SaveChanges();
                                                            }
                                                        }
                                                        break;
                                                    case "контр":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "контр":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Вопросы о платном обучении:",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.ContractForm.contractFormKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "стоим":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Стоимость обучения на 2022-2023 учебный год будет опубликована к 1 июня <a href='https://www.spbstu.ru/abit/bachelor/apply/stoimost-obucheniya/'>здесь</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.ContractForm.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "кредит":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "У студентов есть возможность заплатить за свое обучение самостоятельно, взяв льготный образовательный кредит. Про это мы написали в <a href='https://zen.yandex.ru/media/pokolenie/chto-takoe-obrazovatelnyi-kredit-6241c8ccf911f53a214d9349'>нашей статье</a> на Дзене.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.ContractForm.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "маткап":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Оплатить обучение можно и <a href='https://pfr.gov.ru/grazhdanam/msk/msk_obrazovanie/'>материнским капиталом</a>. На дату начала обучения ребенок должен быть не старше 25 лет. Организация должна находиться на территории России и иметь лицензию на оказание образовательных услуг. Подробнее об оплате обучения материнским капиталом написано <a href='https://www.spbstu.ru/abit/events/kak-eshche-mozhno-oplatit-uchebu-v-politekhe-materinskiy-kapital-i-obrazovatelnyy-kredit/'>здесь</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.ContractForm.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "целевое":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "В Политех можно поступить на целевое обучение, т.е. на бюджет по направлению от государственного ведомства или предприятия. Подробно об этом написано <a href='https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/tselevoe-obuchenie/'>вот здесь</a>.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.School.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "вступ":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "вступ":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Выберите один из пунктов ниже",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.EntranceTests.entranceTestsKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "лица":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Нужно ли и можно ли тебе сдавать вступительные испытания Политеха?\nПроверь на сайте, относишься ли ты <a href='https://www.spbstu.ru/abit/bachelor/entrance-test/allowed-pass-entrance-test/'>к этим категориям</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.EntranceTests.backKeyboard,
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
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.EntranceTests.backKeyboard,
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
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.EntranceTests.backKeyboard,
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
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.EntranceTests.backKeyboard,
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
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.EntranceTests.backKeyboard,
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
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.EntranceTests.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "я олимп":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "я олимп":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Грацуем!",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.OlypmiadMan.olympiadManKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "бви":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "У победителей и призеров олимпиад есть особое право поступления без вступительных испытаний. О том, как им воспользоваться, узнай <a href='https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/olympics/'>по ссылке</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.OlypmiadMan.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "соотв":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Соответствие предметов олимпиад и направлений СПбПУ можно проверить в <a href='https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/olympics/the-line-profile-all-russian-olympiad-/'>этом положении</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.OlypmiadMan.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "достиж":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "1.Максимальная сумма баллов, которые вы можете получить за индивидуальные достижения - 10 баллов.\n\n" +
                                                                "2.Если вы являетесь победителем/призёром заключительного этапа Всероссийской олимпиады школьников, то вы можете быть зачисленными в университет без вступительных испытаний.\n\n" +
                                                                "3.Если вы являетесь победителем/призёром олимпиады РСОШ с ЕГЭ не менее 75 баллов, то вы можете быть зачисленными в университет без вступительных испытаний.\n\n" +
                                                                "4.Для того чтобы получить дополнительные баллы за золотой значок ГТО, необходимо предоставить удостоверение установленного образца, подписанное министром спорта РФ. Сам значок и какие-либо другие документы в приёмную комиссию предоставлять не нужно.\n\n" +
                                                                "5.Более подробнее со списком индивидуальных достижений можно ознакомиться ниже",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.School.IndividualAchievements.individualAchievementsKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "меддоки":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "меддоки":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Вопросы о медосмотре и медицинских документах:",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.MedicalDocuments.medicalDocumentsKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "что":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Существует обязательный медицинский осмотр для поступающих на определенные специальности и программы подготовки:\n" +
                                                                            "— Теплоэнергетика и теплотехника; \n" +
                                                                            "— Электроэнергетика и электротехника; \n" +
                                                                            "— Ядерная энергетика и теплофизика; \n" +
                                                                            "— Атомные станции: проектирование, эксплуатация и инжиниринг;\n" +
                                                                            "— Технология транспортных процессов; \n" +
                                                                            "— Наземные транспортно-технологические комплексы; \n" +
                                                                            "— Транспортные средства специального назначения; \n" +
                                                                            "— Технология продукции и организация общественного питания; \n" +
                                                                            "— Педагогическое образование; \n" +
                                                                            "— Психолого - педагогическое образование\n" +
                                                                            "  Необходимо проходить обязательный предварительный медицинский осмотр(обследование) для выявления наличия либо отсутствия медицинских противопоказаний, препятствующих осуществлению профессиональной деятельности в период обучения в университете и после его окончания.См.<a href='https://base.garant.ru/70434720/'>Постановление Правительства РФ от 14 августа 2013 г. № 697</a>.",
                                                                           replyMarkup: Keyboards.User.Enrollee.Admission.School.MedicalDocuments.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "зачисл":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "После зачисления первокурсникам необходимо будет предоставить некоторые медицинские документы. Ознакомиться с ними можно <a href='https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/meditsinskie-dokumenty-dlya-zachislennykh-studentov/'>здесь</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.School.MedicalDocuments.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "минбаллы":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Минимальные баллы — это то количество баллов, которое необходимо набрать, чтобы участвовать в конкурсе. То есть, набрав, к примеру, 50 баллов по русскому языку (а минимальный 55), абитуриент не сможет подать документы в Политех, к сожалению. Ознакомиться с минимальными баллами можно <a href='https://www.spbstu.ru/abit/bachelor/entrance-test/the-list-of-entrance-examinations/'>по ссылке</a>.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.School.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "льгот":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "У некоторых поступающих есть особое право, которым они могут воспользоваться. Узнать, относишься ли ты к льготной категории лиц и какие условия у льготников есть, можно <a href='https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/beneficiaries/'>здесь</a>.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.School.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case "колл":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "колл":
                                                    case "назад":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Самые популярные вопросы наших абитуриентов:",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.College.afterCollegeKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "доки":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "С полным перечнем документов, необходимых для зачисления, можно ознакомиться <a href='https://www.spbstu.ru/abit/bachelor/apply/the-list-of-documents/'>здесь</a>.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.College.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "баллы":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "баллы":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Выберите год поступления",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.Points.pointsKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                default:
                                                                    {
                                                                        using (ApplicationContext db = new ApplicationContext())
                                                                        {
                                                                            long chatsUserID = callbackQuery.Message.Chat.Id;
                                                                            await botClient.DeleteMessageAsync(
                                                                                chatId: chatsUserID,
                                                                                messageId: db.Users.Find(chatsUserID).messageMenuID
                                                                                );

                                                                            using (FileStream stream = System.IO.File.OpenRead(Lists.pointsList[partsQuery[4]]))
                                                                            {
                                                                                InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, $"Баллы {partsQuery[4]}.pdf");
                                                                                await botClient.SendDocumentAsync(callbackQuery.Message.Chat.Id, inputOnlineFile);
                                                                            }

                                                                            var messageID = await botClient.SendTextMessageAsync(
                                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                                text: "Выберите год поступления",
                                                                                replyMarkup: Keyboards.User.Enrollee.Admission.College.Points.pointsKeyboard
                                                                                );
                                                                            db.Users.Find(chatsUserID).messageMenuID = messageID.MessageId;
                                                                            db.SaveChanges();
                                                                        }
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "колво":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "С количеством мест по разным условиям поступления можно ознакомиться <a href='https://www.spbstu.ru/abit/bachelor/apply/perechen-napravleniy-podgotovki/'>здесь</a>.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.College.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "направ":
                                                        {
                                                            using (ApplicationContext db = new ApplicationContext())
                                                            {
                                                                long chatsUserID = callbackQuery.Message.Chat.Id;
                                                                await botClient.DeleteMessageAsync(
                                                                    chatId: chatsUserID,
                                                                    messageId: db.Users.Find(chatsUserID).messageMenuID
                                                                    );

                                                                using (FileStream stream = System.IO.File.OpenRead("D:/Телеграм бот/SPbPUBOT/SPbPUBOT/Файлы/Obrazovatelnye_programmy_bakalavriata_i_spetsialiteta.pdf"))
                                                                {
                                                                    InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, "Образовательная программа бакалавриата и специалитета.pdf");
                                                                    await botClient.SendDocumentAsync(callbackQuery.Message.Chat.Id, inputOnlineFile);
                                                                }

                                                                var messageID = await botClient.SendTextMessageAsync(
                                                                    chatId: callbackQuery.Message.Chat.Id,
                                                                    text: "Самые популярные вопросы наших абитуриентов:",
                                                                    replyMarkup: Keyboards.User.Enrollee.Admission.College.afterCollegeKeyboard
                                                                    );
                                                                db.Users.Find(chatsUserID).messageMenuID = messageID.MessageId;
                                                                db.SaveChanges();
                                                            }
                                                        }
                                                        break;
                                                    case "контр":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "контр":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Вопросы о платном обучении:",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.ContractForm.contractFormKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "стоим":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Стоимость обучения на 2022-2023 учебный год будет опубликована к 1 июня <a href='https://www.spbstu.ru/abit/bachelor/apply/stoimost-obucheniya/'>здесь</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.ContractForm.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "кредит":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "У студентов есть возможность заплатить за свое обучение самостоятельно, взяв льготный образовательный кредит. Про это мы написали в <a href='https://zen.yandex.ru/media/pokolenie/chto-takoe-obrazovatelnyi-kredit-6241c8ccf911f53a214d9349'>нашей статье</a> на Дзене.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.ContractForm.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "маткап":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Оплатить обучение можно и <a href='https://pfr.gov.ru/grazhdanam/msk/msk_obrazovanie/'>материнским капиталом</a>. На дату начала обучения ребенок должен быть не старше 25 лет. Организация должна находиться на территории России и иметь лицензию на оказание образовательных услуг. Подробнее об оплате обучения материнским капиталом написано <a href='https://www.spbstu.ru/abit/events/kak-eshche-mozhno-oplatit-uchebu-v-politekhe-materinskiy-kapital-i-obrazovatelnyy-kredit/'>здесь</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.ContractForm.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "целевое":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "В Политех можно поступить на целевое обучение, т.е. на бюджет по направлению от государственного ведомства или предприятия. Подробно об этом написано <a href='https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/tselevoe-obuchenie/'>вот здесь</a>.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.College.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "кc":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "кc":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Самые актуальные вопросы:",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.CompetitiveLists.competitiveListsKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "как":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Конкурсные списки ранжируются следующим образом: \n" +
                                                                            "1.По убыванию суммы конкурсных баллов(баллы за ЕГЭ / Вступительные испытания + баллы за индивидуальные достижения).\n" +
                                                                            "2.При равенстве суммы конкурсных баллов — по убыванию суммы баллов, начисленных по результатам вступительных испытаний.\n" +
                                                                            "3.При равенстве суммы баллов, начисленных по результатам вступительных испытаний, — по убыванию количества баллов, начисленных по результатам отдельных вступительных испытаний, в соответствии с их приоритетностью.\n" +
                                                                            "4.При равенстве по критериям, указанным в пунктах 1 - 3, — по наличию преимущественного права, указанного в части 9 статьи 71 Федерального закона № 273 - ФЗ.\n" +
                                                                            "5.При равенстве по критериям, указанным в пунктах 1 - 4, — по наличию преимущественного права, указанного в части 10 статьи 71 Федерального закона № 273 - ФЗ.\n" +
                                                                            "6.При равенстве по критериям, указанным в пунктах 1 - 5, — по индивидуальным достижениям, учитываемым при равенстве поступающих по иным критериям ранжирования.\n" +
                                                                            "Все в подробности написано в <a href='https://vk.com/doc-121255855_615823062?hash=e63251815fba6db516&dl=5ebeb60421b70a7e95'>правилах приема 2022</a> в 8 пункте на 25 странице.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.CompetitiveLists.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "где":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Списки подавших документы можно найти <a href='https://www.spbstu.ru/abit/admission-campaign/'>здесь</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.CompetitiveLists.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "нетчел":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Сначала давайте убедимся, что вас действительно нет в списке. Чтобы абитуриент появился в списке, его заявка в Личном кабинете должна быть уже рассмотрена и одобрена (проверьте уведомления).\n\n" +
                                                                            "После этого абитуриент попадает <a href='https://www.spbstu.ru/abit/admission-campaign/'>в список</a>. Внимательно выбирайте параметры (проверьте, что вы не перепутали институт, форму обучения и т.д.). Часто путают бакалавриат и специалитет.\n\n" +
                                                                            "Далее пролистайте список до конца и раскройте список полностью.Выберете в строчке 'Строк на странице' параметр 'Все'.\n\n" +
                                                                            "Вы проходите под номером своего СНИЛС.Если вы не указывали СНИЛС, вам нужно найти ваш ID в Личном кабинете(правый верхний угол, под вашим именем), вы будете в списках по этому номеру.Чтобы найти свой номер, можно использовать комбинацию клавиш Ctrl + F.\n\n" +
                                                                            "Если не получается найти СНИЛС, то проверьте, не ошиблись ли вы в его написании в вашем ЛК-- это тоже частая ошибка.\n\n" +
                                                                            "Ничего из перечисленного не помогло ? Тогда пишем в техподдержку на почту <a href='http://mailto:support@spbstu.ru/'>support@spbstu.ru</a>. Укажите в письме номер ID из личного кабинета, ФИО и подробно расскажите о проблеме (указывая даты и прилагая скриншоты) — так мы сможем скорее вам помочь.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.CompetitiveLists.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "нетпред":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Без паники, мы вам поможем! Напишите, пожалуйста, нам в техподдержку на почту <a href='http://mailto:support@spbstu.ru/'>support@spbstu.ru</a>. Укажите в письме номер ID из личного кабинета, ФИО и подробно расскажите о проблеме (указывая даты и прилагая скриншоты) — так мы сможем скорее вам помочь.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.CompetitiveLists.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "нетиндивид":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Без паники, мы вам поможем! Напишите, пожалуйста, нам в техподдержку на почту <a href='http://mailto:support@spbstu.ru/'>support@spbstu.ru</a>. Укажите в письме номер ID из личного кабинета, ФИО и подробно расскажите о проблеме (указывая даты и прилагая скриншоты) — так мы сможем скорее вам помочь.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.CompetitiveLists.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "меддоки":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "меддоки":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Вопросы о медосмотре и медицинских документах:",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.MedicalDocuments.medicalDocumentsKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "что":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Существует обязательный медицинский осмотр для поступающих на определенные специальности и программы подготовки:\n" +
                                                                            "— Теплоэнергетика и теплотехника; \n" +
                                                                            "— Электроэнергетика и электротехника; \n" +
                                                                            "— Ядерная энергетика и теплофизика; \n" +
                                                                            "— Атомные станции: проектирование, эксплуатация и инжиниринг;\n" +
                                                                            "— Технология транспортных процессов; \n" +
                                                                            "— Наземные транспортно-технологические комплексы; \n" +
                                                                            "— Транспортные средства специального назначения; \n" +
                                                                            "— Технология продукции и организация общественного питания; \n" +
                                                                            "— Педагогическое образование; \n" +
                                                                            "— Психолого - педагогическое образование\n" +
                                                                            "  Необходимо проходить обязательный предварительный медицинский осмотр(обследование) для выявления наличия либо отсутствия медицинских противопоказаний, препятствующих осуществлению профессиональной деятельности в период обучения в университете и после его окончания.См.<a href='https://base.garant.ru/70434720/'>Постановление Правительства РФ от 14 августа 2013 г. № 697</a>.",
                                                                           replyMarkup: Keyboards.User.Enrollee.Admission.College.MedicalDocuments.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "зачисл":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "После зачисления первокурсникам необходимо будет предоставить некоторые медицинские документы. Ознакомиться с ними можно <a href='https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/meditsinskie-dokumenty-dlya-zachislennykh-studentov/'>здесь</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.MedicalDocuments.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "достиж":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "1.Максимальная сумма баллов, которые вы можете получить за индивидуальные достижения - 10 баллов.\n\n" +
                                                                "2.Если вы являетесь победителем/призёром заключительного этапа Всероссийской олимпиады школьников, то вы можете быть зачисленными в университет без вступительных испытаний.\n\n" +
                                                                "3.Если вы являетесь победителем/призёром олимпиады РСОШ с ЕГЭ не менее 75 баллов, то вы можете быть зачисленными в университет без вступительных испытаний.\n\n" +
                                                                "4.Для того чтобы получить дополнительные баллы за золотой значок ГТО, необходимо предоставить удостоверение установленного образца, подписанное министром спорта РФ. Сам значок и какие-либо другие документы в приёмную комиссию предоставлять не нужно.\n\n" +
                                                                "5.Более подробнее со списком индивидуальных достижений можно ознакомиться ниже",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.College.IndividualAchievements.individualAchievementsKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "ви":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "ви":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Здесь есть все, что вы хотели знать о вступительных испытаниях:",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.EntranceTests.entranceTestsKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "лица":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Нужно ли и можно ли тебе сдавать вступительные испытания Политеха?\nПроверь на сайте, относишься ли ты <a href='https://www.spbstu.ru/abit/bachelor/entrance-test/allowed-pass-entrance-test/'>к этим категориям</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.EntranceTests.backKeyboard,
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
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.EntranceTests.backKeyboard,
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
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.EntranceTests.backKeyboard,
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
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.EntranceTests.backKeyboard,
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
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.EntranceTests.backKeyboard,
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
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.College.EntranceTests.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "вебинар":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Специально для выпускников колледжа мы записали вебинар, где рассказали об особенностях поступления в Политехнический университет. Запись есть <a href='https://vk.com/wall-121255855_47445'>здесь</a>.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.College.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "статья":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Мы подробно написали о поступлении после колледжа в <a href='https://zen.yandex.ru/media/pokolenie/postuplenie-posle-kolledja-chto-nujno-znat-620627117f3eb35498fdd759'>этой статье</a>.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.College.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
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
                                                    case "назад":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Выбери что из указанных разделов тебя наиболее сильно интересует",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.magistrKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "доки":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "доки":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Ответим на все вопросы про подачу документов:",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.Documents.documentsKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "перечень":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "С полным перечнем документов, необходимых для зачисления, можно ознакомиться <a href='https://www.spbstu.ru/abit/master/apply/the-list-of-documents/'>здесь</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.Documents.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "как":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "На данный момент есть несколько способов подачи документов в университет: \n" +
                                                                            "1.Лично по адресу: г.Санкт - Петербург, Политехническая ул., д. 29, Главный учебный корпус\n" +
                                                                            "2.Дистанционно через Личный кабинет абитуриента https://enroll.spbstu.ru/login\n" +
                                                                            "3.По почте на адрес: 195251, г.Санкт - Петербург, ул.Политехническая, д.29(Приемная комиссия)",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.Documents.backKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "сроки":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Прием в магистратуру начинается 4 апреля. С подробным планом-календарем приема в магистратуру можно ознакомиться <a href='https://www.spbstu.ru/abit/master/review-the-regulatory-documents/plan-the-calendar-of-admission/'>здесь</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.Documents.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "колво":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "На любое количество направлений. Обращаем внимание, что экзамены проводятся отдельно по каждому.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.Documents.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "ви":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "ви":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Выберите один из пунктов ниже",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.EntranceTests.entranceTestsKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "прога":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Программы вступительных испытаний всех образовательных программ вы можете найти на <a href='https://www.spbstu.ru/abit/master/pass-the-entrance-tests/program-of-entrance-examinations/'>сайте</a>",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.EntranceTests.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "примеры":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "<a href='https://www.spbstu.ru/abit/master/pass-the-entrance-tests/program-of-entrance-examinations/'>Примеры вступительных испытаний</a> указаны в каждой программе вступительных испытаний в теле документа",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.EntranceTests.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "расписание":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Расписание вступительных испытаний будет доступно по <a href='https://www.spbstu.ru/abit/master/pass-the-entrance-tests/the-list-of-entrance-examinations/'>ссылке</a> до 1 июня 2022 года.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.EntranceTests.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "вступ":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Междисциплинарный экзамен проводится в очно в письменной форме и (или) с использованием дистанционных технологий (при условии идентификации поступающих при сдаче ими вступительных испытаний). Более подробно про вступительные испытания написано <a href='https://www.spbstu.ru/abit/master/pass-the-entrance-tests/entrance-test-and-olympiad/'>здесь</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.EntranceTests.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "колво":
                                                        {
                                                            using (ApplicationContext db = new ApplicationContext())
                                                            {
                                                                long chatsUserID = callbackQuery.Message.Chat.Id;
                                                                await botClient.DeleteMessageAsync(
                                                                    chatId: chatsUserID,
                                                                    messageId: db.Users.Find(chatsUserID).messageMenuID
                                                                    );

                                                                using (FileStream stream = System.IO.File.OpenRead("D:/Телеграм бот/SPbPUBOT/SPbPUBOT/Файлы/master_apply_the_list_of_areas_of_training.pdf"))
                                                                {
                                                                    InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, "Общее количество бюджетных и контрактных мест для поступления в 2021 году.pdf");
                                                                    await botClient.SendDocumentAsync(callbackQuery.Message.Chat.Id, inputOnlineFile);
                                                                }

                                                                var messageID = await botClient.SendTextMessageAsync(
                                                                    chatId: callbackQuery.Message.Chat.Id,
                                                                    text: "Выбери что из указанных разделов тебя наиболее сильно интересует",
                                                                    replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.magistrKeyboard
                                                                    );
                                                                db.Users.Find(chatsUserID).messageMenuID = messageID.MessageId;
                                                                db.SaveChanges();
                                                            }
                                                        }
                                                        break;
                                                    case "направ":
                                                        {
                                                            using (ApplicationContext db = new ApplicationContext())
                                                            {
                                                                long chatsUserID = callbackQuery.Message.Chat.Id;
                                                                await botClient.DeleteMessageAsync(
                                                                    chatId: chatsUserID,
                                                                    messageId: db.Users.Find(chatsUserID).messageMenuID
                                                                    );

                                                                using (FileStream stream = System.IO.File.OpenRead("D:/Телеграм бот/SPbPUBOT/SPbPUBOT/Файлы/magistr2022.pdf"))
                                                                {
                                                                    InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, "Образовательная программа магистра.pdf");
                                                                    await botClient.SendDocumentAsync(callbackQuery.Message.Chat.Id, inputOnlineFile);
                                                                }

                                                                var messageID = await botClient.SendTextMessageAsync(
                                                                    chatId: callbackQuery.Message.Chat.Id,
                                                                    text: "Выбери что из указанных разделов тебя наиболее сильно интересует",
                                                                    replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.magistrKeyboard
                                                                    );
                                                                db.Users.Find(chatsUserID).messageMenuID = messageID.MessageId;
                                                                db.SaveChanges();
                                                            }
                                                        }
                                                        break;
                                                    case "допы":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Дополнительные баллы можно получить за наличие индивидуальных достижений.\n" +
                                                                "диплом с отличием(+10)\n" +
                                                                "статус призёра конкурса портфолио СПбПУ(+6)\n" +
                                                                "статус призёра олимпиады «Я – профессионал» 2021 и 2022(+4)\n" +
                                                                "Подробнее <a href='https://www.spbstu.ru/abit/master/review-the-regulatory-documents/individual-achievements/'>здесь</a>.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "минбаллы":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Минимальное количество баллов за междисциплинарный экзамен составляет 50 баллов.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.backKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "потрф":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "<a href='https://www.spbstu.ru/abit/master/pass-the-entrance-tests/konkurs-portfolio-spbpu/'>Конкурс портфолио</a> – это возможность для абитуриента поступить в магистратуру без вступительных испытаний (в случае победы), либо заработать +6 баллов в качестве индивидуального достижения (если призовое место).\n\n" +
                                                                "Победители конкурса портфолио, поступившие в СПбПУ, будут получать ежемесячно грант от 15000 рублей.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "япроф":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Победители и призеры олимпиады «Я – профессионал» могут поступить в магистратуру без вступительных испытаний, а также получить дополнительно +4 балла к сумме конкурсных баллов. Узнать подробнее об олимпиаде «Я – профессионал» можно <a href='https://www.spbstu.ru/abit/master/pass-the-entrance-tests/olimpiada-ya-professional/'>здесь</a>.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "целевое":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "В Политех можно поступить на целевое обучение, т.е. на бюджет по направлению от государственного ведомства или предприятия. Подробно об этом написано вот <a href='https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/tselevoe-obuchenie/'>здесь</a>.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.Magistr.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case "даты":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Чтобы оставаться в курсе событий рекомендуем установить наши календари",
                                                    replyMarkup: Keyboards.User.Enrollee.Admission.Dates.datesKeyboard
                                                    );
                                            }
                                            break;
                                        case "проц":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "проц":
                                                    case "назад":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Выберите вопрос, ответ на который вы не смогли найти в других разделах или начните чат с представителем приемной комиссии",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.admissionProcessKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "опер":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "опер":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Для того, чтобы начать диалог с представителем Политеха, нажмите кнопку ниже.\n\n" +
                                                                            "А ещё вы можете связаться с Приемной комиссией Политеха по телефону\n" +
                                                                            "8(812) 775 - 05 - 30 — для звонков из Санкт - Петербурга\n" +
                                                                            "8(800) 707 - 18 - 99 — для звонков из любого региона РФ(звонок бесплатный)\n\n" +
                                                                            "Или написать на почту abitur@spbstu.ru.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.Operator.callOperatorKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "вызов":
                                                                    {
                                                                        using (ApplicationContext db = new ApplicationContext())
                                                                        {
                                                                            long chatsUserID = callbackQuery.Message.Chat.Id;
                                                                            await botClient.DeleteMessageAsync(
                                                                                chatId: chatsUserID,
                                                                                messageId: db.Users.Find(chatsUserID).messageMenuID
                                                                                );
                                                                            if (db.UserAssistance.FirstOrDefault(k => k.UserID == chatsUserID) == null)
                                                                            {
                                                                                db.UserAssistance.Add(new UserAssistance()
                                                                                {
                                                                                    UserID = chatsUserID,
                                                                                    Username = callbackQuery.From.Username
                                                                                });

                                                                                await botClient.SendTextMessageAsync(
                                                                                    chatId: chatsUserID,
                                                                                    text: "Вы добавлены в очередь ожиданий, к вам подключится первый освободившийся оператора"
                                                                                    );
                                                                            }
                                                                            else
                                                                            {
                                                                                await botClient.SendTextMessageAsync(
                                                                                    chatId: chatsUserID,
                                                                                    text: "Вы уже добавлены в очередь ожиданий, к вам подключится первый освободившийся оператора"
                                                                                    );
                                                                            }
                                                                            var messageID = await botClient.SendTextMessageAsync(
                                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                                text: "Для того, чтобы начать диалог с представителем Политеха, нажмите кнопку ниже.\n\n" +
                                                                                "А ещё вы можете связаться с Приемной комиссией Политеха по телефону\n" +
                                                                                "8(812) 775 - 05 - 30 — для звонков из Санкт - Петербурга\n" +
                                                                                "8(800) 707 - 18 - 99 — для звонков из любого региона РФ(звонок бесплатный)\n\n" +
                                                                                "Или написать на почту abitur@spbstu.ru.",
                                                                                replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.Operator.callOperatorKeyboard
                                                                                );
                                                                            db.Users.Find(chatsUserID).messageMenuID = messageID.MessageId;
                                                                            db.SaveChanges();
                                                                        }
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "подал":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "подал":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Ответим на все вопросы:",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.WhatsNext.whatsNextKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "прик":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Приказы о зачислении выходят в соответствии с <a href='https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/plan-the-calendar-of-admission-to-the-1st-year/'>календарем</a> приема в <a href='https://www.spbstu.ru/abit/admission-campaign/'>этом разделе</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.WhatsNext.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "согл":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Согласие на зачисление — это основание для твоего зачисления. Именно абитуриенты, подавшие согласие на зачисление, будут создавать конкурсную ситуацию и будут зачислены по конкурсу в университет.\n\n" +
                                                                            "Согласие может быть подано только в один вуз на одно направление подготовки, поскольку этот документ подтверждает, что готовы учиться в выбранном университете и в начале учебного года придете на занятия.Подавать несколько согласий на зачисление на бюджет(вне зависимости от того, один это вуз или несколько), не отозвав предыдущие, нельзя.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.WhatsNext.backKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "дал":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Сначала стоит порадоваться тому, что теперь вы студент Политеха. Поздравляем вас и желаем провести университетские годы с максимальной пользой и интересом! А теперь давайте более конкретно.\n\n" +
                                                                            "В конце августа состоятся встречи первокурсников в институтах и высших школах, куда они поступили.А также иногородние студенты будут заселяться в общежития.Обо всем, что вас ждет, подробно рассказано на <a href='https://www.spbstu.ru/freshman/'>специальном сайте</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.WhatsNext.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "шансы":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Это будет зависеть от количества желающих поступить на то же направление подготовки, что и вы, от количества там бюджетных мест, от результатов вступительных испытаний абитуриентов. Сейчас предсказать все эти факторы невозможно, поэтому ориентируйтесь на <a href='vk.cc/ay5gVJ'>ранжированные списки</a>.\n\n" +
                                                                "Вы можете отсортировать списки по поданным согласиям(нажмите на соответствующий заголовок столбца в списке)-- так увидите более реальную конкурсную ситуацию.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "контр":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "контр":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Вопросы о платном обучении:",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.ContractForm.contractFormKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "стоим":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Стоимость обучения на 2022-2023 учебный год будет опубликована к 1 июня <a href='https://www.spbstu.ru/abit/bachelor/apply/stoimost-obucheniya/'>здесь</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.ContractForm.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "кредит":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "У студентов есть возможность заплатить за свое обучение самостоятельно, взяв льготный образовательный кредит. Про это мы написали в <a href='https://zen.yandex.ru/media/pokolenie/chto-takoe-obrazovatelnyi-kredit-6241c8ccf911f53a214d9349'>нашей статье</a> на Дзене.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.ContractForm.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "маткап":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Оплатить обучение можно и <a href='https://pfr.gov.ru/grazhdanam/msk/msk_obrazovanie/'>материнским капиталом</a>. На дату начала обучения ребенок должен быть не старше 25 лет. Организация должна находиться на территории России и иметь лицензию на оказание образовательных услуг. Подробнее об оплате обучения материнским капиталом написано <a href='https://www.spbstu.ru/abit/events/kak-eshche-mozhno-oplatit-uchebu-v-politekhe-materinskiy-kapital-i-obrazovatelnyy-kredit/'>здесь</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.ContractForm.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "цел":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "В Политех можно поступить на целевое обучение, т.е. на бюджет по направлению от государственного ведомства или предприятия. Подробно об этом написано вот <a href='https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/tselevoe-obuchenie/'>здесь</a>.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "кc":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "кc":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Самые актуальные вопросы:",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.CompetitiveLists.competitiveListsKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "как":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Конкурсные списки ранжируются следующим образом: \n" +
                                                                            "1.По убыванию суммы конкурсных баллов(баллы за ЕГЭ / Вступительные испытания + баллы за индивидуальные достижения).\n" +
                                                                            "2.При равенстве суммы конкурсных баллов — по убыванию суммы баллов, начисленных по результатам вступительных испытаний.\n" +
                                                                            "3.При равенстве суммы баллов, начисленных по результатам вступительных испытаний, — по убыванию количества баллов, начисленных по результатам отдельных вступительных испытаний, в соответствии с их приоритетностью.\n" +
                                                                            "4.При равенстве по критериям, указанным в пунктах 1 - 3, — по наличию преимущественного права, указанного в части 9 статьи 71 Федерального закона № 273 - ФЗ.\n" +
                                                                            "5.При равенстве по критериям, указанным в пунктах 1 - 4, — по наличию преимущественного права, указанного в части 10 статьи 71 Федерального закона № 273 - ФЗ.\n" +
                                                                            "6.При равенстве по критериям, указанным в пунктах 1 - 5, — по индивидуальным достижениям, учитываемым при равенстве поступающих по иным критериям ранжирования.\n" +
                                                                            "Все в подробности написано в <a href='https://vk.com/doc-121255855_615823062?hash=e63251815fba6db516&dl=5ebeb60421b70a7e95'>правилах приема 2022</a> в 8 пункте на 25 странице.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.CompetitiveLists.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "где":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Списки подавших документы можно найти <a href='https://www.spbstu.ru/abit/admission-campaign/'>здесь</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.CompetitiveLists.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "нетчел":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Сначала давайте убедимся, что вас действительно нет в списке. Чтобы абитуриент появился в списке, его заявка в Личном кабинете должна быть уже рассмотрена и одобрена (проверьте уведомления).\n\n" +
                                                                            "После этого абитуриент попадает <a href='https://www.spbstu.ru/abit/admission-campaign/'>в список</a>. Внимательно выбирайте параметры (проверьте, что вы не перепутали институт, форму обучения и т.д.). Часто путают бакалавриат и специалитет.\n\n" +
                                                                            "Далее пролистайте список до конца и раскройте список полностью.Выберете в строчке 'Строк на странице' параметр 'Все'.\n\n" +
                                                                            "Вы проходите под номером своего СНИЛС.Если вы не указывали СНИЛС, вам нужно найти ваш ID в Личном кабинете(правый верхний угол, под вашим именем), вы будете в списках по этому номеру.Чтобы найти свой номер, можно использовать комбинацию клавиш Ctrl + F.\n\n" +
                                                                            "Если не получается найти СНИЛС, то проверьте, не ошиблись ли вы в его написании в вашем ЛК-- это тоже частая ошибка.\n\n" +
                                                                            "Ничего из перечисленного не помогло ? Тогда пишем в техподдержку на почту <a href='http://mailto:support@spbstu.ru/'>support@spbstu.ru</a>. Укажите в письме номер ID из личного кабинета, ФИО и подробно расскажите о проблеме (указывая даты и прилагая скриншоты) — так мы сможем скорее вам помочь.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.CompetitiveLists.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "нетпред":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Без паники, мы вам поможем! Напишите, пожалуйста, нам в техподдержку на почту <a href='http://mailto:support@spbstu.ru/'>support@spbstu.ru</a>. Укажите в письме номер ID из личного кабинета, ФИО и подробно расскажите о проблеме (указывая даты и прилагая скриншоты) — так мы сможем скорее вам помочь.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.CompetitiveLists.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "нетиндивид":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Без паники, мы вам поможем! Напишите, пожалуйста, нам в техподдержку на почту <a href='http://mailto:support@spbstu.ru/'>support@spbstu.ru</a>. Укажите в письме номер ID из личного кабинета, ФИО и подробно расскажите о проблеме (указывая даты и прилагая скриншоты) — так мы сможем скорее вам помочь.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.CompetitiveLists.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "меддоки":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "меддоки":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Вопросы о медосмотре и медицинских документах:",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.MedicalDocuments.medicalDocumentsKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "что":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Существует обязательный медицинский осмотр для поступающих на определенные специальности и программы подготовки:\n" +
                                                                            "— Теплоэнергетика и теплотехника; \n" +
                                                                            "— Электроэнергетика и электротехника; \n" +
                                                                            "— Ядерная энергетика и теплофизика; \n" +
                                                                            "— Атомные станции: проектирование, эксплуатация и инжиниринг;\n" +
                                                                            "— Технология транспортных процессов; \n" +
                                                                            "— Наземные транспортно-технологические комплексы; \n" +
                                                                            "— Транспортные средства специального назначения; \n" +
                                                                            "— Технология продукции и организация общественного питания; \n" +
                                                                            "— Педагогическое образование; \n" +
                                                                            "— Психолого - педагогическое образование\n" +
                                                                            "  Необходимо проходить обязательный предварительный медицинский осмотр(обследование) для выявления наличия либо отсутствия медицинских противопоказаний, препятствующих осуществлению профессиональной деятельности в период обучения в университете и после его окончания.См.<a href='https://base.garant.ru/70434720/'>Постановление Правительства РФ от 14 августа 2013 г. № 697</a>.",
                                                                           replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.MedicalDocuments.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "зачисл":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "После зачисления первокурсникам необходимо будет предоставить некоторые медицинские документы. Ознакомиться с ними можно <a href='https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/meditsinskie-dokumenty-dlya-zachislennykh-studentov/'>здесь</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.MedicalDocuments.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "инст":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Контакты всех институтов есть вот на этой странице <a href='https://contact.spbstu.ru/'>contact.spbstu.ru</a>. Там вы найдете сайты, телефоны и соцсети наших институтов СПбПУ. У некоторых из них есть чаты абитуриентов. А если у вас вопрос по поводу образовательной программы, перспективы направления или вы не можете определиться, куда лучше поступать, то представители институтов сориентируют вас и помогут!",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "госуслуг":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "На сегодняшний день мы обрабатываем всю информацию, которая к нам поступает, но переживаем, что с портала 'Госуслуги' к нам могут прийти не все данные о поступающих из-за большого количество заявок. Поэтому мы рекомендуем вам подать заявление в Личном кабинете абитуриента на сайте университета <a href='https://enroll.spbstu.ru/sign-in'>enroll.spbstu.ru</a> - там наши модераторы скорее смогут вас найти и добавить в ранжированные списки.\n\n" +
                                                                "Это поможет вам оперативно отслеживать свою позицию в списках, а также вносить необходимые изменения быстрее",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.AdmissionProcess.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case "инос":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "инос":
                                                    case "назад":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Есть несколько способов поступления:",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.ForeignStudents.foreignStudentsKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "соот":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Соотечественники имеют право поступать в СПбПУ на общих с гражданами России основаниях (то есть могут претендовать на бюджетное место).\n\n" +
                                                                "К соотечественникам относятся\n" +
                                                                "— граждане Российской Федерации, постоянно проживающие за пределами Российской Федерации\n" +
                                                                "— лица, состоявшие в гражданстве СССР, проживающие в государствах, входивших в состав СССР, получившие гражданство этих государств или ставшие лицами без гражданства\n" +
                                                                "— выходцы(эмигранты) из Российского государства, Российской республики, РСФСР, СССР и Российской Федерации, имевшие соответствующую гражданскую принадлежность и ставшие гражданами иностранного государства либо имеющие вид на жительство или ставшие лицами без гражданства\n" +
                                                                "— потомки лиц, принадлежащих к вышеуказанным группам\n\n" +
                                                                "Статус соотечественника необходимо документально подтвердить.Подробности есть <a href='https://www.spbstu.ru/abit/bachelor/apply/sootechestvennikam/'>здесь</a>.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.ForeignStudents.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "отдел":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Иностранные абитуриенты могут поступить в Политехнический университет через Отдел по работе с иностранными студентами. Подробности на <a href='https://www.spbstu.ru/applicants/admission-of-foreign-citizens/'>странице</a>.",
                                                                replyMarkup: Keyboards.User.Enrollee.Admission.ForeignStudents.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "выбратьпроф": 
                                {
                                    switch (partsQuery[2])
                                    {
                                        case "выбратьпроф":
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
                                                            using (ApplicationContext db = new ApplicationContext())
                                                            {
                                                                long chatsUserID = callbackQuery.Message.Chat.Id;
                                                                await botClient.DeleteMessageAsync(
                                                                    chatId: chatsUserID,
                                                                    messageId: db.Users.Find(chatsUserID).messageMenuID
                                                                    );

                                                                using (FileStream stream = System.IO.File.OpenRead("D:/Телеграм бот/SPbPUBOT/SPbPUBOT/Файлы/Obrazovatelnye_programmy_bakalavriata_i_spetsialiteta.pdf"))
                                                                {
                                                                    InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, "Образовательная программа бакалавриата и специалитета.pdf");
                                                                    await botClient.SendDocumentAsync(callbackQuery.Message.Chat.Id, inputOnlineFile);
                                                                }

                                                                var messageID = await botClient.SendTextMessageAsync(
                                                                    chatId: callbackQuery.Message.Chat.Id,
                                                                    text: "Выберите удобное для вас представление",
                                                                    replyMarkup: Keyboards.User.Enrollee.ChooseProfession.undergraduateKeyboard
                                                                    );
                                                                db.Users.Find(chatsUserID).messageMenuID = messageID.MessageId;
                                                                db.SaveChanges();
                                                            }
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
                                                            using (ApplicationContext db = new ApplicationContext())
                                                            {
                                                                long chatsUserID = callbackQuery.Message.Chat.Id;
                                                                await botClient.DeleteMessageAsync(
                                                                    chatId: chatsUserID,
                                                                    messageId: db.Users.Find(chatsUserID).messageMenuID
                                                                    );

                                                                using (FileStream stream = System.IO.File.OpenRead("D:/Телеграм бот/SPbPUBOT/SPbPUBOT/Файлы/magistr2022.pdf"))
                                                                {
                                                                    InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, "Образовательная программа магистра.pdf");
                                                                    await botClient.SendDocumentAsync(callbackQuery.Message.Chat.Id, inputOnlineFile);
                                                                }

                                                                var messageID = await botClient.SendTextMessageAsync(
                                                                    chatId: callbackQuery.Message.Chat.Id,
                                                                    text: "Выберите удобное для вас представление",
                                                                    replyMarkup: Keyboards.User.Enrollee.ChooseProfession.undergraduateKeyboard
                                                                    );
                                                                db.Users.Find(chatsUserID).messageMenuID = messageID.MessageId;
                                                                db.SaveChanges();
                                                            }
                                                        }
                                                        break;
                                                }
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
                                        case "назад":
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
                                                    replyMarkup: Keyboards.User.Enrollee.AboutUniversity.backKeyboard,
                                                    parseMode: ParseMode.Html
                                                    );
                                            }
                                            break;
                                        case "презентация":
                                            {
                                                using (ApplicationContext db = new ApplicationContext())
                                                {
                                                    long chatsUserID = callbackQuery.Message.Chat.Id;
                                                    await botClient.DeleteMessageAsync(
                                                        chatId: chatsUserID,
                                                        messageId: db.Users.Find(chatsUserID).messageMenuID
                                                        );

                                                    using (FileStream stream = System.IO.File.OpenRead("D:/Телеграм бот/SPbPUBOT/SPbPUBOT/Файлы/Политех.Нам нравится.pdf"))
                                                    {
                                                        InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, "Про Политех.pdf");
                                                        await botClient.SendDocumentAsync(callbackQuery.Message.Chat.Id, inputOnlineFile);
                                                    }

                                                    var messageID = await botClient.SendTextMessageAsync(
                                                        chatId: callbackQuery.Message.Chat.Id,
                                                        text: "Давай знакомиться",
                                                        replyMarkup: Keyboards.User.Enrollee.AboutUniversity.aboutUniversityKeyboard
                                                        );
                                                    db.Users.Find(chatsUserID).messageMenuID = messageID.MessageId;
                                                    db.SaveChanges();
                                                }
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
                                        case "поч":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "поч":
                                                    case "назад":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Сейчас расскажем:",
                                                                replyMarkup: Keyboards.User.Enrollee.AboutUniversity.WhyPolytech.whyPolytechKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "цифры":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Политехнический университет — это:\n" +
                                                                "более 30 тысяч студентов\n" +
                                                                "территория кампуса 120 га\n" +
                                                                "18 корпусов общежитий\n" +
                                                                "70 направлений обучения\n" +
                                                                "12 институтов\n" +
                                                                "123 - летняя история\n" +
                                                                "более 300 партнеров из числа промышленных компаний и вузов\n" +
                                                                "около 50 студенческих объединений",
                                                                replyMarkup: Keyboards.User.Enrollee.AboutUniversity.WhyPolytech.backKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "полет":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Пролететь по территории Политеха и увидеть кампус можно прямо сейчас. Лови <a href='https://youtu.be/6D_iskPXBno'>видео</a>!",
                                                                replyMarkup: Keyboards.User.Enrollee.AboutUniversity.WhyPolytech.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "ресурсы":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "— <a href='https://www.spbstu.ru/abit/bachelor/'>сайт СПбПУ с правилами поступления</a>\n" +
                                                                "— <a href='https://vk.com/polytech_petra'>главная группа Политеха</a>\n" +
                                                                "— <a href='https://vk.com/abit_spbstu'>группа абитуриентов Политеха</a>\n" +
                                                                "— <a href='https://zen.yandex.ru/pokolenie'>блог Политеха на Яндексе.Дзене</a>\n" +
                                                                "— <a href='https://t.me/polytech_petra'>телеграм - канал для политехников</a>",
                                                                replyMarkup: Keyboards.User.Enrollee.AboutUniversity.WhyPolytech.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "экскурс":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Посетить наш вуз можно не только очно, но и дистанционно - с помощью <a href='https://vt.spbstu.ru/gz/'>виртуального тура</a> по Главному учебному корпусу Политеха!",
                                                                replyMarkup: Keyboards.User.Enrollee.AboutUniversity.WhyPolytech.backKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "курсы":
                                {
                                    switch (partsQuery[2])
                                    {
                                        case "курсы":
                                        case "назад":
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
                                                switch (partsQuery[3])
                                                {
                                                    case "пк":
                                                    case "назад":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Курсы какой категории вас интересуют?",
                                                                replyMarkup: Keyboards.User.Enrollee.EventsCourse.TrainingCourses.trainingCoursesKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "1-9":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Для данной категории в Политехническом университете существует несколько различных подразделений",
                                                                replyMarkup: Keyboards.User.Enrollee.EventsCourse.TrainingCourses.for1to9Keyboard
                                                                );
                                                        }
                                                        break;
                                                    case "10":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Курсы для 10 класса проводятся по следующим предметам в любой комбинации:\n" +
                                                                "<b>Математика</b>\n" +
                                                                "<b>Информатика</b>\n" +
                                                                "<b>Физика</b>\n" +
                                                                "Продолжительность - 7 месяцев, октябрь - май.\n" +
                                                                "Время проведения занятий с 18:00 до 19 - 30 по будним дням, 2 академических часа по 1 предмету.\n" +
                                                                "Подробнее о курсе и запись при переходе по кнопке",
                                                                replyMarkup: Keyboards.User.Enrollee.EventsCourse.TrainingCourses.for10CollegeKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "11":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "11":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Какие предметы вы будете сдавать?",
                                                                            replyMarkup: Keyboards.User.Enrollee.EventsCourse.TrainingCourses.for11Keyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "осн":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Курсы для 11 класса проводятся по следующим предметам в любой комбинации:\n" +
                                                                            "<b>Математика</b>\n" +
                                                                            "<b>Информатика</b>\n" +
                                                                            "<b>Физика</b>\n" +
                                                                            "<b>Химия</b>\n" +
                                                                            "<b>Обществознание</b>\n" +
                                                                            "<b>Биология</b>\n" +
                                                                            "<b>История</b>\n" +
                                                                            "<b>Русский язык</b>\n" +
                                                                            "Продолжительность - 8 месяцев, с 13 сентября по 31 мая(русский язык: январь - февраль)\n" +
                                                                            "Форма проведения занятий - очно - заочная(вечерние) или дистанционная на выбор\n" +
                                                                            "Время проведения занятий с 18:00 до 21:15 по будним дням, 4 академических часа в неделю по каждому предмету\n" +
                                                                            "Подробнее о курсе и запись при переходе по кнопке👇",
                                                                            replyMarkup: Keyboards.User.Enrollee.EventsCourse.TrainingCourses.entryFor11Keyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "иняз":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Подробная информация о курсах по иностранным языкам и запись по кнопке ниже👇",
                                                                            replyMarkup: Keyboards.User.Enrollee.EventsCourse.TrainingCourses.entryForeighLangKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "ржк":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Курсы для поступающих на направление 54.03.01 Дизайн.\n" +
                                                                            "Продолжительность - 8 месяцев, с октября по май.\n" +
                                                                            "Время проведения занятий с 18:00 до 21:00 по будним дням, один раз в неделю по 4 академических часа.\n" +
                                                                            "Подробнее о курсах и запись👇",
                                                                            replyMarkup: Keyboards.User.Enrollee.EventsCourse.TrainingCourses.entryTrainCoursesKeyboard
                                                                            );
                                                                    }
                                                                    break;

                                                            }
                                                        }
                                                        break;
                                                    case "колледж":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Подготовительные курсы для абитуриентов, имеющих профессиональное образование и планирующих поступать по результатам вступительных испытаний, проводимых СПбПУ самостоятельно, позволяют ознакомиться со структурой теста и основными требованиями университета к ответам. Занятия ведут разработчики заданий.\n" +
                                                                "Продолжительность - 3 недели, с 21 июня по 10 июля, ежедневно(кроме воскресенья).\n" +
                                                                "Время проведения занятий с 18:00 до 21:15(суббота с 10:00 до 13:15).",
                                                                replyMarkup: Keyboards.User.Enrollee.EventsCourse.TrainingCourses.for10CollegeKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "мага":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Курс разработан специально для выпускников бакалавриата и специалитета, желающих поступить в Магистратуру на направление 08.04.01 Строительство\n" +
                                                                "Продолжительность - 1 месяц\n" +
                                                                "Время записи: май – июнь\n" +
                                                                "Период проведения: июнь – июль\n" +
                                                                "Время проведения занятий с 18:30 до 21:00 по будним дням",
                                                                replyMarkup: Keyboards.User.Enrollee.EventsCourse.TrainingCourses.for10CollegeKeyboard
                                                                );
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case "ом":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "ом":
                                                    case "назад":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Мы активно приглашаем учащихся участвовать в образовательных интенсивах.\n" +
                                                                "Такие интенсивы - возможность улучшить свои знания в одном из любимых предметов, познакомиться с единомышленниками, прочувствовать атмосферу университета, пообщаться с университетскими преподавателями, получить памятные подарки - а в случае победы, получить дополнительные баллы при поступлении!",
                                                                replyMarkup: Keyboards.User.Enrollee.EventsCourse.EducationalPrograms.educationalProgramsKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "илп":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Инженерная лига Политеха — образовательный интенсив для школьников 9-11 классов, которые интересуются физикой, математикой и информатикой. Одна из основных целей мероприятия — полное погружение в профессиональную сферу. Открытые лекции и презентации, мастер-классы и воркшопы — это возможность для участников Инженерной лиги получить понимание отрасли, пообщаться с преподавателями и специалистами профессиональной сферы, а также найти команду единомышленников!",
                                                                replyMarkup: Keyboards.User.Enrollee.EventsCourse.EducationalPrograms.engineeringLeagueKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "лш":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Летняя школа «Твой город-цифровой» - это образовательный интенсив, в рамках которого учащиеся 9-10 классов пробуют себя в качестве специалистов будущего, знакомятся с новыми разработками и технологиями Политехнического университета, участвуют в экскурсиях на ведущие предприятия и компании города, развивают свои компетенции через научно-инженерную и проектную деятельность. Немаловажное значение на Летней школе уделяется развитию soft-skills и профориентационной деятельности.",
                                                                replyMarkup: Keyboards.User.Enrollee.EventsCourse.EducationalPrograms.summerSchoolKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "polycase":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Институт машиностроения, материалов и транспорта СПбПУ является постоянным организатором кейс-чемпионате «PolyCase 2021» для учащихся 9-11 классов.\n" +
                                                                "PolyCase — уникальная возможность попробовать свои силы в решении реальных производственных задач, которые специально адаптированны для учеников среднего и средне - профессионального образования, а также познакомиться с одним из самых передовых университетов страны и получить ценные призы.",
                                                                replyMarkup: Keyboards.User.Enrollee.EventsCourse.EducationalPrograms.polycaseKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "двп":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "«Фестиваль науки – Дорога в Политех» - это школьная научно-практическая конференция для учащихся 8-11 классов. На фестивале ты сможешь представить свою работу на судейство преподавателям Политехнического университета и реализовать себя как ученый.\n" +
                                                                "На фестивале ты можешь представить свою работу в одно из следующих категорий:\n" +
                                                                "Биология и медицина\n" +
                                                                "Физика и медицинская физика\n" +
                                                                "Химия и материаловедение",
                                                                replyMarkup: Keyboards.User.Enrollee.EventsCourse.EducationalPrograms.polycaseKeyboard
                                                                );
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case "олимп":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "олимп":
                                                    case "назад":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Выберите по какому признаку будут разделены олимпиады",
                                                                replyMarkup: Keyboards.User.Enrollee.EventsCourse.Olympiad.olympiadKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "пред":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "пред":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Какой предмет вас интересует?",
                                                                            replyMarkup: Keyboards.User.Enrollee.EventsCourse.Olympiad.subjectKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "физ":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Выбирай ниже",
                                                                            replyMarkup: Keyboards.User.Enrollee.EventsCourse.Olympiad.Physics.physicsKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "мат":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Выбирай ниже",
                                                                            replyMarkup: Keyboards.User.Enrollee.EventsCourse.Olympiad.Mathematics.mathematicsKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "инф":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Выбирай ниже",
                                                                            replyMarkup: Keyboards.User.Enrollee.EventsCourse.Olympiad.Informatics.informaticsKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "хим":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Выбирай ниже",
                                                                            replyMarkup: Keyboards.User.Enrollee.EventsCourse.Olympiad.Chemistry.chemistryKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "гум":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Всероссийская толстовская олимпиада школьников\n" +
                                                                            "Олимпиада носит имя Л.Н.Толстого – выдающегося писателя и мыслителя, художественные произведения и социально - политические работы которого составляют культурное наследие России.Название олимпиады обусловлено тем, что Л.Н.Толстой был не только великим писателем, но и философом, общественным деятелем, педагогом, создавшим свою систему образования и воспитания.\n" +
                                                                            "К участию в олимпиаду приглашаются учащиеся 10 - 11 классов, обучающиеся по образовательным программам основного общего, среднего общего и среднего - профессионального образования.\n" +
                                                                            "Олимпиада проводится по предметам: обществознание, история и литература.\n" +
                                                                            "Олимпиада проводится в два этапа: отборочный и заключительный тур.Отборочный тур проводится дистанционно на сайте олимпиады с ноября по декабрь.Заключительный тур проводится очно в феврале - марте на территории СПбПУ и других площадках по всей России.\n" +
                                                                            "Для участия в олимпиаде необходимо зарегистрироваться на сайте олимпиады и пройти дистанционный отборочный тур.",
                                                                            replyMarkup: Keyboards.User.Enrollee.EventsCourse.Olympiad.Humanitarian.humanitarianKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "назв":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "назв":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Какая категория олимпиад вас интересует?\n\n" +
                                                                            "P.S.\n" +
                                                                            "<a href='https://rsr-olymp.ru/about'>РСОШ</a> - олимпиады Российского совета олимпиад школьников\n" +
                                                                            "<a href='http://www.anichkov.ru/page/olimp/'>ВсОШ</a> - Всероссийская олимпиада школьников",
                                                                            replyMarkup: Keyboards.User.Enrollee.EventsCourse.Olympiad.titleKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "рсош":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Выбирай ниже",
                                                                            replyMarkup: Keyboards.User.Enrollee.EventsCourse.Olympiad.rsochKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case "я олимп":
                                                        {
                                                            switch (partsQuery[4])
                                                            {
                                                                case "я олимп":
                                                                case "назад":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Грацуем!",
                                                                            replyMarkup: Keyboards.User.Enrollee.EventsCourse.Olympiad.olympiadManKeyboard
                                                                            );
                                                                    }
                                                                    break;
                                                                case "бви":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "У победителей и призеров олимпиад есть особое право поступления без вступительных испытаний. О том, как им воспользоваться, узнай <a href='https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/olympics/'>по ссылке</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.EventsCourse.Olympiad.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                case "соотв":
                                                                    {
                                                                        await botClient.EditMessageTextAsync(
                                                                            chatId: callbackQuery.Message.Chat.Id,
                                                                            messageId: callbackQuery.Message.MessageId,
                                                                            text: "Соответствие предметов олимпиад и направлений СПбПУ можно проверить в <a href='https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/olympics/the-line-profile-all-russian-olympiad-/'>этом положении</a>.",
                                                                            replyMarkup: Keyboards.User.Enrollee.EventsCourse.Olympiad.backKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;

                                                            }
                                                        }
                                                        break;
                                                }
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
                                        case "поступ":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "поступ":
                                                    case "назад":
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
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.Admission.admissionKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "кален":
                                                        {
                                                            await botClient.EditMessageReplyMarkupAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.Admission.calendarKeyboard
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
                                                    case "назад":
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
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.Documents.documentsKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    case "кален":
                                                        {
                                                            await botClient.EditMessageReplyMarkupAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.Documents.calendarKeyboard
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
                                                    case "назад":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Здесь есть все, что вы хотели знать о вступительных испытаниях:",
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.EntranceTests.entranceTestsKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "лица":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Нужно ли и можно ли тебе сдавать вступительные испытания Политеха?\nПроверь на сайте, относишься ли ты <a href='https://www.spbstu.ru/abit/bachelor/entrance-test/allowed-pass-entrance-test/'>к этим категориям</a>.",
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.EntranceTests.backKeyboard,
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
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.EntranceTests.backKeyboard,
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
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.EntranceTests.backKeyboard,
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
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.EntranceTests.backKeyboard,
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
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.EntranceTests.backKeyboard,
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
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.EntranceTests.backKeyboard,
                                                                parseMode: ParseMode.Html
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
                                                        replyMarkup: Keyboards.User.Enrollee.Questions.backKeyboard,
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
                                                        replyMarkup: Keyboards.User.Enrollee.Questions.backKeyboard,
                                                        parseMode: ParseMode.Html
                                                        );
                                            }
                                            break;
                                        case "общежития":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "общежития":
                                                    case "назад":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Расскажем все про общежития:",
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.Hostel.hostelKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "репортаж":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Специально для наших абитуриентов мы съездили в один из корпусов общежитий СПбПУ и показали, как живут студенты, а также побеседовали с директором Студенческого городка. Включайте — и вы сразу получите ответы на 99% ваших вопросов об общежитии!\n vk.cc/c9r7IY",
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.Hostel.backKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "получу ли я?":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "В первую очередь заселяют студентов очного отделения, обучающихся на бюджете. \nС порядком распределения мест можно ознакомиться <a href='https://www.spbstu.ru/abit/events/poryadok-ocherednosti-predostavleniya-mest-v-obshchezhitiyakh-studgorodka-spbpu/'>здесь</a>",
                                                                replyMarkup: Keyboards.User.Enrollee.Questions.Hostel.backKeyboard,
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
                                                                            replyMarkup: Keyboards.User.Enrollee.Questions.Hostel.hostelMapKeyboard,
                                                                            parseMode: ParseMode.Html
                                                                            );
                                                                    }
                                                                    break;
                                                                default:
                                                                    {
                                                                        using(ApplicationContext db = new ApplicationContext())
                                                                        {
                                                                            long chatsUserID = callbackQuery.Message.Chat.Id;
                                                                            await botClient.DeleteMessageAsync(
                                                                                chatId: chatsUserID,
                                                                                messageId: db.Users.Find(chatsUserID).messageMenuID
                                                                                );
                                                                            await botClient.SendTextMessageAsync(
                                                                                chatId: chatsUserID,
                                                                                text: $"Общежитие {partsQuery[4]}"
                                                                                );
                                                                            await botClient.SendLocationAsync(
                                                                                chatId: chatsUserID,
                                                                                latitude: Lists.hostelList[partsQuery[4]].Latitude,
                                                                                longitude: Lists.hostelList[partsQuery[4]].Longitude
                                                                                );
                                                                            var messageID = await botClient.SendTextMessageAsync(
                                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                                text: "Выберите нужное общежитие или откройте <a href='https://www.spbstu.ru/campus-map/'>карту кампуса</a>",
                                                                                replyMarkup: Keyboards.User.Enrollee.Questions.Hostel.hostelMapKeyboard,
                                                                                parseMode: ParseMode.Html
                                                                                );
                                                                            db.Users.Find(chatsUserID).messageMenuID = messageID.MessageId;
                                                                            db.SaveChanges();
                                                                        }
                                                                    }
                                                                    break;
                                                            }
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
                                                            using (ApplicationContext db = new ApplicationContext())
                                                            {
                                                                long chatsUserID = callbackQuery.Message.Chat.Id;
                                                                await botClient.DeleteMessageAsync(
                                                                    chatId: chatsUserID,
                                                                    messageId: db.Users.Find(chatsUserID).messageMenuID
                                                                    );
                                                                if (db.UserAssistance.FirstOrDefault(k => k.UserID == chatsUserID) == null)
                                                                {
                                                                    db.UserAssistance.Add(new UserAssistance()
                                                                    {
                                                                        UserID = chatsUserID,
                                                                        Username = callbackQuery.From.Username
                                                                    });

                                                                    await botClient.SendTextMessageAsync(
                                                                        chatId: chatsUserID,
                                                                        text: "Вы добавлены в очередь ожиданий, к вам подключится первый освободившийся оператора"
                                                                        );
                                                                }
                                                                else
                                                                {
                                                                    await botClient.SendTextMessageAsync(
                                                                        chatId: chatsUserID,
                                                                        text: "Вы уже добавлены в очередь ожиданий, к вам подключится первый освободившийся оператора"
                                                                        );
                                                                }
                                                                var messageID = await botClient.SendTextMessageAsync(
                                                                    chatId: callbackQuery.Message.Chat.Id,
                                                                    text: "Для того, чтобы начать диалог с представителем Политеха, нажмите кнопку ниже.\n\n" +
                                                                    "А ещё вы можете связаться с Приемной комиссией Политеха по телефону\n" +
                                                                    "8(812) 775 - 05 - 30 — для звонков из Санкт - Петербурга\n" +
                                                                    "8(800) 707 - 18 - 99 — для звонков из любого региона РФ(звонок бесплатный)\n\n" +
                                                                    "Или написать на почту abitur@spbstu.ru.",
                                                                    replyMarkup: Keyboards.User.callOperatorKeyboard
                                                                    );
                                                                db.Users.Find(chatsUserID).messageMenuID = messageID.MessageId;
                                                                db.SaveChanges();
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case "студент":
                    {
                        switch (partsQuery[1])
                        {
                            case "студент":
                            case "назад":
                                {
                                    await botClient.EditMessageTextAsync(
                                       chatId: callbackQuery.Message.Chat.Id,
                                       messageId: callbackQuery.Message.MessageId,
                                       text: "Выбери одну из категорий, чтобы узнать больше",
                                       replyMarkup: Keyboards.User.Student.startKeyboard
                                       );
                                }
                                break;
                            case "чзв":
                                {
                                    switch (partsQuery[2])
                                    {
                                        case "чзв":
                                        case "назад":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                   chatId: callbackQuery.Message.Chat.Id,
                                                   messageId: callbackQuery.Message.MessageId,
                                                   text: "Выберите интересующую категорию",
                                                   replyMarkup: Keyboards.User.Student.Questions.basicQuestionsKeyboard
                                                   );
                                            }
                                            break;
                                        case "справки":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                   chatId: callbackQuery.Message.Chat.Id,
                                                   messageId: callbackQuery.Message.MessageId,
                                                   text: "В настоящее время в 1-ом учебном корпусе и в Главном учебном корпусе функционируют терминалы (электронные киоски), расположенные в вестибюлях первого этажа зданий.\n" +
                                                   "Посредством указанных терминалов студенты могут самостоятельно оформить и получить на руки ряд справок:\n" +
                                                   "Справки студентам по месту требования\n" +
                                                   "•	по месту работы родителей;\n" +
                                                   "•	по месту жительства иногородним студентам – для предоставления льгот по оплате ЖКХ;\n" +
                                                   "•	для предоставления по месту учебы по программе второго высшего образования, или учебы в другой образовательной организации.\n" +
                                                   "Справки студентам для предоставления в органы социальной защиты населения(для оформления государственной социальной стипендии).\n" +
                                                   "Для оформления справок через терминал(электронный киоск) студентам необходимо осуществить вход в меню системы посредством своего действующего электронного пропуска.\n" +
                                                   "В случае возникновения проблем при самостоятельном оформлении и получении вышеперечисленных видов справок через терминалы, студенты могут обратиться за получением справок в Сектор учета студентов – Политехническая ул., д. 29, 1 - й учебный корпус, 3 - й этаж, комн. 361.\n" +
                                                   "Справки иногородним студентам, находящимся на дистанционном обучении, можно заказать по адресу электронной почты: stud.person.head @spbstu.ru.При заказе справки с корпоративной почты СПбПУ(домен @edu.spbstu.ru или @spbstu.ru) есть возможность получить справку в электронном виде.Оригинал справки будет отправлен сотрудниками университета почтой России(полный адрес и контактную информацию необходимо указать при заказе справки).",
                                                   replyMarkup: Keyboards.User.Student.Questions.referenceKeyboard
                                                   );
                                            }
                                            break;
                                        case "перех":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                   chatId: callbackQuery.Message.Chat.Id,
                                                   messageId: callbackQuery.Message.MessageId,
                                                   text: "О переводе в СПбПУ студентов из других вузов\n\n" +
                                                   "Обращаем внимание на то, что переводы из других образовательных организаций для обучения на первом курсе(году обучения) не осуществляются.Прием на первый курс(год обучения) может производиться только через приемную комиссию.\n\n" +
                                                   "С алгоритмом можно ознакомиться ниже",
                                                   replyMarkup: Keyboards.User.Student.Questions.transitionKeyboard
                                                   );
                                            }
                                            break;
                                        case "вуц":
                                            {
                                            switch (partsQuery[3])
                                            {
                                                    case "вуц":
                                                    case "назад":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                               chatId: callbackQuery.Message.Chat.Id,
                                                               messageId: callbackQuery.Message.MessageId,
                                                               text: "Приём в ВУЦ с 2017 года осуществляется только для подготовки по программам обучения офицеров запаса студентов технического профиля подготовки.\n\n" +
                                                               "В наборе участвуют студенты 2 курса, срок обучения - 5 семестров (4,5,6,7,8 - семестры обучения), после окончания подготовки в ВУЦ, студенты проходят стажировку в одной из воинских частей. Срок стажировки - месяц. Набор студентов, обучающихся (поступающих) в магистратуре - на данный момент - <b>не осуществляется</b>.\n\n" +
                                                               "Прием студентов с других ВУЗов, не проводится, так как, в ВУЦ на данный момент не организовано обучение по программам солдат, сержантов запаса.",
                                                               replyMarkup: Keyboards.User.Student.Questions.MilitaryDepartment.militaryDepartmentKeyboard,
                                                               parseMode: ParseMode.Html
                                                               );
                                                        }
                                                        break;
                                                    case "набкаф":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                               chatId: callbackQuery.Message.Chat.Id,
                                                               messageId: callbackQuery.Message.MessageId,
                                                               text: "<b>Кафедра ВКС</b>\n" +
                                                               "Физико-механический институт\n" +
                                                               "Институт биомедицинских систем и технологий\n" +
                                                               "Институт компьютерных наук и технологий\n" +
                                                               "Инженерно-строительный институт\n" +
                                                               "Институт энергетики\n" +
                                                               "Институт ядерной энергетики (филиал г.Сосновый Бор)\n\n" +
                                                               "<b>Кафедра связи</b>\n" +
                                                               "Институт электроники и телекоммуникаций\n" +
                                                               "Институт энергетики\n" +
                                                               "Институт кибербезопасности и защиты информации\n" +
                                                               "Институт компьютерных наук и технологий (09.03.01;  09.03.03)\n\n" +
                                                               "<b>Цикл ОВП</b>\n" +
                                                               "Институт машиностроения, материалов и транспорта",
                                                               replyMarkup: Keyboards.User.Student.Questions.MilitaryDepartment.backKeyboard,
                                                               parseMode: ParseMode.Html
                                                               );
                                                        }
                                                        break;
                                                    case "предотбор":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                               chatId: callbackQuery.Message.Chat.Id,
                                                               messageId: callbackQuery.Message.MessageId,
                                                               text: "Предварительный отбор\n\n" +
                                                               "Написать заявление и принести его на кафедру ВУЦ (ВКС, связи, цикл ОВП) вместе с 2 фотографиями 3х4 черно-белые(можно цветные) без уголка\n" +
                                                               "Заявление на поступление(Строку - наименование военно - учетной специальности - оставить свободной).\n" +
                                                               "Получить направление на Военно - врачебную комиссию(ВВК).\n" +
                                                               "Распечатать медицинскую карту(на 1 - м листе А - 4 с двух сторон) и пройти ВВК в военном комиссариате по месту приписки.\n" +
                                                               "Карта медицинского освидетельствования.\n" +
                                                               "Сдать ответственному на кафедре ВУЦ результаты медицинского освидетельствования(+копию) и результаты психологического и психофизиологического обследования\n",
                                                               replyMarkup: Keyboards.User.Student.Questions.MilitaryDepartment.backKeyboard,
                                                               parseMode: ParseMode.Html
                                                               );
                                                        }
                                                        break;
                                                    case "оснотбор":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                               chatId: callbackQuery.Message.Chat.Id,
                                                               messageId: callbackQuery.Message.MessageId,
                                                               text: "1. Сдача нормативов по физической подготовке (график сдачи по институтам будет указан сайте ВУЦ)\n" +
                                                               "2. Поступающие в ВУЦ студенты оформляют допуск и договор об обучении (при зачислении)." +
                                                               "3. Проводится работа конкурсной комиссии, по результатам которой составляется приказ о зачислении.\n\n" +
                                                               "Далее доводится информация о зачисленных для прохождения программы обучения по военной подготовке студентах и организации начала занятий.\n\n" +
                                                               "Специальная форма военной одежды приобретается студентами самостоятельно (Основание: Приложение №2 к Положению о ВУЦ)\n" +
                                                               "Нашивки, шевроны, эмблемы приобретаются студентами централизованно, после поступления в ВУЦ в начале обучения.",
                                                               replyMarkup: Keyboards.User.Student.Questions.MilitaryDepartment.backKeyboard,
                                                               parseMode: ParseMode.Html
                                                               );
                                                        }
                                                        break;
                                                    case "инфа":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                               chatId: callbackQuery.Message.Chat.Id,
                                                               messageId: callbackQuery.Message.MessageId,
                                                               text: "Справочная информация:\n\n" +
                                                               "Телефон кафедры ВКС\n" +
                                                               "+7 (812) 552-87-29\n" +
                                                               "Телефон кафедры связи\n" +
                                                               "+7 (812) 552-64-93\n" +
                                                               "Телефон  цикла ОВП\n" +
                                                               "+7 (812) 552-66-15",
                                                               replyMarkup: Keyboards.User.Student.Questions.MilitaryDepartment.backKeyboard,
                                                               parseMode: ParseMode.Html
                                                               );
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case "иностранцу":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                   chatId: callbackQuery.Message.Chat.Id,
                                                   messageId: callbackQuery.Message.MessageId,
                                                   text: "<b>Внимание!</b>\n" +
                                                   "Горячая линия Международных служб с ПН - ВС по всем вопросам\n" +
                                                   "(в т.ч.WhatsApp и Telegram):\n" +
                                                   "+7(921) 864 - 19 - 85\n\n" +
                                                   "<b>АКТУАЛЬНАЯ ИНФОРМАЦИЯ</b>\n" +
                                                   "Уважаемые Студенты!\n" +
                                                   "Просим вас ознакомиться с актуальной информацией по выезду и въезду с/на территорию Российской Федерации(РФ).\n\n" +
                                                   "<b>Перед выездом(Departure)</b> с территории РФ – по личным обстоятельствам, по программе академической мобильности, на каникулы - Вам необходимо проинформировать о Вашем намерении покинуть территорию РФ Паспортно - визовый отдел СПбПУ по электронной почте pvo@spbstu.ru\n" +
                                                   "<b>Перед въездом(Arrival)</b> Все иностранные граждане, въезжающие на территорию РФ с целью въезда «учеба», должны быть занесены на сайт <b>ГОСУСЛУГ</b>.Для этого Вам необходимо <b>перед приездом в Россию не менее чем за 5 рабочих дней</b> уведомить университет о дате въезда в Российскую Федерацию по электронной почте klyusova_ma@spbstu.ru",
                                                   replyMarkup: Keyboards.User.Student.Questions.backKeyboard,
                                                   parseMode: ParseMode.Html
                                                   );
                                            }
                                            break;
                                        case "стип":
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
                                                    replyMarkup: Keyboards.User.Student.Questions.grantKeyboard
                                                    );
                                            }
                                            break;
                                        case "соцоб":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                   chatId: callbackQuery.Message.Chat.Id,
                                                   messageId: callbackQuery.Message.MessageId,
                                                   text: "<b>Социальное обеспечение</b>\n\n" +
                                                   "Социальное обеспечение обучающихся СПбПУ Петра Великого – это система мер, устанавливаемая законами и иными нормативными правовыми актами и направленная на создание и поддержание достойных условий жизнедеятельности обучающихся.\n\n" +
                                                   "Основными направлениями социального обеспечения обучающихся являются: ",
                                                   replyMarkup: Keyboards.User.Student.Questions.socialSecurityKeyboard,
                                                   parseMode: ParseMode.Html
                                                   );
                                            }
                                            break;
                                        case "медобсл":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                   chatId: callbackQuery.Message.Chat.Id,
                                                   messageId: callbackQuery.Message.MessageId,
                                                   text: "<b>Медицинское обслуживание</b>\n\n" +
                                                   "1.Поликлиника №76\n" +
                                                   "2.Центр охраны репродуктивного здоровья\n" +
                                                   "3.Психологическая Служба",
                                                   replyMarkup: Keyboards.User.Student.Questions.medicalServiceKeyboard,
                                                   parseMode: ParseMode.Html
                                                   );
                                            }
                                            break;
                                        case "опер":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "опер":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                                chatId: callbackQuery.Message.Chat.Id,
                                                                messageId: callbackQuery.Message.MessageId,
                                                                text: "Для того, чтобы начать диалог с представителем Политеха, нажмите кнопку ниже.\n\n" +
                                                                "А ещё вы можете связаться с Приемной комиссией Политеха по телефону\n" +
                                                                "8(812) 775 - 05 - 30 — для звонков из Санкт - Петербурга\n" +
                                                                "8(800) 707 - 18 - 99 — для звонков из любого региона РФ(звонок бесплатный)\n\n" +
                                                                "Или написать на почту abitur@spbstu.ru.",
                                                                replyMarkup: Keyboards.User.Student.Questions.callOperatorKeyboard
                                                                );
                                                        }
                                                        break;
                                                    case "вызов":
                                                        {
                                                            using (ApplicationContext db = new ApplicationContext())
                                                            {
                                                                long chatsUserID = callbackQuery.Message.Chat.Id;
                                                                await botClient.DeleteMessageAsync(
                                                                    chatId: chatsUserID,
                                                                    messageId: db.Users.Find(chatsUserID).messageMenuID
                                                                    );
                                                                if (db.UserAssistance.FirstOrDefault(k => k.UserID == chatsUserID) == null)
                                                                {
                                                                    db.UserAssistance.Add(new UserAssistance()
                                                                    {
                                                                        UserID = chatsUserID,
                                                                        Username = callbackQuery.From.Username
                                                                    });

                                                                    await botClient.SendTextMessageAsync(
                                                                        chatId: chatsUserID,
                                                                        text: "Вы добавлены в очередь ожиданий, к вам подключится первый освободившийся оператора"
                                                                        );
                                                                }
                                                                else
                                                                {
                                                                    await botClient.SendTextMessageAsync(
                                                                        chatId: chatsUserID,
                                                                        text: "Вы уже добавлены в очередь ожиданий, к вам подключится первый освободившийся оператора"
                                                                        );
                                                                }
                                                                var messageID = await botClient.SendTextMessageAsync(
                                                                    chatId: callbackQuery.Message.Chat.Id,
                                                                    text: "Для того, чтобы начать диалог с представителем Политеха, нажмите кнопку ниже.\n\n" +
                                                                    "А ещё вы можете связаться с Приемной комиссией Политеха по телефону\n" +
                                                                    "8(812) 775 - 05 - 30 — для звонков из Санкт - Петербурга\n" +
                                                                    "8(800) 707 - 18 - 99 — для звонков из любого региона РФ(звонок бесплатный)\n\n" +
                                                                    "Или написать на почту abitur@spbstu.ru.",
                                                                    replyMarkup: Keyboards.User.Student.Questions.callOperatorKeyboard
                                                                    );
                                                                db.Users.Find(chatsUserID).messageMenuID = messageID.MessageId;
                                                                db.SaveChanges();
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "карта":
                                {
                                    switch (partsQuery[2])
                                    {
                                        case "карта":
                                        case "назад":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                    chatId: callbackQuery.Message.Chat.Id,
                                                    messageId: callbackQuery.Message.MessageId,
                                                    text: "Выберите интересующий тип здания ниже или откройте <a href='https://www.spbstu.ru/campus-map/'>карту кампуса</a>",
                                                    replyMarkup: Keyboards.User.Student.Buildings.chooseKeyboard,
                                                    parseMode: ParseMode.Html
                                                    );
                                            }
                                            break;
                                        case "корпуса":
                                            {
                                                switch (partsQuery[3])
                                                {
                                                    case "корпуса":
                                                        {
                                                            await botClient.EditMessageTextAsync(
                                                               chatId: callbackQuery.Message.Chat.Id,
                                                               messageId: callbackQuery.Message.MessageId,
                                                               text: "Выберите нужный корпус или откройте <a href='https://www.spbstu.ru/campus-map/'>карту кампуса</a>",
                                                               replyMarkup: Keyboards.User.Student.Buildings.corpsMapKeyboard,
                                                               parseMode: ParseMode.Html
                                                               );
                                                        }
                                                        break;
                                                    default:
                                                        {
                                                            using (ApplicationContext db = new ApplicationContext())
                                                            {
                                                                long chatsUserID = callbackQuery.Message.Chat.Id;
                                                                await botClient.DeleteMessageAsync(
                                                                    chatId: chatsUserID,
                                                                    messageId: db.Users.Find(chatsUserID).messageMenuID
                                                                    );

                                                                switch (partsQuery[3])
                                                                {
                                                                    case "гз":
                                                                        {
                                                                            await botClient.SendTextMessageAsync(
                                                                                chatId: chatsUserID,
                                                                                text: "Главный учебный корпус"
                                                                                );
                                                                        }
                                                                        break;
                                                                    case "спорт":
                                                                        {
                                                                            await botClient.SendTextMessageAsync(
                                                                                chatId: chatsUserID,
                                                                                text: "Спортивный комплекс"
                                                                                );
                                                                        }
                                                                        break;
                                                                    case "лаб":
                                                                        {
                                                                            await botClient.SendTextMessageAsync(
                                                                                chatId: chatsUserID,
                                                                                text: "Лабораторный корпус"
                                                                                );
                                                                        }
                                                                        break;
                                                                    case "хим":
                                                                        {
                                                                            await botClient.SendTextMessageAsync(
                                                                                chatId: chatsUserID,
                                                                                text: "Химический корпус"
                                                                                );
                                                                        }
                                                                        break;
                                                                    case "башня":
                                                                        {
                                                                            await botClient.SendTextMessageAsync(
                                                                                chatId: chatsUserID,
                                                                                text: "Гидробашня"
                                                                                );
                                                                        }
                                                                        break;
                                                                    case "мех":
                                                                        {
                                                                            await botClient.SendTextMessageAsync(
                                                                                chatId: chatsUserID,
                                                                                text: "Механический корпуc"
                                                                                );
                                                                        }
                                                                        break;
                                                                    case "г1":
                                                                        {
                                                                            await botClient.SendTextMessageAsync(
                                                                                chatId: chatsUserID,
                                                                                text: "Гидрокорпус-1"
                                                                                );
                                                                        }
                                                                        break;
                                                                    case "г2":
                                                                        {
                                                                            await botClient.SendTextMessageAsync(
                                                                                chatId: chatsUserID,
                                                                                text: "Гидрокорпус-2"
                                                                                );
                                                                        }
                                                                        break;
                                                                    default:
                                                                        {
                                                                            await botClient.SendTextMessageAsync(
                                                                                chatId: chatsUserID,
                                                                                text: $"Корпус {partsQuery[3]}"
                                                                                );
                                                                        }
                                                                        break;
                                                                }

                                                                await botClient.SendLocationAsync(
                                                                    chatId: chatsUserID,
                                                                    latitude: Lists.corpsList[partsQuery[3]].Latitude,
                                                                    longitude: Lists.corpsList[partsQuery[3]].Longitude
                                                                    );

                                                                var messageID = await botClient.SendTextMessageAsync(
                                                                    chatId: callbackQuery.Message.Chat.Id,
                                                                    text: "Выберите нужный корпус или откройте <a href='https://www.spbstu.ru/campus-map/'>карту кампуса</a>",
                                                                    replyMarkup: Keyboards.User.Student.Buildings.corpsMapKeyboard,
                                                                    parseMode: ParseMode.Html
                                                                    );
                                                                db.Users.Find(chatsUserID).messageMenuID = messageID.MessageId;
                                                                db.SaveChanges();
                                                            }
                                                        }
                                                        break;
                                                }
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
                                                                text: "Выберите нужное общежитие или откройте <a href='https://www.spbstu.ru/campus-map/'>карту кампуса</a>",
                                                                replyMarkup: Keyboards.User.Student.Buildings.hostelMapKeyboard,
                                                                parseMode: ParseMode.Html
                                                                );
                                                        }
                                                        break;
                                                    default:
                                                        {
                                                            using (ApplicationContext db = new ApplicationContext())
                                                            {
                                                                long chatsUserID = callbackQuery.Message.Chat.Id;
                                                                await botClient.DeleteMessageAsync(
                                                                    chatId: chatsUserID,
                                                                    messageId: db.Users.Find(chatsUserID).messageMenuID
                                                                    );

                                                                await botClient.SendTextMessageAsync(
                                                                    chatId: chatsUserID,
                                                                    text: $"Общежитие {partsQuery[3]}"
                                                                    );
                                                                await botClient.SendLocationAsync(
                                                                    chatId: chatsUserID,
                                                                    latitude: Lists.hostelList[partsQuery[3]].Latitude,
                                                                    longitude: Lists.hostelList[partsQuery[3]].Longitude
                                                                    );

                                                                var messageID = await botClient.SendTextMessageAsync(
                                                                    chatId: callbackQuery.Message.Chat.Id,
                                                                    text: "Выберите нужное общежитие или откройте <a href='https://www.spbstu.ru/campus-map/'>карту кампуса</a>",
                                                                    replyMarkup: Keyboards.User.Student.Buildings.hostelMapKeyboard,
                                                                    parseMode: ParseMode.Html
                                                                    );
                                                                db.Users.Find(chatsUserID).messageMenuID = messageID.MessageId;
                                                                db.SaveChanges();
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "расписание":
                                {
                                    await botClient.EditMessageTextAsync(
                                       chatId: callbackQuery.Message.Chat.Id,
                                       messageId: callbackQuery.Message.MessageId,
                                       text: "Расписание лучшего всего смотреть на сайте или скачать приложение для телефона",
                                       replyMarkup: Keyboards.User.Student.Timetable.timetableKeyboard
                                       );
                                }
                                break;
                            case "ссылки":
                                {
                                    switch (partsQuery[2])
                                    {
                                        case "ссылки":
                                        case "назад":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                   chatId: callbackQuery.Message.Chat.Id,
                                                   messageId: callbackQuery.Message.MessageId,
                                                   text: "Выберите категорию ниже",
                                                   replyMarkup: Keyboards.User.Student.Links.linksKeyboard
                                                   );
                                            }
                                            break;
                                        case "вк":
                                            {
                                                await botClient.EditMessageTextAsync(
                                                   chatId: callbackQuery.Message.Chat.Id,
                                                   messageId: callbackQuery.Message.MessageId,
                                                   text: "Выберите категорию ниже",
                                                   replyMarkup: Keyboards.User.Student.Links.linksKeyboard
                                                   );
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
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
                                            chatId: callbackQuery.From.Id,
                                            text: $"Оператор{callbackQuery.Message.Text.Split("Оператор")[1]} удален"
                                            );

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
                        await botClient.SendTextMessageAsync(
                            chatId: chatID,
                            text: "Выбери кем ты являешься",
                            replyMarkup: Keyboards.User.chooseKeyboard
                            );
                    }
                    break;
            }
        }

        private static async Task ReceivedFromMainOperator(ITelegramBotClient botClient, Message message) // сообщения от главного оператора
        {
            long chatsMainID = message.Chat.Id;
            Operator oper;

            using (ApplicationContext db = new ApplicationContext())
            {
                oper = db.Operators.Find(chatsMainID);
            }

            if (oper.UserID == null)
            {
                switch (message.Text.ToLower())
                {
                    case "/start":
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: chatsMainID,
                                text: "Привет, главный оператор! \nСписок доступных команд только что открылся, будь острожнее и не добавляй кого попало)))",
                                replyMarkup: Keyboards.MainOperator.basicKeyboard
                                );
                        }
                        break;

                    case "добавить нового оператора":
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: chatsMainID,
                                text: "Введите id пользователя в формате:\n" +
                                "'id:id_пользователя'"
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
                                        chatId: chatsMainID,
                                        text: "Нет ни одного неосновного оператора"
                                        );
                                }
                                else
                                {
                                    foreach (var operat in operatorsList)
                                    {
                                        await botClient.SendTextMessageAsync(
                                            chatId: chatsMainID,
                                            text: "ID " + operat.OperatorID + "\n" +
                                            "Оператор " + operat.Username,
                                            replyMarkup: Keyboards.MainOperator.deleteKeyboard
                                            );
                                    }
                                }
                            }
                        }
                        break;

                    case "помочь":
                        {
                            UserAssistance userForHelp;
                            using (ApplicationContext db = new ApplicationContext())
                            {
                                userForHelp = db.UserAssistance.FirstOrDefault(k => k.OperatorID == null);
                                if (userForHelp != null)
                                {
                                    userForHelp.OperatorID = chatsMainID; // меняем у пользователя operatorID на того, кто сейчас готов помочь
                                    db.Operators.Find(chatsMainID).UserID = userForHelp.UserID; // оператору ставим айди пользователя, которому он помогает
                                    db.SaveChanges();

                                    await botClient.SendTextMessageAsync(
                                        chatId: chatsMainID,
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
                                        chatId: chatsMainID,
                                        text: "На данный момент помощь никому не нужна"
                                        );
                                }
                            }
                        }
                        break;

                    default:
                        {
                            if (message.Text.Substring(0, 3) == "id:")
                            {
                                long userID;
                                bool flag = long.TryParse(message.Text.Substring(3), out userID);
                                if (!flag)
                                {
                                    await botClient.SendTextMessageAsync(
                                        chatId: chatsMainID,
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

                                            if (db.UserAssistance.Find(userID) != null)
                                            {
                                                UserAssistance userForHelp = db.UserAssistance.Find(userID);
                                                db.UserAssistance.Remove(userForHelp);
                                            }

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
                                            chatId: chatsMainID,
                                            text: "Оператор добавлен"
                                            );
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(
                                            chatId: chatsMainID,
                                            text: "Данный пользователь уже является оператором"
                                            );
                                    }
                                }
                            }
                            else
                            {
                                await botClient.DeleteMessageAsync(
                                    chatId: chatsMainID,
                                    messageId: message.MessageId
                                    );
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
                                UserAssistance user = db.UserAssistance.Find(oper.UserID);
                                db.UserAssistance.Remove(user);

                                await botClient.SendTextMessageAsync(
                                    chatId: oper.UserID,
                                    text: "Оператор окончил диалог",
                                    replyMarkup: new ReplyKeyboardRemove()
                                    {
                                        Selective = true
                                    }
                                    );

                                await botClient.DeleteMessageAsync(
                                    chatId: oper.UserID,
                                    messageId: db.Users.Find(oper.UserID).messageMenuID
                                    );

                                var messageMenu = await botClient.SendTextMessageAsync(
                                    chatId: oper.UserID,
                                    text: "Выбери кем ты являешься",
                                    replyMarkup: Keyboards.User.chooseKeyboard
                                    );

                                db.Users.Find(oper.UserID).messageMenuID = messageMenu.MessageId;

                                db.Operators.Find(chatsMainID).UserID = null;
                                db.SaveChanges();

                                await botClient.SendTextMessageAsync(
                                    chatId: chatsMainID,
                                    text: "Диалог закончен",
                                    replyMarkup: Keyboards.MainOperator.basicKeyboard
                                    );
                            }
                        }
                        break;
                    default:
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: oper.UserID,
                                text: message.Text
                                );
                        }
                        break;
                }
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
            if (oper.UserID == null)
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
                                db.SaveChanges();
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
                            UserAssistance userForHelp;
                            using (ApplicationContext db = new ApplicationContext())
                            {
                                userForHelp = db.UserAssistance.FirstOrDefault(k => k.OperatorID == null);
                                if (userForHelp != null)
                                {
                                    userForHelp.OperatorID = chatsOperatorID; // меняем у пользователя operatorID на того, кто сейчас готов помочь
                                    db.Operators.Find(chatsOperatorID).UserID = userForHelp.UserID; // оператору ставим айди пользователя, которому он помогает
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
                                UserAssistance user = db.UserAssistance.Find(oper.UserID);
                                db.UserAssistance.Remove(user);

                                await botClient.SendTextMessageAsync(
                                    chatId: oper.UserID,
                                    text: "Оператор окончил диалог",
                                    replyMarkup: new ReplyKeyboardRemove()
                                    {
                                        Selective = true
                                    }
                                    );

                                await botClient.DeleteMessageAsync(
                                    chatId: oper.UserID,
                                    messageId: db.Users.Find(oper.UserID).messageMenuID
                                    );

                                var messageMenu = await botClient.SendTextMessageAsync(
                                    chatId: oper.UserID,
                                    text: "Выбери кем ты являешься",
                                    replyMarkup: Keyboards.User.chooseKeyboard
                                    );

                                db.Users.Find(oper.UserID).messageMenuID = messageMenu.MessageId;

                                db.Operators.Find(chatsOperatorID).UserID = null;
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
                                chatId: oper.UserID,
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
            UserAssistance userInChat;


            await botClient.SendChatActionAsync( // анимация "Печатать", пока получается информация из бд
                    chatId: chatsUserID,
                    chatAction: ChatAction.Typing
                    );

            using (ApplicationContext db = new ApplicationContext())
            {
                userInChat = db.UserAssistance.FirstOrDefault(k => k.UserID == chatsUserID);
                if (!db.Users.Any(k => k.UserID == chatsUserID))
                {
                    user = new User() // добавление
                    {
                        UserID = chatsUserID,
                        Username = message.From.Username,
                        messageMenuID = -1
                    };
                    db.Users.Add(user);
                    db.SaveChanges(); // сохранение изменений
                }
                else
                {
                    user = db.Users.Find(chatsUserID);
                }
            }

            if (userInChat != null && userInChat.OperatorID != null)
            {
                switch (message.Text.ToLower())
                {
                    case "закончить диалог":
                        {
                            using (ApplicationContext db = new ApplicationContext())
                            {
                                db.Operators.Find(userInChat.OperatorID).UserID = null;

                                if(db.Operators.Find(userInChat.OperatorID).isMain == false)
                                {
                                    await botClient.SendTextMessageAsync(
                                        chatId: userInChat.OperatorID,
                                        text: "Пользователь окончил диалог",
                                        replyMarkup: Keyboards.Operator.mainKeyboard
                                        );
                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(
                                        chatId: userInChat.OperatorID,
                                        text: "Пользователь окончил диалог",
                                        replyMarkup: Keyboards.MainOperator.basicKeyboard
                                        );
                                }

                                db.UserAssistance.Remove(userInChat);

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
                                chatId: userInChat.OperatorID,
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
                            using(ApplicationContext db = new ApplicationContext())
                            {
                                if (db.Users.Find(chatsUserID).messageMenuID == -1)
                                {
                                    await botClient.SendTextMessageAsync(
                                       chatId: chatsUserID,
                                       text: "Привет! 🤖\n\n" +
                                       "Я твой чат-бот для связи с Политехническим университетом."
                                       );
                                }
                                else
                                {
                                    await botClient.DeleteMessageAsync(
                                       chatId: chatsUserID,
                                       messageId: db.Users.Find(chatsUserID).messageMenuID
                                       );
                                }
                            }

                            await botClient.DeleteMessageAsync( // удаление сообщения /start
                                chatId: chatsUserID,
                                messageId: message.MessageId
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
