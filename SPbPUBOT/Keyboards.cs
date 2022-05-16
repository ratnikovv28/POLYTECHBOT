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
                new KeyboardButton[]{ "Список операторов" },
                new KeyboardButton[]{ "Помочь" }
            })
            {
                ResizeKeyboard = true
            };

            public static InlineKeyboardMarkup deleteKeyboard = new(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Удалить", "глоператор|удалить"),
                    },
            });
        }

        public static class Operator
        {
            public static ReplyKeyboardMarkup mainKeyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[]{ "Помочь" }
            })
            {
                ResizeKeyboard = true
            };

            public static ReplyKeyboardMarkup whileChattingKeyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[]{ "Закончить диалог" }
            })
            {
                ResizeKeyboard = true
            };
        }

        public static class User
        {
            public static InlineKeyboardMarkup chooseKeyboard = new(
            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("🅰️битуриент", "абитур|абитур"),
                    InlineKeyboardButton.WithCallbackData("💲тудент", "студент|студент"),
                },
            });

            public static ReplyKeyboardMarkup whileChattingKeyboard = new ReplyKeyboardMarkup(
            new[]
            {
                new KeyboardButton[]{ "Закончить диалог" }
            })
            {
                ResizeKeyboard = true
            };

            public static class Student
            {
                public static InlineKeyboardMarkup startKeyboard = new(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("🌐Полезные ссылки", "студент|ссылки|ссылки"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("📆Расписание", "студент|расписание"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("🗺Карта Политеха", "студент|карта|карта"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("📝Часто задаваемые вопросы", "студент|чзв|чзв"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Назад", "назад"),
                    },
                });

                public static class Links
                {
                    public static InlineKeyboardMarkup linksKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Личный кабинет", "https://lk.spbstu.ru/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Открытое образование", "https://openedu.ru/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "студент|назад"),
                        },
                    });

                    public static InlineKeyboardMarkup sdoKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("СДО СПбПУ", "https://lms.spbstu.ru/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("СДО ИКНТ", "https://openedu.ru/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("СДО институтов", "студент|карта|карта"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Часто задаваемые вопросы", "студент|чзв|чзв"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "студент|назад"),
                        },
                    });

                    public static InlineKeyboardMarkup vkLinksKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("ИКНТ", "студент|ссылки|вк|икнт"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("ИПМЭиТ", "студент|ссылки|вк|ипмэит"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("ИБСиБ", "студент|ссылки|вк|ибсиб"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("ИММиТ", "студент|ссылки|вк|иммит"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("ИЭиТ", "студент|ссылки|вк|иэит"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("ИСИ", "студент|ссылки|вк|иси"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("ИЭ", "студент|ссылки|вк|иэ"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("ГИ", "студент|ссылки|вк|ги"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("ФизМех", "студент|ссылки|вк|физмех"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("ИКиЗИ", "студент|ссылки|вк|икизи"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "назад"),
                        },
                    });
                }

                public static class Timetable
                {
                    public static InlineKeyboardMarkup timetableKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Расписание занятий", "https://ruz.spbstu.ru/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("'Расписание' на IOS", "https://apps.apple.com/ru/app/%D1%80%D0%B0%D1%81%D0%BF%D0%B8%D1%81%D0%B0%D0%BD%D0%B8%D0%B5-%D0%B7%D0%B0%D0%BD%D1%8F%D1%82%D0%B8%D0%B9-sked/id1492625031"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("'Расписание' на Android", "https://play.google.com/store/apps/details?id=com.sked.core&hl=lt"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "студент|назад"),
                        },
                    });
                }

                public static class Questions
                {
                    public static InlineKeyboardMarkup basicQuestionsKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Справки студентам", "студент|чзв|справки"),
                            InlineKeyboardButton.WithUrl("Учиться за границей", "https://www.spbstu.ru/international-cooperation/international-activities/academic-mobility/study-abroad/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Перевод в Политех из другого ВУЗа", "студент|чзв|перех|перех"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Все о военной кафедре", "студент|чзв|вуц|вуц"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Дополнительное образование", "https://www.spbstu.ru/students/additional-education/"),
                            InlineKeyboardButton.WithCallbackData("Иностранному студенту", "студент|чзв|иностранцу"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Практика и трудоустройство", "https://www.spbstu.ru/students/employment/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Стипендии, социальная поддержка", "студент|чзв|стип"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Социальное обеспечение", "студент|чзв|соцоб"),
                            InlineKeyboardButton.WithCallbackData("Медицинское обслуживание", "студент|чзв|медобсл"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Чат с представителем поддержки", "студент|чзв|опер|опер"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "студент|назад"),
                        },
                    });

                    public static InlineKeyboardMarkup grantKeyboard = new(
                    new[]
                        {
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Ознакомиться подробнее", "https://www.spbstu.ru/students/social-security/social-support/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "студент|чзв|назад"),
                        },
                    });

                    public static InlineKeyboardMarkup socialSecurityKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Стипендиальное обеспечение", "https://www.spbstu.ru/students/social-security/social-support/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Бесплатное медицинское обслуживание", "https://www.spbstu.ru/students/social-security/medical-care/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Организация досуга и летнего отдыха", "https://www.spbstu.ru/students/social-security/recreation/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Организация физкультурной и оздоровительной работы", "https://www.spbstu.ru/students/social-security/sports-complex/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Поддержка нуждающихся студентов", "https://www.spbstu.ru/students/social-security/financial-support/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Социально-бытовое обеспечение", "https://umto.spbstu.ru/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Предоставление общежитий иногородним студентам", "https://www.spbstu.ru/students/social-security/hostel/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "студент|чзв|назад"),
                        },
                    });

                    public static InlineKeyboardMarkup medicalServiceKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Узнать подробнее", "https://www.spbstu.ru/students/social-security/medical-care/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "студент|чзв|назад"),
                        },
                    });

                    public static InlineKeyboardMarkup callOperatorKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Вызвать оператора", "студент|чзв|опер|вызов"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "студент|чзв|назад"),
                        },
                    });

                    public static InlineKeyboardMarkup referenceKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                                InlineKeyboardButton.WithUrl("Узнать подробнее", "https://www.spbstu.ru/students/enquiry/"),
                        },
                        new[]
                        {
                                InlineKeyboardButton.WithCallbackData("Назад", "студент|чзв|назад"),
                        },
                    });

                    public static InlineKeyboardMarkup transitionKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                                InlineKeyboardButton.WithUrl("Узнать алгоритм", "https://www.spbstu.ru/students/transferring-students-from-other-universities-to-spbpu/"),
                        },
                        new[]
                        {
                                InlineKeyboardButton.WithCallbackData("Назад", "студент|чзв|назад"),
                        },
                    });

                    public static InlineKeyboardMarkup backKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                                InlineKeyboardButton.WithCallbackData("Назад", "студент|чзв|назад"),
                        },
                    });

                    public static class MilitaryDepartment
                    {
                        public static InlineKeyboardMarkup militaryDepartmentKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                    InlineKeyboardButton.WithCallbackData("Набор кафедр", "студент|чзв|вуц|набкаф"),
                            },
                            new[]
                            {
                                    InlineKeyboardButton.WithCallbackData("Предварительный отбор", "студент|чзв|вуц|предотбор"),
                            },
                            new[]
                            {
                                    InlineKeyboardButton.WithCallbackData("Основной отбор", "студент|чзв|вуц|оснотбор"),
                            },
                            new[]
                            {
                                    InlineKeyboardButton.WithCallbackData("Справочная информация", "студент|чзв|вуц|инфа"),
                            },
                            new[]
                            {
                                    InlineKeyboardButton.WithCallbackData("Назад", "студент|чзв|назад"),
                            },
                        });

                        public static InlineKeyboardMarkup backKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                    InlineKeyboardButton.WithCallbackData("Назад", "студент|чзв|вуц|назад"),
                            },
                        });
                    }
                }

                public static class Buildings
                {
                    public static InlineKeyboardMarkup chooseKeyboard = new(
                       new[]
                       {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Корпуса", "студент|карта|корпуса|корпуса"),
                                InlineKeyboardButton.WithCallbackData("Общежития", "студент|карта|общежития|общежития"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "студент|назад"),
                            },
                       });

                    public static InlineKeyboardMarkup hostelMapKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("1️⃣", "студент|карта|общежития|1"),
                            InlineKeyboardButton.WithCallbackData("3️⃣", "студент|карта|общежития|3"),
                            InlineKeyboardButton.WithCallbackData("4️⃣", "студент|карта|общежития|4, 4а"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("5️⃣", "студент|карта|общежития|5, 5б"),
                            InlineKeyboardButton.WithCallbackData("6️⃣", "студент|карта|общежития|6м, 6ф"),
                            InlineKeyboardButton.WithCallbackData("7️⃣", "студент|карта|общежития|7"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("8️⃣", "студент|карта|общежития|8"),
                            InlineKeyboardButton.WithCallbackData("🔟", "студент|карта|общежития|10"),
                            InlineKeyboardButton.WithCallbackData("1️⃣1️⃣", "студент|карта|общежития|11"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("1️⃣2️⃣", "студент|карта|общежития|12"),
                            InlineKeyboardButton.WithCallbackData("1️⃣3️⃣", "студент|карта|общежития|13"),
                            InlineKeyboardButton.WithCallbackData("1️⃣4️⃣", "студент|карта|общежития|14"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("1️⃣5️⃣", "студент|карта|общежития|15"),
                            InlineKeyboardButton.WithCallbackData("1️⃣6️⃣", "студент|карта|общежития|16"),
                            InlineKeyboardButton.WithCallbackData("1️⃣7️⃣", "студент|карта|общежития|17"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("1️⃣8️⃣", "студент|карта|общежития|18"),
                            InlineKeyboardButton.WithCallbackData("1️⃣9️⃣", "студент|карта|общежития|19"),
                            InlineKeyboardButton.WithCallbackData("2️⃣0️⃣", "студент|карта|общежития|20"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "студент|карта|назад"),
                        },
                    });

                    public static InlineKeyboardMarkup corpsMapKeyboard = new(
                    new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("🎓Главный учебный корпус", "студент|карта|корпуса|гз"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("⚽️Спортивный комплекс", "студент|карта|корпуса|спорт"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("🔬Лабораторный корпус", "студент|карта|корпуса|лаб"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("🧪Химический корпус", "студент|карта|корпуса|хим"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("🗼Гидробашня", "студент|карта|корпуса|башня"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("⚙️Механический корпуc", "студент|карта|корпуса|мех"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Гидрокорпус-1", "студент|карта|корпуса|г1"),
                            InlineKeyboardButton.WithCallbackData("Гидрокорпус-2", "студент|карта|корпуса|г2"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("1️⃣", "студент|карта|корпуса|1"),
                            InlineKeyboardButton.WithCallbackData("2️⃣", "студент|карта|корпуса|2"),
                            InlineKeyboardButton.WithCallbackData("3️⃣", "студент|карта|корпуса|3"),
                            InlineKeyboardButton.WithCallbackData("4️⃣", "студент|карта|корпуса|4"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("5️⃣", "студент|карта|корпуса|5"),
                            InlineKeyboardButton.WithCallbackData("6️⃣", "студент|карта|корпуса|6"),
                            InlineKeyboardButton.WithCallbackData("9️⃣", "студент|карта|корпуса|9"),
                            InlineKeyboardButton.WithCallbackData("🔟", "студент|карта|корпуса|10"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("1️⃣1️⃣", "студент|карта|корпуса|11"),
                            InlineKeyboardButton.WithCallbackData("1️⃣5️⃣", "студент|карта|корпуса|15"),
                            InlineKeyboardButton.WithCallbackData("1️⃣6️⃣", "студент|карта|корпуса|16"),
                            InlineKeyboardButton.WithCallbackData("5️⃣0️⃣", "студент|карта|корпуса|50"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "студент|карта|назад"),
                        },
                    });
                }
            }

            public static class Enrollee
            {
                public static InlineKeyboardMarkup startKeyboard = new(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("🎓О поступлении", "абитур|поступ|поступ"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("👤Выбрать профессию", "абитур|выбратьпроф|выбратьпроф"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("📚Познакомиться с университетом", "абитур|знаком|знаком"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("🗣Мероприятия и курсы", "абитур|курсы|курсы"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("📝Часто задаваемые вопросы ", "абитур|чзв|чзв"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Назад", "назад"),
                    },
                });

                public static class Admission
                {
                    public static InlineKeyboardMarkup basicKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Поступаю после школы", "абитур|поступ|шк|шк"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Поступаю после колледжа", "абитур|поступ|колл|колл"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Поступаю в магистратуру", "абитур|поступ|мага|мага"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Ключевые даты поступления", "абитур|поступ|даты"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("О процессе поступления", "абитур|поступ|проц|проц"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Поступление иностранных граждан", "абитур|поступ|инос|инос"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "абитур|назад"),
                        },
                    });

                    public static class School
                    {
                        public static InlineKeyboardMarkup afterSchoolKeyboard = new(
                           new[]
                           {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Про подачу документов", "абитур|поступ|шк|доки|доки"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Проходные баллы прошлых лет", "абитур|поступ|шк|баллы|баллы"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Количество мест", "абитур|поступ|шк|колво"),
                                     InlineKeyboardButton.WithCallbackData("Выбрать направление", "абитур|поступ|шк|направ"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Контрактная форма обучения", "абитур|поступ|шк|контр|контр"),
                                     InlineKeyboardButton.WithCallbackData("Про целевое поступление", "абитур|поступ|шк|целевое"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Вступительные испытания", "абитур|поступ|шк|вступ|вступ"),
                                     InlineKeyboardButton.WithCallbackData("Я олимпиадник!", "абитур|поступ|шк|я олимп|я олимп"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Индивидуальные достижения и олимпиады", "абитур|поступ|шк|достиж"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Узнать про медицинские документы", "абитур|поступ|шк|меддоки|меддоки"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Минимальные баллы", "абитур|поступ|шк|минбаллы"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Условия для льготной категории лиц", "абитур|поступ|шк|льгот"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|назад"),
                                },
                           });

                        public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|шк|назад"),
                                },
                            });

                        public static class Documents
                        {
                            public static InlineKeyboardMarkup documentsKeyboard = new(
                                new[]
                                {
                                    new[]
                                    {
                                            InlineKeyboardButton.WithCallbackData("Перечень документов", "абитур|поступ|шк|доки|перечень"),
                                    },
                                    new[]
                                    {
                                            InlineKeyboardButton.WithCallbackData("Как можно подать документы?", "абитур|поступ|шк|доки|как"),
                                    },
                                    new[]
                                    {
                                            InlineKeyboardButton.WithCallbackData("Какие сроки приема?", "абитур|поступ|шк|доки|сроки"),
                                    },
                                    new[]
                                    {
                                            InlineKeyboardButton.WithCallbackData("На сколько направлений можно опдать документы?", "абитур|поступ|шк|доки|колво"),
                                    },
                                    new[]
                                    {
                                            InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|шк|назад"),
                                    },
                                });

                            public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|шк|доки|назад"),
                                },
                            });
                        }

                        public static class Points
                        {
                            public static InlineKeyboardMarkup pointsKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("2021", "абитур|поступ|шк|баллы|2021"),
                                     InlineKeyboardButton.WithCallbackData("2020", "абитур|поступ|шк|баллы|2020"),
                                     InlineKeyboardButton.WithCallbackData("2019", "абитур|поступ|шк|баллы|2019"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("2018", "абитур|поступ|шк|баллы|2018"),
                                     InlineKeyboardButton.WithCallbackData("2017", "абитур|поступ|шк|баллы|2017"),
                                     InlineKeyboardButton.WithCallbackData("2016", "абитур|поступ|шк|баллы|2016"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|шк|назад"),
                                },
                            });
                        }

                        public static class ContractForm
                        {
                            public static InlineKeyboardMarkup contractFormKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Стоимость", "абитур|поступ|шк|контр|стоим"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Образовательный кредит", "абитур|поступ|шк|контр|кредит"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Оплата обучения материнском капиталом", "абитур|поступ|шк|контр|маткап"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|шк|назад"),
                                },
                            });

                            public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|шк|контр|назад"),
                                },
                            });
                        }

                        public static class EntranceTests
                        {
                            public static InlineKeyboardMarkup entranceTestsKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Кому надо сдавать?", "абитур|поступ|шк|вступ|лица"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Перечень вступительных испытаний", "абитур|поступ|шк|вступ|перечень"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Расписание вступительных испытаний", "абитур|поступ|шк|вступ|расписание"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Программы и образцы заданий", "абитур|поступ|шк|вступ|программы"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("\"Дизайн\" и \"Дизайн архитектурной среды\"", "абитур|поступ|шк|вступ|дизайн"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Вступительные испытания в 2022 году", "абитур|поступ|шк|вступ|2022"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|шк|назад"),
                                },
                            });

                            public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|шк|вступ|назад"),
                                },
                            });
                        }

                        public static class OlypmiadMan
                        {
                            public static InlineKeyboardMarkup olympiadManKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Поступление без вступительных испытаний", "абитур|поступ|шк|я олимп|бви"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Соответствие предметов олимпиад и направлений СПбПУ", "абитур|поступ|шк|я олимп|соотв"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|шк|назад"),
                                },
                            });

                            public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|шк|я олимп|назад"),
                                },
                            });
                        }

                        public static class IndividualAchievements
                        {
                            public static InlineKeyboardMarkup individualAchievementsKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                    InlineKeyboardButton.WithUrl("Индивидуальные достижения Бакалавриата", "https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/individual-achievements/"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithUrl("Индивидуальные достижения Магистратура", "https://www.spbstu.ru/abit/master/review-the-regulatory-documents/individual-achievements/"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|шк|назад"),
                                },
                            });
                        }

                        public static class MedicalDocuments
                        {
                            public static InlineKeyboardMarkup medicalDocumentsKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Что такое обязательный медицинский осмотр?", "абитур|поступ|шк|меддоки|что"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Мед.документы для зачисления студентов", "абитур|поступ|шк|меддоки|зачисл"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|шк|назад"),
                                },
                            });

                            public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|шк|меддоки|назад"),
                                },
                            });
                        }
                    }

                    public static class College
                    {
                        public static InlineKeyboardMarkup afterCollegeKeyboard = new(
                           new[]
                           {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Перечень документов", "абитур|поступ|колл|доки|доки"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Проходные баллы прошлых лет", "абитур|поступ|колл|баллы|баллы"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Количество мест", "абитур|поступ|колл|колво"),
                                     InlineKeyboardButton.WithCallbackData("Выбрать направление", "абитур|поступ|колл|направ"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Контрактная форма обучения", "абитур|поступ|колл|контр|контр"),
                                     InlineKeyboardButton.WithCallbackData("Целевое обучение", "абитур|поступ|колл|целевое"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Про конкурсные списки", "абитур|поступ|колл|кc|кc"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Узнать про медицинские документы", "абитур|поступ|колл|меддоки|меддоки"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Индивидуальные достижения", "абитур|поступ|колл|достиж"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Вступительные испытания", "абитур|поступ|колл|ви|ви"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Посмотреть вебинар", "абитур|поступ|колл|вебинар"),
                                     InlineKeyboardButton.WithCallbackData("Изучить статью", "абитур|поступ|колл|статья"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|назад"),
                                },
                           });

                        public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|колл|назад"),
                                },
                            });

                        public static class Points
                        {
                            public static InlineKeyboardMarkup pointsKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("2021", "абитур|поступ|колл|баллы|2021"),
                                     InlineKeyboardButton.WithCallbackData("2020", "абитур|поступ|колл|баллы|2020"),
                                     InlineKeyboardButton.WithCallbackData("2019", "абитур|поступ|колл|баллы|2019"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("2018", "абитур|поступ|колл|баллы|2018"),
                                     InlineKeyboardButton.WithCallbackData("2017", "абитур|поступ|колл|баллы|2017"),
                                     InlineKeyboardButton.WithCallbackData("2016", "абитур|поступ|колл|баллы|2016"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|колл|назад"),
                                },
                            });
                        }

                        public static class ContractForm
                        {
                            public static InlineKeyboardMarkup contractFormKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Стоимость", "абитур|поступ|колл|контр|стоим"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Образовательный кредит", "абитур|поступ|колл|контр|кредит"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Оплата обучения материнском капиталом", "абитур|поступ|колл|контр|маткап"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|колл|назад"),
                                },
                            });

                            public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|колл|контр|назад"),
                                },
                            });
                        }

                        public static class CompetitiveLists
                        {
                            public static InlineKeyboardMarkup competitiveListsKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Как ранжируются списки?", "абитур|поступ|колл|кc|как"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Где списки подавших документы?", "абитур|поступ|колл|кc|где"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Меня нет в списке, что делать?", "абитур|поступ|колл|кc|нетчел"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("В списке нет моих баллов по предмету", "абитур|поступ|колл|кc|нетпред"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Нет баллов за индивидуальные достижения", "абитур|поступ|колл|кc|нетиндивид"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|колл|назад"),
                                },
                            });

                            public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|колл|кc|назад"),
                                },
                            });
                        }

                        public static class MedicalDocuments
                        {
                            public static InlineKeyboardMarkup medicalDocumentsKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Что такое обязательный медицинский осмотр?", "абитур|поступ|колл|меддоки|что"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Мед.документы для зачисления студентов", "абитур|поступ|колл|меддоки|зачисл"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|колл|назад"),
                                },
                            });

                            public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|колл|меддоки|назад"),
                                },
                            });
                        }

                        public static class IndividualAchievements
                        {
                            public static InlineKeyboardMarkup individualAchievementsKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                    InlineKeyboardButton.WithUrl("Индивидуальные достижения Бакалавриата", "https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/individual-achievements/"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithUrl("Индивидуальные достижения Магистратура", "https://www.spbstu.ru/abit/master/review-the-regulatory-documents/individual-achievements/"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|колл|назад"),
                                },
                            });
                        }

                        public static class EntranceTests
                        {
                            public static InlineKeyboardMarkup entranceTestsKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Кому надо сдавать?", "абитур|поступ|колл|ви|лица"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Перечень вступительных испытаний", "абитур|поступ|колл|ви|перечень"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Расписание вступительных испытаний", "абитур|поступ|колл|ви|расписание"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Программы и образцы заданий", "абитур|поступ|колл|ви|программы"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("\"Дизайн\" и \"Дизайн архитектурной среды\"", "абитур|поступ|колл|ви|дизайн"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Вступительные испытания в 2022 году", "абитур|поступ|колл|ви|2022"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|колл|назад"),
                                },
                            });

                            public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|колл|ви|назад"),
                                },
                            });
                        }
                    }

                    public static class Magistr
                    {
                        public static InlineKeyboardMarkup magistrKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Про подачу документов", "абитур|поступ|мага|доки|доки"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Вступительные испытания", "абитур|поступ|мага|ви|ви"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Количество мест", "абитур|поступ|мага|колво"),
                                InlineKeyboardButton.WithCallbackData("Выбрать направление", "абитур|поступ|мага|направ"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("За что дают доп.баллы?", "абитур|поступ|мага|допы"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithUrl("Телеграм-канал для будущих магистров", "https://t.me/polytech_master"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Минимальные баллы", "абитур|поступ|мага|минбаллы"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Конкурс портфолио", "абитур|поступ|мага|потрф"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Поступаю по олимпиаде 'Я - профессионал'", "абитур|поступ|мага|япроф"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Про целевое поступление", "абитур|поступ|мага|целевое"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|назад"),
                            },
                        });

                        public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|мага|назад"),
                                },
                            });

                        public static class Documents
                        {
                            public static InlineKeyboardMarkup documentsKeyboard = new(
                                new[]
                                {
                                    new[]
                                    {
                                            InlineKeyboardButton.WithCallbackData("Перечень документов", "абитур|поступ|мага|доки|перечень"),
                                    },
                                    new[]
                                    {
                                            InlineKeyboardButton.WithCallbackData("Как можно подать документы?", "абитур|поступ|мага|доки|как"),
                                    },
                                    new[]
                                    {
                                            InlineKeyboardButton.WithCallbackData("Какие сроки приема?", "абитур|поступ|мага|доки|сроки"),
                                    },
                                    new[]
                                    {
                                            InlineKeyboardButton.WithCallbackData("На сколько направлений можно опдать документы?", "абитур|поступ|мага|доки|колво"),
                                    },
                                    new[]
                                    {
                                            InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|мага|назад"),
                                    },
                                });

                            public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|мага|доки|назад"),
                                },
                            });
                        }

                        public static class EntranceTests
                        {
                            public static InlineKeyboardMarkup entranceTestsKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Программы вступительных испытаний", "абитур|поступ|мага|ви|прога"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Примеры прошлых лет", "абитур|поступ|мага|ви|примеры"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Расписание экзаменов", "абитур|поступ|мага|ви|расписание"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Как проходят вступительные в СПбПУ?", "абитур|поступ|мага|ви|вступ"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|мага|назад"),
                                },
                            });

                            public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|мага|ви|назад"),
                                },
                            });
                        }
                    }

                    public static class ForeignStudents
                    {
                        public static InlineKeyboardMarkup foreignStudentsKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Соотечественникам", "абитур|поступ|инос|соот"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Через отдел по работе с иностранцами", "абитур|поступ|инос|отдел"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|назад"),
                            },
                        });

                        public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|инос|назад"),
                                },
                            });
                    }

                    public static class Dates
                    {
                        public static InlineKeyboardMarkup datesKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithUrl("Календарь поступления в Бакалавриат", "https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/plan-the-calendar-of-admission-to-the-1st-year/"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithUrl("Календарь будущего Магистра", "https://www.spbstu.ru/abit/master/review-the-regulatory-documents/plan-the-calendar-of-admission/"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|назад"),
                            },
                        });
                    }

                    public static class AdmissionProcess
                    {
                        public static InlineKeyboardMarkup admissionProcessKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Я подал документы, что дальше?", "абитур|поступ|проц|подал|подал"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Какие у меня шансы поступить?", "абитур|поступ|проц|шансы"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Платное обучение", "абитур|поступ|проц|контр|контр"),
                                InlineKeyboardButton.WithCallbackData("Целевое обучение", "абитур|поступ|проц|цел"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Конкурсные списки", "абитур|поступ|проц|кc|кc"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Про медицинские документы", "абитур|поступ|проц|меддоки|меддоки"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Связь со своим институтом", "абитур|поступ|проц|инст"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Вы получили мое заявление через Госуслуги?", "абитур|поступ|проц|госуслуг"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Чат с представителем поддержки", "абитур|поступ|проц|опер|опер"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|назад"),
                            },
                        });

                        public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|проц|назад"),
                                },
                            });

                        public static class WhatsNext
                        {
                            public static InlineKeyboardMarkup whatsNextKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Приказы о зачислении", "абитур|поступ|проц|подал|прик"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Согласие на зачисление", "абитур|поступ|проц|подал|согл"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Я в приказе, что делать?", "абитур|поступ|проц|подал|дал"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|проц|назад"),
                                },
                            });

                            public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|проц|подал|назад"),
                                },
                            });
                        }

                        public static class MedicalDocuments
                        {
                            public static InlineKeyboardMarkup medicalDocumentsKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Что такое обязательный медицинский осмотр?", "абитур|поступ|проц|меддоки|что"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Мед.документы для зачисления студентов", "абитур|поступ|проц|меддоки|зачисл"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|проц|назад"),
                                },
                            });

                            public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|проц|меддоки|назад"),
                                },
                            });
                        }

                        public static class CompetitiveLists
                        {
                            public static InlineKeyboardMarkup competitiveListsKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Как ранжируются списки?", "абитур|поступ|проц|кc|как"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Где списки подавших документы?", "абитур|поступ|проц|кc|где"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Меня нет в списке, что делать?", "абитур|поступ|проц|кc|нетчел"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("В списке нет моих баллов по предмету", "абитур|поступ|проц|кc|нетпред"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Нет баллов за индивидуальные достижения", "абитур|поступ|проц|кc|нетиндивид"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|проц|назад"),
                                },
                            });

                            public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|проц|кc|назад"),
                                },
                            });
                        }

                        public static class ContractForm
                        {
                            public static InlineKeyboardMarkup contractFormKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Стоимость", "абитур|поступ|проц|контр|стоим"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Образовательный кредит", "абитур|поступ|проц|контр|кредит"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Оплата обучения материнском капиталом", "абитур|поступ|проц|контр|маткап"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|проц|назад"),
                                },
                            });

                            public static InlineKeyboardMarkup backKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|проц|контр|назад"),
                                },
                            });
                        }

                        public static class Operator
                        {
                            public static InlineKeyboardMarkup callOperatorKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Вызвать оператора", "абитур|поступ|проц|опер|вызов"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Назад", "абитур|поступ|проц|назад"),
                                },
                            });
                        }
                    }
                }

                public static class ChooseProfession
                {
                    public static InlineKeyboardMarkup chooseKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Бакалавриат", "абитур|выбратьпроф|бака|бака"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Магистратура", "абитур|выбратьпроф|мага|мага"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Анкета абитуриента", "https://school.spbstu.ru/application/"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Профориентационный тест", "https://school.spbstu.ru/prof_tests/"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Назад", "абитур|назад"),
                        },
                    });

                    public static InlineKeyboardMarkup undergraduateKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Буклет", "абитур|выбратьпроф|бака|буклет"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Сайт с фильтром направлений", "https://www.spbstu.ru/abit/bachelor/to-choose-the-direction-of-training/bachelor-s-degree-programs/"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Назад", "абитур|выбратьпроф|назад"),
                        },
                    });

                    public static InlineKeyboardMarkup magistracyKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Буклет", "абитур|выбратьпроф|мага|буклет"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Сайт с фильтром образовательных программ", "https://www.spbstu.ru/abit/master/to-choose-the-direction-of-training/education-program/"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Назад", "абитур|выбратьпроф|назад"),
                        },
                    });
                }

                public static class AboutUniversity
                {
                    public static InlineKeyboardMarkup aboutUniversityKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Ролик об университете", "абитур|знаком|ролик"),
                            InlineKeyboardButton.WithCallbackData("Презентация", "абитур|знаком|презентация"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Дни открытых дверей", "абитур|знаком|дод"),
                            InlineKeyboardButton.WithCallbackData("Аудиогид по кампусу", "абитур|знаком|аудиогид"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Наши соцсети и сайты", "абитур|знаком|медиа"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Почему политех?", "абитур|знаком|поч|поч"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "абитур|назад"),
                        },
                    });

                    public static InlineKeyboardMarkup scheduleOpenDoorsKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Расписание дней открытых дверей", "https://www.spbstu.ru/applicants/open-days/"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Назад", "абитур|знаком|назад"),
                        },
                    });

                    public static InlineKeyboardMarkup toursKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Кампус", "абитур|знаком|экскур|кампус"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Лаборатории", "абитур|знаком|экскур|лабор"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Консультация приемной комиссии", "абитур|знаком|экскур|кпк"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Назад", "назад"),
                        },
                    });

                    public static InlineKeyboardMarkup audioGuideKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Политех на ладони.Узнать за 60 минут", "https://www.spbstu.ru/applicants/open-days/"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Назад", "абитур|знаком|назад"),
                        },
                    });

                    public static InlineKeyboardMarkup mediaKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Группа абитуриентам ВК", "https://vk.com/abit_spbstu"),
                             InlineKeyboardButton.WithUrl("Оф.группа Политеха ВК", "https://vk.com/polytech_petra"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Подготовительные курсы", "https://vk.com/courses.spbstu"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Оф.канал Политеха", "https://t.me/polytech_petra"),
                             InlineKeyboardButton.WithUrl("Образовательный интенсив", "https://vk.com/education_spbstu"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Политехническая олимпиада", "https://vk.com/olympspbstu"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Сайт Политеха школьникам", "https://school.spbstu.ru/"),
                             InlineKeyboardButton.WithUrl("Летняя школа", "https://summer.spbstu.ru/"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Образовательный форум", "https://winter.spbstu.ru/"),
                             InlineKeyboardButton.WithUrl("Канал поступления в магистратуру", "https://t.me/polytech_master"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Назад", "абитур|знаком|назад"),
                        },
                    });

                    public static class WhyPolytech
                    {
                        public static InlineKeyboardMarkup whyPolytechKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                    InlineKeyboardButton.WithCallbackData("Университет в цифрах", "абитур|знаком|поч|цифры"),
                            },
                            new[]
                            {
                                    InlineKeyboardButton.WithCallbackData("Полёт над кампусом СПбПУ", "абитур|знаком|поч|полет"),
                            },
                            new[]
                            {
                                    InlineKeyboardButton.WithCallbackData("Онлайн-ресурсы Политеха", "абитур|знаком|поч|ресурсы"),
                            },
                            new[]
                            {
                                    InlineKeyboardButton.WithCallbackData("Виртуальная экскурсия", "абитур|знаком|поч|экскурс"),
                            },
                            new[]
                            {
                                    InlineKeyboardButton.WithCallbackData("Назад", "абитур|знаком|назад"),
                            },
                        });

                        public static InlineKeyboardMarkup backKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Назад", "абитур|знаком|поч|назад"),
                            },
                        });
                    }

                    public static InlineKeyboardMarkup backKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                                InlineKeyboardButton.WithCallbackData("Назад", "абитур|знаком|назад"),
                        },
                    });
                }

                public static class Questions
                {
                    public static InlineKeyboardMarkup basicQuestionsKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Порядок поступления", "абитур|чзв|поступ|поступ"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Документы", "абитур|чзв|доки|доки"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Индивидуальные достижения", "абитур|чзв|индивид.достижения"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Вступительные испытания", "абитур|чзв|вступ.исп|вступ.исп"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("После поступления", "абитур|чзв|после поступления"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Стипендии и гранты для первокурсников", "абитур|чзв|стип/гранты"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Хочу перевестись в Политех", "абитур|чзв|перевод в политех"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Узнать про общежития", "абитур|чзв|общежития|общежития"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Чат с представителем поддержки", "абитур|чзв|оператор|оператор"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "абитур|назад"),
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
                            InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|назад"),
                        },
                    });

                    public static class Hostel 
                    {
                        public static InlineKeyboardMarkup hostelKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Посмотреть репортраж из общежития", "абитур|чзв|общежития|репортаж"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Я получу общежитие?", "абитур|чзв|общежития|получу ли я?"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Карта общежитий", "абитур|чзв|общежития|карта|карта"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|назад"),
                            },
                        });

                        public static InlineKeyboardMarkup hostelMapKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("1️⃣", "абитур|чзв|общежития|карта|1"),
                                InlineKeyboardButton.WithCallbackData("3️⃣", "абитур|чзв|общежития|карта|3"),
                                InlineKeyboardButton.WithCallbackData("4️⃣", "абитур|чзв|общежития|карта|4, 4а"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("5️⃣", "абитур|чзв|общежития|карта|5, 5б"),
                                InlineKeyboardButton.WithCallbackData("6️⃣", "абитур|чзв|общежития|карта|6м, 6ф"),
                                InlineKeyboardButton.WithCallbackData("7️⃣", "абитур|чзв|общежития|карта|7"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("8️⃣", "абитур|чзв|общежития|карта|8"),
                                InlineKeyboardButton.WithCallbackData("🔟", "абитур|чзв|общежития|карта|10"),
                                InlineKeyboardButton.WithCallbackData("1️⃣1️⃣", "абитур|чзв|общежития|карта|11"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("1️⃣2️⃣", "абитур|чзв|общежития|карта|12"),
                                InlineKeyboardButton.WithCallbackData("1️⃣3️⃣", "абитур|чзв|общежития|карта|13"),
                                InlineKeyboardButton.WithCallbackData("1️⃣4️⃣", "абитур|чзв|общежития|карта|14"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("1️⃣5️⃣", "абитур|чзв|общежития|карта|15"),
                                InlineKeyboardButton.WithCallbackData("1️⃣6️⃣", "абитур|чзв|общежития|карта|16"),
                                InlineKeyboardButton.WithCallbackData("1️⃣7️⃣", "абитур|чзв|общежития|карта|17"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("1️⃣8️⃣", "абитур|чзв|общежития|карта|18"),
                                InlineKeyboardButton.WithCallbackData("1️⃣9️⃣", "абитур|чзв|общежития|карта|19"),
                                InlineKeyboardButton.WithCallbackData("2️⃣0️⃣", "абитур|чзв|общежития|карта|20"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|общежития|назад"),
                            },
                        });

                        public static InlineKeyboardMarkup backKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|общежития|назад"),
                            },
                        });
                    }

                    public static class EntranceTests
                    {
                        public static InlineKeyboardMarkup entranceTestsKeyboard = new(
                            new[]
                            {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Кому надо сдавать?", "абитур|чзв|вступ.исп|лица"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Перечень вступительных испытаний", "абитур|чзв|вступ.исп|перечень"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Расписание вступительных испытаний", "абитур|чзв|вступ.исп|расписание"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Программы и образцы заданий", "абитур|чзв|вступ.исп|программы"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("\"Дизайн\" и \"Дизайн архитектурной среды\"", "абитур|чзв|вступ.исп|дизайн"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Вступительные испытания в 2022 году", "абитур|чзв|вступ.исп|2022"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|назад"),
                            },
                        });

                        public static InlineKeyboardMarkup backKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|вступ.исп|назад"),
                            },
                        });
                    }

                    public static class Admission 
                    {
                        public static InlineKeyboardMarkup admissionKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Календарь поступления", "абитур|чзв|поступ|кален"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|назад"),
                            },
                        });

                        public static InlineKeyboardMarkup calendarKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithUrl("Календарь поступления в бакалавриат", "https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/plan-the-calendar-of-admission-to-the-1st-year/"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithUrl("Календарь будущего магистра", "https://www.spbstu.ru/abit/master/review-the-regulatory-documents/plan-the-calendar-of-admission/"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|поступ|назад"),
                            },
                        });
                    }

                    public static class Documents
                    {
                        public static InlineKeyboardMarkup documentsKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithUrl("Личный кабинет абитуриента", "https://enroll.spbstu.ru/sign-in"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Календарь поступления", "абитур|чзв|доки|кален"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithUrl("Стоимость обучения", "https://www.spbstu.ru/abit/bachelor/apply/stoimost-obucheniya/"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|назад"),
                            },
                        });
                        public static InlineKeyboardMarkup calendarKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithUrl("Календарь поступления в бакалавриат", "https://www.spbstu.ru/abit/bachelor/oznakomitsya-with-the-regulations/plan-the-calendar-of-admission-to-the-1st-year/"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithUrl("Календарь будущего магистра", "https://www.spbstu.ru/abit/master/review-the-regulatory-documents/plan-the-calendar-of-admission/"),
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|доки|назад"),
                            },
                        });
                    }

                    public static InlineKeyboardMarkup grantKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Ознакомиться подробнее", "https://www.spbstu.ru/students/social-security/social-support/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|назад"),
                        },
                    });

                    public static InlineKeyboardMarkup backKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|назад"),
                        },
                    });
                }

                public static class EventsCourse
                {
                    public static InlineKeyboardMarkup basicKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Подготовительные курсы", "абитур|курсы|пк|пк"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Образовательные мероприятия", "абитур|курсы|ом|ом"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Олимпиады", "абитур|курсы|олимп|олимп"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Ближайшие события", "абитур|курсы|соб"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Назад", "абитур|назад"),
                            },
                        });

                    public static InlineKeyboardMarkup closeEventsKeyboard = new(
                        new[]
                        {
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Перейти на сайт", "https://school.spbstu.ru/"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|назад"),
                        },
                        });

                    public static class TrainingCourses
                    {
                        public static InlineKeyboardMarkup trainingCoursesKeyboard = new(
                        new[]
                        {
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Школьникам 1-9 класса", "абитур|курсы|пк|1-9"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Школьникам 10 класса", "абитур|курсы|пк|10"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Школьникам 11 класса", "абитур|курсы|пк|11|11"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Выпускникам колледжей", "абитур|курсы|пк|колледж"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Поступающим в магистратуру", "абитур|курсы|пк|мага"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|назад"),
                        },
                        });

                        public static InlineKeyboardMarkup for1to9Keyboard = new(
                        new[]
                        {
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Академия информатики для школьников", "https://www.avalon.ru/SchoolAcademy/"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Фаблаб политех", "https://fablab.spbstu.ru/"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Дом ученых в Лесном", "https://vk.com/sc.club"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|пк|назад"),
                        },
                        });

                        public static InlineKeyboardMarkup for10CollegeKeyboard = new(
                        new[]
                        {
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Запись на курсы", "https://courses.spbstu.ru/"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|пк|назад"),
                        },
                        });

                        public static InlineKeyboardMarkup entryFor11Keyboard = new(
                        new[]
                        {
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Запись на курсы", "https://courses.spbstu.ru/"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|пк|11|назад"),
                        },
                        });

                        public static InlineKeyboardMarkup entryForMagKeyboard = new(
                        new[]
                        {
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Подробнее и запись", "https://ice.spbstu.ru/podgotovitelnye_kursy/"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|пк|назад"),
                        },
                        });

                        public static InlineKeyboardMarkup entryForeighLangKeyboard = new(
                        new[]
                        {
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Запись на курсы", "https://lingua.spbstu.ru/#languages"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|пк|11|назад"),
                        },
                        });

                        public static InlineKeyboardMarkup entryTrainCoursesKeyboard = new(
                        new[]
                        {
                        new[]
                        {
                             InlineKeyboardButton.WithUrl("Запись на курсы", "https://design.spbstu.ru/podgotovitelnye_kursy/"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|пк|11|назад"),
                        },
                        });

                        public static InlineKeyboardMarkup for11Keyboard = new(
                        new[]
                        {
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Основные предметы", "абитур|курсы|пк|11|осн"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Иностранные языки", "абитур|курсы|пк|11|иняз"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Рисунок/Живопись/Композиция", "абитур|курсы|пк|11|ржк"),
                        },
                        new[]
                        {
                             InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|пк|назад"),
                        },
                        });
                    }

                    public static class EducationalPrograms
                    {
                        public static InlineKeyboardMarkup educationalProgramsKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Инженерная лига Политеха", "абитур|курсы|ом|илп"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Летняя школа", "абитур|курсы|ом|лш"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Кейс-чемпионат 'Polycase'", "абитур|курсы|ом|polycase"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Фестиваль науки - дорога в Политех", "абитур|курсы|ом|двп"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|назад"),
                            },
                        });

                        public static InlineKeyboardMarkup engineeringLeagueKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                 InlineKeyboardButton.WithUrl("Участвовать", "https://vk.com/education_spbstu"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|ом|назад"),
                            },
                        });

                        public static InlineKeyboardMarkup polycaseKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                 InlineKeyboardButton.WithUrl("Участвовать", "https://vk.com/polycase"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|ом|назад"),
                            },
                        });

                        public static InlineKeyboardMarkup summerSchoolKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                 InlineKeyboardButton.WithUrl("Подробнее", "https://vk.com/education_spbstu"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithUrl("Участвовать", "https://summer.spbstu.ru/"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|ом|назад"),
                            },
                        });

                        public static InlineKeyboardMarkup scienceFestivalKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                 InlineKeyboardButton.WithUrl("Участвовать", "https://vk.com/ibsb_spbstu"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|ом|назад"),
                            },
                        });
                    }

                    public static class Olympiad
                    {
                        public static InlineKeyboardMarkup olympiadKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("По предметам", "абитур|курсы|олимп|пред|пред"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("По названию", "абитур|курсы|олимп|назв|назв"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Я олимпиадник!", "абитур|курсы|олимп|я олимп|я олимп"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|назад"),
                            },
                        });

                        public static InlineKeyboardMarkup subjectKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Физика", "абитур|курсы|олимп|пред|физ"),
                                 InlineKeyboardButton.WithCallbackData("Математика", "абитур|курсы|олимп|пред|мат"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Информатика", "абитур|курсы|олимп|пред|инф"),
                                 InlineKeyboardButton.WithCallbackData("Химия", "абитур|курсы|олимп|пред|хим"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Гуманитарные науки", "абитур|курсы|олимп|пред|гум"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|олимп|назад"),
                            },
                        });

                        public static InlineKeyboardMarkup olympiadManKeyboard = new(
                           new[]
                           {
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Поступление без вступительных испытаний", "абитур|курсы|олимп|я олимп|бви"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Соответствие предметов олимпиад и направлений СПбПУ", "абитур|курсы|олимп|я олимп|соотв"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|олимп|назад"),
                                },
                           });

                        public static InlineKeyboardMarkup titleKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("РСОШ", "абитур|курсы|олимп|назв|рсош"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithUrl("Всеросс олимпиада школьников", "https://school.spbstu.ru/olympiads/vserossiyskaya_olimpiada_shkolnikov_po_fizike/"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithUrl("Дает доп.баллы за победу", "https://school.spbstu.ru/olympiads/politehnicheskaya_olimpiada/"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|олимп|назад"),
                            },
                        });

                        public static InlineKeyboardMarkup backKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|олимп|я олимп|назад"),
                            },
                        });

                        public static InlineKeyboardMarkup rsochKeyboard = new(
                        new[]
                        {
                            new[]
                            {
                                 InlineKeyboardButton.WithUrl("Отраслевая олимпиада Газпром", "https://school.spbstu.ru/olympiads/otraslevaya_olimpiada_shkolnikov_gazprom/"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithUrl("Олимпиада НТИ", "https://school.spbstu.ru/olympiads/olimpiada_nacionalnoy_tehnologicheskoy_iniciativy/"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithUrl("ОММО", "https://school.spbstu.ru/olympiads/obedin_nnaya_mezghvuzovskaya_matematicheskaya_olimpiada_shkolnikov/"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithUrl("Технокубок", "https://school.spbstu.ru/olympiads/olimpiada_shkolnikov_po_programmirovaniu_tehnokubok/"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithUrl("Межрегиональная олимпиада им.Верченко", "https://school.spbstu.ru/olympiads/mezghregionalnaya_olimpiada_shkolnikov_im_i_ya_verchenko/"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithUrl("Всероссийская толстовская олимпиада", "https://tsput.ru/olympiad/entry_form/"),
                            },
                            new[]
                            {
                                 InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|олимп|назв|назад"),
                            },
                        });

                        public static class Physics
                        {
                            public static InlineKeyboardMarkup physicsKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithUrl("Всероссийская олимпиада школьников", "https://school.spbstu.ru/olympiads/vserossiyskaya_olimpiada_shkolnikov_po_fizike/"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithUrl("Отраслевая олимпиада Газпром", "https://school.spbstu.ru/olympiads/otraslevaya_olimpiada_shkolnikov_gazprom/"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithUrl("Политехническая олимпиада", "https://school.spbstu.ru/olympiads/politehnicheskaya_olimpiada/"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithUrl("Олимпиада НТИ", "https://school.spbstu.ru/olympiads/olimpiada_nacionalnoy_tehnologicheskoy_iniciativy/"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|олимп|пред|назад"),
                                },
                            });
                        }

                        public static class Mathematics
                        {
                            public static InlineKeyboardMarkup mathematicsKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithUrl("ОММО", "https://school.spbstu.ru/olympiads/obedin_nnaya_mezghvuzovskaya_matematicheskaya_olimpiada_shkolnikov/"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithUrl("Межрегиональная олимпиада им.Верченко", "https://school.spbstu.ru/olympiads/mezghregionalnaya_olimpiada_shkolnikov_im_i_ya_verchenko/"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithUrl("Политехническая олимпиада", "https://school.spbstu.ru/olympiads/politehnicheskaya_olimpiada/"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithUrl("Отраслевая олимпиада Газпром", "https://school.spbstu.ru/olympiads/otraslevaya_olimpiada_shkolnikov_gazprom/"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|олимп|пред|назад"),
                                },
                            });
                        }

                        public static class Informatics
                        {
                            public static InlineKeyboardMarkup informaticsKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithUrl("Технокубок", "https://school.spbstu.ru/olympiads/olimpiada_shkolnikov_po_programmirovaniu_tehnokubok/"),
                                     InlineKeyboardButton.WithUrl("Олимпиада НТИ", "https://school.spbstu.ru/olympiads/olimpiada_nacionalnoy_tehnologicheskoy_iniciativy/"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithUrl("Межрегиональная олимпиада им.Верченко", "https://school.spbstu.ru/olympiads/mezghregionalnaya_olimpiada_shkolnikov_im_i_ya_verchenko/"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithUrl("Политехническая олимпиада", "https://school.spbstu.ru/olympiads/politehnicheskaya_olimpiada/"),
                                     InlineKeyboardButton.WithUrl("Мартовские КИТы", "http://hse.spbstu.ru/SchoolAcademy/Activities/Olympics/"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithUrl("Отраслевая олимпиада Газпром", "https://school.spbstu.ru/olympiads/otraslevaya_olimpiada_shkolnikov_gazprom/"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|олимп|пред|назад"),
                                },
                            });
                        }

                        public static class Chemistry
                        {
                            public static InlineKeyboardMarkup chemistryKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithUrl("Политехническая олимпиада", "https://school.spbstu.ru/olympiads/politehnicheskaya_olimpiada/"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithUrl("Отраслевая олимпиада Газпром", "https://school.spbstu.ru/olympiads/otraslevaya_olimpiada_shkolnikov_gazprom/"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|олимп|пред|назад"),
                                },
                            });
                        }

                        public static class Humanitarian
                        {
                            public static InlineKeyboardMarkup humanitarianKeyboard = new(
                            new[]
                            {
                                new[]
                                {
                                     InlineKeyboardButton.WithUrl("Участвовать", "https://tsput.ru/olympiad/entry_form/"),
                                },
                                new[]
                                {
                                     InlineKeyboardButton.WithCallbackData("Назад", "абитур|курсы|олимп|пред|назад"),
                                },
                            });
                        }
                    }
                }

                public static InlineKeyboardMarkup callOperatorKeyboard = new(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Вызвать оператора", "абитур|чзв|оператор|вызов"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|назад"),
                    },
                });
            }
        }
    }
}
