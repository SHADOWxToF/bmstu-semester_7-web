using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using BL.DTO;
using BL.ForAPI.Interfaces;
using BL.Models.Interfaces;
using BL.Presenters.Implementations;
using BL.Exceptions;
namespace Tests.BLTests
{
    [TestClass]
    public class DPresenterTests
    {
        private EventArgs args = new();
        private object obj = new();
        private Mock<IAPI> apiMock = new();
        private Mock<IDiscipline> disciplineMock = new();
        private DPresenter dpresenter;
        private List<DisciplineData> disciplineDBContent = new List<DisciplineData>
        {
            new DisciplineData("discipline 1", "description 1", 5),
            new DisciplineData("discipline 2", "description 2", 12),
        };
        private List<TaskData> taskDBContent = new List<TaskData>
        {
            new TaskData(1, "task 1", "description 1", "discipline 2", new DateTime(2023, 6, 4), 6, false),
            new TaskData(2, "task 1", "description 1", "discipline 1", new DateTime(2023, 6, 3), 5, false),
            new TaskData(3, "task 2", "description 1", "discipline 2", new DateTime(2023, 6, 11), 6, true),
        };
        public DPresenterTests()
        {
            dpresenter = new(apiMock.Object, disciplineMock.Object);
        }

        [TestMethod] // в базе есть задачи
        public void GetTasksTest1()
        {
            DateTime from = new(2023, 4, 1);
            DateTime to = new(2023, 4, 28);
            EventArgsGetTasksFromTo args = new EventArgsGetTasksFromTo
            {
                From = from,
                To = to,
            };
            disciplineMock.Setup(x => x.GetTasks(from, to)).Returns(taskDBContent);

            dpresenter.GetTasks(obj, args);
            apiMock.Verify(x => x.ShowTasksFromTo(taskDBContent));
        }

        [TestMethod] // в базе нет задач
        public void GetTasksTest2()
        {
            DateTime from = new(2023, 4, 1);
            DateTime to = new(2023, 4, 28);
            EventArgsGetTasksFromTo args = new EventArgsGetTasksFromTo
            {
                From = from,
                To = to,
            };
            List<TaskData>? tasks = null;
            disciplineMock.Setup(x => x.GetTasks(from, to)).Returns(tasks);

            dpresenter.GetTasks(obj, args);
            apiMock.Verify(x => x.ShowTasksFromTo(null));
        }

        [TestMethod] // ошибка подключения к базе
        public void GetTasksTest3()
        {
            DateTime from = new(2023, 4, 1);
            DateTime to = new(2023, 4, 28);
            EventArgsGetTasksFromTo args = new EventArgsGetTasksFromTo
            {
                From = from,
                To = to,
            };
            disciplineMock.Setup(x => x.GetTasks(from, to)).Throws<NoDBConnection>();

            dpresenter.GetTasks(obj, args);
        }

        [TestMethod] // в базе есть дисциплины
        public void GetDisciplinesTest1()
        {
            disciplineMock.Setup(x => x.GetDisciplines()).Returns(disciplineDBContent);

            dpresenter.GetDisciplines(obj, args);
            apiMock.Verify(x => x.ShowDisciplines(disciplineDBContent));
        }

        [TestMethod] // в базе нет дисциплин
        public void GetDisciplinesTest2()
        {
            List<DisciplineData>? disciplines = null;
            disciplineMock.Setup(x => x.GetDisciplines()).Returns(disciplines);

            dpresenter.GetDisciplines(obj, args);
            apiMock.Verify(x => x.ShowWarning("Не создано ни одной дисциплины"));
        }

        [TestMethod] // ошибка подключения к базе
        public void GetDisciplinesTest3()
        {
            disciplineMock.Setup(x => x.GetDisciplines()).Throws<NoDBConnection>();

            dpresenter.GetDisciplines(obj, args);
        }

        [TestMethod] // в базе есть задачи
        public void GetDTasksTest1()
        {
            string disciplineName = "discipline 1";
            disciplineMock.Setup(x => x.GetDTasks(disciplineName)).Returns(taskDBContent);

            dpresenter.GetDTasks(obj, disciplineName);
            apiMock.Verify(x => x.ShowDTasks(taskDBContent));
        }

