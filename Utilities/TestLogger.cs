using System;
using System.IO;
using Serilog;

namespace reqnroll_project.Utilities;

public static class TestLogger
{
    private static readonly Serilog.Core.Logger LoggerInstance;

    static TestLogger()
    {
        var logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        Directory.CreateDirectory(logsDirectory);

        LoggerInstance = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(
                Path.Combine(logsDirectory, "test_execution_.log"),
                rollingInterval: RollingInterval.Day,
                flushToDiskInterval: TimeSpan.FromSeconds(1), 
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        Serilog.Log.Logger = LoggerInstance;
    }

    public static Serilog.ILogger Log => LoggerInstance;
}