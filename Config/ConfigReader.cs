using System.IO;
using Microsoft.Extensions.Configuration;

namespace reqnroll_project.Config;

public static class ConfigReader
{
    public static TestSettings Load()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        return config.GetSection("TestSettings").Get<TestSettings>()
               ?? throw new InvalidOperationException("Could not bind TestSettings from appsettings.json");
    }
}