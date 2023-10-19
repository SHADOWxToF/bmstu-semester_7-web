using BL.DTO;
using BL.ForAPI.Interfaces;
namespace GUI
{
    public class GUI : IAPI
    {
        public event EventHandler<EventArgs> EvGetAllCalendars;
        private static MainForm? form;
        public void OnEvGetAllCalendars(EventArgs args)
        {
            EvGetAllCalendars?.Invoke(this, args);
        }

        public event EventHandler<int> EvGetCalendar;
        public void OnEvGetCalendar(int args)
        {
            EvGetCalendar?.Invoke(this, args);
        }

        public event EventHandler<CalendarData> EvCreateCalendar;
        public void OnEvCreateCalendar(CalendarData args)
        {
            EvCreateCalendar?.Invoke(this, args);
        }

        public event EventHandler<CalendarData> EvUpdateCalendar;
        public void OnEvUpdateCalendar(CalendarData args)
        {
            EvUpdateCalendar?.Invoke(this, args);
        }

        public event EventHandler<int> EvDeleteCalendar;
        public void OnEvDeleteCalendar(int args)
        {
            EvDeleteCalendar?.Invoke(this, args);
        }

        public event EventHandler<EventArgsGetTasksFromTo> EvGetTasksFromTo;
        public void OnEvGetTasksFromTo(EventArgsGetTasksFromTo args)
        {
            EvGetTasksFromTo?.Invoke(this, args);
        }

        public event EventHandler<EventArgs> EvGetDisciplines;
        public void OnEvGetDisciplines(EventArgs args)
        {
            EvGetDisciplines?.Invoke(this, args);
        }

        public event EventHandler<string> EvGetDTasks;
        public void OnEvGetDTasks(string args)
        {
            EvGetDTasks?.Invoke(this, args);
        }

        public event EventHandler<string> EvGetDiscipline;
        public void OnEvGetDiscipline(string args)
        {
            EvGetDiscipline?.Invoke(this, args);
        }

        public event EventHandler<DisciplineData> EvCreateDiscipline;
        public void OnEvCreateDiscipline(DisciplineData args)
        {
            EvCreateDiscipline?.Invoke(this, args);
        }

        public event EventHandler<DisciplineData> EvUpdateDiscipline;
        public void OnEvUpdateDiscipline(DisciplineData args)
        {
            EvUpdateDiscipline?.Invoke(this, args);
        }

        public event EventHandler<string> EvDeleteDiscipline;
        public void OnEvDeleteDiscipline(string args)
        {
            EvDeleteDiscipline?.Invoke(this, args);
        }

        public event EventHandler<EventArgsDisciplineTaskNames> EvGetTask;
        public void OnEvGetTask(EventArgsDisciplineTaskNames args)
        {
            EvGetTask?.Invoke(this, args);
        }

        public event EventHandler<TaskData> EvCreateTask;
        public void OnEvCreateTask(TaskData args)
        {
            EvCreateTask?.Invoke(this, args);
        }

        public event EventHandler<TaskData> EvUpdateTask;
        public void OnEvUpdateTask(TaskData args)
        {
            EvUpdateTask?.Invoke(this, args);
        }
        public event EventHandler<EventArgsDisciplineTaskNames> EvDeleteTask;
        public void OnEvDeleteTask(EventArgsDisciplineTaskNames args)
        {
            EvDeleteTask?.Invoke(this, args);
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

        public void CalendarNotification(string notification)
        {
            form?.CalendarNotification(notification);
        }

        public void ShowWarning(string warning)
        {
            form?.ShowWarning(warning);
        }
        public void ShowCalendar(CalendarData calendar)
        {
            form?.ShowCalendar(calendar);
        }
        public void ShowCalendars(List<CalendarData> calendars)
        {
            form?.ShowCalendars(calendars);
        }
        public void ShowTasksFromTo(List<TaskData>? tasks)
        {
            form?.ShowTasksFromTo(tasks);
        }
        public void ShowDisciplines(List<DisciplineData> disciplines)
        {
            form?.ShowDisciplines(disciplines);
        }
        public void ShowDiscipline(DisciplineData discipline)
        {
            form?.ShowDiscipline(discipline);
        }
        public void ShowDTasks(List<TaskData>? tasks)
        {
            form?.ShowDTasks(tasks);
        }
        public void ShowTask(TaskData task)
        {
            form?.ShowTask(task);
        }

        public void ShowHoliday(HolidayData holiday) 
        {
            form?.ShowHoliday(holiday);
        }
        public void ShowHolidaysFromTo(List<HolidayData>? holidays)
        {
            form?.ShowHolidaysFromTo(holidays);
        }


        public void Start()
        {
            form = new MainForm(this);
            int shift = (DateTime.Now.DayOfWeek - DayOfWeek.Monday + 7) % 7;
            DateTime from = DateTime.Now.Subtract(new TimeSpan(shift, 0, 0, 0));
            DateTime to = from.AddDays(27);
            OnEvGetTasksFromTo(new EventArgsGetTasksFromTo(from, to));
            OnEvGetHolidaysFromTo(new EventArgsGetTasksFromTo(from, to));
            //ApplicationConfiguration.Initialize();
            Application.Run(form);
        }
    }
}
