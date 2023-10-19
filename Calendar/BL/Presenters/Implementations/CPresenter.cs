using BL.DTO;
using BL.ForAPI.Interfaces;
using BL.Models.Interfaces;
using BL.Exceptions;
namespace BL.Presenters.Implementations
{
    public class CPresenter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private IAPI api;
        private ICalendar calendar;
        private Thread notificationRoutine;
        public CPresenter(IAPI api, ICalendar calendar)
        {
            this.api = api;
            this.calendar = calendar;

            this.api.EvGetAllCalendars += GetAllCalendars;
            this.api.EvGetCalendar += GetCalendar;
            this.api.EvCreateCalendar += CreateCalendar;
            this.api.EvUpdateCalendar += UpdateCalendar;
            this.api.EvDeleteCalendar += DeleteCalendar;

            notificationRoutine = new(new ThreadStart(NotificationRoutine));
            notificationRoutine.IsBackground = true;
            notificationRoutine.Start();
            Thread.Sleep(new TimeSpan(0, 0, 3));
        }

        private void NotificationRoutine()
        {
            while (true)
            {
                try
                {
                    List<CalendarData>? allCalendars = calendar.GetAllCalendars();
                    if (allCalendars == null)
                        return;
                    string notification = "";
                    foreach (CalendarData calendarData in allCalendars)
                    {
                        try
                        {
                            List<TaskData>? tasks = calendar.GetNearestTasks(calendarData.Day);
                            if (tasks != null)
                            {
                                notification += $"{calendarData.Message} до сдачи:\r\n";
                                foreach (var task in tasks)
                                {
                                    notification += $"\t{task.Name} по дисциплине {task.DisciplineName}, которая стоит {task.Cost} баллов\r\n";
                                }
                            }
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
                    if (notification != "")
                        api.CalendarNotification(notification);
                }
                catch (NoDBConnection e)
                {
                    logger.Error(e, "Отсутсвует соедиение с БД");
                }
                catch (Exception e)
                {
                    throw new UnpredictableException(e);
                }
                Thread.Sleep(new TimeSpan(24, 0, 0)); // 24 часа
            }
        }

        public void GetAllCalendars(object? sender, EventArgs args)
        {
            List<CalendarData>? calendars = null;
            try
            {
                calendars = calendar.GetAllCalendars();
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
            if (calendars != null)
                api.ShowCalendars(calendars);
            else
                api.ShowWarning("Не было создано ни одного оповещания");
        }

        public void GetCalendar(object? sender, int days)
        {
            CalendarData? calendarData = null;
            try
            {
                calendarData = calendar.GetCalendar(days);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
            if (calendarData != null)
                api.ShowCalendar(calendarData);
            else
                api.ShowWarning($"{days}-дневного оповещания ещё не было создано");
        }

        public void CreateCalendar(object? sender, CalendarData calendarData)
        {
            try
            {
                calendar.CreateCalendar(calendarData);
            }
            catch (ExistingName)
            {
                api.ShowWarning($"{calendarData.Day}-дневное оповещание уже существует");
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

        public void UpdateCalendar(object? sender, CalendarData calendarData)
        {
            try
            {
                calendar.UpdateCalendar(calendarData);
            }
            catch (NoRecord)
            {
                api.ShowWarning($"{calendarData.Day}-дневного оповещания ещё не было создано");
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

        public void DeleteCalendar(object? sender, int days)
        {
            try
            {
                calendar.DeleteCalendar(days);
            }
            catch (NoRecord e) 
            {
                logger.Info(e, "Нет оповещания в БД");
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
