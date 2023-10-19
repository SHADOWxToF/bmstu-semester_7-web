using NLog;
namespace EntryPoint.Application
{ 
    public class Program
    {
        public static void Main()
        {
            Logger.Logger.SetNLogConfig();
            LogManager.GetCurrentClassLogger().Info("Приложение запущено");
            var app = new App();
            app.execute();
        }
    }
}
