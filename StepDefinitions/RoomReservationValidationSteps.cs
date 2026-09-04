using NUnit.Framework;
using Reqnroll;
using reqnroll_project.Config;
using reqnroll_project.Pages;

namespace reqnroll_project.StepDefinitions;

[Binding]
public sealed class RoomReservationValidationSteps
{
    private readonly RoomReservationPage _roomPage;
    private readonly TestSettings _settings;

    public RoomReservationValidationSteps(RoomReservationPage roomPage, TestSettings settings)
    {
        _roomPage = roomPage;
        _settings = settings;
    }

    [Given("the guest opens the Shady Meadows booking site")]
    public void OpenBookingSite()
    {
        _roomPage.Open(_settings.ShadyMeadowsUrl);
    }

    [When("the guest searches for the displayed one-night stay")]
    public void SearchDisplayedStay()
    {
        _roomPage.SearchDisplayedStay();
    }

    [When("opens the first available room")]
    public void OpenFirstAvailableRoom()
    {
        _roomPage.OpenFirstAvailableRoom();
    }

    [When("starts the reservation")]
    public void StartReservation()
    {
        _roomPage.StartReservation();
    }

    [When("submits the guest details form without entering details")]
    public void SubmitEmptyGuestForm()
    {
        _roomPage.SubmitEmptyGuestForm();
    }

    [Then("booking validation errors should be displayed")]
    public void VerifyValidationErrors()
    {
        var message = _roomPage.GetValidationErrors();

        Assert.Multiple(() =>
        {
            Assert.That(message, Does.Contain("Firstname should not be blank"));
            Assert.That(message, Does.Contain("Lastname should not be blank"));
        });
    }
}