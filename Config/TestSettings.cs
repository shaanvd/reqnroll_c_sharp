using System.Collections.Generic;
using reqnroll_project.Models;

namespace reqnroll_project.Config;

public sealed class TestSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ShadyMeadowsUrl { get; set; } = string.Empty;
    public string Browser { get; set; } = "Chrome";
    public int TimeoutSeconds { get; set; } = 15;
    public string LogDirectory { get; set; } = "TestResults/Logs";
    public bool UseGrid { get; set; } = false;
    public string GridUrl { get; set; } = "http://localhost:4444/wd/hub";

    public Dictionary<string, UserCredentials> Users { get; set; } = new();
}