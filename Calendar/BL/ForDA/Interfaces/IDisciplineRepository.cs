using BL.DTO;

namespace BL.ForDA.Interfaces
{
    public interface IDisciplineRepository
    {
        public List<DisciplineData>? GetDisciplines();
        public DisciplineData? GetDiscipline(string disciplineName);
        public void CreateDiscipline(DisciplineData discipline);
        public void UpdateDiscipline(DisciplineData discipline);
        public void DeleteDiscipline(string disciplineName);
    }
}