using BL.ForAPI.Interfaces;
using BL.DTO;
namespace UI
{
    public class ConsoleUI : IAPI
    {
        public event EventHandler<EventArgs> EvGetAllCalendars;
        public void OnEvGetAllCalendars(EventArgs args)
        {
            EvGetAllCalendars?.Invoke(this, args);
        }
        public void GetAllCalendars()
        {
            OnEvGetAllCalendars(new EventArgs());
        }

        public event EventHandler<int> EvGetCalendar;
        public void OnEvGetCalendar(int args)
        {
            EvGetCalendar?.Invoke(this, args);
        }
        public void GetCalendar()
        {
            int days = ReadInt32("Введите количество дней до оповещания");
            while (days < 1)
            {
                Console.WriteLine("Количество дней должно быть больше 1");
                days = ReadInt32("Повторите ввод");
            }
            OnEvGetCalendar(days);
        }

        public event EventHandler<CalendarData> EvCreateCalendar;
        public void OnEvCreateCalendar(CalendarData args)
        {
            EvCreateCalendar?.Invoke(this, args);
        }
        public void CreateCalendar()
        {
            int days = ReadInt32("Введите количество дней до оповещания");
            while (days < 1)
            {
                Console.WriteLine("Количество дней должно быть больше 1");
                days = ReadInt32("Повторите ввод");
            }
            string message = ReadString("Введите текст оповещания");
            OnEvCreateCalendar(new CalendarData(days, message));
        }

        public event EventHandler<CalendarData> EvUpdateCalendar;
        public void OnEvUpdateCalendar(CalendarData args)
        {
            EvUpdateCalendar?.Invoke(this, args);
        }
        public void UpdateCalendar()
        {
            int days = ReadInt32("Введите количество дней до оповещания");
            while (days < 1)
            {
                Console.WriteLine("Количество дней должно быть больше 1");
                days = ReadInt32("Повторите ввод");
            }
            string message = ReadString("Введите текст оповещания");
            OnEvUpdateCalendar(new CalendarData(days, message));
        }

        public event EventHandler<int> EvDeleteCalendar;
        public void OnEvDeleteCalendar(int args)
        {
            EvDeleteCalendar?.Invoke(this, args);
        }
        public void DeleteCalendar()
        {
            int days = ReadInt32("Введите количество дней до оповещания");
            while (days < 1)
            {
                Console.WriteLine("Количество дней должно быть не меньше 1");
                days = ReadInt32("Повторите ввод");
            }
            OnEvDeleteCalendar(days);
        }

        public event EventHandler<EventArgsGetTasksFromTo> EvGetTasksFromTo;
        public void OnEvGetTasksFromTo(EventArgsGetTasksFromTo args)
        {
            EvGetTasksFromTo?.Invoke(this, args);
        }
        public void GetTasksFromTo()
        {
            DateTime from = ReadDate("Введите дату начала поиска задач");
            DateTime to = ReadDate("Введите дату окончания поиска задач");
            OnEvGetTasksFromTo(new EventArgsGetTasksFromTo(from, to));
        }

        public event EventHandler<EventArgs> EvGetDisciplines;
        public void OnEvGetDisciplines(EventArgs args)
        {
            EvGetDisciplines?.Invoke(this, args);
        }
        public void GetDisciplines()
        {
            OnEvGetDisciplines(new EventArgs());
        }

        public event EventHandler<string> EvGetDTasks;
        public void OnEvGetDTasks(string args)
        {
            EvGetDTasks?.Invoke(this, args);
        }
        public void GetDTasks()
        {
            string dName = ReadString("Введите название дисциплины");
            OnEvGetDTasks(dName);
        }

        public event EventHandler<string> EvGetDiscipline;
        public event EventHandler<DisciplineData> EvCreateDiscipline;
        public void OnEvCreateDiscipline(DisciplineData args)
        {
            EvCreateDiscipline?.Invoke(this, args);
        }
        public void CreateDiscipline()
        {
            string name = ReadString("Введите название дисциплины");
            string description = ReadString("Введите описание дисциплины");
            OnEvCreateDiscipline(new DisciplineData(name, description, 0));
        }

