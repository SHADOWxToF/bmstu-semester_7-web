using BL.DTO;
using BL.Exceptions;
using BL.ForDA.Interfaces;
using BL.Models.Implementations;
using Npgsql;

namespace DataAccess.DA.Implementations
{
    public class HolidayRepository : IHolidayRepository
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public NpgsqlConnection Connection { get; private set; }
        public HolidayRepository(NpgsqlConnection connection)
        {
            Connection = connection;
        }
        public List<HolidayData>? GetHolidays(DateTime from, DateTime to) 
        {
            NpgsqlCommand command = new("select * from public.holidays where date>=date(@p1) and date<=date(@p2)", Connection);
            command.Parameters.AddWithValue("p1", $"{from.Year}-{from.Month}-{from.Day}"); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p2", $"{to.Year}-{to.Month}-{to.Day}"); // позволяет избежать sql инъекций
            logger.Info($"sql-запрос: {command.CommandText}");
            List<HolidayData>? holidays = null;
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                holidays = new List<HolidayData>();
                while (reader.Read())
                    holidays.Add(new HolidayData(reader.GetDateTime(0), reader.GetString(1)));
                //Connection.Close();
                if (holidays.Count == 0)
                    holidays = null;
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
            return holidays;
        }
        public HolidayData? GetHoliday(DateTime date) 
        {
            NpgsqlCommand command = new("select * from public.holidays where date=date(@p1)", Connection);
            command.Parameters.AddWithValue("p1", $"{date.Year}-{date.Month}-{date.Day}"); // позволяет избежать sql инъекций
            logger.Info($"sql-запрос: {command.CommandText}");
            HolidayData? holiday = null;
            try
            {
                Connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    holiday = new HolidayData(reader.GetDateTime(0), reader.GetString(1));
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
            return holiday;
        }
        public void CreateHoliday(HolidayData holiday) 
        {
            NpgsqlCommand command = new("insert into public.holidays (select date(@p1), @p2 from (select count(*) from public.holidays) f where 0=(select count(*) from public.tasks where date=date(@p1)))", Connection);
            command.Parameters.AddWithValue("p1", $"{holiday.Date.Year}-{holiday.Date.Month}-{holiday.Date.Day}"); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p2", holiday.Message); // позволяет избежать sql инъекций
            logger.Info($"sql-запрос: {command.CommandText}");
            int updatedRows = 0;
            try
            {
                Connection.Open();
                updatedRows = command.ExecuteNonQuery();
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
                throw new BusyDate();
        }
        public void UpdateHoliday(HolidayData holiday) 
        {
            NpgsqlCommand command = new("update public.holidays set message=@p2 where date=date(@p1)", Connection);
            command.Parameters.AddWithValue("p1", $"{holiday.Date.Year}-{holiday.Date.Month}-{holiday.Date.Day}"); // позволяет избежать sql инъекций
            command.Parameters.AddWithValue("p2", holiday.Message); // позволяет избежать sql инъекций
            logger.Info($"sql-запрос: {command.CommandText}");
            int updatedRows = 0;
            try
            {
                Connection.Open();
                command.ExecuteNonQuery();
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
        public void DeleteHoliday(DateTime date) 
        {
            NpgsqlCommand command = new("delete from public.holidays where date=date(@p1)", Connection);
            command.Parameters.AddWithValue("p1", $"{date.Year}-{date.Month}-{date.Day}"); // позволяет избежать sql инъекций
            logger.Info($"sql-запрос: {command.CommandText}");
            int updatedRows = 0;
            try
            {
                Connection.Open();
                command.ExecuteNonQuery();
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
