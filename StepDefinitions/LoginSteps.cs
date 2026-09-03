using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Reqnroll;

[Binding]
public sealed class LoginUiSteps
{
    private IWebDriver? _driver;
    private WebDriverWait? _wait;

    [BeforeScenario]
    public void StartBrowser()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");

        _driver = new ChromeDriver(options);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    [Given("the user is on the login page")]
    public void GivenTheUserIsOnTheLoginPage()
    {
        _driver!.Navigate().GoToUrl("https://www.saucedemo.com/");
        _wait!.Until(d => d.FindElement(By.Id("user-name")).Displayed);
    }

    [When("they enter a valid username and password")]
    public void WhenTheyEnterValidCredentials()
    {
        var usernameInput = _wait!.Until(d => d.FindElement(By.Id("user-name")));
        var passwordInput = _driver!.FindElement(By.Id("password"));
        var loginButton = _driver.FindElement(By.Id("login-button"));

        usernameInput.Clear();
        usernameInput.SendKeys("standard_user");

        passwordInput.Clear();
        passwordInput.SendKeys("secret_sauce");

        loginButton.Click();
    }

    [Then("they should be redirected to the dashboard")]
    public void ThenTheySeeTheDashboard()
    {
        // SauceDemo lands on inventory.html upon successful authentication
        var redirected = _wait!.Until(d => d.Url.Contains("inventory.html"));
        Assert.That(redirected, Is.True, $"Expected URL to contain 'inventory.html', but was '{_driver!.Url}'.");
    }

    [AfterScenario]
    public void StopBrowser()
    {
        try
        {
            _driver?.Quit();
        }
        finally
        {
            _driver?.Dispose();
            _driver = null;
        }
    }
}