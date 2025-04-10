using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringDataSorter.App.Settings
{
    public static class ApplicationConfiguration
    {
        public static AppSettings GetSettings()
        {
            var appSettings = new AppSettings();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) 
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            configuration.GetSection("AppSettings").Bind(appSettings);

            return appSettings;
        }
    }
}
