Feature: User Authentication

Scenario: Successful login with valid credentials
    Given the user navigates to the login portal
    When they enter valid credentials from configuration
    Then they should be redirected to the inventory dashboard

Scenario: Unsuccessful login with invalid credentials
	Given the user navigates to the login portal
	When they enter invalid credentials from configuration
    Then an error message should display "Epic sadface: Sorry, this user has been locked out."