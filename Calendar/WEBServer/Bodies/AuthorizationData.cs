namespace WEBServer.Bodies
{
    public class AuthorizationData
    {
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
        public AuthorizationData()
        {
            Login = "Login";
            Password = "Password";
        }
        public AuthorizationData(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