        public event EventHandler<DisciplineData> EvUpdateDiscipline;
        public void OnEvUpdateDiscipline(DisciplineData args)
        {
            EvUpdateDiscipline?.Invoke(this, args);
        }
        public void UpdateDiscipline()
        {
            string name = ReadString("Введите название дисциплины");
            string description = ReadString("Введите описание дисциплины");
            OnEvUpdateDiscipline(new DisciplineData(name, description, 0));
        }

        public event EventHandler<string> EvDeleteDiscipline;
        public void OnEvDeleteDiscipline(string args)
        {
            EvDeleteDiscipline?.Invoke(this, args);
        }
        public void DeleteDiscipline()
        {
            string name = ReadString("Введите название дисциплины");
            OnEvDeleteDiscipline(name);
        }

        public event EventHandler<EventArgsDisciplineTaskNames> EvGetTask;
        public void OnEvGetTask(EventArgsDisciplineTaskNames args)
        {
            EvGetTask?.Invoke(this, args);
        }
        public void GetTask()
        {
            string tName = ReadString("Введите название задачи");
            string dName = ReadString("Введите название дисциплины");
            OnEvGetTask(new EventArgsDisciplineTaskNames(tName, dName));
        }

        public event EventHandler<TaskData> EvCreateTask;
        public void OnEvCreateTask(TaskData args)
        {
            EvCreateTask?.Invoke(this, args);
        }
        public void CreateTask()
        {
            string tName = ReadString("Введите название задачи");
            string description = ReadString("Введите описание задачи");
            string dName = ReadString("Введите название дисциплины");
            DateTime date = ReadDate("Введите срок окончания задачи");
            int cost = ReadInt32("Введите стоимость задачи в баллах");
            bool finished = ReadBool("Задача завершена?");
            OnEvCreateTask(new TaskData(0, tName, description, dName, date, cost, finished));
        }

        public event EventHandler<TaskData> EvUpdateTask;
        public void OnEvUpdateTask(TaskData args)
        {
            EvUpdateTask?.Invoke(this, args);
        }
        public void UpdateTask()
        {
            string tName = ReadString("Введите название задачи");
            string description = ReadString("Введите описание задачи");
            string dName = ReadString("Введите название дисциплины");
            DateTime date = ReadDate("Введите срок окончания задачи");
            int cost = ReadInt32("Введите стоимость задачи в баллах");
            bool finished = ReadBool("Задача завершена?");
            OnEvUpdateTask(new TaskData(0, tName, description, dName, date, cost, finished));
        }
        public event EventHandler<EventArgsDisciplineTaskNames> EvDeleteTask;
        public void OnEvDeleteTask(EventArgsDisciplineTaskNames args)
        {
            EvDeleteTask?.Invoke(this, args);
        }
        public void DeleteTask()
        {
            string tName = ReadString("Введите название задачи");
            string dName = ReadString("Введите название дисциплины");
            OnEvDeleteTask(new EventArgsDisciplineTaskNames(tName, dName));
        }

        public event EventHandler<EventArgsGetTasksFromTo> EvGetHolidaysFromTo;
        public void OnEvGetHolidaysFromTo(EventArgsGetTasksFromTo args)
        {
            EvGetHolidaysFromTo?.Invoke(this, args);
        }
        public event EventHandler<DateTime> EvGetHoliday;
        public void OnEvGetHoliday(DateTime args)
        {
            EvGetHoliday?.Invoke(this, args);
        }
        public event EventHandler<HolidayData> EvCreateHoliday;
        public void OnEvCreateHoliday(HolidayData args)
        {
            EvCreateHoliday?.Invoke(this, args);
        }
        public event EventHandler<HolidayData> EvUpdateHoliday;
        public void OnEvUpdateHoliday(HolidayData args)
        {
            EvUpdateHoliday?.Invoke(this, args);
        }
        public event EventHandler<DateTime> EvDeleteHoliday;
        public void OnEvDeleteHoliday(DateTime args)
        {
            EvDeleteHoliday?.Invoke(this, args);
        }

        private string? notification;
        public void CalendarNotification(string notification)
        {
            this.notification = notification;
        }
        
