namespace BL.ForDA.DTO
{
    public class NotificationData
    {
        public int ID {  get; set; }
        public int TaskID { get; set; }
        public int UserID { get; set; }
        public int Days { get; set; }
        public string Message { get; set; }
        public NotificationData(int id, int taskID, int userID, int days, string message)
        {
            ID = id;
            TaskID = taskID;
            UserID = userID;
            Days = days;
            Message = message;
        }
    }
}
