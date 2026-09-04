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

    protected void SafeClick(By locator)
    {
        var element = Wait.Until(d =>
        {
            var el = d.FindElement(locator);
            return (el.Displayed && el.Enabled) ? el : null;
        });

        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline: 'nearest'});", element);

        try
        {
            element!.Click();
        }
        catch (ElementClickInterceptedException)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
        }
    }
}