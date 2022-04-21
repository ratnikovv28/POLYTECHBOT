using Telegram.Bot.Types.ReplyMarkups;

namespace SPbPUBOT
{
    public class Keyboards
    {
        public static class MainOperator
        {
            public static ReplyKeyboardMarkup basicKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]{ "Добавить нового оператора" },
            new KeyboardButton[]{ "Список операторов" }
        })
            {
                ResizeKeyboard = true
            };
        }

        public static class Operator
        {

        }

        public static class User
        {
            public static InlineKeyboardMarkup chooseKeyboard = new(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Абитуриент", "абитуриент|абитуриент"),
                        InlineKeyboardButton.WithCallbackData("Студент", "студент|студент"),
                    },
                });
            public static class Student
            {

            }

            public static class Enrollee
            {
                public static InlineKeyboardMarkup basicKeyboard = new(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Порядок поступления", "абитуриент|поступление"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Документы", "абитуриент|документы"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Индивидуальные достижения", "абитуриент|индивид.достижения"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Вступительные испытания", "абитуриент|вступ.исп|вступ.исп"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("После поступления", "абитуриент|после поступления"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Стипендии и гранты для первокурсников", "абитуриент|стип/гранты"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Хочу перевестись в Политех", "абитуриент|перевод в политех"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Узнать про общежития", "абитуриент|общежития|общежития"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Чат с представителем поддержки", "абитуриент|оператор"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Назад", "назад"),
                    },
                });
                public static InlineKeyboardMarkup hostelKeyboard = new(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Посмотреть репортраж из общежития", "абитуриент|общежития|репортаж"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Я получу общежитие?", "абитуриент|общежития|получу ли я?"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Карта общежитий", "абитуриент|общежития|карта|карта"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Назад", "абитуриент|общежития|назад"),
                    },
                });
                public static InlineKeyboardMarkup hostelMapKeyboard = new(
                    new[] 
                    {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("1️⃣", "абитуриент|общежития|карта|1"),
                        InlineKeyboardButton.WithCallbackData("3️⃣", "абитуриент|общежития|карта|3"),
                        InlineKeyboardButton.WithCallbackData("4️⃣", "абитуриент|общежития|карта|4"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("5️⃣", "абитуриент|общежития|карта|5"),
                        InlineKeyboardButton.WithCallbackData("6️⃣", "абитуриент|общежития|карта|6"),
                        InlineKeyboardButton.WithCallbackData("7️⃣", "абитуриент|общежития|карта|7"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("8️⃣", "абитуриент|общежития|карта|8"),
                        InlineKeyboardButton.WithCallbackData("🔟", "абитуриент|общежития|карта|10"),
                        InlineKeyboardButton.WithCallbackData("1️⃣1️⃣", "абитуриент|общежития|карта|11"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("1️⃣2️⃣", "абитуриент|общежития|карта|12"),
                        InlineKeyboardButton.WithCallbackData("1️⃣3️⃣", "абитуриент|общежития|карта|13"),
                        InlineKeyboardButton.WithCallbackData("1️⃣4️⃣", "абитуриент|общежития|карта|14"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("1️⃣5️⃣", "абитуриент|общежития|карта|15"),
                        InlineKeyboardButton.WithCallbackData("1️⃣6️⃣", "абитуриент|общежития|карта|16"),
                        InlineKeyboardButton.WithCallbackData("1️⃣7️⃣", "абитуриент|общежития|карта|17"),
                    }, 
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("1️⃣8️⃣", "абитуриент|общежития|карта|18"),
                        InlineKeyboardButton.WithCallbackData("1️⃣9️⃣", "абитуриент|общежития|карта|19"),
                        InlineKeyboardButton.WithCallbackData("2️⃣0️⃣", "абитуриент|общежития|карта|20"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Назад", "абитуриент|общежития|карта|назад"),
                    },
                 });
                public static InlineKeyboardMarkup achievementsKeyboard = new(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithUrl("Бакалавриат", "https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/individual-achievements/"),
                        InlineKeyboardButton.WithUrl("Магистратура", "https://www.spbstu.ru/abit/master/review-the-regulatory-documents/individual-achievements/"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Назад", "назад"),
                    },
                });
                public static InlineKeyboardMarkup entranceTestsKeyboard = new(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Кому надо сдавать?", "абитуриент|вступ.исп|лица"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Перечень вступительных испытаний", "абитуриент|вступ.исп|перечень"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Расписание вступительных испытаний", "абитуриент|вступ.исп|расписание"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Программы и образцы заданий", "абитуриент|вступ.исп|программы"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("\"Дизайн\" и \"Дизайн архитектурной среды\"", "абитуриент|вступ.исп|дизайн"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Вступительные испытания в 2022 году", "абитуриент|вступ.исп|2022"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Назад", "абитуриент|вступ.исп|назад"),
                    },
                });
            }
        }

        public static InlineKeyboardMarkup backKeyboard = new(
            new[]
            {
                new[]
                {
                     InlineKeyboardButton.WithCallbackData("Назад", "назад"),
                },
            });
    }
}
