using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using reqnroll_project.Config;

namespace reqnroll_project.Utilities;

public static class DriverFactory
{
    public static IWebDriver CreateDriver(TestSettings settings)
    {
        // Fallback: Read Docker environment variables directly if configuration binder missed them
        var envUseGrid = Environment.GetEnvironmentVariable("TestSettings__UseGrid");
        if (bool.TryParse(envUseGrid, out bool gridOverride))
        {
            settings.UseGrid = gridOverride;
        }

        var envGridUrl = Environment.GetEnvironmentVariable("TestSettings__GridUrl");
        if (!string.IsNullOrWhiteSpace(envGridUrl))
        {
            settings.GridUrl = envGridUrl;
        }

        var options = GetChromeOptions();

        if (settings.UseGrid)
        {
            return new RemoteWebDriver(new Uri(settings.GridUrl), options);
        }

        return settings.Browser.ToLowerInvariant() switch
        {
            "chrome" => new ChromeDriver(options),
            _ => throw new NotSupportedException($"Browser '{settings.Browser}' is not configured.")
        };
    }

    public static IWebDriver CreateDriver(string browser)
    {
        return CreateDriver(new TestSettings { Browser = browser, UseGrid = false });
    }

    private static ChromeOptions GetChromeOptions()
    {
        var options = new ChromeOptions
        {
            AcceptInsecureCertificates = true
        };

        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--disable-gpu");

        return options;
    }
}