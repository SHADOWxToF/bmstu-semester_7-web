using BL.DTO;

namespace BL.ForDA.Interfaces
{
    public interface ICalendarRepository
    {
        public List<CalendarData>? GetAllCalendars();
        public CalendarData? GetCalendar(int days);
        public void CreateCalendar(CalendarData calendar);
        public void UpdateCalendar(CalendarData calendar);
        public void DeleteCalendar(int days);
    }
}