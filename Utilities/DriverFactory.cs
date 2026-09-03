using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace reqnroll_project.Utilities;

public static class DriverFactory
{
    public static IWebDriver CreateDriver(string browser)
    {
        return browser.ToLowerInvariant() switch
        {
            "chrome" => new ChromeDriver(new ChromeOptions { AcceptInsecureCertificates = true }),
            _ => throw new NotSupportedException($"Browser '{browser}' is not configured.")
        };
    }
}