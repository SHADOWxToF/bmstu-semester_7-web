using BL.ForDA.DTO;

namespace BL.ForDA.Interfaces
{
    public interface ITaskRepository
    {
        public Task<List<TaskData>?> GetDTasks(int userID, int disciplineID);
        public Task<List<TaskData>?> GetTasks(int userID, DateTime from, DateTime to);
        public Task<List<TaskData>?> GetTasks(int userID);
        public Task<TaskData?> GetTask(int userID, int taskID);
        public Task<List<TaskData>?> GetNearestTasks(int userID, int days);
        public Task CreateTask(TaskData task);
        public Task UpdateTask(TaskData task);
        public Task DeleteTask(int userID, int taskID);
    }
}