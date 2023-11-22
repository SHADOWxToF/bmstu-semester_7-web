using BL.ForAPI.DTO;
namespace BL.Models.Interfaces
{
    public interface IDiscipline
    {
        public Task<List<TaskData>?> GetTasks(int userID, DateTime from, DateTime to);
        public Task<List<TaskData>?> GetTasks(int userID);
        public Task<List<DisciplineData>?> GetDisciplines(int userID);
        public Task<List<TaskData>?> GetDTasks(int userID, int disciplineID);
        public Task<DisciplineData?> GetDiscipline(int userID, int disciplineID);
        public Task CreateDiscipline(DisciplineData discipline);
        public Task UpdateDiscipline(DisciplineData discipline);
        public Task DeleteDiscipline(int userID, int disciplineID);
        public Task<TaskData?> GetTask(int userID, int taskID);
        public Task CreateTask(TaskData task);
        public Task UpdateTask(TaskData task);
        public Task DeleteTask(int userID, int taskID);
    }
}
