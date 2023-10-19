namespace BL.DTO
{
    public class TaskData
    {
        public int Id { get; set; }
        public string Name { get; set; } = "No name";
        public string Description { get; set; } = "No descripition";
        public string DisciplineName { get; set; } = "No name";
        public DateTime Date { get; set; } = new DateTime();
        public int Cost { get; set; } = 1;
        public bool Finished { get; set; } = false;
        public TaskData() { }
        public TaskData(int id, string name, string description, string dName, DateTime date, int cost, bool finished)
        {
            Id = id;
            Name = name;
            Description = description;
            DisciplineName = dName;
            Date = date;
            Cost = cost;
            Finished = finished;
        }
    }
}
