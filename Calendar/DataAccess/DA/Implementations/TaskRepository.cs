using BL.ForDA.Interfaces;
using Npgsql;
using BL.ForDA.DTO;
using BL.Exceptions;
using BL.Models.Implementations;

namespace DataAccess.DA.Implementations
{
    public class TaskRepository : ITaskRepository
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public NpgsqlConnection Connection { get; private set; }
        public TaskRepository(NpgsqlConnection connection)
        {
            Connection = connection;
        }
        public async Task<List<TaskData>?> GetDTasks(int userID, int disciplineID)
        {
            NpgsqlCommand command = new($"select * from public.tasks where discipline_id={disciplineID}", Connection);
            logger.Info($"sql-запрос: {command.CommandText}");
            List<TaskData>? tasks = null;
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                tasks = new List<TaskData>();
                while (await reader.ReadAsync())
                    tasks.Add(new TaskData(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetDateTime(5), reader.GetInt32(6), reader.GetBoolean(7)));
                //Connection.Close();
                if (tasks.Count == 0)
                    tasks = null;
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
            return tasks;
        }

        public async Task<List<TaskData>?> GetTasks(int userID, DateTime from, DateTime to)
        {
            NpgsqlCommand command = new("select * from public.tasks where date>=date(@p1) and date<=date(@p2)", Connection);
            command.Parameters.AddWithValue("p1", $"{from.Year}-{from.Month}-{from.Day}"); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p2", $"{to.Year}-{to.Month}-{to.Day}"); // позволяет избежать sql инъекций
            logger.Info($"sql-запрос: {command.CommandText}"); 
            List<TaskData>? tasks = null;
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                tasks = new List<TaskData>();
                while (await reader.ReadAsync())
                    tasks.Add(new TaskData(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetDateTime(5), reader.GetInt32(6), reader.GetBoolean(7)));
                //Connection.Close();
                if (tasks.Count == 0)
                    tasks = null;
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
            return tasks;
        }

        public async Task<List<TaskData>?> GetTasks(int userID)
        {
            NpgsqlCommand command = new($"select * from public.tasks where user_id={userID}", Connection);
            logger.Info($"sql-запрос: {command.CommandText}");
            List<TaskData>? tasks = null;
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                tasks = new List<TaskData>();
                while (await reader.ReadAsync())
                    tasks.Add(new TaskData(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetDateTime(5), reader.GetInt32(6), reader.GetBoolean(7)));
                //Connection.Close();
                if (tasks.Count == 0)
                    tasks = null;
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
            return tasks;
        }

        public async Task<TaskData?> GetTask(int userID, int taskID)
        {
            NpgsqlCommand command = new($"select * from public.tasks where id={taskID} and user_id={userID}", Connection);
            logger.Info($"sql-запрос: {command.CommandText}"); 
            TaskData? task = null;
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    task = new TaskData(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetDateTime(5), reader.GetInt32(6), reader.GetBoolean(7));
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
            return task;
        }

        public async Task<List<TaskData>?> GetNearestTasks(int userID, int days)
        {
            NpgsqlCommand command = new($"select * from public.tasks where date-current_date={days} and finished=false and user_id={userID}", Connection);
            logger.Info($"sql-запрос: {command.CommandText}"); 
            List<TaskData>? tasks = new List<TaskData>();
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    tasks.Add(new TaskData(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetDateTime(5), reader.GetInt32(6), reader.GetBoolean(7)));
                //Connection.Close();
                if (tasks.Count == 0)
                    tasks = null;
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
            return tasks;
        }

        public async Task CreateTask(TaskData task)
        {
            NpgsqlCommand command = new($"insert into public.tasks values (default, @p1, @p2, @p3, @p4, @p5, @p6, @p7)", Connection);
            command.Parameters.AddWithValue("p1", task.DisciplineID); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p2", task.UserID); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p3", task.Name); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p4", task.Description); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p5", $"{task.Date.Year}-{task.Date.Month}-{task.Date.Day}"); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p6", task.Cost); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p7", task.Finished); // позволяет избежать sql инъекций
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
                else if(e.SqlState == "23503")
                    throw new NoRecord();
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

        public async Task UpdateTask(TaskData task)
        {
            NpgsqlCommand command = new($"update public.tasks set name=@p1, description=@p2, date=date(@p3), cost=@p4, finished=@p5 where id={task.ID} and user_id={task.UserID}", Connection);
            command.Parameters.AddWithValue("p1", task.Name); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p2", task.Description); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p3", $"{task.Date.Year}-{task.Date.Month}-{task.Date.Day}"); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p4", task.Cost); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p5", task.Finished); // позволяет избежать sql инъекций
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
            if (updatedRows <= 1)
                throw new NoRecord();
        }

        public async Task DeleteTask(int userID, int taskID)
        {
            string deletenotifications = $"delete from public.notifications where task_id={taskID}";
            NpgsqlCommand command = new($"begin; delete from public.tasks where id={taskID} and user_id={userID}; {deletenotifications}; commit;", Connection);
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
