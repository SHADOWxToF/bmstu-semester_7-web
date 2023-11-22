using DataAccess.DA.Implementations;
using BL.Models.Implementations;
using BL.Presenters.Implementations;
using Npgsql;
using UI;
using BL.ForAPI.Interfaces;
using BL.ForDA.Interfaces;
using BL.Models.Interfaces;
using Newtonsoft.Json;
using EntryPoint.DTO;
using GUI;
namespace EntryPoint.Application
{
    public class App
    {
        private IDisciplineRepository dRepository;
        private ITaskRepository tRepository;
        private INotificationRepository cRepository;
        private IHolidayRepository hRepository;
        private INotification calendar;
        private IDiscipline discipline;
        private IHoliday holiday;
        private IAPI ui;
        private CPresenter cPresenter;
        private DPresenter dPresenter;
        private HPresenter hPresenter;
        private bool executable = true;
        public App()
        {
            var config = ReadConfig();
            if (config.DAType == "postgres")
            {
                NpgsqlConnection connection = new NpgsqlConnection(config.DBConnection);
                dRepository = new DisciplineRepository(connection);
                tRepository = new TaskRepository(connection);
                hRepository = new HolidayRepository(connection);
                connection = new NpgsqlConnection(config.DBConnection);
                cRepository = new NotificationRepository(connection);
            }
            else
            {
                executable = false;
                return;
            }
            if (config.UIType == "console")
                ui = new ConsoleUI();
            else if (config.UIType == "gui")
                ui = new GUI.GUI();
            else
            {
                executable = false;
                return;
            }
            calendar = new Notification(dRepository, tRepository, cRepository);
            discipline = new Discipline(dRepository, tRepository);
            holiday = new Holiday(hRepository);
            cPresenter = new CPresenter(ui, calendar);
            dPresenter = new DPresenter(ui, discipline);
            hPresenter = new HPresenter(ui, holiday);
        }

        private Configurations ReadConfig()
        {
            string path = "..\\..\\..\\..\\config.json";
            Configurations? config = null;
            if (File.Exists(path))
            {
                string jsonString = File.ReadAllText(path);
                config = JsonConvert.DeserializeObject<Configurations>(jsonString);
            }
            if (config == null)
                config = new();
            return config;
        }
        public void execute()
        {
            if (executable)
            {
                try
                {
                    ui.Start();
                }
                catch (Exception e)
                {
                    NLog.LogManager.GetCurrentClassLogger().Fatal(e, "Неизвестный тип ошибки");
                }
            }
            else
                NLog.LogManager.GetCurrentClassLogger().Fatal("Неверное содержимое конфигурационного файла");
        }
    }
}
