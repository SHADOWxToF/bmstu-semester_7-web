namespace BL.DTO
{
    public class DisciplineData
    {
        public string Name { get; set; } = "No name";
        public string Description { get; set; } = "No description";
        public int Sum { get; set; } = 0;
        public DisciplineData() { }
        public DisciplineData(string name, string description, int sum)
        {
            Name = name;
            Description = description;
            Sum = sum;
        }
    }
}
