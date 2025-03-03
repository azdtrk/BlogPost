using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Blog.Persistance.Configurations
{
    public static class ConnectionStringConfiguration
    {
        static public string ConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new ConfigurationManager();
                
                var currentDir = Directory.GetCurrentDirectory();
                
                string webApiPath = Path.Combine(currentDir, "../../Presentation/Blog.WebApi");
                
                string persistencePath = Path.Combine(currentDir, "../../../Presentation/Blog.WebApi");
                
                string rootPath = Path.Combine(currentDir, "src/Presentation/Blog.WebApi");
                
                string[] possiblePaths = new[] { webApiPath, persistencePath, rootPath };
                bool foundConfig = false;
                
                foreach (var path in possiblePaths)
                {
                    string settingsPath = Path.Combine(path, "appsettings.json");
                    if (File.Exists(settingsPath))
                    {
                        configurationManager.SetBasePath(path);
                        configurationManager.AddJsonFile("appsettings.json");
                        foundConfig = true;
                        break;
                    }
                }
                
                if (!foundConfig)
                {
                    throw new FileNotFoundException("Could not find appsettings.json in any expected locations.");
                }

                return configurationManager.GetConnectionString("DefaultConnectionString");
            }
        }
    }
}
