using Microsoft.Extensions.Configuration;

namespace Blog.Persistance.Configurations
{
    public static class ConnectionStringConfiguration
    {
        static public string ConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new ConfigurationManager();
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../WebApi/Blog.WebApi"));
                configurationManager.AddJsonFile("appsettings.json");

                return configurationManager.GetConnectionString("DefaultConnectionString");
            }
        }
    }
}
