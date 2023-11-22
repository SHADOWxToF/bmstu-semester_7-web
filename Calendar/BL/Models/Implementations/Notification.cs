using BL.ForDA.Interfaces;
using BL.ForAPI.DTO;
using BL.Models.Interfaces;
using BL.Converters;
namespace BL.Models.Implementations
{
    public class Notification : INotification
    {
        private IDisciplineRepository disciplineRepository;
        private ITaskRepository taskRepository;
        private INotificationRepository notificationRepository;

        public Notification(IDisciplineRepository disciplineRepository, ITaskRepository taskRepository, INotificationRepository notificationRepository)
        {
            this.disciplineRepository = disciplineRepository;
            this.taskRepository = taskRepository;
            this.notificationRepository = notificationRepository;
        }

        public async Task<List<NotificationData>?> GetNotifications(int userID)
        {
            return NotificationConverter.ConvertFromDAToAPI(await notificationRepository.GetNotifications(userID));
        }

        public async Task<List<NotificationData>?> GetTodayNotifications(int userID)
        {
            return NotificationConverter.ConvertFromDAToAPI(await notificationRepository.GetTodayNotifications(userID));
        }

        public async Task<NotificationData?> GetNotification(int userID, int notificationID)
        {
            return NotificationConverter.ConvertFromDAToAPI(await notificationRepository.GetNotification(userID, notificationID));
        }

        public async Task CreateNotification(int userID, NotificationData notification)
        {
            await notificationRepository.CreateNotification(userID, NotificationConverter.ConvertFromAPIToDA(notification));
        }

        public async Task UpdateNotification(int userID, NotificationData notification)
        {
            await notificationRepository.UpdateNotification(userID, NotificationConverter.ConvertFromAPIToDA(notification));
        }

        public async Task DeleteNotification(int userID, int id)
        {
            await notificationRepository.DeleteNotification(userID, id);
        }

        public async Task<List<TaskData>?> GetNearestTasks(int userID, int days)
        {
            return TaskConverter.ConvertFromDAToAPI(await taskRepository.GetNearestTasks(userID, days));
        }
    }

}
