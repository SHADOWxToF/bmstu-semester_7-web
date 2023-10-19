using BL.DTO;
using BL.ForAPI.Interfaces;
using BL.Models.Interfaces;
using BL.Exceptions;
namespace BL.Presenters.Implementations
{
    public class DPresenter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private IAPI api;
        private IDiscipline discipline;
        public DPresenter(IAPI api, IDiscipline discipline)
        {
            this.api = api;
            this.discipline = discipline;

            this.api.EvGetTasksFromTo += GetTasks;
            this.api.EvGetDisciplines += GetDisciplines;
            this.api.EvGetDTasks += GetDTasks;
            this.api.EvGetDiscipline += GetDiscipline;
            this.api.EvCreateDiscipline += CreateDiscipline;
            this.api.EvUpdateDiscipline += UpdateDiscipline;
            this.api.EvDeleteDiscipline += DeleteDiscipline;
            this.api.EvGetTask += GetTask;
            this.api.EvCreateTask += CreateTask;
            this.api.EvUpdateTask += UpdateTask;
            this.api.EvDeleteTask += DeleteTask;

        }

        public void GetTasks(object? sender, EventArgsGetTasksFromTo args)
        {
            List<TaskData>? tasks = null;
            try
            {
                tasks = discipline.GetTasks(args.From, args.To);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
            api.ShowTasksFromTo(tasks);
        }

        public void GetDisciplines(object? sender, EventArgs args)
        {
            List<DisciplineData>? disciplines = null;
            try
            {
                disciplines = discipline.GetDisciplines();
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
            if (disciplines != null)
                api.ShowDisciplines(disciplines);
            else
                api.ShowWarning("Не создано ни одной дисциплины");
        }

        public void GetDTasks(object? sender, string disciplineName)
        {
            List<TaskData>? tasks = null;
            try
            {
                tasks = discipline.GetDTasks(disciplineName);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
            api.ShowDTasks(tasks);
        }

        public void GetDiscipline(object? sender, string args)
        {
            DisciplineData? disciplineData = null;
            try
            {
                disciplineData = discipline.GetDiscipline(args);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
            if (disciplineData != null)
                api.ShowDiscipline(disciplineData);
            else
                api.ShowWarning("Нет дисциплины с таким именем");
        }

        public void CreateDiscipline(object? sender, DisciplineData disciplineData)
        {
            try
            {
                discipline.CreateDiscipline(disciplineData);
            }
            catch (ExistingName)
            {
                api.ShowWarning($"Дисциплина с таким именем уже существует");
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

        public void UpdateDiscipline(object? sender, DisciplineData disciplineData)
        {
            try
            {
                discipline.UpdateDiscipline(disciplineData);
            }
            catch (NoRecord)
            {
                api.ShowWarning($"Такой дисциплины ещё не было создано");
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

        public void DeleteDiscipline(object? sender, string disciplineName)
        {
            try
            {
                discipline.DeleteDiscipline(disciplineName);
            }
            catch (NoRecord e) 
            {
                logger.Info(e, "Нет дисциплины в БД");
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

        public void GetTask(object? sender, EventArgsDisciplineTaskNames args)
        {
            TaskData? task = null;
            try
            {
                task = discipline.GetTask(args.DisciplineName, args.TaskName);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
            }
            catch (Exception e)
            {
                throw new UnpredictableException(e);
            }
            if (task != null)
                api.ShowTask(task);
            else
                api.ShowWarning("Нет задачи с таким именем");
        }

        public void CreateTask(object? sender, TaskData task)
        {
            try
            {
                discipline.CreateTask(task);
            }
            catch (ExistingName)
            {
                api.ShowWarning($"Задача с таким именем уже существует");
            }
            catch (NoRecord)
            {
                api.ShowWarning($"Сначала создайте дисциплину с таким именем");
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

        public void UpdateTask(object? sender, TaskData task)
        {
            try
            {
                discipline.UpdateTask(task);
            }
            catch (NoRecord)
            {
                api.ShowWarning($"Такой задачи ещё не было создано");
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

        public void DeleteTask(object? sender, EventArgsDisciplineTaskNames args)
        {
            try
            {
                discipline.DeleteTask(args.TaskName, args.DisciplineName);
            }
            catch (NoRecord e) 
            {
                logger.Info(e, "Отсутсвует соедиение с БД");
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
