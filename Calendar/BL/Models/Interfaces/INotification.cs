using BL.ForAPI.DTO;
namespace BL.Models.Interfaces
{
    public interface INotification
    {
        public Task<List<NotificationData>?> GetNotifications(int userID);
        public Task<List<NotificationData>?> GetTodayNotifications(int userID);
        public Task<NotificationData?> GetNotification(int userID, int notificationID);
        public Task CreateNotification(int userID, NotificationData calendar);
        public Task UpdateNotification(int userID, NotificationData calendar);
        public Task DeleteNotification(int userID, int id);
        public Task<List<TaskData>?> GetNearestTasks(int userID, int days);
    }
}
