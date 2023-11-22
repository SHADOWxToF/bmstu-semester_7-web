namespace BL.ForDA.DTO
{
    public class DisciplineData
    {
        public int ID { get; set; }
        public int UID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DisciplineData(int id, int uid, string name, string description)
        {
            ID = id;
            UID = uid;
            Name = name;
            Description = description;
        }
    }
}
