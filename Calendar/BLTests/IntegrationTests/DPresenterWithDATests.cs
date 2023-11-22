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
    public class DPresenterWithDATests
    {
        private EventArgs args = new();
        private object obj = new();
        private NpgsqlConnection connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=root;Database=ppo");
        NpgsqlCommand command;
        private DisciplineRepository dRepository;
        private TaskRepository tRepository;
        private Discipline discipline;
        private DPresenter dPresenter;
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
        public DPresenterWithDATests()
        {
            dRepository = new DisciplineRepository(connection);
            tRepository = new TaskRepository(connection);
            discipline = new Discipline(dRepository, tRepository);
            dPresenter = new DPresenter(apiMock.Object, discipline);
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

        [TestMethod] // в базе есть задачи
        public void GetTasksTest1()
        {
            setupDB();
            DateTime from = new(2023, 5, 1);
            DateTime to = new(2023, 5, 28);
            EventArgsGetTasksFromTo args = new EventArgsGetTasksFromTo
            {
                From = from,
                To = to,
            };

            List<TaskData> result = new List<TaskData>();
            apiMock.Setup(x => x.ShowTasksFromTo(It.IsAny<List<TaskData>>())).Callback<List<TaskData>>(s => result = s);

            dPresenter.GetTasks(obj, args);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(taskDB[0].Name, result[0].Name);
            Assert.AreEqual(taskDB[0].DisciplineName, result[0].DisciplineName);
            Assert.AreEqual(taskDB[1].Name, result[1].Name);
            Assert.AreEqual(taskDB[1].DisciplineName, result[1].DisciplineName);
        }

        [TestMethod] // в базе нет задач
        public void GetTasksTest2()
        {
            setupDB();
            DateTime from = new(2022, 5, 1);
            DateTime to = new(2022, 5, 28);
            EventArgsGetTasksFromTo args = new EventArgsGetTasksFromTo
            {
                From = from,
                To = to,
            };

            dPresenter.GetTasks(obj, args);
            apiMock.Verify(x => x.ShowTasksFromTo(null));
        }

        [TestMethod] // в базе есть дисциплины
        public void GetDisciplinesTest1()
        {
            setupDB();
            List<DisciplineData> result = new List<DisciplineData>();
            apiMock.Setup(x => x.ShowDisciplines(It.IsAny<List<DisciplineData>>())).Callback<List<DisciplineData>>(s => result = s);

            dPresenter.GetDisciplines(obj, args);
            Assert.AreEqual(disciplineDB.Count, result.Count);
            Assert.AreEqual(disciplineDB[0].Name, result[0].Name);
            Assert.AreEqual(disciplineDB[1].Name, result[1].Name);
        }

        [TestMethod] // в базе есть задачи
        public void GetDTasksTest1()
        {
            setupDB();
            string disciplineName = disciplineDB[0].Name;

            List<TaskData> result = new List<TaskData>();
            apiMock.Setup(x => x.ShowDTasks(It.IsAny<List<TaskData>>())).Callback<List<TaskData>>(s => result = s);

            dPresenter.GetDTasks(obj, disciplineName);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(taskDB[0].Name, result[0].Name);
            Assert.AreEqual(taskDB[0].Name, result[0].Name);
            Assert.AreEqual(taskDB[0].DisciplineName, result[0].DisciplineName);
            Assert.AreEqual(taskDB[1].Name, result[1].Name);
            Assert.AreEqual(taskDB[1].DisciplineName, result[1].DisciplineName);

        }

        [TestMethod] // в базе нет задач
        public void GetDTasksTest2()
        {
            setupDB();
            string disciplineName = "d3";

            dPresenter.GetDTasks(obj, disciplineName);
            apiMock.Verify(x => x.ShowDTasks(null));
        }

        [TestMethod] // в базе есть задача
        public void GetTaskTest1()
        {
            setupDB();
            string taskName = taskDB[0].Name;
            string disciplineName = taskDB[0].DisciplineName;
            EventArgsDisciplineTaskNames args = new EventArgsDisciplineTaskNames(taskName, disciplineName);

            TaskData result = new TaskData();
            apiMock.Setup(x => x.ShowTask(It.IsAny<TaskData>())).Callback<TaskData>(s => result = s);

            dPresenter.GetTask(obj, args);

            Assert.AreEqual(taskDB[0].Name, result.Name);
            Assert.AreEqual(taskDB[0].DisciplineName, result.DisciplineName);
        }

        [TestMethod] // в базе нет задачи
        public void GetTaskTest2()
        {
            setupDB();
            string taskName = "t8";
            string disciplineName = "d1";
            EventArgsDisciplineTaskNames args = new EventArgsDisciplineTaskNames(taskName, disciplineName);

            dPresenter.GetTask(obj, args);
            apiMock.Verify(x => x.ShowWarning("Нет задачи с таким именем"));
        }

        [TestMethod] // в базе нет задачи
        public void CreateTaskTest1()
        {
            setupDB();
            TaskData taskData = new TaskData(1, "t4", "desc4", "d2", new DateTime(2023, 4, 15), 5, true);

            dPresenter.CreateTask(obj, taskData);

            string taskName = taskData.Name;
            string disciplineName = taskData.DisciplineName;
            EventArgsDisciplineTaskNames args = new EventArgsDisciplineTaskNames(taskName, disciplineName);

            TaskData result = new TaskData();
            apiMock.Setup(x => x.ShowTask(It.IsAny<TaskData>())).Callback<TaskData>(s => result = s);

            dPresenter.GetTask(obj, args);

            Assert.AreEqual(taskData.Name, result.Name);
            Assert.AreEqual(taskData.DisciplineName, result.DisciplineName);
        }

        [TestMethod] // в базе есть задача
        public void CreateTaskTest2()
        {
            setupDB();
            TaskData taskData = taskDB[0];

            dPresenter.CreateTask(obj, taskData);
            apiMock.Verify(x => x.ShowWarning("Задача с таким именем уже существует"));
        }

        [TestMethod] // в базе нет дисциплины
        public void CreateTaskTest3()
        {
            setupDB();
            TaskData taskData = taskDB[0];
            taskData.DisciplineName = "d5";
            
            dPresenter.CreateTask(obj, taskData);
            apiMock.Verify(x => x.ShowWarning("Сначала создайте дисциплину с таким именем"));
        }

        [TestMethod] // в базе есть задача
        public void UpdateTaskTest1()
        {
            setupDB();
            TaskData taskData = taskDB[0];
            string desc = "new desc";
            taskData.Description = desc;

            dPresenter.UpdateTask(obj, taskData);

            string taskName = taskDB[0].Name;
            string disciplineName = taskDB[0].DisciplineName;
            EventArgsDisciplineTaskNames args = new EventArgsDisciplineTaskNames(taskName, disciplineName);

            TaskData result = new TaskData();
            apiMock.Setup(x => x.ShowTask(It.IsAny<TaskData>())).Callback<TaskData>(s => result = s);

            dPresenter.GetTask(obj, args);

            Assert.AreEqual(taskData.Name, result.Name);
            Assert.AreEqual(taskData.DisciplineName, result.DisciplineName);
            Assert.AreEqual(desc, result.Description);
        }

        [TestMethod] // в базе нет задачи
        public void UpdateTaskTest2()
        {
            setupDB();
            TaskData taskData = new TaskData(1, "t4", "desc4", "d2", new DateTime(2023, 4, 15), 5, true);

            dPresenter.UpdateTask(obj, taskData);
            apiMock.Verify(x => x.ShowWarning("Такой задачи ещё не было создано"));
        }

        [TestMethod] // в базе есть задача
        public void DeleteTaskTest1()
        {
            setupDB();
            string taskName = taskDB[0].Name;
            string disciplineName = taskDB[0].DisciplineName;
            EventArgsDisciplineTaskNames args = new EventArgsDisciplineTaskNames(taskName, disciplineName);

            dPresenter.DeleteTask(obj, args);

            TaskData result = new TaskData();
            apiMock.Setup(x => x.ShowTask(It.IsAny<TaskData>())).Callback<TaskData>(s => result = s);

            dPresenter.GetTask(obj, args);
            apiMock.Verify(x => x.ShowWarning("Нет задачи с таким именем"));
        }

        [TestMethod] // в базе нет задачи
        public void DeleteTaskTest2()
        {
            setupDB();
            string taskName = "t4";
            string disciplineName = "d3";
            EventArgsDisciplineTaskNames args = new EventArgsDisciplineTaskNames(taskName, disciplineName);

            dPresenter.DeleteTask(obj, args);

            TaskData result = new TaskData();
            apiMock.Setup(x => x.ShowTask(It.IsAny<TaskData>())).Callback<TaskData>(s => result = s);

            dPresenter.GetTask(obj, args);
            apiMock.Verify(x => x.ShowWarning("Нет задачи с таким именем"));
        }

    }
}
