using BL.ForDA.Interfaces;
using Npgsql;
using BL.ForDA.DTO;
using BL.Exceptions;
using System.Threading.Tasks;

namespace DataAccess.DA.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public NpgsqlConnection Connection { get; private set; }
        public NotificationRepository(NpgsqlConnection connection)
        {
            Connection = connection;
        }
        public async Task<List<NotificationData>?> GetNotifications(int userID)
        {
            NpgsqlCommand command = new($"select * from public.notifications where user_id={userID}", Connection);
            logger.Info($"sql-запрос: {command.CommandText}"); 
            List<NotificationData>? notifications = null;
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                notifications = new List<NotificationData>();
                while (await reader.ReadAsync())
                    notifications.Add(new NotificationData(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetString(4)));
                //Connection.Close();
                if (notifications.Count == 0)
                    notifications = null;
            }
            catch (NpgsqlException e)
            {
                if (e.Message == "Failed to connect to 127.0.0.1:5432")
                    throw new NoDBConnection();
                else if (e.SqlState == "25006")
                    throw new NoAccessRight(e);
                else
                    throw new UnpredictableException(e);
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
            finally
            {
                Connection.Close();
            }
            return notifications;

        }

        public async Task<List<NotificationData>?> GetTodayNotifications(int userID)
        {
            NpgsqlCommand command = new($"select notifications.id, task_id, notifications.user_id, days, message from notifications join tasks on notifications.task_id=tasks.id where current_date-tasks.date=notifications.days", Connection);
            logger.Info($"sql-запрос: {command.CommandText}");
            List<NotificationData>? notifications = null;
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                notifications = new List<NotificationData>();
                while (await reader.ReadAsync())
                    notifications.Add(new NotificationData(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetString(4)));
                //Connection.Close();
                if (notifications.Count == 0)
                    notifications = null;
            }
            catch (NpgsqlException e)
            {
                if (e.Message == "Failed to connect to 127.0.0.1:5432")
                    throw new NoDBConnection();
                else if (e.SqlState == "25006")
                    throw new NoAccessRight(e);
                else
                    throw new UnpredictableException(e);
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
            finally
            {
                Connection.Close();
            }
            return notifications;
        }
            public async Task<NotificationData?> GetNotification(int userID, int notificationID)
        {
            NpgsqlCommand command = new($"select * from public.notifications where id={notificationID} and user_id={userID}", Connection);
            logger.Info($"sql-запрос: {command.CommandText}"); 
            NotificationData? notification = null;
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    notification = new NotificationData(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetString(4));
                //Connection.Close();
            }
            catch (NpgsqlException e)
            {
                if (e.Message == "Failed to connect to 127.0.0.1:5432")
                    throw new NoDBConnection();
                else if (e.SqlState == "25006")
                    throw new NoAccessRight(e);
                else
                    throw new UnpredictableException(e);
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
            finally
            {
                Connection.Close();
            }
            return notification;
        }
        public async Task CreateNotification(int userID, NotificationData notification)
        {
            NpgsqlCommand command = new($"insert into public.notifications values (default, {notification.TaskID}, {notification.UserID}, {notification.Days}, @p1)", Connection);
            command.Parameters.AddWithValue("p1", notification.Message); // позволяет избежать sql инъекций
            logger.Info($"sql-запрос: {command.CommandText}"); 
            try
            {
                Connection.Open();
                await command.ExecuteNonQueryAsync();
                //Connection.Close();
            }
            catch (PostgresException e)
            {
                if (e.SqlState == "23505")
                    throw new ExistingName();
                else
                    throw new UnpredictableException(e);
            }
            catch (NpgsqlException e)
            {
                if (e.Message == "Failed to connect to 127.0.0.1:5432")
                    throw new NoDBConnection();
                else if (e.SqlState == "25006")
                    throw new NoAccessRight(e);
                else
                    throw new UnpredictableException(e);
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
            finally
            {
                Connection.Close();
            }
        }
        public async Task UpdateNotification(int userID, NotificationData notification)
        {
            NpgsqlCommand command = new($"update public.notifications set message=@p1, days={notification.Days} where id={notification.ID} and user_id={userID}", Connection);
            command.Parameters.AddWithValue("p1", notification.Message); // позволяет избежать sql инъекций
            logger.Info($"sql-запрос: {command.CommandText}"); 
            int updatedRows = 0;
            try
            {
                Connection.Open();
                updatedRows = await command.ExecuteNonQueryAsync();
                //Connection.Close();
            }
            catch (NpgsqlException e)
            {
                if (e.Message == "Failed to connect to 127.0.0.1:5432")
                    throw new NoDBConnection();
                else if (e.SqlState == "25006")
                    throw new NoAccessRight(e);
                else
                    throw new UnpredictableException(e);
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
            finally
            {
                Connection.Close();
            }
            if (updatedRows == 0)
                throw new NoRecord();
        }
        public async Task DeleteNotification(int userID, int id)
        {
            NpgsqlCommand command = new($"delete from public.notifications where id={id} and user_id={userID}", Connection);
            logger.Info($"sql-запрос: {command.CommandText}"); 
            int updatedRows = 0;
            try
            {
                Connection.Open();
                updatedRows = await command.ExecuteNonQueryAsync();
                //Connection.Close();
            }
            catch (NpgsqlException e)
            {
                if (e.Message == "Failed to connect to 127.0.0.1:5432")
                    throw new NoDBConnection();
                else if (e.SqlState == "25006")
                    throw new NoAccessRight(e);
                else
                    throw new UnpredictableException(e);
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
            finally
            {
                Connection.Close();
            }
            if (updatedRows == 0)
                throw new NoRecord();
        }
    }
}