using BL.DTO;

namespace BL.ForDA.Interfaces
{
    public interface ITaskRepository
    {
        public List<TaskData>? GetDTasks(string discipline_name);
        public List<TaskData>? GetTasks(DateTime from, DateTime to);
        public TaskData? GetTask(string disciplineName, string taskName);
        public List<TaskData>? GetNearestTasks(int days);
        public void CreateTask(TaskData task);
        public void UpdateTask(TaskData task);
        public void DeleteTask(string taskName, string disciplineName);
    }
}