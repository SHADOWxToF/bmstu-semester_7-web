using BL.DTO;
using BL.ForDA.Interfaces;
using BL.Models.Interfaces;

namespace BL.Models.Implementations
{
    public class Holiday : IHoliday
    {
        private IHolidayRepository holidayRepository;

        public Holiday(IHolidayRepository holidayRepository)
        {
            this.holidayRepository = holidayRepository;
        }
        public List<HolidayData>? GetHolidays(DateTime from, DateTime to)
        {
            return holidayRepository.GetHolidays(from, to);
        }
        public HolidayData? GetHoliday(DateTime date)
        {
            return holidayRepository.GetHoliday(date);
        }
        public void CreateHoliday(HolidayData holiday)
        {
            holidayRepository.CreateHoliday(holiday);
        }
        public void UpdateHoliday(HolidayData holiday)
        {
            holidayRepository.UpdateHoliday(holiday);
        }
        public void DeleteHoliday(DateTime date) 
        {
            holidayRepository.DeleteHoliday(date);
        }
    }
}
