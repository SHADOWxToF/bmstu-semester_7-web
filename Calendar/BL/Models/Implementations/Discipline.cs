using BL.ForDA.Interfaces;
using BL.DTO;
using BL.Models.Interfaces;
namespace BL.Models.Implementations
{
    public class Discipline : IDiscipline
    {
        private IDisciplineRepository disciplineRepository;
        private ITaskRepository taskRepository;

        public Discipline(IDisciplineRepository disciplineRepository, ITaskRepository taskRepository)
        {
            this.disciplineRepository = disciplineRepository;
            this.taskRepository = taskRepository;
        }

        public List<TaskData>? GetTasks(DateTime from, DateTime to)
        {
            return taskRepository.GetTasks(from, to);
        }

        public List<DisciplineData>? GetDisciplines()
        {
            return disciplineRepository.GetDisciplines();
        }

        public List<TaskData>? GetDTasks(string disciplineName)
        {
            return taskRepository.GetDTasks(disciplineName);
        }

        public DisciplineData? GetDiscipline(string disciplineName)
        {
            return disciplineRepository.GetDiscipline(disciplineName);
        }

        public void CreateDiscipline(DisciplineData discipline)
        {
            disciplineRepository.CreateDiscipline(discipline);
        }

        public void UpdateDiscipline(DisciplineData discipline)
        {
            disciplineRepository.UpdateDiscipline(discipline);
        }

        public void DeleteDiscipline(string disciplineName)
        {
            disciplineRepository.DeleteDiscipline(disciplineName);
        }

        public TaskData? GetTask(string disciplineName, string taskName)
        {
            return taskRepository.GetTask(disciplineName, taskName);
        }

        public void CreateTask(TaskData task)
        {
            taskRepository.CreateTask(task);
        }
        public void UpdateTask(TaskData task)
        {
            taskRepository.UpdateTask(task);
        }
        public void DeleteTask(string taskName, string disciplineName)
        {
            taskRepository.DeleteTask(taskName, disciplineName);
        }
    }
}
