using BL.ForDA.Interfaces;
using Npgsql;
using BL.DTO;
using BL.Exceptions;
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
        public List<TaskData>? GetDTasks(string discipline_name)
        {
            NpgsqlCommand command = new("select * from public.tasks where discipline_name=@p1", Connection);
            command.Parameters.AddWithValue("p1", discipline_name); // позволяет избежать sql инъекций
            logger.Info($"sql-запрос: {command.CommandText}");
            List<TaskData>? tasks = null;
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                tasks = new List<TaskData>();
                while (reader.Read())
                    tasks.Add(new TaskData(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetDateTime(4), reader.GetInt32(5), reader.GetBoolean(6)));
                //Connection.Close();
                if (tasks.Count == 0)
                    tasks = null;
            }
            catch (NpgsqlException e)
            {
                if (e.Message == "Failed to connect to 127.0.0.1:5432")
                    throw new NoDBConnection();
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

        public List<TaskData>? GetTasks(DateTime from, DateTime to)
        {
            NpgsqlCommand command = new("select * from public.tasks where date>=date(@p1) and date<=date(@p2)", Connection);
            command.Parameters.AddWithValue("p1", $"{from.Year}-{from.Month}-{from.Day}"); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p2", $"{to.Year}-{to.Month}-{to.Day}"); // позволяет избежать sql инъекций
            logger.Info($"sql-запрос: {command.CommandText}"); 
            List<TaskData>? tasks = null;
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                tasks = new List<TaskData>();
                while (reader.Read())
                    tasks.Add(new TaskData(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetDateTime(4), reader.GetInt32(5), reader.GetBoolean(6)));
                //Connection.Close();
                if (tasks.Count == 0)
                    tasks = null;
            }
            catch (NpgsqlException e)
            {
                if (e.Message == "Failed to connect to 127.0.0.1:5432")
                    throw new NoDBConnection();
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

        public TaskData? GetTask(string disciplineName, string taskName)
        {
            NpgsqlCommand command = new("select * from public.tasks where discipline_name=@p1 and name=@p2", Connection);
            command.Parameters.AddWithValue("p1", disciplineName); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p2", taskName); // позволяет избежать sql инъекций
            logger.Info($"sql-запрос: {command.CommandText}"); 
            TaskData? task = null;
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    task = new TaskData(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetDateTime(4), reader.GetInt32(5), reader.GetBoolean(6));
                //Connection.Close();
            }
            catch (NpgsqlException e)
            {
                if (e.Message == "Failed to connect to 127.0.0.1:5432")
                    throw new NoDBConnection();
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

        public List<TaskData>? GetNearestTasks(int days)
        {
            NpgsqlCommand command = new($"select * from public.tasks where date-current_date={days} and finished=false", Connection);
            logger.Info($"sql-запрос: {command.CommandText}"); 
            List<TaskData>? tasks = new List<TaskData>();
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    tasks.Add(new TaskData(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetDateTime(4), reader.GetInt32(5), reader.GetBoolean(6)));
                //Connection.Close();
                if (tasks.Count == 0)
                    tasks = null;
            }
            catch (NpgsqlException e)
            {
                if (e.Message == "Failed to connect to 127.0.0.1:5432")
                    throw new NoDBConnection();
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

        public void CreateTask(TaskData task)
        {
            string updateDiscipline = "update public.disciplines set sum=(select sum(cost) from public.tasks where discipline_name=@p3) where name=@p3;";
            NpgsqlCommand command = new($"begin; insert into public.tasks values ((select coalesce((select id from tasks where name=@p1 and discipline_name=@p3), max(id)+1, 1) from public.tasks), @p1, @p2, @p3, date(@p4), @p5, @p6);{updateDiscipline} commit;", Connection);
            command.Parameters.AddWithValue("p1", task.Name); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p2", task.Description); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p3", task.DisciplineName); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p4", $"{task.Date.Year}-{task.Date.Month}-{task.Date.Day}"); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p5", task.Cost); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p6", task.Finished); // позволяет избежать sql инъекций
            logger.Info($"sql-запрос: {command.CommandText}");
            try
            {
                Connection.Open();
                command.ExecuteNonQuery();
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

        public void UpdateTask(TaskData task)
        {
            string updateDiscipline = "update public.disciplines set sum=(select sum(cost) from public.tasks where discipline_name=@p6) where name=@p6;";
            NpgsqlCommand command = new($"begin; update public.tasks set description=@p1, date=date(@p2), cost=@p3, finished=@p4 where name=@p5 and discipline_name=@p6; {updateDiscipline} commit;", Connection);
            command.Parameters.AddWithValue("p1", task.Description); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p2", $"{task.Date.Year}-{task.Date.Month}-{task.Date.Day}"); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p3", task.Cost); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p4", task.Finished); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p5", task.Name); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p6", task.DisciplineName); // позволяет избежать sql инъекций
            logger.Info($"sql-запрос: {command.CommandText}"); 
            int updatedRows = 0;
            try
            {
                Connection.Open();
                updatedRows = command.ExecuteNonQuery();
                //Connection.Close();
            }
            catch (NpgsqlException e)
            {
                if (e.Message == "Failed to connect to 127.0.0.1:5432")
                    throw new NoDBConnection();
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

        public void DeleteTask(string taskName, string disciplineName)
        {
            string updateDiscipline = "update public.disciplines set sum=(select sum(cost) from public.tasks where discipline_name=@p2) where name=@p2;";
            NpgsqlCommand command = new($"begin; delete from public.tasks where name=@p1 and discipline_name=@p2; {updateDiscipline} commit;", Connection);
            command.Parameters.AddWithValue("p1", taskName); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p2", disciplineName); // позволяет избежать sql инъекций
            logger.Info($"sql-запрос: {command.CommandText}"); 
            int updatedRows = 0;
            try
            {
                Connection.Open();
                updatedRows = command.ExecuteNonQuery();
                //Connection.Close();
            }
            catch (NpgsqlException e)
            {
                if (e.Message == "Failed to connect to 127.0.0.1:5432")
                    throw new NoDBConnection();
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