        [TestMethod] // в базе нет задач
        public void GetDTasksTest2()
        {
            string disciplineName = "discipline 1";
            List<TaskData>? tasks = null;
            disciplineMock.Setup(x => x.GetDTasks(disciplineName)).Returns(tasks);

            dpresenter.GetDTasks(obj, disciplineName);
            apiMock.Verify(x => x.ShowDTasks(null));
        }

        [TestMethod] // ошибка подключения к базе
        public void GetDTasksTest3()
        {
            string disciplineName = "discipline 1";
            disciplineMock.Setup(x => x.GetDTasks(disciplineName)).Throws<NoDBConnection>();

            dpresenter.GetDTasks(obj, disciplineName);
        }

        [TestMethod] // в базе нет дисциплины
        public void CreateDisciplineTest1()
        {
            DisciplineData disciplineData = new DisciplineData();

            dpresenter.CreateDiscipline(obj, disciplineData);
            disciplineMock.Verify(x => x.CreateDiscipline(disciplineData));
        }

        [TestMethod] // в базе есть дисциплина
        public void CreateDisciplineTest2()
        {
            DisciplineData disciplineData = new DisciplineData();
            disciplineMock.Setup(x => x.CreateDiscipline(disciplineData)).Throws<ExistingName>();

            dpresenter.CreateDiscipline(obj, disciplineData);
            apiMock.Verify(x => x.ShowWarning("Дисциплина с таким именем уже существует"));
        }

        [TestMethod] // ошибка подключения к базе
        public void CreateDisciplineTest3()
        {
            DisciplineData disciplineData = new DisciplineData();
            disciplineMock.Setup(x => x.CreateDiscipline(disciplineData)).Throws<NoDBConnection>();

            dpresenter.CreateDiscipline(obj, disciplineData);
        }

        [TestMethod] // в базе есть дисциплина
        public void UpdateDisciplineTest1()
        {
            DisciplineData disciplineData = new DisciplineData();

            dpresenter.UpdateDiscipline(obj, disciplineData);
            disciplineMock.Verify(x => x.UpdateDiscipline(disciplineData));
        }

        [TestMethod] // в базе нет дисциплины
        public void UpdateDisciplineTest2()
        {
            DisciplineData disciplineData = new DisciplineData();
            disciplineMock.Setup(x => x.UpdateDiscipline(disciplineData)).Throws<NoRecord>();

            dpresenter.UpdateDiscipline(obj, disciplineData);
            apiMock.Verify(x => x.ShowWarning("Такой дисциплины ещё не было создано"));
        }

        [TestMethod] // ошибка подключения к базе
        public void UpdateDisciplineTest3()
        {
            DisciplineData disciplineData = new DisciplineData();
            disciplineMock.Setup(x => x.UpdateDiscipline(disciplineData)).Throws<NoDBConnection>();

            dpresenter.UpdateDiscipline(obj, disciplineData);
        }

        [TestMethod] // в базе есть дисциплина
        public void DeleteDisciplineTest1()
        {
            string disciplineName = "discipline 1";

            dpresenter.DeleteDiscipline(obj, disciplineName);
            disciplineMock.Verify(x => x.DeleteDiscipline(disciplineName));
        }

        [TestMethod] // в базе нет дисциплины
        public void DeleteDisciplineTest2()
        {
            string disciplineName = "discipline 1";
            disciplineMock.Setup(x => x.DeleteDiscipline(disciplineName)).Throws<NoRecord>();

            dpresenter.DeleteDiscipline(obj, disciplineName);
        }

        [TestMethod] // ошибка подключения к базе
        public void DeleteDisciplineTest3()
        {
            string disciplineName = "discipline 1";
            disciplineMock.Setup(x => x.DeleteDiscipline(disciplineName)).Throws<NoDBConnection>();

            dpresenter.DeleteDiscipline(obj, disciplineName);
        }

        [TestMethod] // в базе есть задача
        public void GetTaskTest1()
        {
            string taskName = "task";
            string disciplineName = "discipline";
            EventArgsDisciplineTaskNames args = new EventArgsDisciplineTaskNames(taskName, disciplineName);

            TaskData task = new TaskData();
            disciplineMock.Setup(x => x.GetTask(disciplineName, taskName)).Returns(task);

            dpresenter.GetTask(obj, args);
            apiMock.Verify(x => x.ShowTask(task));
        }

