using BL.DTO;
namespace BL.Models.Interfaces
{
    public interface IDiscipline
    {
        public List<TaskData>? GetTasks(DateTime from, DateTime to);
        public List<DisciplineData>? GetDisciplines();
        public List<TaskData>? GetDTasks(string disciplineName);
        public DisciplineData? GetDiscipline(string disciplineName);
        public void CreateDiscipline(DisciplineData discipline);
        public void UpdateDiscipline(DisciplineData discipline);
        public void DeleteDiscipline(string disciplineName);
        public TaskData? GetTask(string disciplineName, string taskName);
        public void CreateTask(TaskData task);
        public void UpdateTask(TaskData task);
        public void DeleteTask(string taskName, string disciplineName);
    }
}
