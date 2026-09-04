using System;
using Allure.Net.Commons;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;
using reqnroll_project.Config;
using reqnroll_project.Pages;
using reqnroll_project.Utilities;
using Serilog;

namespace reqnroll_project.Support;

[Binding]
public sealed class Hooks
{
    private readonly IObjectContainer _container;
    private readonly ScenarioContext _scenarioContext;
    private IWebDriver? _driver;

    public Hooks(IObjectContainer container, ScenarioContext scenarioContext)
    {
        _container = container;
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        TestLogger.Log.Information("=== Starting Scenario: {Title} ===", _scenarioContext.ScenarioInfo.Title);
        TestContext.Progress.WriteLine($"[LOG FOLDER]: {TestLogger.LogsDirectory}");

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
        try
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
                TestLogger.Log.Information("=== Scenario Passed Successfully ===");
            }
        }
        finally
        {
            _driver?.Quit();
            _driver?.Dispose();
            _driver = null;
        }
    }

    [AfterTestRun]
    public static void FlushLogs()
    {
        Log.CloseAndFlush();
    }
}