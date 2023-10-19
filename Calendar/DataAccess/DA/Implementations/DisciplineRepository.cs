using BL.ForDA.Interfaces;
using Npgsql;
using BL.DTO;
using BL.Exceptions;
namespace DataAccess.DA.Implementations
{
    public class DisciplineRepository : IDisciplineRepository
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public NpgsqlConnection Connection { get; private set; }
        public DisciplineRepository(NpgsqlConnection connection)
        {
            Connection = connection;
        }
        public List<DisciplineData>? GetDisciplines()
        {
            NpgsqlCommand command = new("select * from public.disciplines", Connection);
            List<DisciplineData>? disciplines = null;
            logger.Info($"sql-запрос: {command.CommandText}"); 
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                disciplines = new List<DisciplineData>();
                while (reader.Read())
                    disciplines.Add(new DisciplineData(reader.GetString(0), reader.GetString(1), reader.GetInt32(2)));
                //Connection.Close();
                if (disciplines.Count == 0)
                    disciplines = null;
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
            return disciplines;
        }
        public DisciplineData? GetDiscipline(string disciplineName) 
        {
            NpgsqlCommand command = new($"select * from public.disciplines where name=@p1", Connection);
            command.Parameters.AddWithValue("p1", disciplineName); // позволяет избежать sql инъекций
            logger.Info($"sql-запрос: {command.CommandText}");
            DisciplineData? discipline = null;
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    discipline = new DisciplineData(disciplineName, reader.GetString(1), reader.GetInt32(2));
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
            return discipline;
        }
        public void CreateDiscipline(DisciplineData discipline) 
        {
            NpgsqlCommand command = new($"insert into public.disciplines values (@p1, @p2, {discipline.Sum})", Connection);
            command.Parameters.AddWithValue("p1", discipline.Name); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p2", discipline.Description); // позволяет избежать sql инъекций
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
        public void UpdateDiscipline(DisciplineData discipline) 
        {
            NpgsqlCommand command = new($"update public.disciplines set description=@p1, sum={discipline.Sum} where name=@p2", Connection);
            command.Parameters.AddWithValue("p1", discipline.Description); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p2", discipline.Name); // позволяет избежать sql инъекций
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
        public void DeleteDiscipline(string disciplineName) 
        {
            string deletetasks = "delete from public.tasks where discipline_name=@p1;";
            NpgsqlCommand command = new($"begin; {deletetasks}delete from public.disciplines where name=@p1;commit;", Connection);
            command.Parameters.AddWithValue("p1", disciplineName); // позволяет избежать sql инъекций
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
