namespace BL.DTO
{
    public class HolidayData
    {
        public DateTime Date { get; set; } = new DateTime();
        public string Message { get; set; } = "";
        public HolidayData(DateTime date, string message)
        {
            Date = date;
            Message = message;
        }
    }
}