        [TestMethod] // в базе нет задачи
        public void GetTaskTest2()
        {
            string taskName = "task";
            string disciplineName = "discipline";
            EventArgsDisciplineTaskNames args = new EventArgsDisciplineTaskNames(taskName, disciplineName);

            TaskData? task = null;

            disciplineMock.Setup(x => x.GetTask(disciplineName, taskName)).Returns(task);

            dpresenter.GetTask(obj, args);
            apiMock.Verify(x => x.ShowWarning("Нет задачи с таким именем"));
        }

        [TestMethod] // ошибка подключения к базе
        public void GetTaskTest3()
        {
            string taskName = "task";
            string disciplineName = "discipline";
            EventArgsDisciplineTaskNames args = new EventArgsDisciplineTaskNames(taskName, disciplineName);

            disciplineMock.Setup(x => x.GetTask(disciplineName, taskName)).Throws<NoDBConnection>();

            dpresenter.GetTask(obj, args);
        }

        [TestMethod] // в базе нет задачи
        public void CreateTaskTest1()
        {
            TaskData taskData = new TaskData();

            dpresenter.CreateTask(obj, taskData);
            disciplineMock.Verify(x => x.CreateTask(taskData));
        }

        [TestMethod] // в базе есть задача
        public void CreateTaskTest2()
        {
            TaskData taskData = new TaskData();
            disciplineMock.Setup(x => x.CreateTask(taskData)).Throws<ExistingName>();

            dpresenter.CreateTask(obj, taskData);
            apiMock.Verify(x => x.ShowWarning("Задача с таким именем уже существует"));
        }

        [TestMethod] // в базе нет дисциплины
        public void CreateTaskTest3()
        {
            TaskData taskData = new TaskData();
            disciplineMock.Setup(x => x.CreateTask(taskData)).Throws<NoRecord>();

            dpresenter.CreateTask(obj, taskData);
            apiMock.Verify(x => x.ShowWarning("Сначала создайте дисциплину с таким именем"));
        }

        [TestMethod] // ошибка подключения к базе
        public void CreateTaskTest4()
        {
            TaskData taskData = new TaskData();
            disciplineMock.Setup(x => x.CreateTask(taskData)).Throws<NoDBConnection>();

            dpresenter.CreateTask(obj, taskData);
        }

        [TestMethod] // в базе есть задача
        public void UpdateTaskTest1()
        {
            TaskData taskData = new TaskData();

            dpresenter.UpdateTask(obj, taskData);
            disciplineMock.Verify(x => x.UpdateTask(taskData));
        }

        [TestMethod] // в базе нет задачи
        public void UpdateTaskTest2()
        {
            TaskData taskData = new TaskData();
            disciplineMock.Setup(x => x.UpdateTask(taskData)).Throws<NoRecord>();

            dpresenter.UpdateTask(obj, taskData);
            apiMock.Verify(x => x.ShowWarning("Такой задачи ещё не было создано"));
        }

        [TestMethod] // ошибка подключения к базе
        public void UpdateTaskTest3()
        {
            TaskData taskData = new TaskData();
            disciplineMock.Setup(x => x.UpdateTask(taskData)).Throws<NoDBConnection>();

            dpresenter.UpdateTask(obj, taskData);
        }

        [TestMethod] // в базе есть задача
        public void DeleteTaskTest1()
        {
            string taskName = "task";
            string disciplineName = "discipline";
            EventArgsDisciplineTaskNames args = new EventArgsDisciplineTaskNames(taskName, disciplineName);

            dpresenter.DeleteTask(obj, args);
            disciplineMock.Verify(x => x.DeleteTask(taskName, disciplineName));
        }

        [TestMethod] // в базе нет задачи
        public void DeleteTaskTest2()
        {
            string taskName = "task";
            string disciplineName = "discipline";
            EventArgsDisciplineTaskNames args = new EventArgsDisciplineTaskNames(taskName, disciplineName);

            disciplineMock.Setup(x => x.DeleteTask(taskName, disciplineName)).Throws<NoRecord>();

            dpresenter.DeleteTask(obj, args);
        }

        [TestMethod] // ошибка подключения к базе
        public void DeleteTaskTest3()
        {
            string taskName = "task";
            string disciplineName = "discipline";
            EventArgsDisciplineTaskNames args = new EventArgsDisciplineTaskNames(taskName, disciplineName);

            disciplineMock.Setup(x => x.DeleteTask(taskName, disciplineName)).Throws<NoDBConnection>();

            dpresenter.DeleteTask(obj, args);
        }
    }
}
