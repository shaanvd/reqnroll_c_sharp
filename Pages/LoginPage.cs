using OpenQA.Selenium;

namespace reqnroll_project.Pages;

public sealed class LoginPage : BasePage
{
    private readonly By _usernameField = By.Id("user-name");
    private readonly By _passwordField = By.Id("password");
    private readonly By _loginButton = By.Id("login-button");

    public LoginPage(IWebDriver driver, int timeoutSeconds) : base(driver, timeoutSeconds) { }

    public void NavigateTo(string url) => Driver.Navigate().GoToUrl(url);

    public void Login(string username, string password)
    {
        Wait.Until(d => d.FindElement(_usernameField)).SendKeys(username);
        Driver.FindElement(_passwordField).SendKeys(password);
        Driver.FindElement(_loginButton).Click();
    }

    public bool IsInventoryPageDisplayed()
    {
        return Wait.Until(d => d.Url.Contains("inventory.html"));
    }
}