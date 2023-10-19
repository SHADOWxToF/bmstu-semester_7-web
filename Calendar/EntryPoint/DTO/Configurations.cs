using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntryPoint.DTO
{
    public class Configurations
    {
        public string UIType { get; set; } = "console";
        public string DAType { get; set; } = "postgres";
        public string DBConnection { get; set; } = "Host=localhost;Username=postgres;Password=root;Database=ppo";
        public Configurations() { }
        public Configurations(string uiType, string daType, string dbConnection)
        {
            UIType = uiType;
            DAType = daType;
            DBConnection = dbConnection;
        }

    }
}
