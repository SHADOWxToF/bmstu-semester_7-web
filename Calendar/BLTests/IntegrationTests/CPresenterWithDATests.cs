using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using BL.ForAPI.Interfaces;
using BL.Models.Interfaces;
using BL.Presenters.Implementations;
using BL.Exceptions;
using DataAccess.DA.Implementations;
using Npgsql;
using BL.Models.Implementations;
using BL.ForAPI.DTO;

namespace Tests.IntegrationsTests
{
    [TestClass]
    public class CPresenterWithDATests
    {
        private EventArgs args = new();
        private object obj = new();
        private NpgsqlConnection connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=root;Database=ppo");
        NpgsqlCommand command;
        private NotificationRepository cRepository;
        private DisciplineRepository dRepository;
        private TaskRepository tRepository;
        private Notification calendar;
        private CPresenter cPresenter;
        private Mock<IAPI> apiMock = new();
        List<CalendarData> calendarDB = new List<CalendarData>
        {
            new CalendarData(1, "qwerty"),
            new CalendarData(4, "qqqqwerty"),
        };
        List<DisciplineData> disciplineDB = new List<DisciplineData>
        {
            new DisciplineData("d1", "desc1", 4),
            new DisciplineData("d2", "desc2", 6),
        };
        List<TaskData> taskDB = new List<TaskData>
        {
            new TaskData(1, "t1", "desc1", "d1", new DateTime(2023, 5, 16), 3, true),
            new TaskData(2, "t2", "desc2", "d1", new DateTime(2023, 5, 16), 1, false),
            new TaskData(3, "t3", "desc3", "d2", new DateTime(2023, 6, 16), 6, true),
        };
        public CPresenterWithDATests()
        {
            dRepository = new DisciplineRepository(connection);
            tRepository = new TaskRepository(connection);
            cRepository = new NotificationRepository(connection);
            calendar = new Notification(dRepository, tRepository, cRepository);
            cPresenter = new CPresenter(apiMock.Object, calendar);
            command = new NpgsqlCommand("begin; delete from calendars;delete from tasks;delete from disciplines;insert into public.calendars values(1, 'qwerty'),(4, 'qqqqwerty');insert into public.disciplines values('d1', 'desc1', 4),('d2', 'desc2', 6);insert into public.tasks values(1, 't1', 'desc1', 'd1', '2023-05-16', 3, 'true'),(2, 't2', 'desc2', 'd1', '2023-05-16', 1, 'false'),(3, 't3', 'desc3', 'd2', '2023-06-16', 6, 'true'); commit;", connection);
        }

        /*
        Содержание БД:
        public.calendar:

        |day|  message  |
        |---|-----------|
        |1  |  "qwerty" |
        |4  |"qqqqwerty"|
        |---|-----------|
        
        public.disciplines:

        |name|description|sum|
        |----|-----------|---|
        |"d1"|  "desc1"  | 4 |
        |"d2"|  "desc2"  | 6 |
        |----|-----------|---|

        public.tasks:

        |id|name|description|discipline_name|   date   |cost|finished|
        |--|----|-----------|---------------|----------|----|--------|
        |1 |"t1"|  "desc1"  |     "d1"      |2023-05-16| 3  | true   |
        |2 |"t2"|  "desc2"  |     "d1"      |2023-05-16| 1  | false  |
        |3 |"t3"|  "desc3"  |     "d2"      |2023-06-16| 6  | true   |
        |--|----|-----------|---------------|----------|----|--------|
        */

