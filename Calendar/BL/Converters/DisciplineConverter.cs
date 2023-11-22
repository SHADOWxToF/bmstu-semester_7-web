namespace BL.Converters
{
    public class DisciplineConverter
    {
        public static ForDA.DTO.DisciplineData? ConvertFromAPIToDA(ForAPI.DTO.DisciplineData? apiDisciplineData)
        {
            if (apiDisciplineData == null)
                return null;
            return new ForDA.DTO.DisciplineData(apiDisciplineData.ID,
                                          apiDisciplineData.UID,
                                          apiDisciplineData.Name,
                                          apiDisciplineData.Description);
        }

        public static ForAPI.DTO.DisciplineData? ConvertFromDAToAPI(ForDA.DTO.DisciplineData? daDisciplineData)
        {
            if (daDisciplineData == null)
                return null;
            return new ForAPI.DTO.DisciplineData(daDisciplineData.ID,
                                    daDisciplineData.UID,
                                    daDisciplineData.Name,
                                    daDisciplineData.Description);
        }

        public static List<ForDA.DTO.DisciplineData>? ConvertFromAPIToDA(List<ForAPI.DTO.DisciplineData>? apiDisciplineData)
        {
            if (apiDisciplineData == null)
                return null;
            List<ForDA.DTO.DisciplineData>? daList = new();
            foreach (var item in apiDisciplineData)
                daList.Add(ConvertFromAPIToDA(item));
            return daList;
        }

        public static List<ForAPI.DTO.DisciplineData>? ConvertFromDAToAPI(List<ForDA.DTO.DisciplineData>? daDisciplineData)
        {
            if (daDisciplineData == null)
                return null;
            List<ForAPI.DTO.DisciplineData>? apiList = new();
            foreach (var item in daDisciplineData)
                apiList.Add(ConvertFromDAToAPI(item));
            return apiList;
        }
    }
}
