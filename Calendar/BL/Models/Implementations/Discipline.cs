using BL.ForDA.Interfaces;
using BL.ForAPI.DTO;
using BL.Models.Interfaces;
using BL.Converters;

namespace BL.Models.Implementations
{
    public class Discipline : IDiscipline
    {
        private readonly IDisciplineRepository disciplineRepository;
        private ITaskRepository taskRepository;

        public Discipline(IDisciplineRepository disciplineRepository, ITaskRepository taskRepository)
        {
            this.disciplineRepository = disciplineRepository;
            this.taskRepository = taskRepository;
        }

        public async Task<List<TaskData>?> GetTasks(int userID, DateTime from, DateTime to)
        {
            return TaskConverter.ConvertFromDAToAPI(await taskRepository.GetTasks(userID, from, to));
        }

        public async Task<List<TaskData>?> GetTasks(int userID)
        {
            return TaskConverter.ConvertFromDAToAPI(await taskRepository.GetTasks(userID));
        }

        public async Task<List<DisciplineData>?> GetDisciplines(int userID)
        {
            return DisciplineConverter.ConvertFromDAToAPI(await disciplineRepository.GetDisciplines(userID));
        }

        public async Task<List<TaskData>?> GetDTasks(int userID, int disciplineID)
        {
            return TaskConverter.ConvertFromDAToAPI(await taskRepository.GetDTasks(userID, disciplineID));
        }

        public async Task<DisciplineData?> GetDiscipline(int userID, int disciplineID)
        {
            return DisciplineConverter.ConvertFromDAToAPI(await disciplineRepository.GetDiscipline(userID, disciplineID));
        }

        public async Task CreateDiscipline(DisciplineData discipline)
        {
            await disciplineRepository.CreateDiscipline(DisciplineConverter.ConvertFromAPIToDA(discipline));
        }

        public async Task UpdateDiscipline(DisciplineData discipline)
        {
            await disciplineRepository.UpdateDiscipline(DisciplineConverter.ConvertFromAPIToDA(discipline));
        }

        public async Task DeleteDiscipline(int userID, int disciplineID)
        {
            await disciplineRepository.DeleteDiscipline(userID, disciplineID);
        }

        public async Task<TaskData?> GetTask(int userID, int taskID)
        {
            return TaskConverter.ConvertFromDAToAPI(await taskRepository.GetTask(userID, taskID));
        }

        public async Task CreateTask(TaskData task)
        {
            await taskRepository.CreateTask(TaskConverter.ConvertFromAPIToDA(task));
        }
        public async Task UpdateTask(TaskData task)
        {
            await taskRepository.UpdateTask(TaskConverter.ConvertFromAPIToDA(task));
        }
        public async Task DeleteTask(int userID, int taskID)
        {
            await taskRepository.DeleteTask(userID, taskID);
        }
    }
}
