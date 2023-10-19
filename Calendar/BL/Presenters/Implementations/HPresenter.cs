using BL.DTO;
using BL.Exceptions;
using BL.ForAPI.Interfaces;
using BL.Models.Implementations;
using BL.Models.Interfaces;

namespace BL.Presenters.Implementations
{
    public class HPresenter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private IAPI api;
        private IHoliday holiday;

        public HPresenter(IAPI api, IHoliday holiday)
        {
            this.api = api;
            this.holiday = holiday;

            this.api.EvGetHolidaysFromTo += GetHolidays;
            this.api.EvGetHoliday += GetHoliday;
            this.api.EvCreateHoliday += CreateHoliday;
            this.api.EvUpdateHoliday += UpdateHoliday;
            this.api.EvDeleteHoliday += DeleteHoliday;
    }


        public void GetHolidays(object? sender, EventArgsGetTasksFromTo args) 
        {
            List<HolidayData>? holidays = null;
            try
            {
                holidays = holiday.GetHolidays(args.From, args.To);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
            api.ShowHolidaysFromTo(holidays);
        }
        public void GetHoliday(object? sender, DateTime args)
        {
            HolidayData? holidayData = null;
            try
            {
                holidayData = holiday.GetHoliday(args);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
            if (holidayData != null)
                api.ShowHoliday(holidayData);
            else
                api.ShowWarning("Нет выходного в эту дату");
        }
        public void CreateHoliday(object? sender, HolidayData holidayData)
        {
            try
            {
                holiday.CreateHoliday(holidayData);
            }
            catch (ExistingName)
            {
                api.ShowWarning($"Выходной на даную дату уже создан");
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
        }
        public void UpdateHoliday(object? sender, HolidayData holidayData)
        {
            try
            {
                holiday.UpdateHoliday(holidayData);
            }
            catch (NoRecord)
            {
                api.ShowWarning($"Такого выходного ещё не было создано");
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
        }
        public void DeleteHoliday(object? sender, DateTime date)
        {
            try
            {
                holiday.DeleteHoliday(date);
            }
            catch (NoRecord e)
            {
                logger.Info(e, "Нет выходного в БД");
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
            
        }
    }
}
