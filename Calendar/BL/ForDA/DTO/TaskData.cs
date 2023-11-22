namespace BL.ForDA.DTO
{
    public class TaskData
    {
        public int ID { get; set; }
        public int DisciplineID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Cost { get; set; }
        public bool Finished { get; set; }
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
