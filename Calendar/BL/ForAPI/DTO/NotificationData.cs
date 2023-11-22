namespace BL.ForAPI.DTO
{
    public class NotificationData
    {
        public int ID { get; }
        public int TaskID { get; }
        public int UserID { get; }
        public int Days { get; }
        public string Message { get; }
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
