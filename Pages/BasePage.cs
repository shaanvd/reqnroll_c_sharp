using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace reqnroll_project.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;

    protected BasePage(IWebDriver driver, int timeoutSeconds)
    {
        Driver = driver;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
    }
}