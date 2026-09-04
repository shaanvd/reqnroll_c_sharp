using System.IO;
using System.Runtime.CompilerServices;

namespace reqnroll_project.Support;

public static class TestInitializer
{
    [ModuleInitializer]
    public static void EnsureAllureDirectoriesExist()
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;

        // Ensure whatever directory Allure needs exists before Allure initializes
        Directory.CreateDirectory(Path.Combine(baseDir, "TestResults"));
        Directory.CreateDirectory(Path.Combine(baseDir, "allure-results"));
    }
}