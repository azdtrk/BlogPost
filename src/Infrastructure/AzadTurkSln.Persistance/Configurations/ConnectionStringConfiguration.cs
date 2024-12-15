using Microsoft.Extensions.Configuration;

namespace AzadTurkSln.Persistance.Configurations
{
    public static class ConnectionStringConfiguration
    {
        static public string ConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new ConfigurationManager();
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../WebApi/AzadTurkSln.WebApi"));
                configurationManager.AddJsonFile("appsettings.json");

                return configurationManager.GetConnectionString("DefaultConnectionString");
            }
        }
    }
}
