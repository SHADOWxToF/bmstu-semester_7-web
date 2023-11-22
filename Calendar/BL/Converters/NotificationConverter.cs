namespace BL.Converters
{
    public class NotificationConverter
    {
        public static ForDA.DTO.NotificationData? ConvertFromAPIToDA(ForAPI.DTO.NotificationData? apiNotificationData)
        {
            if (apiNotificationData == null)
                return null;
            return new ForDA.DTO.NotificationData(apiNotificationData.ID,
                                                  apiNotificationData.TaskID,
                                                  apiNotificationData.UserID,
                                                  apiNotificationData.Days,
                                                  apiNotificationData.Message);
        }

        public static ForAPI.DTO.NotificationData? ConvertFromDAToAPI(ForDA.DTO.NotificationData? daNotificationData)
        {
            if (daNotificationData == null)
                return null;
            return new ForAPI.DTO.NotificationData(daNotificationData.ID,
                                            daNotificationData.TaskID,
                                            daNotificationData.UserID,
                                            daNotificationData.Days,
                                            daNotificationData.Message);
        }

        public static List<ForDA.DTO.NotificationData>? ConvertFromAPIToDA(List<ForAPI.DTO.NotificationData>? apiNotificationData)
        {
            if (apiNotificationData == null)
                return null;
            List<ForDA.DTO.NotificationData>? daList = new();
            foreach (var item in apiNotificationData)
                daList.Add(ConvertFromAPIToDA(item));
            return daList;
        }

        public static List<ForAPI.DTO.NotificationData>? ConvertFromDAToAPI(List<ForDA.DTO.NotificationData>? daNotificationData)
        {
            if (daNotificationData == null)
                return null;
            List<ForAPI.DTO.NotificationData>? apiList = new();
            foreach (var item in daNotificationData)
                apiList.Add(ConvertFromDAToAPI(item));
            return apiList;
        }
    }
}
