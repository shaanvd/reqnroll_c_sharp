using System;
using System.IO;
using NUnit.Framework;
using Serilog;

namespace reqnroll_project.Utilities;

public static class TestLogger
{
    private static readonly Serilog.Core.Logger LoggerInstance;
    public static readonly string LogsDirectory;

    static TestLogger()
    {
        // 1. Pipe internal Serilog write failures directly to NUnit console
        Serilog.Debugging.SelfLog.Enable(msg => TestContext.Progress.WriteLine($"[Serilog Error] {msg}"));

        // 2. Cross-platform path resolution:
        // In Docker: resolves cleanly to /app/Logs
        // On Windows: resolves to <repo>\reqnroll_project\Logs
        LogsDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Logs"));

        // 3. Ensure the target directory exists
        Directory.CreateDirectory(LogsDirectory);

        // 4. Configure file logger
        LoggerInstance = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(
                Path.Combine(LogsDirectory, "test_execution_.log"),
                rollingInterval: RollingInterval.Day,
                flushToDiskInterval: TimeSpan.FromSeconds(1),
                shared: true,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        Serilog.Log.Logger = LoggerInstance;
    }

    public static Serilog.ILogger Log => LoggerInstance;
}