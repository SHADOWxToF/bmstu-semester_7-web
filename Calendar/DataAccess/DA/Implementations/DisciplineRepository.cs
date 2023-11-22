using BL.ForDA.Interfaces;
using Npgsql;
using BL.ForDA.DTO;
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
        public async Task<List<DisciplineData>?> GetDisciplines(int userID)
        {
            NpgsqlCommand command = new($"select * from public.disciplines where user_id={userID}", Connection);
            List<DisciplineData>? disciplines = null;
            logger.Info($"sql-запрос: {command.CommandText}"); 
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                //NpgsqlDataReader reader = command.ExecuteReader();
                disciplines = new List<DisciplineData>();
                while (await reader.ReadAsync())
                    disciplines.Add(new DisciplineData(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3)));
                //Connection.Close();
                if (disciplines.Count == 0)
                    disciplines = null;
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
            return disciplines;
        }
        public async Task<DisciplineData?> GetDiscipline(int userID, int disciplineID) 
        {
            NpgsqlCommand command = new($"select * from public.disciplines where id={disciplineID} and user_id={userID}", Connection);
            logger.Info($"sql-запрос: {command.CommandText}");
            DisciplineData? discipline = null;
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    discipline = new DisciplineData(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3));
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
            return discipline;
        }
        public async Task CreateDiscipline(DisciplineData discipline) 
        {
            NpgsqlCommand command = new($"insert into public.disciplines values (default, {discipline.UID}, @p1, @p2)", Connection);
            command.Parameters.AddWithValue("p1", discipline.Name); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p2", discipline.Description); // позволяет избежать sql инъекций
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
        public async Task UpdateDiscipline(DisciplineData discipline) 
        {
            NpgsqlCommand command = new($"update public.disciplines set description=@p1 where id={discipline.ID} and user_id={discipline.UID}", Connection);
            command.Parameters.AddWithValue("p1", discipline.Description); // позволяет избежать sql инъекций
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
        public async Task DeleteDiscipline(int userID, int disciplineID) 
        {
            string deletetasks = $"delete from public.tasks where discipline_id={disciplineID} and user_id={userID};";
            NpgsqlCommand command = new($"begin; {deletetasks}delete from public.disciplines where id={disciplineID} and user_id={userID};commit;", Connection);
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
