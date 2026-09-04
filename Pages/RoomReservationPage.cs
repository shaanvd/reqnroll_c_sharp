using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace reqnroll_project.Pages;

public sealed class RoomReservationPage : BasePage
{
    private readonly By _checkAvailabilityBtn = By.XPath("//button[normalize-space()='Check Availability']");
    private readonly By _searchStayBtn = By.CssSelector("#booking button.btn-primary");
    private readonly By _roomLinks = By.CssSelector("#rooms a.btn.btn-primary, #rooms button.btn.btn-primary");
    private readonly By _bookThisRoomHeader = By.XPath("//h2[normalize-space()='Book This Room']");
    private readonly By _reserveNowBtn = By.XPath("//button[normalize-space()='Reserve Now']");
    private readonly By _firstNameInput = By.CssSelector("input[name='firstname']");
    private readonly By _validationAlert = By.CssSelector(".alert.alert-danger");

    public RoomReservationPage(IWebDriver driver, int timeoutSeconds) : base(driver, timeoutSeconds) { }

    private IWebElement Visible(By by) => Wait.Until(driver =>
    {
        var element = driver.FindElement(by);
        return element.Displayed ? element : null;
    })!;

    private void SafeClick(IWebElement element)
    {
        ((IJavaScriptExecutor)Driver).ExecuteScript(
            "arguments[0].scrollIntoView({block: 'center', inline: 'nearest'});",
            element);

        try
        {
            Wait.Until(d => element.Displayed && element.Enabled);
            element.Click();
        }
        catch (ElementClickInterceptedException)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
        }
    }

    public void Open(string url)
    {
        Driver.Navigate().GoToUrl(url);
        Visible(_checkAvailabilityBtn);
    }

    public void SearchDisplayedStay()
    {
        var button = Visible(_searchStayBtn);
        SafeClick(button);
    }

    public void OpenFirstAvailableRoom()
    {
        Wait.Until(driver =>
        {
            try
            {
                var link = driver.FindElements(_roomLinks)
                    .FirstOrDefault(element => element.Displayed &&
                        (element.Text.Contains("Book now", StringComparison.OrdinalIgnoreCase) ||
                         element.Text.Contains("Book this room", StringComparison.OrdinalIgnoreCase)));

                if (link == null)
                {
                    return false;
                }

                SafeClick(link);
                return true;
            }
            catch (Exception ex) when (ex is StaleElementReferenceException or ElementClickInterceptedException)
            {
                return false;
            }
        });

        Visible(_bookThisRoomHeader);
    }

    public void StartReservation()
    {
        Wait.Until(driver =>
        {
            try
            {
                var reserveBtn = driver.FindElement(_reserveNowBtn);
                if (!reserveBtn.Displayed || !reserveBtn.Enabled)
                {
                    return false;
                }

                SafeClick(reserveBtn);
                return true;
            }
            catch (Exception ex) when (ex is StaleElementReferenceException or ElementClickInterceptedException)
            {
                return false;
            }
        });

        Visible(_firstNameInput);
    }

    public void SubmitEmptyGuestForm()
    {
        Wait.Until(driver =>
        {
            try
            {
                var reserveBtn = driver.FindElement(_reserveNowBtn);
                SafeClick(reserveBtn);
                return true;
            }
            catch (Exception ex) when (ex is StaleElementReferenceException or ElementClickInterceptedException)
            {
                return false;
            }
        });
    }

    public string GetValidationErrors()
    {
        return Visible(_validationAlert).Text;
    }
}