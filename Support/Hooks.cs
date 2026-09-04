using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;
using reqnroll_project.Config;
using reqnroll_project.Pages;
using reqnroll_project.Utilities;

namespace reqnroll_project.Support;

[Binding]
public sealed class Hooks
{
    private readonly IObjectContainer _container;
    private IWebDriver? _driver;

    public Hooks(IObjectContainer container)
    {
        _container = container;
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        var settings = ConfigReader.Load();
        _driver = DriverFactory.CreateDriver(settings.Browser);
        _driver.Manage().Window.Maximize();

        _container.RegisterInstanceAs(settings);
        _container.RegisterInstanceAs(_driver);
        _container.RegisterInstanceAs(new LoginPage(_driver, settings.TimeoutSeconds));
        _container.RegisterInstanceAs(new RoomReservationPage(_driver, settings.TimeoutSeconds));
    }

    [AfterScenario]
    public void AfterScenario()
    {
        _driver?.Quit();
        _driver?.Dispose();
    }
}