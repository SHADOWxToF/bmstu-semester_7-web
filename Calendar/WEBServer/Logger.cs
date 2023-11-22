using NLog;
namespace Logger
{
    public class Logger
    {
        public static void SetNLogConfig()
        {
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration("NLog.config");
            //var config = new NLog.Config.LoggingConfiguration();

            //var logfile = new NLog.Targets.FileTarget("logfile")
            //{   
            //    FileName = "logs\\${shortdate}-logfile.txt",
            //    Layout = "${longdate} (UTC+3:00) ${level} ${message}  ${exception}"
            //};

            //config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);

            //LogManager.Configuration = config;
        }
    }
}
