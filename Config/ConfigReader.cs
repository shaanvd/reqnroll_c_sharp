using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace reqnroll_project.Config;

public static class ConfigReader
{
    public static TestSettings Load()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables() // Reads TestSettings__UseGrid and TestSettings__GridUrl
            .Build();

        var settings = new TestSettings();
        configuration.GetSection("TestSettings").Bind(settings);

        return settings;
    }
}