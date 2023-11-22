//using BL.DTO;

//namespace BL.ForAPI.Interfaces
//{
//    public interface IAPI
//    {
//        public event EventHandler<EventArgs> EvGetAllCalendars;
//        public event EventHandler<int> EvGetCalendar;
//        public event EventHandler<CalendarData> EvCreateCalendar;
//        public event EventHandler<CalendarData> EvUpdateCalendar;
//        public event EventHandler<int> EvDeleteCalendar;

//        public event EventHandler<EventArgsGetTasksFromTo> EvGetTasksFromTo;
//        public event EventHandler<EventArgs> EvGetDisciplines;
//        public event EventHandler<string> EvGetDTasks;
//        public event EventHandler<string> EvGetDiscipline;
//        public event EventHandler<DisciplineData> EvCreateDiscipline;
//        public event EventHandler<DisciplineData> EvUpdateDiscipline;
//        public event EventHandler<string> EvDeleteDiscipline;
//        public event EventHandler<EventArgsDisciplineTaskNames> EvGetTask;
//        public event EventHandler<TaskData> EvCreateTask;
//        public event EventHandler<TaskData> EvUpdateTask;
//        public event EventHandler<EventArgsDisciplineTaskNames> EvDeleteTask;

//        public event EventHandler<EventArgsGetTasksFromTo> EvGetHolidaysFromTo;
//        public event EventHandler<DateTime> EvGetHoliday;
//        public event EventHandler<DateTime> EvDeleteHoliday;

//        public void CalendarNotification(string notification);
//        public void ShowWarning(string warning);
//        public void ShowCalendar(CalendarData calendar);
//        public void ShowCalendars(List<CalendarData> calendars);
//        public void ShowTasksFromTo(List<TaskData>? tasks);
//        public void ShowDisciplines(List<DisciplineData> disciplines);
//        public void ShowDiscipline(DisciplineData discipline);
//        public void ShowDTasks(List<TaskData>? tasks);
//        public void ShowTask(TaskData task);
//        public void Start();
//    }
//}
