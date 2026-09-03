using NUnit.Framework;
using Reqnroll;
using reqnroll_project.Config;
using reqnroll_project.Pages;

namespace reqnroll_project.StepDefinitions;

[Binding]
public sealed class LoginSteps
{
    private readonly LoginPage _loginPage;
    private readonly TestSettings _settings;

    public LoginSteps(LoginPage loginPage, TestSettings settings)
    {
        _loginPage = loginPage;
        _settings = settings;
    }

    [Given("the user navigates to the login portal")]
    public void GivenTheUserNavigatesToTheLoginPortal()
    {
        _loginPage.NavigateTo(_settings.BaseUrl);
    }

    [When("they enter valid credentials from configuration")]
    public void WhenTheyEnterValidCredentialsFromConfiguration()
    {
        var credentials = _settings.Users["StandardUser"];
        _loginPage.Login(credentials.Username, credentials.Password);
    }

    [Then("they should be redirected to the inventory dashboard")]
    public void ThenTheyShouldBeRedirectedToTheInventoryDashboard()
    {
        Assert.That(_loginPage.IsInventoryPageDisplayed(), Is.True,
            "The user was not redirected to the inventory dashboard.");
    }
}