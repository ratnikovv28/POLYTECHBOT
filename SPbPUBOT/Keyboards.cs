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
                        InlineKeyboardButton.WithCallbackData("Абитуриент", "абитур|абитур"),
                        InlineKeyboardButton.WithCallbackData("Студент", "студент|студент"),
                    },
                });

            public static InlineKeyboardMarkup callOperatorKeyboard = new(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Вызвать оператора", "абитур|чзв|оператор|вызов"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Назад", "назад"),
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
                public static InlineKeyboardMarkup basicKeyboard = new(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Справки", "студ|справки"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Учебный план", "студ|учплан"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Расписание", "студ|расписание"),
                    },
                });
            }

            public static class Enrollee
            {
                public static InlineKeyboardMarkup startKeyboard = new(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("О поступлении", "абитур|поступ|поступ"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Выбрать профессию", "абитур|выбратьпроф|выбратьпроф"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Познакомиться с университетом", "абитур|знаком|знаком"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Мероприятия и курсы для абитуриентов", "абитур|курсы|курсы"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Часто задаваемые вопросы", "абитур|чзв|чзв"),
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
                            InlineKeyboardButton.WithCallbackData("Поступаю после школы", "абитур|поступ|поступ|поступ"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Поступаю после колледжа", "абитур|поступ|доки|доки"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Поступаю в магистратуру", "абитур|поступ|индивид.достижения"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Ключевые даты поступления", "абитур|поступ|вступ.исп|вступ.исп"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("О процессе поступления", "абитур|поступ|после поступления"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Поступление иностранных граждан", "абитур|поступ|стип/гранты"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "назад"),
                        },
                    });

                    public static InlineKeyboardMarkup afterSchoolKeyboard = new(
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
                             InlineKeyboardButton.WithCallbackData("Назад", "назад"),
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
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Экскурсии", "абитур|знаком|экскур|экскур"),
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
                            InlineKeyboardButton.WithCallbackData("Назад", "назад"),
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
                             InlineKeyboardButton.WithCallbackData("Назад", "назад"),
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
                             InlineKeyboardButton.WithCallbackData("Назад", "назад"),
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
                             InlineKeyboardButton.WithCallbackData("Назад", "назад"),
                        },
                    });

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
                                 InlineKeyboardButton.WithCallbackData("Назад", "назад"),
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
                            InlineKeyboardButton.WithCallbackData("Назад", "назад"),
                        },
                    });
                    #region Клавиатуры общежитий
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
                            InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|общежития|назад"),
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
                            InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|общежития|карта|назад"),
                        },
                         });
                        #endregion
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
                            InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|вступ.исп|назад"),
                        },
                        });
                    #region Клавиатуры поступления
                    public static InlineKeyboardMarkup admissionKeyboard = new(
                        new[]
                        {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Календарь поступления", "абитур|чзв|поступ|кален|кален"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|поступ|назад"),
                        },
                        });
                    public static InlineKeyboardMarkup calendar1Keyboard = new(
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
                            InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|поступ|кален|назад"),
                        },
                        });
                        #endregion
                    #region Клавиатуры документов
                    public static InlineKeyboardMarkup documentsKeyboard = new(
                        new[]
                        {
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Личный кабинет абитуриента", "https://enroll.spbstu.ru/sign-in"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Календарь поступления", "абитур|чзв|доки|кален|кален"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Стоимость обучения", "https://www.spbstu.ru/abit/bachelor/apply/stoimost-obucheniya/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|доки|назад"),
                        },
                        });
                    public static InlineKeyboardMarkup calendar2Keyboard = new(
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
                            InlineKeyboardButton.WithCallbackData("Назад", "абитур|чзв|доки|кален|назад"),
                        },
                       });
                        #endregion
                    public static InlineKeyboardMarkup grantKeyboard = new(
                        new[]
                        {
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Ознакомиться подробнее", "https://www.spbstu.ru/students/social-security/social-support/"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", "назад"),
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
                                 InlineKeyboardButton.WithCallbackData("Назад", "назад"),
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
                             InlineKeyboardButton.WithCallbackData("Назад", "назад"),
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
