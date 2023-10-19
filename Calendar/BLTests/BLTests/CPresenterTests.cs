using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using BL.DTO;
using BL.ForAPI.Interfaces;
using BL.Models.Interfaces;
using BL.Presenters.Implementations;
using BL.Exceptions;
namespace Tests
{
    [TestClass]
    public class CPresenterTests
    {
        private EventArgs args = new();
        private object obj = new();
        private Mock<IAPI> apiMock = new();
        private Mock<ICalendar> calendarMock = new();
        private CPresenter cpresenter;
        private List<CalendarData> dbContent = new List<CalendarData>
        {
            new CalendarData(5),
            new CalendarData(3, "3 days"),
        };
        public CPresenterTests()
        {
            cpresenter = new(apiMock.Object, calendarMock.Object);
        }

        [TestMethod] // в базе есть оповещания
        public void GetAllCalendarsTest1()
        {
            calendarMock.Setup(x => x.GetAllCalendars()).Returns(dbContent);

            cpresenter.GetAllCalendars(obj, args);
            apiMock.Verify(x => x.ShowCalendars(dbContent));
        }

        [TestMethod] // в базе нет оповещаний
        public void GetAllCalendarsTest2()
        {
            List<CalendarData>? calendarReturn2 = null;
            calendarMock.Setup(x => x.GetAllCalendars()).Returns(calendarReturn2);

            cpresenter.GetAllCalendars(obj, args);
            apiMock.Verify(x => x.ShowWarning("Не было создано ни одного оповещания"));
        }

        [TestMethod] // ошибка подключения к базе
        public void GetAllCalendarsTest3()
        {
            calendarMock.Setup(x => x.GetAllCalendars()).Throws<NoDBConnection>();

            cpresenter.GetAllCalendars(obj, args);
        }

        [TestMethod] // в базе есть оповещание
        public void GetCalendarTest1()
        {
            int days = dbContent[0].Day;
            calendarMock.Setup(x => x.GetCalendar(It.IsAny<int>())).Returns(dbContent[0]);

            cpresenter.GetCalendar(obj, days);
            apiMock.Verify(x => x.ShowCalendar(dbContent[0]));
        }

        [TestMethod] // в базе нет оповещания
        public void GetCalendarTest2()
        {
            int days = 1;
            CalendarData? returns = null;
            calendarMock.Setup(x => x.GetCalendar(It.IsAny<int>())).Returns(returns);

            cpresenter.GetCalendar(obj, days);
            apiMock.Verify(x => x.ShowWarning("1-дневного оповещания ещё не было создано"));
        }

        [TestMethod] // ошибка подключения к базе
        public void GetCalendarTest3()
        {
            int days = 5;
            calendarMock.Setup(x => x.GetCalendar(It.IsAny<int>())).Throws<NoDBConnection>();

            cpresenter.GetCalendar(obj, days);
        }

        [TestMethod] // в базе нет оповещения
        public void CreateCalendarTest1()
        {
            CalendarData newCalendar = new(4, "message 4");

            cpresenter.CreateCalendar(obj, newCalendar);

            calendarMock.Verify(x => x.CreateCalendar(newCalendar));

        }

        [TestMethod] // в базе уже есть оповещание
        public void CreateCalendarTest2()
        {
            CalendarData newCalendar = dbContent[1];

            calendarMock.Setup(x => x.CreateCalendar(newCalendar)).Throws<ExistingName>();

            cpresenter.CreateCalendar(obj, newCalendar);

            apiMock.Verify(x => x.ShowWarning($"{newCalendar.Day}-дневное оповещание уже существует"));
        }

        [TestMethod] // ошибка подключения к базе
        public void CreateCalendarTest3()
        {
            CalendarData newCalendar = new(4, "message 4");
            calendarMock.Setup(x => x.CreateCalendar(newCalendar)).Throws<NoDBConnection>();

            cpresenter.CreateCalendar(obj, newCalendar);
        }


        [TestMethod] // в базе уже есть оповещание
        public void UpdateCalendarTest1()
        {
            CalendarData updateCalendar = dbContent[1];
            updateCalendar.Message = "new message";

            cpresenter.UpdateCalendar(obj, updateCalendar);

            calendarMock.Verify(x => x.UpdateCalendar(updateCalendar));

        }

        [TestMethod] // в базе нет оповещения
        public void UpdateCalendarTest2()
        {
            CalendarData updateCalendar = dbContent[1];
            updateCalendar.Message = "new message";

            calendarMock.Setup(x => x.UpdateCalendar(updateCalendar)).Throws<NoRecord>();

            cpresenter.UpdateCalendar(obj, updateCalendar);

            apiMock.Verify(x => x.ShowWarning($"{updateCalendar.Day}-дневного оповещания ещё не было создано"));
        }

        [TestMethod] // ошибка подключения к базе
        public void UpdateCalendarTest3()
        {
            CalendarData updateCalendar = dbContent[1];
            calendarMock.Setup(x => x.UpdateCalendar(updateCalendar)).Throws<NoDBConnection>();

            cpresenter.UpdateCalendar(obj, updateCalendar);
        }

        [TestMethod] // в базе уже есть оповещание
        public void DeleteCalendarTest1()
        {
            int days = dbContent[1].Day;

            cpresenter.DeleteCalendar(obj, days);

            calendarMock.Verify(x => x.DeleteCalendar(days));
        }

        [TestMethod] // в базе нет оповещения
        public void DeleteCalendarTest2()
        {
            int days = 4;

            calendarMock.Setup(x => x.DeleteCalendar(days)).Throws<NoRecord>();

            cpresenter.DeleteCalendar(obj, days);
        }

        [TestMethod] // ошибка подключения к базе
        public void DeleteCalendarTest3()
        {
            int days = dbContent[1].Day;
            calendarMock.Setup(x => x.DeleteCalendar(days)).Throws<NoDBConnection>();

            cpresenter.DeleteCalendar(obj, days);
        }
    }

}

