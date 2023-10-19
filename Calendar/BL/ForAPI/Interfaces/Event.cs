namespace BL.ForAPI.Interfaces
{
    public class EventArgsDisciplineTaskNames : EventArgs
    {
        public string TaskName { get; set; } = "";
        public string DisciplineName { get; set; } = "";
        public EventArgsDisciplineTaskNames() { }
        public EventArgsDisciplineTaskNames(string taskName, string disciplineName)
        {
            TaskName = taskName;
            DisciplineName = disciplineName;
        }
    }

    public class EventArgsGetTasksFromTo : EventArgs
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public EventArgsGetTasksFromTo() { }
        public EventArgsGetTasksFromTo(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }
    }
}
