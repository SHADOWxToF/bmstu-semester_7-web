using BL.ForDA.Interfaces;
using BL.DTO;
using BL.Models.Interfaces;
namespace BL.Models.Implementations
{
    public class Calendar : ICalendar
    {
        private IDisciplineRepository disciplineRepository;
        private ITaskRepository taskRepository;
        private ICalendarRepository calendarRepository;

        public Calendar(IDisciplineRepository disciplineRepository, ITaskRepository taskRepository, ICalendarRepository calendarRepository)
        {
            this.disciplineRepository = disciplineRepository;
            this.taskRepository = taskRepository;
            this.calendarRepository = calendarRepository;
        }

        public List<CalendarData>? GetAllCalendars()
        {
            return calendarRepository.GetAllCalendars();
        }

        public CalendarData? GetCalendar(int days)
        {
            return calendarRepository.GetCalendar(days);
        }

        public void CreateCalendar(CalendarData calendar)
        {
            calendarRepository.CreateCalendar(calendar);
        }

        public void UpdateCalendar(CalendarData calendar)
        {
            calendarRepository.UpdateCalendar(calendar);
        }

        public void DeleteCalendar(int days)
        {
            calendarRepository.DeleteCalendar(days);
        }

        public List<TaskData>? GetNearestTasks(int days)
        {
            return taskRepository.GetNearestTasks(days);
        }
    }

}
