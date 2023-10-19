namespace BL.DTO
{
    public class CalendarData
    {
        public int Day { get; set; }
        public string Message { get; set; }
        public CalendarData(int day, string message)
        {
            Day = day;
            Message = $"Осталось {Day} дней, " + message;
        }
        public CalendarData(int day)
        {
            Day = day;
            Message = $"Осталось {Day} дней";
        }
    }
}
