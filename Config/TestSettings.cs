using reqnroll_project.Models;

namespace reqnroll_project.Config;

public sealed class TestSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public string Browser { get; set; } = "Chrome";
    public int TimeoutSeconds { get; set; } = 10;
    public Dictionary<string, UserCredentials> Users { get; set; } = new();
}
