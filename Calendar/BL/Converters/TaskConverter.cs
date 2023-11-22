namespace BL.Converters
{
    public class TaskConverter
    {
        public static ForDA.DTO.TaskData? ConvertFromAPIToDA(ForAPI.DTO.TaskData? apiTaskData)
        {
            if (apiTaskData == null)
                return null;
            return new ForDA.DTO.TaskData(apiTaskData.ID,
                                          apiTaskData.DisciplineID,
                                          apiTaskData.UserID,
                                          apiTaskData.Name,
                                          apiTaskData.Description,
                                          apiTaskData.Date,
                                          apiTaskData.Cost,
                                          apiTaskData.Finished);
        }

        public static ForAPI.DTO.TaskData? ConvertFromDAToAPI(ForDA.DTO.TaskData? daTaskData)
        {
            if (daTaskData == null)
                return null;
            return new ForAPI.DTO.TaskData(daTaskData.ID,
                                    daTaskData.DisciplineID,
                                    daTaskData.UserID,
                                    daTaskData.Name,
                                    daTaskData.Description,
                                    daTaskData.Date,
                                    daTaskData.Cost,
                                    daTaskData.Finished);
        }

        public static List<ForDA.DTO.TaskData>? ConvertFromAPIToDA(List<ForAPI.DTO.TaskData>? apiTaskData)
        {
            if (apiTaskData == null)
                return null;
            List<ForDA.DTO.TaskData>? daList = new();
            foreach (var item in apiTaskData)
                daList.Add(ConvertFromAPIToDA(item));
            return daList;
        }

        public static List<ForAPI.DTO.TaskData>? ConvertFromDAToAPI(List<ForDA.DTO.TaskData>? daTaskData)
        {
            if (daTaskData == null)
                return null;
            List<ForAPI.DTO.TaskData>? apiList = new();
            foreach (var item in daTaskData)
                apiList.Add(ConvertFromDAToAPI(item));
            return apiList;
        }
    }
}
