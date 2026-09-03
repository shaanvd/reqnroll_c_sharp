Feature: User Authentication

Scenario: Successful login with valid credentials
    Given the user navigates to the login portal
    When they enter valid credentials from configuration
    Then they should be redirected to the inventory dashboard