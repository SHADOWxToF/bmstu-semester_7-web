namespace BL.ForDA.DTO
{
    public class UserData
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserData(int id, string login, string password)
        {
            ID = id;
            Login = login;
            Password = password;
        }
    }
}