        private void setupDB()
        {
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void GetAllCalendarsTest1()
        {
            setupDB();
            List<CalendarData> result = new List<CalendarData>();
            apiMock.Setup(x => x.ShowCalendars(It.IsAny<List<CalendarData>>())).Callback<List<CalendarData>>(s => result = s);
            cPresenter.GetAllCalendars(obj, args);
            Assert.AreEqual(result.Count, calendarDB.Count);
            Assert.AreEqual(result[0].Day, calendarDB[0].Day);
            Assert.AreEqual(result[0].Message, calendarDB[0].Message);
            Assert.AreEqual(result[1].Day, calendarDB[1].Day);
            Assert.AreEqual(result[1].Message, calendarDB[1].Message);
        }
        
        [TestMethod] // в базе есть оповещание
        public void GetCalendarTest1()
        {
            setupDB();
            int days = 4;
            CalendarData result = new CalendarData(4);
            apiMock.Setup(x => x.ShowCalendar(It.IsAny<CalendarData>())).Callback<CalendarData>(s => result = s);

            cPresenter.GetCalendar(obj, days);
            Assert.AreEqual(result.Day, calendarDB[1].Day);
            Assert.AreEqual(result.Message, calendarDB[1].Message);
        }

        [TestMethod] // в базе нет оповещания
        public void GetCalendarTest2()
        {
            setupDB();
            int days = 5;
            cPresenter.GetCalendar(obj, days);
            apiMock.Verify(x => x.ShowWarning($"{days}-дневного оповещания ещё не было создано"));
        }

        [TestMethod] // в базе нет оповещения
        public void CreateCalendarTest1()
        {
            setupDB();
            int days = 2;
            CalendarData newCalendar = new(days, "message 2");

            cPresenter.CreateCalendar(obj, newCalendar);

            CalendarData result = new CalendarData(days);
            apiMock.Setup(x => x.ShowCalendar(It.IsAny<CalendarData>())).Callback<CalendarData>(s => result = s);

            cPresenter.GetCalendar(obj, days);
            Assert.AreEqual(result.Day, newCalendar.Day);
            Assert.AreEqual(result.Message, newCalendar.Message);

            cPresenter.DeleteCalendar(obj, days);

        }

        [TestMethod] // в базе уже есть оповещание
        public void CreateCalendarTest2()
        {
            setupDB();
            CalendarData newCalendar = calendarDB[1];

            cPresenter.CreateCalendar(obj, newCalendar);

            apiMock.Verify(x => x.ShowWarning($"{newCalendar.Day}-дневное оповещание уже существует"));
        }

        [TestMethod] // в базе уже есть оповещание
        public void UpdateCalendarTest1()
        {
            setupDB();
            CalendarData updateCalendar = calendarDB[1];
            updateCalendar.Message = "new message";

            cPresenter.UpdateCalendar(obj, updateCalendar);

            CalendarData result = new CalendarData(1);
            apiMock.Setup(x => x.ShowCalendar(It.IsAny<CalendarData>())).Callback<CalendarData>(s => result = s);

            cPresenter.GetCalendar(obj, updateCalendar.Day);
            Assert.AreEqual(result.Day, updateCalendar.Day);
            Assert.AreEqual(result.Message, updateCalendar.Message);
        }

        [TestMethod] // в базе нет оповещения
        public void UpdateCalendarTest2()
        {
            setupDB();
            CalendarData updateCalendar = new CalendarData(5, "new message");

            cPresenter.UpdateCalendar(obj, updateCalendar);

            apiMock.Verify(x => x.ShowWarning($"{updateCalendar.Day}-дневного оповещания ещё не было создано"));
        }

        [TestMethod] // в базе уже есть оповещание
        public void DeleteCalendarTest1()
        {
            setupDB();
            int days = calendarDB[1].Day;

            cPresenter.DeleteCalendar(obj, days);
            
            cPresenter.GetCalendar(obj, days);
            apiMock.Verify(x => x.ShowWarning($"{days}-дневного оповещания ещё не было создано"));
        }

        [TestMethod] // в базе нет оповещения
        public void DeleteCalendarTest2()
        {
            setupDB();
            int days = 5;

            cPresenter.DeleteCalendar(obj, days);

            cPresenter.GetCalendar(obj, days);
            apiMock.Verify(x => x.ShowWarning($"{days}-дневного оповещания ещё не было создано"));
        }

    }
}
