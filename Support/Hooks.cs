using Allure.Net.Commons;
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
    private readonly ScenarioContext _scenarioContext; // 1. Needed to detect failures
    private IWebDriver? _driver;

    public Hooks(IObjectContainer container, ScenarioContext scenarioContext)
    {
        _container = container;
        _scenarioContext = scenarioContext;
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
        TestLogger.Log.Information("Starting scenario: {Title}", _scenarioContext.ScenarioInfo.Title);
    }
    

  [AfterScenario]
    public void AfterScenario()
    {
        if (_scenarioContext.TestError != null)
        {
            TestLogger.Log.Error("Test failed: {Message}", _scenarioContext.TestError.Message);

            if (_driver is ITakesScreenshot screenshotDriver)
            {
                var screenshot = screenshotDriver.GetScreenshot().AsByteArray;
                AllureApi.AddAttachment("Failure Screenshot", "image/png", screenshot);
            }
        }
        else
        {
            TestLogger.Log.Information("Test passed successfully.");
        }

        _driver?.Quit();
        _driver?.Dispose();
    }

    [AfterTestRun]
    public static void FlushLogs()
    {
        Serilog.Log.CloseAndFlush();
    }
}