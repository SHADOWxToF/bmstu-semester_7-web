using BL.DTO;

namespace BL.ForDA.Interfaces
{
    public interface IHolidayRepository
    {
        public List<HolidayData>? GetHolidays(DateTime from, DateTime to);
        public HolidayData? GetHoliday(DateTime date);
        public void CreateHoliday(HolidayData holiday);
        public void UpdateHoliday(HolidayData holiday);
        public void DeleteHoliday(DateTime date);
    }
}
