namespace BL.ForAPI.DTO
{
    public class UserData
    {
        public int ID { get; }
        public string Login { get; }
        public string Password { get; }
        public UserData(int id, string login, string password)
        {
            ID = id;
            Login = login;
            Password = password;
        }
    }
}
