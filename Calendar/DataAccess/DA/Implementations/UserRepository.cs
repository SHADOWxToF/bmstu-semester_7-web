using BL.ForDA.Interfaces;
using BL.ForDA.DTO;
using Npgsql;
using BL.Exceptions;
using System.Threading.Tasks;

namespace DataAccess.DA.Implementations
{
    public class UserRepository : IUserRepository
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public NpgsqlConnection Connection { get; private set; }
        public UserRepository(NpgsqlConnection connection)
        {
            Connection = connection;
        }
        public async Task<UserData?> GetUser(UserData userData)
        {
            NpgsqlCommand command = new($"select * from public.users where login=@p1 and password=@p2", Connection);
            command.Parameters.AddWithValue("p1", userData.Login); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p2", userData.Password); // позволяет избежать sql инъекций
            logger.Info($"sql-запрос: {command.CommandText}");
            UserData? user = null;
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    user = new UserData(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
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
            return user;
        }

        public async Task CreateUser(UserData user)
        {
            NpgsqlCommand command = new($"insert into public.users values (default, @p1, @p2)", Connection);
            command.Parameters.AddWithValue("p1", user.Login); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p2", user.Password); // позволяет избежать sql инъекций
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
                else if (e.SqlState == "23503")
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
    }
}
