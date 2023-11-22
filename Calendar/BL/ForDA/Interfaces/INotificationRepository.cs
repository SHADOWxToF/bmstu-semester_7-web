using BL.ForDA.DTO;

namespace BL.ForDA.Interfaces
{
    public interface INotificationRepository
    {
        public Task<List<NotificationData>?> GetNotifications(int userID);
        public Task<List<NotificationData>?> GetTodayNotifications(int userID);
        public Task<NotificationData?> GetNotification(int userID, int notificationID);
        public Task CreateNotification(int userID, NotificationData notification);
        public Task UpdateNotification(int userID, NotificationData notification);
        public Task DeleteNotification(int userID, int id);
    }
}