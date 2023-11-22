namespace BL.ForAPI.DTO
{
    public class DisciplineData
    {
        public int ID { get; }
        public int UID { get; }
        public string Name { get; }
        public string Description { get; }
        public DisciplineData(int id, int uid, string name, string description)
        {
            ID = id;
            UID = uid;
            Name = name;
            Description = description;
        }
    }
}