        public void ShowWarning(string warning)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"Вызван метод showWarning: {warning}");
            Console.WriteLine($"Предупреждение!!!\n{warning}\n");
        }
        public void ShowCalendar(CalendarData calendar)
        {
            Console.WriteLine($"Текст оповещания: {calendar.Message}");
        }
        public void ShowCalendars(List<CalendarData> calendars)
        {
            foreach (CalendarData calendar in calendars)
                Console.WriteLine($"\tКоличество дней до оповещания: {calendar.Day}\n\tТекст оповещания: {calendar.Message}\n");
        }
        public void ShowTasksFromTo(List<TaskData>? tasks)
        {
            if (tasks == null)
                Console.WriteLine("Нет задач назначенных на эти даты");
            else
            {
                tasks.Sort((TaskData task1, TaskData task2) => task1.Date.CompareTo(task2.Date));
                DateTime date = tasks[0].Date;
                Console.WriteLine($"Задачи назначенные на {date.Day}.{date.Month}.{date.Year}:");
                foreach (TaskData task in tasks)
                {
                    var finished = task.Finished ? "да" : "нет";
                    if (date == task.Date)
                    {
                        Console.WriteLine($"\tназвание: {task.Name}\n\tназвание дисциплины: {task.DisciplineName}\n\tописание: {task.Description}\n\tколичество баллов: {task.Cost}\n\tзавершено: {finished}\n");
                    }
                    else
                    {
                        date = task.Date;
                        Console.WriteLine($"\nЗадачи назначенные на {date.Day}.{date.Month}.{date.Year}:");
                        Console.WriteLine($"\tназвание: {task.Name}\n\tназвание дисциплины: {task.DisciplineName}\n\tописание: {task.Description}\n\tколичество баллов: {task.Cost}\n\tзавершено: {finished}\n");
                    }
                }
            }
        }
        public void ShowDisciplines(List<DisciplineData> disciplines)
        {
            foreach (DisciplineData discipline in disciplines)
                Console.WriteLine($"\tназвание: {discipline.Name}\n\tописание: {discipline.Description}\n\tсумма баллов: {discipline.Sum}\n");
        }
        public void ShowDiscipline(DisciplineData discipline)
        {
            Console.WriteLine($"\tназвание: {discipline.Name}\n\tописание: {discipline.Description}\n\tсумма баллов: {discipline.Sum}\n");
        }
        public void ShowDTasks(List<TaskData>? tasks)
        {
            if (tasks == null)
                Console.WriteLine("Для данной дисциплины ещё не создано задач");
            else
            {
                Console.WriteLine($"\nСписок задач для дисциплины \"{tasks[0].DisciplineName}\":");
                foreach (var task in tasks)
                {
                    var finished = task.Finished ? "да" : "нет";
                    Console.WriteLine($"\tназвание: {task.Name}\n\tописание: {task.Description}\n\tсрок: {task.Date.Day}.{task.Date.Month}.{task.Date.Year}\n\tколичество баллов: {task.Cost}\n\tзавершено: {finished}\n");
                }
            }
        }
        public void ShowTask(TaskData task)
        {
            var finished = task.Finished ? "да" : "нет";
            Console.WriteLine($"\tописание: {task.Description}\n\tколичество баллов: {task.Cost}\n\tзавершено: {finished}\n");
        }

        public void ShowHoliday(HolidayData holiday)
        {
            Console.WriteLine("ShowHoliday");
        }
        public void ShowHolidaysFromTo(List<HolidayData>? holidays)
        {
            Console.WriteLine("ShowHolidaysFromTo");
        }

        public void Start()
        {
            Menu();
        }
        private void Menu()
        {
            int choice = -1;
            while (choice != 0)
            {
                if (notification != null)
                {
                    Console.WriteLine($"Вам оповещание:\n{notification}");
                    notification = null;
                }
                Console.WriteLine("\nВыберите необходимую задачу и введите её номер для запуска.");
                Console.WriteLine("1 - отобразить все оповещания");
                Console.WriteLine("2 - отобразить сообщение определённого оповещания");
                Console.WriteLine("3 - создать оповещание");
                Console.WriteLine("4 - изменить сообщение существующего оповещания");
                Console.WriteLine("5 - удалить оповещание");
                Console.WriteLine("6 - отобразить данные всех дисциплин");
                Console.WriteLine("7 - создать дисциплину");
                Console.WriteLine("8 - изменить существующую дисциплину");
                Console.WriteLine("9 - удалить дисциплину");
                Console.WriteLine("10 - отобразить задачи определённого временного промежутка");
                Console.WriteLine("11 - отобразить задачи определённой дисциплины");
                Console.WriteLine("12 - отобразить данные определённой задачи");
                Console.WriteLine("13 - создать задачу");
                Console.WriteLine("14 - изменить существующую задачу");
                Console.WriteLine("15 - удалить задачу");
                Console.WriteLine("0 - выход");
                Console.Write("\nВаш выбор: ");
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    choice = -1;
                }
                try
                {

                    switch (choice)
                    {
                        case 1:
                            choice = -1;
                            GetAllCalendars();
                            break;
                        case 2:
                            choice = -1;
                            GetCalendar();
                            break;
                        case 3:
                            choice = -1;
                            CreateCalendar();
                            break;
                        case 4:
                            choice = -1;
                            UpdateCalendar();
                            break;
                        case 5:
                            choice = -1;
                            DeleteCalendar();
                            break;
                        case 6:
                            choice = -1;
                            GetDisciplines();
                            break;
                        case 7:
                            choice = -1;
                            CreateDiscipline();
                            break;
                        case 8:
                            choice = -1;
                            UpdateDiscipline();
                            break;
                        case 9:
                            choice = -1;
                            DeleteDiscipline();
                            break;
                        case 10:
                            choice = -1;
                            GetTasksFromTo();
                            break;
                        case 11:
                            choice = -1;
                            GetDTasks();
                            break;
                        case 12:
                            choice = -1;
                            GetTask();
                            break;
                        case 13:
                            choice = -1;
                            CreateTask();
                            break;
                        case 14:
                            choice = -1;
                            UpdateTask();
                            break;
                        case 15:
                            choice = -1;
                            DeleteTask();
                            break;
                        case 0:
                            break;

                        default:
                            Console.WriteLine("\nНеверный ввод, повторите попытку!!!\n");
                            break;
                    }
                    if (choice == -1)
                    {
                        Console.Write("\nНажмите клавишу enter для продолжения...");
                        Console.ReadLine();
                    }
                }
                catch (Exception e)
                {
                    NLog.LogManager.GetCurrentClassLogger().Warn(e, "Неизвестный тип ошибки");
                    Console.WriteLine($"Упс, что-то пошло не так: {e}");
                }
            }
        }

        private int ReadInt32(string message)
        {
            Console.Write($"{message}: ");
            int number = 0;
            bool fl = false;
            try
            {
                number = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                fl = true;
            }
            while (fl)
            {
                Console.Write("Неверный ввод. Попробуйте ещё раз: ");
                try
                {
                    number = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    fl = true;
                }
            }
            return number;
        }

        private string ReadString(string message)
        {
            Console.Write($"{message}: ");
            string? rString = Console.ReadLine();
            while (rString == null || rString.Length == 0)
            {
                Console.Write("Неверный ввод. Попробуйте ещё раз: ");
                rString = Console.ReadLine();
            }
            return rString;
        }
        private DateTime ReadDate(string message)
        {
            Console.Write($"{message} (DD/MM/YYYY): ");
            DateTime date = new();
            string? rString = Console.ReadLine();
            try
            {
                if (rString != null)
                    date = DateTime.Parse(rString);
            }
            catch
            {
                rString = null;
            }
            while (rString == null)
            {
                Console.Write("Неверный ввод. Попробуйте ещё раз: ");
                rString = Console.ReadLine();
                try
                {
                    if (rString != null)
                        date = DateTime.Parse(rString);
                }
                catch
                {
                    rString = null;
                }
            }
            return date;
        }
        public bool ReadBool(string message)
        {
            Console.Write($"{message} (да/нет): ");
            string? rString = Console.ReadLine();
            while (rString == null || (rString != "да" && rString != "нет"))
            {
                Console.Write("Неверный ввод. Попробуйте ещё раз: ");
                rString = Console.ReadLine();
            }
            return rString == "да";
        }
    }
}
