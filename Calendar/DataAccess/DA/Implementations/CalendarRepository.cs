using BL.ForDA.Interfaces;
using Npgsql;
using BL.DTO;
using BL.Exceptions;
namespace DataAccess.DA.Implementations
{
    public class CalendarRepository : ICalendarRepository
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public NpgsqlConnection Connection { get; private set; }
        public CalendarRepository(NpgsqlConnection connection)
        {
            Connection = connection;
        }
        public List<CalendarData>? GetAllCalendars()
        {
            NpgsqlCommand command = new("select * from public.calendars", Connection);
            logger.Info($"sql-запрос: {command.CommandText}"); 
            List<CalendarData>? calendars = null;
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                calendars = new List<CalendarData>();
                while (reader.Read())
                    calendars.Add(new CalendarData(reader.GetInt32(0), reader.GetString(1)));
                //Connection.Close();
                if (calendars.Count == 0)
                    calendars = null;
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
            return calendars;

        }
        public CalendarData? GetCalendar(int days)
        {
            NpgsqlCommand command = new($"select * from public.calendars where day={days}", Connection);
            logger.Info($"sql-запрос: {command.CommandText}"); 
            CalendarData? calendar = null;
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    calendar = new CalendarData(days, reader.GetString(1));
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
            return calendar;
        }
        public void CreateCalendar(CalendarData calendar)
        {
            NpgsqlCommand command = new($"insert into public.calendars values ({calendar.Day}, @p1)", Connection);
            command.Parameters.AddWithValue("p1", calendar.Message); // позволяет избежать sql инъекций
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
        public void UpdateCalendar(CalendarData calendar)
        {
            NpgsqlCommand command = new($"update public.calendars set message=@p1 where day={calendar.Day}", Connection);
            command.Parameters.AddWithValue("p1", calendar.Message); // позволяет избежать sql инъекций
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
        public void DeleteCalendar(int days)
        {
            NpgsqlCommand command = new($"delete from public.calendars where day={days}", Connection);
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