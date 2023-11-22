using BL.ForAPI.DTO;
using BL.ForAPI.Interfaces;
using BL.Models.Interfaces;
using BL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WEBServer.APIControllers.Implementations
{
    [Authorize]
    [ApiController]
    [Route("api/users/{userID}/notifications")]
    public class NotificationController : ControllerBase
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private INotification notification;
        public NotificationController(INotification notification)
        {
            this.notification = notification;
        }

        [HttpGet]
        public async Task<ActionResult<List<NotificationData>>> GetAllNotifications(int userID, bool today=false)
        {
            int responceUserID = Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (responceUserID != userID)
                return StatusCode(403);
            List<NotificationData>? notifications = null;
            try
            {
                if (today)
                    notifications = await notification.GetTodayNotifications(userID);
                else
                    notifications = await notification.GetNotifications(userID);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }
            if (notifications != null)
                return notifications;
            else
                return new List<NotificationData>();
        }

        [HttpPost]
        public async Task<ActionResult> CreateNotification(int userID, NotificationData notificationData)
        {
            int responceUserID = Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (responceUserID != userID || userID != notificationData.UserID)
                return StatusCode(403);
            try
            {
                await notification.CreateNotification(userID, notificationData);
                return StatusCode(201);
            }
            catch (ExistingName)
            {
                return StatusCode(409);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }
        }
        [HttpGet("{notificationID}")]
        public async Task<ActionResult<NotificationData>> GetNotification(int userID, int notificationID)
        {
            int responceUserID = Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (responceUserID != userID)
                return StatusCode(403);
            NotificationData? notificationData = null;
            try
            {
                notificationData = await notification.GetNotification(userID, notificationID);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }
            if (notificationData != null)
                return notificationData;
            else
                return StatusCode(404);
        }

        [HttpPut("{notificationID}")]
        public async Task<ActionResult> UpdateNotification(int userID, int notificationID, NotificationData notificationData)
        {
            int responceUserID = Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (responceUserID != userID || userID != notificationData.UserID)
                return StatusCode(403);
            try
            {
                await notification.UpdateNotification(userID, notificationData);
                return StatusCode(200);
            }
            catch (NoRecord)
            {
                return StatusCode(409);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }
        }

        [HttpDelete("{notificationID}")]
        public async Task<ActionResult> DeleteNotification(int userID, int notificationID)
        {
            int responceUserID = Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (responceUserID != userID)
                return StatusCode(403);
            try
            {
                await notification.DeleteNotification(userID, notificationID);
                return StatusCode(200);
            }
            catch (NoRecord e)
            {
                logger.Info(e, "Нет оповещания в БД");
                return StatusCode(409);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }
        }
    }
}
