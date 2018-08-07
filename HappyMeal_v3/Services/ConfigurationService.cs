using System;
using Microsoft.Extensions.Configuration;

namespace HappyMeal_v3.Services
{
    public class ConfigurationService
    {
        public static IConfigurationRoot Configuration { get; set; }

        static ConfigurationService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("C:/Users/adelamare/Downloads/Thomas aka le stagiaire/HappyMeal_v3/HappyMeal_v3/appsettings.json");

            Configuration = builder.Build();
        }

        public static string GetValue(string key)
        {
            return Configuration[key] ?? string.Empty;
        }
    }
}
