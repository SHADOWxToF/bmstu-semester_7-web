using BL.ForDA.DTO;

namespace BL.ForDA.Interfaces
{
    public interface IDisciplineRepository
    {
        public Task<List<DisciplineData>?> GetDisciplines(int userID);
        public Task<DisciplineData?> GetDiscipline(int userID, int disciplineID);
        public Task CreateDiscipline(DisciplineData discipline);
        public Task UpdateDiscipline(DisciplineData discipline);
        public Task DeleteDiscipline(int userID, int disciplineID);
    }
}