using BL.DTO;
namespace BL.Models.Interfaces
{
    public interface ICalendar
    {
        public List<CalendarData>? GetAllCalendars();
        public CalendarData? GetCalendar(int days);
        public void CreateCalendar(CalendarData calendar);
        public void UpdateCalendar(CalendarData calendar);
        public void DeleteCalendar(int days);
        public List<TaskData>? GetNearestTasks(int days);
    }
}
