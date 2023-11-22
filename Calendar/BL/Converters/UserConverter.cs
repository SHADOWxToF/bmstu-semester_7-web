namespace BL.Converters
{
    public class UserConverter
    {
        public static ForDA.DTO.UserData? ConvertFromAPIToDA(ForAPI.DTO.UserData? apiUserData)
        {
            if (apiUserData == null)
                return null;
            return new ForDA.DTO.UserData(apiUserData.ID, 
                                          apiUserData.Login, 
                                          apiUserData.Password);
        }

        public static ForAPI.DTO.UserData? ConvertFromDAToAPI(ForDA.DTO.UserData? daUserData)
        {
            if (daUserData == null)
                return null;
            return new ForAPI.DTO.UserData(daUserData.ID,
                                    daUserData.Login,
                                    daUserData.Password);
        }

        public static List<ForDA.DTO.UserData>? ConvertFromAPIToDA(List<ForAPI.DTO.UserData>? apiUserData)
        {
            if (apiUserData == null)
                return null;
            List<ForDA.DTO.UserData>? daList = new();
            foreach (var item in apiUserData)
                daList.Add(ConvertFromAPIToDA(item));
            return daList;
        }

        public static List<ForAPI.DTO.UserData>? ConvertFromDAToAPI(List<ForDA.DTO.UserData>? daUserData)
        {
            if (daUserData == null)
                return null;
            List<ForAPI.DTO.UserData>? apiList = new();
            foreach (var item in daUserData)
                apiList.Add(ConvertFromDAToAPI(item));
            return apiList;
        }
    }
}
