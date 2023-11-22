namespace BL.ForAPI.DTO
{
    public class TaskData
    {
        public int ID { get; }
        public int DisciplineID { get; }
        public int UserID { get; }
        public string Name { get; }
        public string Description { get; }
        public DateTime Date { get; }
        public int Cost { get; }
        public bool Finished { get; }
        public TaskData(int id, int disciplineID, int userID, string name, string description, DateTime date, int cost, bool finished)
        {
            ID = id;
            DisciplineID = disciplineID;
            UserID = userID;
            Name = name;
            Description = description;
            Date = date;
            Cost = cost;
            Finished = finished;
        }
    }
}
